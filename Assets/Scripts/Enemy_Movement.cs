using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    private bool isChasing;
    private int facingDirection = -1;
    
    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;
    private EnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(EnemyState.Idle); // Changes state to Idle to begin game.
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing on " + gameObject.name);
        }
    }

    // FixedUpdate is called at fixed intervals for physics
    void FixedUpdate()
    {
        if(isChasing && player != null)
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
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {   
        if(collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            isChasing = false;
            player = null;
        }
    }

    void ChangeState(EnemyState newState)
    {
        if(enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
}
