using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovable : MonoBehaviour
{

    public SliderJoint2D slider;
    public JointMotor2D motor;

    public float xLimitRight = 4f;
    public float xLimitLeft = -4f;
    public bool moveRight = false;
    public float speed = 3;

    void Start()
    {
        xLimitRight = transform.position.x + xLimitRight;
        xLimitLeft = transform.position.x + xLimitLeft;
    }

    void Update()
    {
        if(transform.position.x > xLimitRight)
        {
            moveRight = false;
        }

        if (transform.position.x < xLimitLeft)
        {
            moveRight = true;
        }

        if(moveRight)
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        } else
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }
    }


    /**
     * Make the player move with the platform
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    /**
     * Remove the parent when the player exits the platform
     */
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
