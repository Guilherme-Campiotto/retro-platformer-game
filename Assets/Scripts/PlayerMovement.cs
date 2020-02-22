using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

    public float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;
    bool isInAir = false;
    private bool wallSliding = false;
    private bool wallJump = false;
    public float maxWallSlidingVelocity = 0.5f;
    private MenuOptions menuOptions;
    public bool isGamePaused = false;

    public float rigidBodyValocityY;

    private void Start()
    {
        menuOptions = GameObject.Find("Canvas").GetComponent<MenuOptions>();
        isGamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!controller.dead && !isGamePaused)
        {
            CheckMovimentation();
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            CheckJump();
            CheckWallSlide();
        }

        if(Input.GetButtonDown("Cancel"))
        {
            isGamePaused = true;
            menuOptions.ChangeGameState();
        }

        //teste

        rigidBodyValocityY = GetComponent<Rigidbody2D>().velocity.y;
    }

    // Update a fixed amount amount of time per second
    // Fixed delta time makes sure the player has the same speed no matter the fps on the platform
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, wallJump, wallSliding);

        jump = false;
        wallJump = false;
    }

    /**
     * O personagem só desliza se estiver encontando na parede e o direcional pressionado no sentido que ele esta.
     */
    private void CheckWallSlide()
    {
        if (controller.wallCheckHit && controller.m_Rigidbody2D.velocity.y <= 0 && !controller.m_Grounded && horizontalMove != 0f)
        {
            controller.canDoubleJump = true;
            wallSliding = true;
            animator.SetBool("isSlinding", wallSliding);
            animator.SetBool("isDoubleJumping", false);
            animator.SetBool("isJumping", false);
        } else
        {
            wallSliding = false;
            animator.SetBool("isSlinding", wallSliding);
        }

        if (wallSliding)
        {
            if (controller.m_Rigidbody2D.velocity.y < -maxWallSlidingVelocity)
            {
                controller.m_Rigidbody2D.velocity = new Vector2(controller.m_Rigidbody2D.velocity.x, -maxWallSlidingVelocity);
            }

            if (controller.m_Rigidbody2D.velocity.y < 0)
            {
                // wall jump
                if (Input.GetButtonDown("Jump"))
                {
                    jump = false;
                    controller.doubleJump = false;
                    wallJump = true;
                }
            }
        }
    }

    /**
     * Pulo e pulo duplo só podem ser executados uma vez cada enquanto o personagem esta no ar
     */
    private void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && jump == false && !wallSliding)
        {
            jump = true;
            isInAir = true;
            animator.SetBool("isJumping", true);
        }
        if (Input.GetButtonDown("Jump") && !controller.m_Grounded)
        {
            controller.enableDoubleJump();
        }
    }

    private void CheckMovimentation()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if(isInAir && ((Physics2D.gravity.y < 0f && GetComponent<Rigidbody2D>().velocity.y < 0f) || (Physics2D.gravity.y > 0f && GetComponent<Rigidbody2D>().velocity.y > 0.2f)))
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", false);
        }

        if(GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            animator.SetBool("isFalling", false);
        } else
        {
            animator.SetBool("isFalling", true);
        }

    }

    public void onLanding()
    {
        // Caso a aceleração seja 0
        if((Physics2D.gravity.y < 0 && GetComponent<Rigidbody2D>().velocity.y < 0f) || (Physics2D.gravity.y > 0 && GetComponent<Rigidbody2D>().velocity.y > 0f))
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", false);
            isInAir = false;
        }
    }
}
