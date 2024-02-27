using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    [SerializeField] private float hurtJumpForce = 5f;
    public float currentHealth{ get; private set; }
    private Animator anim;
    private bool dead = false;

    [Header("iframes")]
    [SerializeField] private float iframesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    private Rigidbody2D rb2d;

    // [SerializeField] private AudioSource deathSoundEffect;
    // [SerializeField] private AudioSource hurtSoundEffect;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float _damage, string damageSourceTag)
    {
        if (!dead)
        {
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            if (currentHealth > 0)
            {   
                anim.SetTrigger("hurt");
                StartCoroutine(Invulnerability());
                // if(CompareTag("Player"))
                // {
                //     hurtSoundEffect.Play();
                // }
                if (CompareTag("Player") && damageSourceTag == "Traps")
                {
                    if (rb2d != null)
                    {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, hurtJumpForce);
                    }
                }
                if (CompareTag("Enemy") && GetComponent<MeleeEnemy>() != null)
                {
                    GetComponent<MeleeEnemy>().HandleHurt();
                }
                if (CompareTag("FlyEnemy") && GetComponent<MeleeEnemy>() != null)
                {
                    GetComponent<MeleeEnemy>().HandleHurt();
                }
            }
            else
            {
                HandleDeath();
            }
        }
    }

    private void HandleDeath()
    {
        if(!dead)
        {
            dead = true;
            // deathSoundEffect.Play();
            anim.SetTrigger("death");
            

            if(CompareTag("Player"))
            {
                spriteRend = GetComponent<SpriteRenderer>();
                Color newColor = new Color(1f, 0f, 0f, 0.8f);
                spriteRend.color = newColor;
                if(GetComponent<PlayerMovement>() != null)
                {
                    GetComponent<PlayerMovement>().enabled = false;
                }
            }
            if(CompareTag("Enemy"))
            {
                if (rb2d != null)
                    rb2d.simulated = false;
                if(GetComponentInParent<EnemyPatrol>() != null)
                    GetComponentInParent<EnemyPatrol>().enabled = false;  
                             
                if(GetComponent<MeleeEnemy>() != null)
                    GetComponent<MeleeEnemy>().enabled = false;    
            }    
            if(CompareTag("FlyEnemy"))
            {
                if (rb2d != null)
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    rb2d.gravityScale = 1.0f; 
                    rb2d.AddForce(Vector2.down * 0.5f, ForceMode2D.Impulse);
                }
                if(GetComponentInParent<EnemyPatrol>() != null)
                    GetComponentInParent<EnemyPatrol>().enabled = false;  
                                
                if(GetComponent<MeleeEnemy>() != null)
                    GetComponent<MeleeEnemy>().enabled = false;
            }
        }
    }

    public void disable()
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.simulated = false;
    }

    public void AddHealth(float _value)
    {
         currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulnerability()
    {
        if(CompareTag("Player"))
        {
            for (int i=0; i< numberOfFlashes; i++ )
            {
                spriteRend.color = new Color(1, 0,0, 0.5f);
                yield return new WaitForSeconds(iframesDuration/(numberOfFlashes * 2));
                spriteRend.color = Color.white;
                yield return new WaitForSeconds(iframesDuration/(numberOfFlashes * 2));
            }
            Physics2D.IgnoreLayerCollision(7 , 8, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((CompareTag("Player") && collision.gameObject.CompareTag("Enemy"))||(CompareTag("Player") && collision.gameObject.CompareTag("FlyEnemy")))
        {
            float playerY = transform.position.y;
            float enemyY = collision.transform.position.y;

            float jumpOverThreshold = 1f;

            if (playerY > enemyY + jumpOverThreshold)
            {
                // Player successfully jumps over the enemy, apply damage here.
                TakeDamage(0.5f, "JumpingOverEnemy");
                Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, hurtJumpForce);
                }
            }
        }
    }

    private void RestartLevel()
    {
        anim.SetTrigger("death");
        StartCoroutine(ReloadSceneAfterAnimation());
    }

    IEnumerator ReloadSceneAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length+10);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}