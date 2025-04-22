using UnityEngine;

public class Chicken_movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool isPlaying = false;

    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float jumpDuration = 1f;
    private float jumpTimer = 0f;
    private float currentJumpDuration;
    public bool isJumping = false;

    private float[] railPositions = new float[] { -3f, 0f, 3f };
    private int currentRail = 1;
    private Vector3 jumpStartPos;
    private Vector3 jumpTargetPos;
    public bool isRailJump = false;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlaying)
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.LeftArrow) && currentRail > 0 && !isJumping)
            {
                currentRail--;
                StartRailJump();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && currentRail < 2 && !isJumping)
            {
                currentRail++;
                StartRailJump();
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
                StartJump(jumpDuration, false);

            if (isJumping)
                AnimateJump();
            else
                PlayRunningAnimation();
        }
    }

    void StartJump(float duration, bool railJump)
    {
        jumpStartPos = transform.position;
        currentJumpDuration = duration;
        jumpTimer = 0f;
        isJumping = true;
        isRailJump = railJump;

        if (railJump)
        {
            jumpTargetPos = new Vector3(railPositions[currentRail], jumpStartPos.y, jumpStartPos.z);
        }
        else
        {
            jumpTargetPos = jumpStartPos;
        }

        animator.SetBool("isJumping", true);
        animator.SetBool("isRunning", false);
    }

    void StartRailJump()
    {
        StartJump(jumpDuration * 0.75f, true);
    }

    void AnimateJump()
    {
        jumpTimer += Time.deltaTime;
        float t = jumpTimer / currentJumpDuration;
        float verticalOffset = 4 * jumpHeight * t * (1 - t);

        float newZ = transform.position.z;
        float newX = transform.position.x;
        if (isRailJump)
        {
            newX = Mathf.Lerp(jumpStartPos.x, jumpTargetPos.x, t);
        }

        float newY = jumpStartPos.y + verticalOffset;
        transform.position = new Vector3(newX, newY, newZ);

        if (jumpTimer >= currentJumpDuration)
        {
            isJumping = false;
            jumpTimer = 0f;

            if (isRailJump)
                transform.position = new Vector3(jumpTargetPos.x, jumpStartPos.y, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, jumpStartPos.y, transform.position.z);

            animator.SetBool("isJumping", false);
        }
    }

    void PlayRunningAnimation()
    {
        if (!isJumping)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isRunning", true);
        }
    }
}
