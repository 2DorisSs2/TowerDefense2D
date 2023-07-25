using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



[Serializable]
public class DataVersion
{
    public int major = 1;
    public int minor = 0;
}


[Serializable]
public class GameProgressData
{
	public System.DateTime saveTime = DateTime.MinValue;	
    public string lastCompetedLevel = "";					
	public List<string> openedLevels = new List<string>();	
}


[Serializable]
public class GameConfigurations
{
	public float soundVolume = 0.5f;
	public float musicVolume = 0.5f;
}


public class DataManager : MonoBehaviour
{

	public static DataManager instance;


    public GameProgressData progress = new GameProgressData();

	public GameConfigurations configs = new GameConfigurations();


	private DataVersion dataVersion = new DataVersion();
	
    private string dataVersionFile = "/DataVersion.dat";
	
    private string gameProgressFile = "/GameProgress.dat";
	
	private string gameConfigsFile = "/GameConfigs.dat";

	
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            UpdateDataVersion();
            LoadGameProgress();
			LoadGameConfigs();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

	
	void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	
    private void UpdateDataVersion()
    {
        if (File.Exists(Application.persistentDataPath + dataVersionFile) == true)
        {
            BinaryFormatter bfOpen = new BinaryFormatter();
            FileStream fileToOpen = File.Open(Application.persistentDataPath + dataVersionFile, FileMode.Open);
            DataVersion version = (DataVersion)bfOpen.Deserialize(fileToOpen);
            fileToOpen.Close();

            switch (version.major)
            {
                case 1:
                    break;
            }
        }
        BinaryFormatter bfCreate = new BinaryFormatter();
        FileStream fileToCreate = File.Create(Application.persistentDataPath + dataVersionFile);
        bfCreate.Serialize(fileToCreate, dataVersion);
        fileToCreate.Close();
    }

	
	public void DeleteGameProgress()
	{
		File.Delete(Application.persistentDataPath + gameProgressFile);
		progress = new GameProgressData();
		Debug.Log("Saved game progress deleted");
	}

	
    public void SaveGameProgress()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + gameProgressFile);
        progress.saveTime = DateTime.Now;
        bf.Serialize(file, progress);
        file.Close();
    }

	
    public void LoadGameProgress()
    {
        if (File.Exists(Application.persistentDataPath + gameProgressFile) == true)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + gameProgressFile, FileMode.Open);
            progress = (GameProgressData)bf.Deserialize(file);
            file.Close();
        }
    }

	
	public void SaveGameConfigs()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + gameConfigsFile);
		bf.Serialize(file, configs);
		file.Close();
	}

	
	public void LoadGameConfigs()
	{
		if (File.Exists(Application.persistentDataPath + gameConfigsFile) == true)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + gameConfigsFile, FileMode.Open);
			configs = (GameConfigurations)bf.Deserialize(file);
			file.Close();
		}
	}
}
