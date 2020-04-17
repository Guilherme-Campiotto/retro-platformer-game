using System.Collections;
using UnityEngine;

public class Missel : MonoBehaviour
{
    public float speed = 15f;
    public Vector2 direction;
    private SoundController soundController;
    public AudioClip missileExplosionSound;
    public bool exploded = false;

    public Animator animator;

    private void Start()
    {
        soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!exploded)
        {
            gameObject.transform.position += gameObject.transform.right * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.Play("MissileExplosion");

        //Destroy(gameObject);
        exploded = true;
        //this.GetComponent<Rigidbody2D>().simulated = false;
        StartCoroutine(DestroyObjectWithDelay(gameObject));
        soundController.PlayAudioOnce(missileExplosionSound, 0.3f);
    }

    IEnumerator DestroyObjectWithDelay(GameObject gameObject)
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

}