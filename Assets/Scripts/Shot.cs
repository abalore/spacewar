using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    private Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + speed;
        transform.Rotate(0, 0, 15, Space.Self);
        if (transform.position.x < -4 || transform.position.x > 4 || transform.position.y < -5 || transform.position.y > 5)
            Destroy(gameObject);
    }

    public void Initialize(Vector2 position, Vector2 speed)
    {
        transform.position = position;
        this.speed = new Vector3(speed.x, speed.y, 0);
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}
