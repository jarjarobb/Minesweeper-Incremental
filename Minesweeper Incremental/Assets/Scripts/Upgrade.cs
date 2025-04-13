using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    
    [SerializeField] string description;
    [SerializeField] string currencyNeeded;
    [SerializeField] int level;
    [SerializeField] int basePrice;
    [SerializeField] float scaling;
    [SerializeField] float effect;
    [SerializeField] string currencyAffected;
    [SerializeField] bool isMulti;
    [SerializeField] int levelCap;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string GetDescription() { return description; }
    public int GetLevel() { return level; }
    public int GetPrice() { return basePrice; }
    public float GetScaling() { return scaling; }
    public float GetEffect() { return effect; }
    public string GetCurrencyAffected() { return currencyAffected; }
    public bool GetIsMulti() { return isMulti; }
    public int GetLevelCap() { return levelCap; }
    public string GetCurrencyNeeded() { return currencyNeeded; }

    public void LevelUp()
    {
        level++;
    }

}
