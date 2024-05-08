using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Litkey.Stat
{

[System.Serializable]
    public struct StatData
    {
        public eSubStatType statType;
        public Color color;
        public Sprite statIcon;
    }

    [CreateAssetMenu(fileName = "StatColor", menuName = "Litkey/StatColor")]
    public class StatColor : ScriptableObject
    {

        public List<StatData> statDatabase = new List<StatData>();


        [Button("InitStatColors")]
        public void InitStatColor()
        {
            statDatabase.Clear();
            statDatabase.Add(new StatData { statType = eSubStatType.health, color = new Color32(255, 0, 0, 255) }); // Red
            statDatabase.Add(new StatData { statType = eSubStatType.attack, color = new Color32(255, 128, 0, 255) }); // Orange
            statDatabase.Add(new StatData { statType = eSubStatType.magicAttack, color = new Color32(0, 128, 255, 255) }); // Blue
            statDatabase.Add(new StatData { statType = eSubStatType.defense, color = new Color32(0, 255, 0, 255) }); // Green
            statDatabase.Add(new StatData { statType = eSubStatType.magicDefense, color = new Color32(128, 0, 255, 255) }); // Purple
            statDatabase.Add(new StatData { statType = eSubStatType.attackSpeed, color = new Color32(255, 255, 0, 255) }); // Yellow
            statDatabase.Add(new StatData { statType = eSubStatType.moveSpeed, color = new Color32(255, 128, 255, 255) }); // Pink
            statDatabase.Add(new StatData { statType = eSubStatType.critChance, color = new Color32(255, 255, 255, 255) }); // White
            statDatabase.Add(new StatData { statType = eSubStatType.critDamage, color = new Color32(128, 128, 128, 255) }); // Gray
            statDatabase.Add(new StatData { statType = eSubStatType.�߰����, color = new Color32(255, 215, 0, 255) }); // Gold
            statDatabase.Add(new StatData { statType = eSubStatType.�߰�����ġ, color = new Color32(0, 255, 255, 255) }); // Cyan
            statDatabase.Add(new StatData { statType = eSubStatType.����, color = new Color32(128, 255, 0, 255) }); // Lime
            statDatabase.Add(new StatData { statType = eSubStatType.ȸ��, color = new Color32(255, 128, 128, 255) }); // Salmon
            statDatabase.Add(new StatData { statType = eSubStatType.��������, color = new Color32(128, 128, 0, 255) }); // Olive
            statDatabase.Add(new StatData { statType = eSubStatType.��������, color = new Color32(0, 128, 128, 255) }); // Teal
            statDatabase.Add(new StatData { statType = eSubStatType.���������, color = new Color32(255, 0, 255, 255) }); // Magenta
            statDatabase.Add(new StatData { statType = eSubStatType.���������, color = new Color32(0, 255, 128, 255) }); // Emerald
        }
        public Color GetColor(eSubStatType statType)
        {
            for (int i = 0; i < statDatabase.Count; i++)
            {
                if (statDatabase[i].statType == statType) return statDatabase[i].color;
            }
            return Color.white;
        }

        public Sprite GetStatIcon(eSubStatType statType)
        {
            for (int i = 0; i < statDatabase.Count; i++)
            {
                if (statDatabase[i].statType == statType) return statDatabase[i].statIcon;
            }
            return null;
        }
    }

}