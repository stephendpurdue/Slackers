using UnityEngine;

public class DisguiseChanger : MonoBehaviour
{
    [Header("Disguise Sprites")]
    [SerializeField] private Sprite disguise1;
    [SerializeField] private Sprite disguise2;
    [SerializeField] private Sprite disguise3;
    
    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;
    
    private GameObject player;
    private bool playerInRange = false;
    private int currentDisguiseIndex = 0;
    
    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)))
        {
            ChangeDisguise();
        }
    }

    void ChangeDisguise()
    {
        if (player == null) return;

        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
        if (playerSprite == null)
        {
            playerSprite = player.GetComponentInChildren<SpriteRenderer>();
        }
        
        if (playerSprite == null) return;

        Animator animator = player.GetComponent<Animator>();
        if (animator == null)
        {
            animator = player.GetComponentInChildren<Animator>();
        }

        currentDisguiseIndex = (currentDisguiseIndex + 1) % 3;

        switch (currentDisguiseIndex)
        {
            case 0:
                if (disguise1 != null)
                {
                    playerSprite.sprite = disguise1;
                    if (animator != null) animator.enabled = false;
                }
                break;
            case 1:
                if (disguise2 != null)
                {
                    playerSprite.sprite = disguise2;
                    if (animator != null) animator.enabled = false;
                }
                break;
            case 2:
                if (disguise3 != null)
                {
                    playerSprite.sprite = disguise3;
                    if (animator != null) animator.enabled = true;
                }
                break;
        }

        ResetAllEnemyDetection();
    }

    void ResetAllEnemyDetection()
    {
        Enemy_Movement[] allEnemies = FindObjectsOfType<Enemy_Movement>();
        foreach (Enemy_Movement enemy in allEnemies)
        {
            enemy.ForceStopChase();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            playerInRange = true;
            
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }
}