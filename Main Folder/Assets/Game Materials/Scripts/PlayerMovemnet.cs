using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovemnet : MonoBehaviour
{
    private CharacterController controller;
    #region Movement Variables
    [Header("Movement Variables")]
    public float maxSpeed;
    public float acceleration;
    public float jumpHeight;
    #endregion
    #region Jump Variables
    [Header("Jump Variables")]
    public int maxJumpCount = 2;
    public float gravityFall;
    #endregion

    public Transform childTransform;

    #region Private Variables
    private float maxJumpSpeed = 5f;
    private float currentSpeed;
    private int jumpCount = 0;
    private float turnSpeed = 50.0f;
    private float fallMultiplier = 2.5f;
    #endregion

    #region Vector Variables
    private Vector3 playerVelocity;
    private Vector3 moveDirection;
    private bool isJumping;
    private float moveHorizontal;
    #endregion
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        //jumpHeight *= Time.deltaTime;
    }

    void Update()
    {
        Move();

        MoveInput();
    }

    public void Move()
    {
        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, 0f);

        if (!controller.isGrounded)
        {
            if (playerVelocity.y < 0)
            {
                playerVelocity.y -= gravityFall * fallMultiplier * Time.deltaTime;
            }
            else
            {
                playerVelocity.y -= gravityFall * Time.deltaTime;
            }
        }

        controller.Move((moveDirection * currentSpeed + playerVelocity));
    }

    public void MoveInput()
    {
        moveHorizontal = Input.GetAxis("Horizontal");

        float maxCurrentSpeed = (!isJumping) ? maxSpeed : maxJumpSpeed;
        if (Mathf.Abs(moveHorizontal) > 0.1f)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxCurrentSpeed);

            CharacterTurn();
        }
        else
        {
            currentSpeed = 0;
        }

        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            isJumping = false;

            jumpCount = 0;
        }
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            jumpCount++;
            playerVelocity.y = 0;
            float tempSpeed = currentSpeed;
            playerVelocity.y += CalculateJumpForce();
            isJumping = true;
            currentSpeed = tempSpeed;
        }
    }

    private void CharacterTurn()
    {
        float targetRotationAngle = Mathf.Sign(moveHorizontal) == 1 ? 0 : -180;

        Quaternion targetRotation = Quaternion.Euler(0, targetRotationAngle, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private float CalculateJumpForce()
    {
        return Mathf.Sqrt(jumpHeight * -1f * Physics.gravity.y);
    }
}

