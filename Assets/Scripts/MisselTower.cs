using System.Collections;
using UnityEngine;

public class MisselTower : MonoBehaviour
{
    public GameObject prefabMissel;
    public float shootInterval = 5f;
    public float firstShootTime = 1f;
    public Transform missilePoint;
    public Animator animator;
    GameObject player;

    private SoundController soundController;
    public AudioClip missileLaunchSound;

    // Start is called before the first frame update
    void Start()
    {
        soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
        player = GameObject.Find("Player");
        StartCoroutine("ShootIndicator", firstShootTime - 0.6f);
        InvokeRepeating("Shoot", firstShootTime, shootInterval);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    /**
     * Shoot at the player's direction, if he is alive.
     */
    void Shoot()
    {
        if (!player.GetComponent<CharacterController2D>().dead)
        {
            StartCoroutine("ShootIndicator", shootInterval- 0.6f);
            soundController.PlayAudioOnce(missileLaunchSound, 0.3f);
            Instantiate(prefabMissel, new Vector2(missilePoint.position.x, missilePoint.position.y), transform.rotation);
        }
    }

    IEnumerator ShootIndicator(float time)
    {
        yield return new WaitForSeconds(time);
        if (!player.GetComponent<CharacterController2D>().dead && animator != null)
        {
            animator.Play("MisselPipe");
        }
    }

}
