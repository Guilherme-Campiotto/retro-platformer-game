using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Candle : MonoBehaviour
{
	public float magnitude = .5f;
	public float frequency = 2f;

    Light2D myLight;

	float startIntensity;

    // Start is called before the first frame update
    void Start()
    {
		myLight = GetComponent<Light2D>();
		startIntensity = myLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
		myLight.intensity = startIntensity * (1f + Mathf.PerlinNoise(Time.time * frequency, 0f) * magnitude);
    }
}
