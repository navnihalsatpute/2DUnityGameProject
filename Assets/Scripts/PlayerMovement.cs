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


    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;
    private bool isFacingRight = true;

    private enum MovementState {idle, running, jumping, attack}

    public GameObject pauseMenuScreen;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(GetComponent<Health>().currentHealth > 0)
        {
            dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(!pauseMenuScreen.activeSelf)
                {
                    Time.timeScale = 0;
                    pauseMenuScreen.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    pauseMenuScreen.SetActive(false);
                }
            }

            if(Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            if (transform.position.y < -18f)
            {
                RestartLevel();
            }

            UpdateAnimationState();
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle;
        
        if(GetComponent<Health>().currentHealth > 0)
        {
            if (dirX > 0f)
            {
                state = MovementState.running;
                sprite.flipX = false;
                isFacingRight = true;
            }
            else if (dirX < 0f)
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
            if (Input.GetButtonDown("Fire1"))
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
}