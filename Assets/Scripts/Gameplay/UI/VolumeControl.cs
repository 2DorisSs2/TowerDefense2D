using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
	public Slider sound;
	
	public Slider music;

	
	void Start()
	{
		Debug.Assert(sound && music, "Wrong initial settings");
		sound.value = DataManager.instance.configs.soundVolume;
		music.value = DataManager.instance.configs.musicVolume;
		sound.onValueChanged.AddListener(delegate {OnVolumeChanged();});
		music.onValueChanged.AddListener(delegate {OnVolumeChanged();});
	}

	
	private void OnVolumeChanged()
	{
		
		DataManager.instance.configs.soundVolume = sound.value;
		DataManager.instance.configs.musicVolume = music.value;
		DataManager.instance.SaveGameConfigs();
		AudioManager.instance.SetVolume(DataManager.instance.configs.soundVolume, DataManager.instance.configs.musicVolume);
	}
}
