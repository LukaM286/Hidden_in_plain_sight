using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    private NavMeshAgent _agent;

    [SerializeField] private List<Transform> _waypoints;
    private int _currentWaypoint;

    private enum PathMode { Looping, Random, Wandering }
    [SerializeField] private PathMode currentPathMode;

    [SerializeField] private bool _pauseAtEnd, _pauseAtEachWaypoint;
    [SerializeField] private float _minPause, _maxPause;
    [SerializeField] private float _wanderRadius = 5f;

    [Header("Look At Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float lookAtDistance = 6f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float lookChance = 0.1f; // 50% NPC-jev gleda

    


    private bool canLookAtPlayer;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
            Debug.LogError("NavMeshAgent component missing on " + gameObject.name);

        if (_waypoints.Count > 0 || currentPathMode == PathMode.Wandering)
            StartCoroutine(MoveRoutine());

        if (_waypoints.Count > 0)
            _currentWaypoint = Random.Range(0, _waypoints.Count);

        _agent.speed *= Random.Range(0.8f, 1.2f);

        canLookAtPlayer = Random.value < lookChance;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        canLookAtPlayer = Random.value < lookChance;

    }

    void Update()
    {
        LookAtPlayer();
    }

    void LookAtPlayer()
    {
        if (!canLookAtPlayer || player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > lookAtDistance) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f; // brez nagiba gor/dol

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            yield return null;

            if (currentPathMode == PathMode.Wandering)
            {
                if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
                {
                    Vector3 randomDir = Random.insideUnitSphere * _wanderRadius + transform.position;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomDir, out hit, _wanderRadius, NavMesh.AllAreas))
                        _agent.SetDestination(hit.position);
                }
            }
            else
            {
                if (_waypoints.Count == 0) yield break;

                _agent.SetDestination(_waypoints[_currentWaypoint].position);

                if (Vector3.Distance(transform.position, _waypoints[_currentWaypoint].position) < 1f)
                {
                    if (currentPathMode == PathMode.Looping)
                        _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Count;
                    else
                        _currentWaypoint = Random.Range(0, _waypoints.Count);

                    if (_pauseAtEachWaypoint)
                        yield return new WaitForSeconds(Random.Range(_minPause, _maxPause));
                }
            }
        }
    }
}
