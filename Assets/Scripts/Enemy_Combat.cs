using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    
    public int damage = 1;
    public float damageInterval = 1f; // Time between damage ticks
    
    private float damageTimer = 0f;
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            damageTimer -= Time.deltaTime;
            
            if(damageTimer <= 0f)
            {
                collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
                damageTimer = damageInterval;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            damageTimer = 0f; // Reset timer when player leaves
        }
    }
}   
