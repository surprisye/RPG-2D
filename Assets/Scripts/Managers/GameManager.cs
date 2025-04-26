using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public  static GameManager instance;
    
    private Transform player;
    
    [SerializeField] private CheckPoint[] checkPoints;
    [SerializeField] private string closestCheckpointId;

    
    [Header("Lost Currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        
        
    }

    private void Start()
    {
        checkPoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);
        
        player = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));
    }

    private void LoadCheckPoints(GameData _data)
    {
        foreach (KeyValuePair<string,bool> pair in _data.checkPoints)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.id == pair.Key && pair.Value)
                {
                    checkPoint.ActiveCheckPoint();
                }
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, 
                new Vector3(lostCurrencyX, lostCurrencyY),Quaternion.identity);
            
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }
        
        lostCurrencyAmount = 0;
    }

    IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);
        
        LoadCheckPoints(_data);
        LoadClosestCheckPoint(_data);
        LoadLostCurrency(_data);

    }
    
    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = this.lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if (closestCheckpointId != null)
            _data.closestCheckPointId = FindClosestCheckPoint().id;
        
        _data.checkPoints.Clear();
        
        foreach (CheckPoint checkPoint in  checkPoints)
        {
            _data.checkPoints.Add(checkPoint.id,checkPoint.activationStatus);
        }
    }

    private void LoadClosestCheckPoint(GameData _data)
    {
        if (_data.closestCheckPointId == null)
            return;
        
        closestCheckpointId = _data.closestCheckPointId;
        
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (closestCheckpointId == checkPoint.id)
                player.position = checkPoint.transform.position;
        }
    }
    
    private CheckPoint FindClosestCheckPoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckPoint = null;

        foreach (CheckPoint checkPoint in checkPoints)
        {
            float distanceToCheckPoint = Vector2.Distance(player.position, checkPoint.transform.position);

            if (distanceToCheckPoint < closestDistance && checkPoint.activationStatus == true)
            {
                closestDistance = distanceToCheckPoint;
                closestCheckPoint = checkPoint;
            }
        }
        return closestCheckPoint;
    }
}
