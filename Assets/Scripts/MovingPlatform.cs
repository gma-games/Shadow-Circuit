using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    public bool isMoving = true;
    public float speed = 2f;
    public List<Transform> waypoints; 

    private int currentPointIndex = 0;

    void Start()
    {
        if (waypoints.Count > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        if (isMoving && waypoints.Count > 0)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        if (Vector2.Distance(transform.position, waypoints[currentPointIndex].position) < 0.01f)
        {
            currentPointIndex++;
            if (currentPointIndex >= waypoints.Count)
            {
                currentPointIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPointIndex].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}