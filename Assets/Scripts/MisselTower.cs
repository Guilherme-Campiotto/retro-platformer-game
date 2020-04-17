using UnityEngine;

public class MisselTower : MonoBehaviour
{
    public GameObject prefabMissel;
    public float shootInterval = 5f;
    public float firstShootTime = 1f;
    public Transform missilePoint;
    GameObject player;

    private SoundController soundController;
    public AudioClip missileLaunchSound;

    // Start is called before the first frame update
    void Start()
    {
        soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
        player = GameObject.Find("Player");
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
            soundController.PlayAudioOnce(missileLaunchSound, 0.3f);
            Instantiate(prefabMissel, new Vector2(missilePoint.position.x, missilePoint.position.y), transform.rotation);
        }
    }
}
