using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{

    public float translationXSpeed = -1;
    public float translationYSpeed = -1;
    public Vector2 originPoint;

    // Start is called before the first frame update
    void Start()
    {
        originPoint = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(new Vector2(translationXSpeed * Time.deltaTime, translationYSpeed * Time.deltaTime));
        if (transform.position.y < -6)
        {
            returnOriginPoint();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        returnOriginPoint();
    }

    void returnOriginPoint()
    {
        transform.position = originPoint;
    }
}
