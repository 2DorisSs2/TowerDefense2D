using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]

public class WavesInfoInspector : MonoBehaviour
{


	public List<float> timeouts
	{
		get
		{
			return wavesInfo.wavesTimeouts;
		}
		set
		{
			wavesInfo.wavesTimeouts = value;
		}
	}

	
	private WavesInfo wavesInfo;

	void OnEnable()
	{
		wavesInfo = GetComponent<WavesInfo>();
		Debug.Assert(wavesInfo, "Wrong stuff settings");
	}

	
	public void Update()
	{
		wavesInfo.Update();
	}
}
#endif
