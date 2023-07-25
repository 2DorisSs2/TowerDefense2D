using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;


	public AudioSource soundSource;

	public AudioSource musicSource;

	public AudioClip track;

	public AudioClip waveStart;

	public AudioClip captured;

	public AudioClip towerClick;

	public AudioClip unitClick;

	public AudioClip uiClick;

	public AudioClip towerBuild;

	public AudioClip towerSell;

	public AudioClip defeat;

	public AudioClip victory;

	private bool attackCoroutine = false;

	private bool dieCoroutine = false;

	void OnEnable()
	{
		instance = this;
		EventManager.StartListening("GamePaused", GamePaused);
		EventManager.StartListening("WaveStart", WaveStart);
		EventManager.StartListening("Captured", Captured);
		EventManager.StartListening("UserClick", UserClick);
		EventManager.StartListening("UserUiClick", UserUiClick);
		EventManager.StartListening("TowerBuild", TowerBuild);
		EventManager.StartListening("TowerSell", TowerSell);
		EventManager.StartListening("Defeat", Defeat);
		EventManager.StartListening("Victory", Victory);
	}

	void OnDisable()
	{
		EventManager.StopListening("GamePaused", GamePaused);
		EventManager.StopListening("WaveStart", WaveStart);
		EventManager.StopListening("Captured", Captured);
		EventManager.StopListening("UserClick", UserClick);
		EventManager.StopListening("UserUiClick", UserUiClick);
		EventManager.StopListening("TowerBuild", TowerBuild);
		EventManager.StopListening("TowerSell", TowerSell);
		EventManager.StopListening("Defeat", Defeat);
		EventManager.StopListening("Victory", Victory);
	}


	void Start()
	{
		Debug.Assert(soundSource && musicSource, "Wrong initial settings");
		
		SetVolume(DataManager.instance.configs.soundVolume, DataManager.instance.configs.musicVolume);
	}


	void OnDestroy()
	{
		StopAllCoroutines();
		if (instance == this)
		{
			instance = null;
		}
	}


	private void GamePaused(GameObject obj, string param)
	{
		if (param == bool.TrueString) 
		{
	
			musicSource.Pause();
		}
		else 
		{
		
			if (track != null)
			{
				musicSource.clip = track;
				musicSource.Play();
			}
		}
	}


	public void SetVolume(float sound, float music)
	{
		soundSource.volume = sound;
		musicSource.volume = music;
	}



	public void PlaySound(AudioClip audioClip)
	{
		soundSource.PlayOneShot(audioClip, soundSource.volume);
	}

	public void PlayAttack(AudioClip audioClip)
	{
		if (attackCoroutine == false)
		{
			StartCoroutine(AttackCoroutine(audioClip));
		}
	}


	private IEnumerator AttackCoroutine(AudioClip audioClip)
	{
		attackCoroutine = true;
		PlaySound(audioClip);

		yield return new WaitForSeconds(audioClip.length);
		attackCoroutine = false;
	}


	public void PlayDie(AudioClip audioClip)
	{
		if (dieCoroutine == false)
		{
			StartCoroutine(DieCoroutine(audioClip));
		}
	}

	private IEnumerator DieCoroutine(AudioClip audioClip)
	{
		dieCoroutine = true;
		PlaySound(audioClip);

		yield return new WaitForSeconds(audioClip.length);
		dieCoroutine = false;
	}


	private void WaveStart(GameObject obj, string param)
	{
		if (waveStart != null)
		{
			PlaySound(waveStart);
		}
	}


	private void Captured(GameObject obj, string param)
	{
		if (captured != null)
		{
			PlaySound(captured);
		}
	}


	private void UserUiClick(GameObject obj, string param)
	{
		if (obj != null)
		{
			PlaySound(uiClick);
		}
	}


	private void UserClick(GameObject obj, string param)
	{
		if (obj != null)
		{
			Tower tower = obj.GetComponent<Tower>();
			if (tower != null)
			{
				PlaySound(towerClick);
			}
			else
			{
				UnitInfo unitInfo = obj.GetComponent<UnitInfo>();
				if (unitInfo != null)
				{
					PlaySound(unitClick);
				}
			}
		}
	}


	private void TowerBuild(GameObject obj, string param)
	{
		if (towerBuild != null)
		{
			PlaySound(towerBuild);
		}
	}


	private void TowerSell(GameObject obj, string param)
	{
		if (towerSell != null)
		{
			PlaySound(towerSell);
		}
	}

	private void Defeat(GameObject obj, string param)
	{
		if (defeat != null)
		{
			PlaySound(defeat);
		}
	}

	private void Victory(GameObject obj, string param)
	{
		if (victory != null)
		{
			PlaySound(victory);
		}
	}
}
