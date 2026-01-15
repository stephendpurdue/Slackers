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
        if(enemyState == EnemyState.Chasing)
        {
            if(player.position.x > transform.position.x && facingDirection == 1 || // Flips the Enemy left or right based on player vicinity.
                player.position.x < transform.position.x && facingDirection == -1 )
            {
                Flip();
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
            isChasing = true;
        }
        ChangeState(EnemyState.Chasing);
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
        // Exit the current animation
        if(enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);

        // Update our current state
        enemyState = newState;

        // Update the new animation
        if(enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
}
