using UnityEngine;

public class SwitchDoor : MonoBehaviour
{
    public bool isOpen = false;

    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();

        if (isOpen) Open();
        else Close();
    }

    public void Open()
    {
        isOpen = true;
        spriteRenderer.enabled = false; 
        doorCollider.enabled = false;   
    }

    public void Close()
    {
        isOpen = false;
        spriteRenderer.enabled = true;  
        doorCollider.enabled = true;    
    }

    public void Toggle()
    {
        if (isOpen) Close();
        else Open();
    }
}