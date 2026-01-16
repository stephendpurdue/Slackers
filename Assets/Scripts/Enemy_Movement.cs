using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    private int facingDirection = -1;
    private EnemyState enemyState;
    
    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(EnemyState.Idle); // Changes state to Idle to begin game.
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing on " + gameObject.name);
        }
    }

    // FixedUpdate is called at fixed intervals for physics
    void FixedUpdate()
    {
        if(enemyState == EnemyState.Chasing && player != null)
        {
            float horizontalDistance = player.position.x - transform.position.x;
         
            // Only flip if there's a significant horizontal distance (dead zone to prevent jittering)
            if(Mathf.Abs(horizontalDistance) > 0.1f)
            {
                if(horizontalDistance > 0 && facingDirection == -1) // Player is to the right, enemy facing left
                {
                    Flip();
                }
                else if(horizontalDistance < 0 && facingDirection == 1) // Player is to the left, enemy facing right
                {
                    Flip();
                }
            }
            
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
            ChangeState(EnemyState.Chasing);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {   
        if(collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            player = null;
            ChangeState(EnemyState.Idle);
        }
    }

    void ChangeState(EnemyState newState)
    {
        if (anim != null)
        {
            // Exit the current animation
            if(enemyState == EnemyState.Idle)
                anim.SetBool("isIdle", false);
            else if (enemyState == EnemyState.Chasing)
                anim.SetBool("isChasing", false);
        }

        // Update our current state
        enemyState = newState;

        if (anim != null)
        {
            // Update the new animation
            if(enemyState == EnemyState.Idle)
                anim.SetBool("isIdle", true);
            else if (enemyState == EnemyState.Chasing)
                anim.SetBool("isChasing", true);
        }
    }

    public void ForceStopChase()
{
    // Stop movement
    if (rb != null)
        rb.velocity = Vector2.zero;
    
    // Clear player reference
    player = null;
    
    // Return to idle state
    ChangeState(EnemyState.Idle);
    
    Debug.Log($"{gameObject.name} stopped chasing due to disguise change");
}
}

public enum EnemyState
{
    Idle,
    Chasing,
}
