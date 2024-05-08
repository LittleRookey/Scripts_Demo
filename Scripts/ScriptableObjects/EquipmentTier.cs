using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentTier", menuName = "Litkey/EquipmentTier")]
public class EquipmentTier : ScriptableObject
{
    [Header("��� ����")]
    public int requiredGold; // ���� ������ �ʿ��� ���
    public int initialRequiredGold = 100; // ó���� �ʿ��� ���
    public int extraGoldForNextLevel = 5; // ���� �������� �ʿ��� �߰����
    public float growthFactor = 1.1f;
    public AnimationCurve goldCurve;
    public List<int> requiredGolds;

    [Header("���Ƽ�� ����")]
    public int currentTier; // ����� Ƽ��, ������ ���� �ٴٸ��� Ƽ� �ø��� �ִ�
    public int currentLevel; // ���� ����
    public Sprite[] equipmentSprite;
    
    public int maxLevel = 20;
    public int totalUpgradeLevel;

    [Header("��� �������")]
    public string equipmentName;
    [TextArea]
    public string itemExplanation;

    private void OnEnable()
    {
        UpdateRequiredGold();
        //requiredGold = (int)(requiredGold * goldCurve.Evaluate(currentTier));
    }
    public void UpgradeLevel()
    {
        if (currentLevel >= maxLevel) return;

        currentLevel++;
        totalUpgradeLevel++;
        UpdateRequiredGold();
    }
    private Sprite UpgradeTier()
    {
        currentTier++;
        return equipmentSprite[currentTier];
    }

    private void UpdateRequiredGold()
    {
        //requiredGold = (int)(requiredGold * (1f + goldCurve.Evaluate(currentLevel)));
        requiredGold = requiredGolds[totalUpgradeLevel];
    }

    [Button("Update Max Exps Per Level")]
    public void UpdateMaxGolds()
    {
        requiredGolds.Clear();
        requiredGolds.Add(initialRequiredGold);
        for (int i = 0; i < maxLevel; i++)
        {
            int _level = i + 1;
            float growth = 1 + goldCurve.Evaluate((float)_level / (float)maxLevel);
            int fin_gold = (int)((initialRequiredGold + (extraGoldForNextLevel * i)) * Mathf.Pow(growthFactor, _level) * growth);

            requiredGolds.Add(fin_gold);
        }
    }
}
