using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    public GameObject prefab;
    public GameObject indicator1;
    public GameObject indicator3;
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

        RespawnIndicators(inicialTime);
        InvokeRepeating("Respawn", inicialTime, respawnTime);
    }

    void Respawn()
    {
        if(!gameController.levelComplete)
        {
            Instantiate(prefab, new Vector2(inicialPositionX, inicialPositionY), Quaternion.identity);
            RespawnIndicators(respawnTime);
        }
    }

    void RespawnIndicators(float time)
    {
        if(indicator1 != null && indicator3 != null)
        {
            StartCoroutine(RespawnIndicator(time * 0.7f, indicator1, 0.1f));
            StartCoroutine(RespawnIndicator(time * 0.8f, indicator3, - 0.1f));
            //StartCoroutine(RespawnIndicator(time * 0.8f, indicator2, 0.1f));
        }
    }

    IEnumerator RespawnIndicator(float time, GameObject indicator, float moveVariation)
    {
        yield return new WaitForSeconds(time);
        Instantiate(indicator, new Vector2(inicialPositionX + moveVariation, inicialPositionY), Quaternion.identity);
    }
}
