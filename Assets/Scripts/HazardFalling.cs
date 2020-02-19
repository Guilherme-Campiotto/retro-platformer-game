using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float limitScreenBottomY;

    void Update()
    {
        if(GetComponent<Transform>().position.y < limitScreenBottomY)
        {
            Destroy(gameObject);
        }
    }
}
