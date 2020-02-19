using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private CharacterController2D controller;
    public float trampolineForce = 500f;

    private SoundController soundController;
    public AudioClip trampolineSound;

    private void Start()
    {
        soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
        controller = GameObject.Find("Player").GetComponent<CharacterController2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            controller.canDoubleJump = true;

            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * trampolineForce);
            soundController.PlayAudioOnce(trampolineSound);
        }

    }
}
