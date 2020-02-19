using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballArc : MonoBehaviour
{
    public float launchForceX = 500;
    public float launchForceY = 100;
    public float angle = -120;
    public float distanceFire = 0f;

    Vector2 originPoint;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        originPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(launchForceX, launchForceY));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < originPoint.y)
        {
            rb.velocity = new Vector2(0, 0);
            launchForceX = launchForceX * -1;
            angle = angle * -1;
            //transform.position = originPoint;
            launchFire();
        }
    }

    void launchFire()
    {
        transform.Rotate(0, 0, angle);
        rb.AddForce(new Vector2(launchForceX, launchForceY));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO sound when it hits the player.
    }
}
