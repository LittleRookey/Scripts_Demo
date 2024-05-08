using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Litkey.Interface;

public class LevelSystem : MonoBehaviour, ILoadable, ISavable
{
    [SerializeField] public UnitLevel unitLevel;

    Health health;

    PlayerData playerData;

    [SerializeField] private GameDatas gameDatas;

    private void Awake()
    {
        gameDatas.OnGameDataLoaded.AddListener(Load);
        unitLevel.OnGainExp += SaveOnGain;
        unitLevel.OnLevelUp += SaveOnGain;

    }
    public float GetCurrentExp()
    {
        return unitLevel.CurrentExp;
    }

    public float GetMaxExp()
    {
        return unitLevel.MaxExp;
    }

    public int GetLevel()
    {
        //unitLevel.Init()
        return unitLevel.level;
    }

    public void GainExp(int val)
    {
        unitLevel.GainExp(val);
    }
    public float GetCurrentExpRate()
    {
        return unitLevel.CurrentExp / unitLevel.MaxExp;
    }

    public void Load()
    { 
        Debug.Log(gameDatas);
        Debug.Log(gameDatas.dataSettings);
        Debug.Log(gameDatas.dataSettings.playerData);

        playerData = gameDatas.dataSettings.playerData;
        Debug.Log(playerData.leftAbilityPoint);
        Debug.Log(playerData.currentExp);
        this.unitLevel.SetLevel(playerData.level, playerData.currentExp);
    }


    private void SaveOnGain(float current, float max) => Save();

    public void Save()
    {
        playerData.SetLevel(this.unitLevel);
        gameDatas.SaveDataLocal();
    }
}
