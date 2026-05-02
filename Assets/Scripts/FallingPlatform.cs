using Unity.VisualScripting;
using UnityEngine;


public class FallingPlatform : MonoBehaviour
{
    public float timeBeforeFall;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.AddComponent<Rigidbody2D>();
            Destroy(transform.parent.gameObject, 3);
        }
    }
}
