using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public delegate void DeathHandler();
    public event DeathHandler Death;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Death?.Invoke();
    }

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void AddRelativeForce(Vector2 force)
    {
        rb.AddRelativeForce(force);
    }

    public void SetAngularVelocity(float velocity)
    {
        rb.angularVelocity = velocity;
    }
}
