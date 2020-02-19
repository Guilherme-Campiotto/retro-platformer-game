using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    public GameObject platformToOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            openPlatform();
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void openPlatform()
    {
        platformToOpen.SetActive(true);
    }

    public void ResetButton()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        platformToOpen.SetActive(false);
    }
}
