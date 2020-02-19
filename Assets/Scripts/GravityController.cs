using UnityEngine;

public class GravityController : MonoBehaviour
{
    private bool canInvert = true;
    public bool gravityInverted;
    public AudioClip gravitySound;
    public CharacterController2D player;
    private SoundController soundController;

    public void Start()
    {
        soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
        setGravityDefault();
    }

    public void InvertGravity()
    {

        float gravityLevel = Physics2D.gravity.y;

        if (gravityInverted && Physics2D.gravity.y < 0)
        {
            soundController.PlayAudioOnce(gravitySound);
            gravityLevel = Mathf.Abs(gravityLevel);
            player.InvertGravityControls();
        }
        else if(!gravityInverted && Physics2D.gravity.y > 0)
        {
            soundController.PlayAudioOnce(gravitySound);
            gravityLevel = -Mathf.Abs(gravityLevel);
            player.InvertGravityControls();
        }

        Physics2D.gravity = new Vector2(Physics2D.gravity.x, gravityLevel);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && canInvert)
        {
            canInvert = false;
            InvertGravity();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInvert = true;
            InvertGravity();
        }
    }

    public void setGravityDefault()
    {
        float gravityLevel = Physics2D.gravity.y;
        if (gravityLevel > 0)
        {
            // Reset gravity to normal if it is the start of the level.
            gravityLevel = -Mathf.Abs(gravityLevel);
            Physics2D.gravity = new Vector2(Physics2D.gravity.x, gravityLevel);
            if(player.invertedGravity)
            {
                player.InvertGravityControls();
            }
        }
    }

}
