using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    
    [SerializeField] private string fileName;
    [SerializeField] private bool encrypData;
    
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler fileDataHandler;

    [ContextMenu("Delete save file")]
    private void DeleteSavedData()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encrypData);
        fileDataHandler.Delete();
    }
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }


    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encrypData);
        saveManagers = FindAllSaveManagers();
        
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.Load();
        
        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
        
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        
        fileDataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID).OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }
}
