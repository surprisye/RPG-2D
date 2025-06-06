using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string,int> inventory;
    public List<string> equipmentId;
    
    public SerializableDictionary<string,bool> checkPoints;
    public string closestCheckPointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public GameData()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;
        
        
        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
        
        closestCheckPointId = string.Empty;
        checkPoints = new SerializableDictionary<string, bool>();
    }
}
