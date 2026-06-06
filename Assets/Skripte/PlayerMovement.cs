using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 1f;
    public float gravity = 300f;
    public float crouchHeight = 1f;       // višina med sklanjanjem
    public float standingHeight = 2f;     // normalna višina
    public float crouchSpeed = 3f;        // hitrost med sklanjanjem

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    private bool isCrouching = false;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        #region Handle Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        if (isCrouching) speed = crouchSpeed; // počasneje med sklanjanjem

        float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        #endregion

        #region Handle Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !isCrouching)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        #endregion

        #region Handle Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCrouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StopCrouch();
        }
        #endregion

        #region Handle Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion
    }

    void StartCrouch()
    {
        characterController.height = crouchHeight;
        isCrouching = true;
        // premik kamere ni potreben, CharacterController avtomatsko prilagodi center
    }

    void StopCrouch()
    {
        characterController.height = standingHeight;
        isCrouching = false;
    }
}
