using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    int currentFrame = 1;
    public List<GameObject> listShots;
    public List<GameObject> listFrames;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChangeFrame(int time)
    {
        listShots[currentFrame].SetActive(true);
        yield return new WaitForSeconds(time);
    }
}
