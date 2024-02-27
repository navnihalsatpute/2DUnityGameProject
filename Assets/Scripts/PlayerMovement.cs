using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    [SerializeField] private LayerMask jumpableGround;

    // [SerializeField] private AudioSource attackSoundEffect;
    [SerializeField] private AudioSource jumpSoundEffect;

    // private float dirX = 0f;
    [SerializeField] private float speed = 7f;
    private float horizontalMove;
    private bool moveRight;
    private bool moveLeft;
    private bool isjumping;
    private bool isattack;

    [SerializeField] private float jumpForce = 14f;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    private bool isFacingRight = true;

    private enum MovementState {idle, running, jumping, attack, falling}

    public GameObject pauseMenuScreen;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        moveLeft = false;
        moveRight = false;
        isjumping = false;
        isattack = false;
    }

    private void Update()
    {
        Movement();
    }

    // private void Update()
    // {
    //     if(GetComponent<Health>().currentHealth > 0)
    //     {
    //         dirX = Input.GetAxisRaw("Horizontal");
    //         rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

    //         if (Input.GetKeyDown(KeyCode.Escape))
    //         {
    //             if(!pauseMenuScreen.activeSelf)
    //             {
    //                 Time.timeScale = 0;
    //                 pauseMenuScreen.SetActive(true);
    //             }
    //             else
    //             {
    //                 Time.timeScale = 1;
    //                 pauseMenuScreen.SetActive(false);
    //             }
    //         }

    //         if(Input.GetButtonDown("Jump") && IsGrounded())
    //         {
    //             rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    //         }

    //         if (transform.position.y < -18f)
    //         {
    //             RestartLevel();
    //         }

    //         UpdateAnimationState();
    //     }
    // }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle;
        
        if(GetComponent<Health>().currentHealth > 0)
        {
            if (moveRight)
            {
                state = MovementState.running;
                sprite.flipX = false;
                isFacingRight = true;
            }
            else if (moveLeft)
            {
                state = MovementState.running;
                sprite.flipX = true;
                isFacingRight = false;
            }
            else
            {
                state = MovementState.idle;
            }
            if (rb.velocity.y > 0.1f)
            {
                state = MovementState.jumping;
            }
            if (rb.velocity.y < -0.1f)
            {
                state = MovementState.falling;
                if(IsGrounded()){
                state = MovementState.idle;
            }
            }
            if (isattack == true && moveLeft == false && moveRight == false)
            {
                state = MovementState.attack;
            }
            
            anim.SetInteger("state", (int)state);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void AttackAnimationEvent()
    {
        Vector2 attackDirection = isFacingRight ? Vector2.right : Vector2.left;
        // This function is triggered by the animation event.
        // Access the enemy's health component and applying damage.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll((Vector2)transform.position + attackDirection * attackRange, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.tag == "Enemy")
                enemy.GetComponent<Health>().TakeDamage(1, gameObject.tag);
        }
        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.tag == "FlyEnemy")
                enemy.GetComponent<Health>().TakeDamage(1, gameObject.tag);
        }
    }

    public void pointerDownLeft()
    {
        if(GetComponent<Health>().currentHealth > 0)
        {
            moveLeft = true;
        }
    }
    public void pointerUpLeft()
    {
        if(GetComponent<Health>().currentHealth > 0)
        {
            moveLeft = false;
        }
    }
    public void pointerDownRight()
    {
        if(GetComponent<Health>().currentHealth > 0)
        {
            moveRight = true;
        }
    }
    public void pointerUpRight()
    {
        if(GetComponent<Health>().currentHealth > 0)
        {
            moveRight = false;
        }
    }
    public void pointerDownJump()
    {
        if(GetComponent<Health>().currentHealth > 0)
        {
            isjumping = true;
        }
    }
    public void pointerUpJump()
    {
        if(GetComponent<Health>().currentHealth > 0)
        {
            isjumping = false;
        }
    }
    public void pointerDownAttack()
    {
        if(GetComponent<Health>().currentHealth > 0)
        {
            isattack = true;
        }
    }
    public void pointerUpAttack()
    {
        if(GetComponent<Health>().currentHealth > 0)
        {
            isattack = false;
        }
    }

    private void Movement()
    {
        if(moveLeft)
        {
            horizontalMove = -speed;
        }
        else if(moveRight)
        {
            horizontalMove = speed;
        }
        else
        {
            horizontalMove = 0;
        }

        if(isjumping && IsGrounded())
        {
            // jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if(isattack && !moveLeft && !moveRight)
        {
            // attackSoundEffect.Play();
            anim.SetInteger("state", 3);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalMove, rb.velocity.y);

        if (transform.position.y < -18f)
        {
            RestartLevel();
        }

        UpdateAnimationState();
    }
}