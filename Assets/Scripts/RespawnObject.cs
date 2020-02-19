using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    public GameObject prefab;
    public GameController gameController;
    private float inicialPositionX;
    private float inicialPositionY;
    public float respawnTime;
    public float inicialTime;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        inicialPositionX = transform.position.x;
        inicialPositionY = transform.position.y;
        InvokeRepeating("Respawn", inicialTime, respawnTime);
    }

    void Respawn()
    {
        if(!gameController.levelComplete)
        {
            Instantiate(prefab, new Vector2(inicialPositionX, inicialPositionY), Quaternion.identity);
        }
    }
}
