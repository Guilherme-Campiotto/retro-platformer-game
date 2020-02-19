using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float launchForce = 400;
    Vector2 originPoint;
    Rigidbody2D rb;
    float gravityLevel;

    // Start is called before the first frame update
    void Start()
    {
        originPoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gravityLevel = Physics2D.gravity.y;
        if ((transform.position.y < originPoint.y && gravityLevel < 0) || (transform.position.y > originPoint.y && gravityLevel > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            launchFire();
        }
    }

    void launchFire()
    {
        rb.AddForce(new Vector2(0, launchForce));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO sound when it hits the player.
    }

}
