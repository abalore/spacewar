using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    public delegate void DeathHandler(Ufo ufo, bool natural);
    public event DeathHandler Death;

    public Vector3 speed = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + speed;
        transform.Rotate(0, 0, 5, Space.Self);
        if (transform.position.x < -4 || transform.position.x > 4 || transform.position.y < -5 || transform.position.y > 5)
            Destroy(gameObject);
    }

    public void Initialize(float px, float py, float sx, float sy)
    {
        transform.position = new Vector3(px, py, 0);
        speed = new Vector3(sx, sy, 0);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Death?.Invoke(this, other.gameObject.layer == 10);
    }
}
