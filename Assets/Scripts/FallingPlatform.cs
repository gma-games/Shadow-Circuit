using Unity.VisualScripting;
using UnityEngine;


public class FallingPlatform : MonoBehaviour
{
    public float timeBeforeFall;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound("PlatformFall");
            }
            gameObject.AddComponent<Rigidbody2D>();
            Destroy(transform.parent.gameObject, 3);
        }
    }
}
