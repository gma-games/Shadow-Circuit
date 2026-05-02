using UnityEngine;

public class Keycard : MonoBehaviour
{
    public GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(door);
            Destroy(gameObject);
        }
    }
}
