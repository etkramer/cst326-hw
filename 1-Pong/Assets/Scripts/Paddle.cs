using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    public string moveActionName;
    public float moveSpeed;

    InputAction moveAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction(moveActionName);
    }

    void Update()
    {
        // Compute movement
        var moveValue = moveAction.ReadValue<Vector2>().y;
        var moveDelta = moveValue * moveSpeed * Time.deltaTime;

        // Compute bounds
        var minPosY = -GameManager.s_instance.playArea.y + transform.localScale.y / 2;
        var maxPosY = GameManager.s_instance.playArea.y - transform.localScale.y / 2;

        // Add movement to pos
        var pos = transform.position;
        pos.y = Mathf.Clamp(pos.y + (float)moveDelta, minPosY, maxPosY);
        transform.position = pos;
    }

    void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        var speed = rb.linearVelocity.magnitude;

        // Compute ball's relative pos
        var selfMinY = transform.position.y - transform.localScale.y / 2;
        var ballY = other.transform.position.y;
        var ballYRelative = ballY - selfMinY;

        // Compute percent distance along paddle (0 to 1)
        var hitYPercent = ballYRelative / transform.localScale.y;

        // Compute new direction
        var newDir = new Vector3(-Mathf.Sign(rb.linearVelocity.x), (hitYPercent * 2) - 1, 0);
        newDir.x *= GameManager.s_instance.playArea.x;
        newDir.y *= GameManager.s_instance.playArea.y;
        newDir = newDir.normalized;

        // Compute final velocity with speed increment
        rb.linearVelocity = newDir * (speed + GameManager.s_instance.ballSpeedIncrement);
    }
}
