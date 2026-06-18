using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerHealth : MonoBehaviour
{
    [Header("Életerő")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Sebződés után")]
    public float knockbackForceUp = 8.5f;
    public float knockbackForceHorizontal = 5f;
    public float iFrameDuration = 1f;
    private bool isInvincible;

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

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthDisplay((float)currentHealth / maxHealth);
        }

        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        startPosition = transform.position;
    }

    void Update()
    {
        if (transform.position.y < -10)
        {
            Die();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            int pushDirection = transform.position.x < collision.transform.position.x ? -1 : 1;
            TakeDamage(25, pushDirection);
        }
    }

    public void TakeDamage(int damageAmount, int pushDirection = 0)
    {
        if (isInvincible) return;

        currentHealth -= damageAmount;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthDisplay((float)currentHealth / maxHealth);
        }
        AudioManager.Instance.PlaySound("Hurt");

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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Death");
        }
        if (PersistenceManager.Instance != null)
        {
            PersistenceManager.Instance.ResetCurrentRun();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied(this);
        }
    }

    public void Respawn()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Respawn");
        }
        currentHealth = maxHealth;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthDisplay(1f);
        }

        Vector2 savedPos = Checkpoint.GetSavedPositionForCurrentScene();
        if (savedPos != Vector2.zero)
        {
            transform.position = savedPos;
        }
        else
        {
            transform.position = startPosition;
        }

        string currentScene = SceneManager.GetActiveScene().name;
        int savedScore = PlayerPrefs.GetInt("CheckpointScore_" + currentScene, 0);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.score = savedScore;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateScoreDisplay(savedScore);
            }
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}