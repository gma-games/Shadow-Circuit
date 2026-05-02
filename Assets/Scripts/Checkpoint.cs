using System.Security;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector2 savedPoisiton = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            savedPoisiton = collision.transform.position;
        }
    }
}
