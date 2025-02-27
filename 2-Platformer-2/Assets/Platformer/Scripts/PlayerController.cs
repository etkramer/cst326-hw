using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10;
    public float turnSpeed = 1;
    public float jumpForce = 10;
    public float jumpTime = 0.25f;

    int jumpIdx = 0;
    int jumpIdxLastHit = 0;

    float jumpTimeRemaining = 0;

    Animator animator;
    CharacterController characterController;
    float moveValue = 0;
    bool isGrounded = false;
    float gravity = -9.81f;
    Vector3 velocity;
    Vector3 prevMoveDir;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Reset y velocity
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        // Basic movement
        if (Input.GetKey(KeyCode.A))
        {
            moveValue = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveValue = 1;
        }
        else
        {
            moveValue = 0;
        }

        // Basic movement
        var moveDir = new Vector3(moveValue, 0, 0);
        characterController.Move(moveDir * Time.deltaTime * moveSpeed);

        // Rotate player
        if (moveDir != Vector3.zero)
        {
            prevMoveDir = moveDir;
        }
        gameObject.transform.rotation = Quaternion.Lerp(
            gameObject.transform.rotation,
            Quaternion.Euler(0, prevMoveDir.x < 0 ? 270 : 90, 0),
            turnSpeed * Time.deltaTime
        );

        // Begin jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpIdx++;
            jumpTimeRemaining = jumpTime;
        }
        else
        {
            jumpTimeRemaining = Math.Max(0, jumpTimeRemaining - Time.deltaTime);
        }

        // Hold jump
        if (Input.GetKey(KeyCode.Space) && jumpTimeRemaining > 0)
        {
            velocity.y += jumpForce * Time.deltaTime;
        }

        // Animator state
        animator.SetBool("IsJumping", !isGrounded);
        animator.SetBool("IsRunning", Math.Abs(moveValue) > 0.1f && isGrounded);

        // Gravity
        velocity.y += gravity * Time.deltaTime;

        // Velocity
        characterController.Move(velocity * Time.deltaTime);
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Use block (if it's a block)
        var block = hit.collider.GetComponent<Block>();
        if (
            block
            && velocity.y > 0
            && Vector3.Dot(hit.normal, Vector3.down) > 0.5f
            && !isGrounded
            && jumpIdx != jumpIdxLastHit
        )
        {
            // Debounce
            jumpIdxLastHit = jumpIdx;

            // Use block
            block.OnBlockUse();
        }

        // Use goal (if it's a flag)
        var goalFlag = hit.collider.GetComponent<GoalFlag>();
        if (goalFlag)
        {
            GameManager.s_instance.timeReachedGoal = (int)Time.time;
        }
    }
}
