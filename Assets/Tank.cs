using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public GameObject prefabMissel;
    public float shootInterval = 2f;
    public float firstShootTime = 2f;
    public Transform missilePoint;

    public AudioClip missileLaunchSound;
    private SoundController soundController;

    void Start()
    {
        soundController = GameObject.Find("SoundController").GetComponent<SoundController>();

        InvokeRepeating("ShootMissile", firstShootTime, shootInterval);
    }

    void ShootMissile()
    {
        if (soundController != null && missileLaunchSound != null) {
            soundController.PlayAudioOnce(missileLaunchSound);
        } else
        {
            Debug.Log("Sound controller or audioclip not found");
        }

        Instantiate(prefabMissel, new Vector2(missilePoint.position.x, missilePoint.position.y), transform.rotation);

    }
}
