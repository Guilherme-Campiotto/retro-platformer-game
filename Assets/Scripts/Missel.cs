using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missel : MonoBehaviour
{
    public float speed = 15f;
    public Vector2 direction;
    private SoundController soundController;
    public AudioClip missileExplosionSound;

    private void Start()
    {
        soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += gameObject.transform.right * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        soundController.PlayAudioOnce(missileExplosionSound);
    }
}