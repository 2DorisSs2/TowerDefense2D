using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class WavesTimer : MonoBehaviour
{
	
	public Image timeBar;
  
    public Text currentWaveText;
	
    public Text maxWaveNumberText;
	
	public GameObject highlightedFX;
	
	public float highlightedTO = 0.2f;

	
	private WavesInfo wavesInfo;
   
	private List<float> waves = new List<float>();
  
    private int currentWave;

    private float currentTimeout;

    private float counter;

    private bool finished;


	void OnDisable()
	{
		StopAllCoroutines ();
	}

	
    void Awake()
    {
		wavesInfo = FindObjectOfType<WavesInfo>();
		Debug.Assert(timeBar && highlightedFX && wavesInfo && timeBar && currentWaveText && maxWaveNumberText, "Wrong initial settings");
    }


	void Start()
    {
		highlightedFX.SetActive(false);
		waves = wavesInfo.wavesTimeouts;
        currentWave = 0;
        counter = 0f;
        finished = false;
        GetCurrentWaveCounter();
        maxWaveNumberText.text = waves.Count.ToString();
        currentWaveText.text = "0";
	}
	

	void FixedUpdate()
    {
        if (finished == false)
        {
			
            if (counter <= 0f)
            {
				
				EventManager.TriggerEvent("WaveStart", null, currentWave.ToString());
                currentWave++;
                currentWaveText.text = currentWave.ToString();
				
				StartCoroutine("HighlightTimer");
			
                if (GetCurrentWaveCounter() == false)
                {
                    finished = true;
				
					EventManager.TriggerEvent("TimerEnd", null, null);
                    return;
                }
            }
			counter -= Time.fixedDeltaTime;
            if (currentTimeout > 0f)
            {
                timeBar.fillAmount = counter / currentTimeout;
            }
            else
            {
                timeBar.fillAmount = 0f;
            }
        }
	}

	
    private bool GetCurrentWaveCounter()
    {
        bool res = false;
        if (waves.Count > currentWave)
        {
            counter = currentTimeout = waves[currentWave];
            res = true;
        }
        return res;
    }

	
	private IEnumerator HighlightTimer()
	{
		highlightedFX.SetActive(true);
		yield return new WaitForSeconds(highlightedTO);
		highlightedFX.SetActive(false);
	}

	
	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
