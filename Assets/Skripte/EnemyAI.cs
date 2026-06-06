using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent; // uniformno ime
    
    private Vector3 _target; //currentPlayer, lastKnownDest, startPosition

    [SerializeField]
    private Vector3 _startingPos;

    private Vector3 _lastKnownLocation;
    private GameObject _player;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;


    [Header("Weapon")]
    [SerializeField] private GameObject enemyGun; // gun v roki (na začetku DISABLED)
    [SerializeField] private Transform gunPickupWaypoint; // waypoint kjer je gun
    [SerializeField] private float gunPickupDistance = 2.5f;

    private bool hasGun = false;
    private float shootCooldown = 0.80f;
    private float lastShootTime;

    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootRange = 100f;

    
    private Animator animator;
    private NavMeshAgent agent;












    private enum NPCBehaviors{
        NotAlert,
        Engaged,
        Searching
    } //enum like drop down list

    private enum PathMode
    {
        Looping,
        Random
    }
    [SerializeField]
    private PathMode currentPathMode;

    [SerializeField] //see it and change in inspector
    private NPCBehaviors currentState;
    [SerializeField] 
    private float _engagedTime;

    [SerializeField] 
    private float _searchTime;
    

    private bool _isPlayerInRange;
    [SerializeField]
    private List<Transform> _waypoints;

    private int _currentWaypoint;
    private bool _stateChanged;
    [SerializeField]
    private bool _pauseAtEnd, _pauseAtEachWaypoint;
    [SerializeField]
    private float _minPause, _maxPause;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();


        
        _agent = GetComponent<NavMeshAgent>();

        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent component missing on " + gameObject.name);
        }
        currentState = NPCBehaviors.NotAlert;

        _player = GameObject.FindGameObjectWithTag("Player");
        if(_player == null)
        {
            Debug.Log("Player is null");
        }

        _target = _startingPos;
        _stateChanged = true;

        hasGun = false;
        if (enemyGun != null)
        enemyGun.SetActive(false);

    }

    
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (_agent != null && _target != null)
        {
            //_agent.SetDestination(_target.transform.position);
            switch(currentState) 
            {
                case NPCBehaviors.NotAlert:

                    if (!hasGun && gunPickupWaypoint != null)
                    {
                        float dist = Vector3.Distance(transform.position, gunPickupWaypoint.position);
                        //Debug.Log("Dist to gun: " + dist);

                        if (dist <= gunPickupDistance)
                        {
                            PickupGun();
                        }
                    }

                    if (_stateChanged)
                    {
                        StartCoroutine(MoveRoutine());
                        _stateChanged = false;
                    }
                    break;


                   

                    break;
                    case NPCBehaviors.Engaged:
                        animator.SetBool("isShooting", true);
                        
                        _target = _player.transform.position;
                        _lastKnownLocation = _player.transform.position;
                        _agent.SetDestination(_target);
                        Vector3 lookDir = _player.transform.position - transform.position;
                        lookDir.y = 0;

                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            Quaternion.LookRotation(lookDir),
                            Time.deltaTime * 25f
                        );
                        //_agent.isStopped = hasGun;


                            if (hasGun && enemyGun != null && !enemyGun.activeSelf)
                            {
                            enemyGun.SetActive(true);
                            }

                        if (hasGun && enemyGun != null)
                            //enemyGun.SetActive(true);
                            ShootAtPlayer();

                        break;

                case NPCBehaviors.Searching:
                    _target =  _lastKnownLocation;
                    _agent.SetDestination(_target);
                    break;  
                          
            }
        }
    }

    private IEnumerator MoveRoutine()
    {
        while(currentState == NPCBehaviors.NotAlert)
        {
            yield return null;
            if(_waypoints.Count > 1 && _waypoints[_currentWaypoint] != null)
            {
                _agent.SetDestination(_waypoints[_currentWaypoint].position);
                float distance = Vector3.Distance(transform.position, _waypoints[_currentWaypoint].position);
                if(distance < 1.0f)
                {
                    switch(currentPathMode) 
                    { 
                        case PathMode.Looping:
                            if(_currentWaypoint == _waypoints.Count - 1)
                            {
                                _currentWaypoint = 0;
                                if (_pauseAtEnd)
                                {
                                    yield return new WaitForSeconds(Random.Range(_minPause,_maxPause));
                                }
                            }
                            else
                            {
                                _currentWaypoint++;
                                if (_pauseAtEachWaypoint)
                                {
                                    yield return new WaitForSeconds(Random.Range(_minPause,_maxPause));
                                }
                            }
                            break;
                        
                        case PathMode.Random:
                            _currentWaypoint = Random.Range(0, _waypoints.Count);
                            if (_pauseAtEachWaypoint)
                            {
                            yield return new WaitForSeconds(Random.Range(_minPause,_maxPause));
                            }
                            break;
                    }
                }
            }
        }

    }
    void PickupGun()
    {
    hasGun = true;

    //if (enemyGun != null)
    //    enemyGun.SetActive(true);

    // Optional: uniči gun model na waypointu
    foreach (Transform child in gunPickupWaypoint)
        Destroy(child.gameObject);

    Debug.Log("Enemy picked up gun");
    }


void ShootAtPlayer()
{
    if (Time.time < lastShootTime + shootCooldown) return;
    lastShootTime = Time.time;

    Vector3 origin = transform.position + Vector3.up * 1.5f;
    Vector3 target = _player.transform.position + Vector3.up;
    Vector3 dir = (target - origin).normalized;

    int layerMask = ~LayerMask.GetMask("Enemy", "NPC");

    if (Physics.Raycast(origin, dir, out RaycastHit hit, shootRange, layerMask))
    {
        Debug.Log("Ray hit: " + hit.collider.name);

        if (hit.collider.CompareTag("Player"))
        {
            PlayerHealth ph = hit.collider.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(20);
        }
    }

    if (audioSource != null && shootSound != null)
        audioSource.PlayOneShot(shootSound);

    ParticleSystem muzzle = enemyGun.GetComponentInChildren<ParticleSystem>();
    if (muzzle) muzzle.Play();
}





    

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            
            _isPlayerInRange = true;
            currentState = NPCBehaviors.Engaged;

        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            
            _isPlayerInRange = false;
            StartCoroutine(EngagedCooldownRoutine());

        }
    }

    //coroutinie
    private IEnumerator EngagedCooldownRoutine()
    {
        yield return new  WaitForSeconds(_engagedTime);
        if(_isPlayerInRange == false)
        { 
            currentState = NPCBehaviors.Searching; 
            animator.SetBool("isShooting", false);
            StartCoroutine(SearchingCooldownRoutine());
        }
    
    }
    private IEnumerator SearchingCooldownRoutine()
    {
        yield return new WaitForSeconds(_searchTime);
        if(_isPlayerInRange == false && currentState == NPCBehaviors.Searching)
        { 
            _stateChanged = true;
            currentState = NPCBehaviors.NotAlert;
            animator.SetBool("isShooting", false); 
            if (enemyGun != null)
            enemyGun.SetActive(false);

        }
    
    }

}
