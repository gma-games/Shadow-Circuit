using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Életerő")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Sebződés után")]
    public float knockbackForceUp = 8.5f; // Sebződés után felfelé ugrik
    public float knockbackForceHorizontal = 5f; //Oldal irányú ellökés
    public float iFrameDuration = 1f;   // Sebezhetetlenség ideje
    private bool isInvincible;

    // Komponens referenciák
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 startPosition;

    private int playerLayer;
    private int enemyLayer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        // Induláskor max életerő
        UIManager.Instance.UpdateHealthDisplay((float)currentHealth / maxHealth);

        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        startPosition = transform.position;
    }

    void Update()
    {
        // Pályáról kizuhanás
        if (transform.position.y < -10)
        {
            Die();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            // Kiszámoljuk, hogy a Damage taggel rendelkező objemktum tőlünk jobbra vagy balra va
            int pushDirection = transform.position.x < collision.transform.position.x ? -1 : 1;
            
            TakeDamage(25, pushDirection);
        }
    }

    public void TakeDamage(int damageAmount, int pushDirection = 0)
    {
        if (isInvincible) return; // Ha éppen az adott státuszban van akkor nem sebződik 

        currentHealth -= damageAmount;
        UIManager.Instance.UpdateHealthDisplay((float)currentHealth / maxHealth);
        AudioManager.Instance.PlaySound("Hurt");

        // Visszalökődés 
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(pushDirection * knockbackForceHorizontal, knockbackForceUp);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BlinkRed());
            StartCoroutine(InvincibilityFrames());
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = new Color(Color.red.r, Color.red.g, Color.red.b, spriteRenderer.color.a);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(Color.white.r, Color.white.g, Color.white.b, spriteRenderer.color.a);
    }

    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        float elapsed = 0f;
        while (elapsed < iFrameDuration)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.3f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

        isInvincible = false;
    }

    private void Die()
    {
        // a game managerrel pálya újratöltése
        GameManager.Instance.PlayerDied(this);
    }

    public void Respawn()
    {
        // Életerő visszaállítása
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealthDisplay(1f);

        // Legutolsó elmentett helyre teleportálsá
        if (Checkpoint.savedPoisiton != Vector2.zero)
        {
            transform.position = Checkpoint.savedPoisiton;
        }
        else
        {
            transform.position = startPosition;
        }

        //A checkpointnál elmenmtett pontszáom töltődnek vissza

        int savedScore = PlayerPrefs.GetInt("CheckpointScore", 0);
        GameManager.Instance.score = savedScore;
        UIManager.Instance.UpdateScoreDisplay(savedScore);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}