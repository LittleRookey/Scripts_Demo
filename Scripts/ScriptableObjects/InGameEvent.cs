using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum eEventType
{
    �Ϲݸ������, // ��ġ�� ���� ���� ���
    �������, // Ư�� ���͵� ������ ��� ����Ʈ
    ����Ʈ����, // �Ϲݸ��͵�, ����Ʈ����
    ��������, // �Ϲݸ��͵�, ����Ʈ, ����
}

public enum eDifficulty
{
    I,
    II,
    III,
    IV,
    V
}

[CreateAssetMenu(fileName = "Quest", menuName = "Litkey/Quest/InGameEvent")]
public class InGameEvent : ScriptableObject
{
    public string questID;
    public eDifficulty difficulty;
    [HideInInspector] public Vector2 eventPosition;
    public eEventType eventType;

    public string normalMonsterName;
    public int[] normalMonsterCount = { 10, 25, 50, 75, 100 };

    public string eliteMonsterName;
    public int[] eliteMonsterCount = { 2, 4, 6, 8, 10 };

    public string bossMonsterName;
    public int[] bossMonsterCount = { 1, 2, 3, 4, 5 };

    
    public void MakeRandomEvent()
    {
        // Reset monster counts


        // Based on eventType, set appropriate monster counts
        switch (eventType)
        {
            case eEventType.�Ϲݸ������:
                GenerateNormalMonsters();
                break;
            case eEventType.�������:
                GenerateNormalMonsters();
                GenerateEliteMonsters();
                break;
            case eEventType.����Ʈ����:
                GenerateNormalMonsters();
                GenerateEliteMonsters();
                break;
            case eEventType.��������:
                GenerateNormalMonsters();
                GenerateEliteMonsters();
                GenerateBossMonsters();
                break;
        }
    }

    private void GenerateNormalMonsters()
    {
        // Set normalMonsterName and normalMonsterCount based on difficulty
    }

    private void GenerateEliteMonsters()
    {
        // Set eliteMonsterName and eliteMonsterCount based on difficulty
    }

    private void GenerateBossMonsters()
    {
        // Set bossMonsterName and bossMonsterCount based on difficulty
    }
}

[System.Serializable]
public class EventWeight
{
    public InGameEvent Event;
    public int weight;

}
