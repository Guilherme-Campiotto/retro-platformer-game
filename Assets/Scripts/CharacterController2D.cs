using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    public GameController gameController;
    private SoundController soundController;
    public Animator animator;
    public Animator animatorFadeLevel;

    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_WhatIsWall;                            // A mask determining what is wall to the character
    [SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
    [SerializeField] private Transform m_WallCheck;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    public float wallCheckDistance;

    public RaycastHit2D wallCheckHit;
    public RaycastHit2D wallLeftCheckHit;
    public Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

    public AudioClip deathSound;
    public AudioClip jumpSound;
    public AudioClip doubleJumpSound;
    public AudioClip collectedItem;

    public bool invertedGravity = false;

    [Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

    public bool doubleJump;
    public bool canDoubleJump = true;

    public bool dead = false;

    public int deathCount;
    public int deathCountStage;
    public int jumpCount;
    public float stageTime;

    private GameObject steamAchievements;
    SteamAchievements scriptAchievments;

    private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
        {
			OnLandEvent = new UnityEvent();
        }

	}

    private void Start()
    {
        soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        GameObject levelLoader = GameObject.Find("LevelLoader");
        animatorFadeLevel = levelLoader.gameObject.transform.Find("Crossfade").gameObject.GetComponent<Animator>();
        LoadStatistics();
        steamAchievements = GameObject.Find("SteamAchievements");

        if (steamAchievements != null)
        {
            scriptAchievments = steamAchievements.GetComponent<SteamAchievements>();
        }
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

        wallCheckHit = Physics2D.Raycast(m_WallCheck.position, m_WallCheck.right, wallCheckDistance, m_WhatIsWall);

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
                doubleJump = false;
                if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

        // Remove when game is launched.
        DebugControls();

    }


	public void Move(float move, bool jump, bool wallJump, bool wallSliding)
	{

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
		{

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}

        }
		// If the player should jump...
		if (m_Grounded && jump && !wallJump)
		{
            // Add a vertical force to the player.
            soundController.PlayAudioOnce(jumpSound);
            m_Grounded = false;
			m_Rigidbody2D.AddForce(Vector2.up * m_JumpForce);
            canDoubleJump = true;

            jumpCount++;
            SaveJumpStatistics();
		}

        if(!m_Grounded && doubleJump)
        {
            soundController.PlayAudioOnce(doubleJumpSound);
            // zerar a velocity em y garante que tamanho do pulo seja sempre o mesmo, se o personagem estiver caindo ou não
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(Vector2.up * m_JumpForce);
            doubleJump = false;
            canDoubleJump = false;

            jumpCount++;
            SaveJumpStatistics();
        }

        if(wallJump)
        {
            if(!m_FacingRight)
            {
                m_Rigidbody2D.AddForce(new Vector2(m_JumpForce * 2, m_JumpForce));
            } else
            {
                m_Rigidbody2D.AddForce(new Vector2(m_JumpForce * -2, m_JumpForce));
            }

            jumpCount++;
            SaveJumpStatistics();

        }

    }

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, 180f, 0);
	}

    public void enableDoubleJump()
    {
        if(canDoubleJump)
        {
            doubleJump = true;
            animator.SetBool("isDoubleJumping", true);
            animator.SetBool("isJumping", false);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("trampoline"))
        {
            animator.SetBool("isJumping", true);
        }

        CheckHazard(collision.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish") && !dead)
        {
            animator.speed = 0;
            soundController.PlayAudioOnce(collectedItem);
            collision.gameObject.SetActive(false);
            GetComponent<Rigidbody2D>().simulated = false;
            dead = true;
            StartCoroutine(GoToNextLevel());
        }

        CheckHazard(collision.gameObject);

    }

    public void CheckHazard(GameObject player)
    {
        if (player.CompareTag("Hazard") && !dead)
        {
            dead = true;
            StartCoroutine(MakePlayerDie());

            deathCount++;
            deathCountStage++;
            SaveDeathStatistics();
        }
    }

    IEnumerator MakePlayerDie()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        soundController.PlayAudioOnce(deathSound);
        ResetVelocity();

        animator.SetBool("isDoubleJumping", false);
        animator.SetBool("isSlinding", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("IsDead", true);

        yield return new WaitForSeconds(0.8f);
        GetComponent<SpriteRenderer>().enabled = false;

        StartCoroutine(gameController.RestartGame());

    }

    IEnumerator GoToNextLevel()
    {
        animatorFadeLevel.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        gameController.nextLevel();
    }

    public void InvertGravityControls()
    {
        m_Rigidbody2D.velocity = new Vector2(0, 0); // reset velocitys
        invertedGravity = !invertedGravity;
        transform.Rotate(180f, 0, 0);
        m_JumpForce *= -1;
    }

    /**
     * Reset all the movements the player was doing when he died.
     */
    public void ResetVelocity()
    {
        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
    }

    /**
    * Controls used only during development
    */
    public void DebugControls()
    {
        if (Input.GetButtonDown("NextLevel")) {
            gameController.nextLevel();
        }
    }

    /**
     * Keep count of jumps to unluck achievements
     */
    private void SaveJumpStatistics()
    {
        PlayerPrefs.SetInt("jumpCount", jumpCount);

        // Conquista pular 100 vezes
        if(jumpCount >= 100 && scriptAchievments != null)
        {
            scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_9");
        }
    }

    /**
     * Keep count of death to unluck achievements
     */
    private void SaveDeathStatistics()
    {
        PlayerPrefs.SetInt("deathCount", deathCount);
        Debug.Log("Deaths total: " + deathCount);

        // Conquista morrer 20 vezes
        if(deathCount >= 20 && scriptAchievments != null)
        {
            scriptAchievments.UnlockSteamAchievement("NEW_ACHIEVEMENT_1_7");
        }
    }

    /**
     * Load informations to keep track of achievements progress.
     */
    private void LoadStatistics()
    {
        deathCount = PlayerPrefs.GetInt("deathCount");
        jumpCount = PlayerPrefs.GetInt("jumpCount");

        Debug.Log("Jumps total: " + jumpCount);
    }

    private void UpdateTimer()
    {
        stageTime += Time.deltaTime;
    }
}
