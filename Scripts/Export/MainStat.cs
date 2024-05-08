using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Litkey.InventorySystem;
using System.Linq;

namespace Litkey.Stat
{

    public struct Damage
    {
        public float damage;
        public bool isCrit;
        public bool isPhysicalDmg;
        public Damage(float dmg, bool crit)
        {
            this.damage = dmg;
            this.isCrit = crit;
            this.isPhysicalDmg = false;
        }

        public Damage(float dmg, bool crit, bool isPhysicalDmg)
        {
            this.damage = dmg;
            this.isCrit = crit;
            this.isPhysicalDmg = isPhysicalDmg;
        }
    }

    [System.Serializable]  
    public class MainStat
    {

        public string StatName { get; private set; } // ���� ID
        [HideInInspector] public int BaseStat { get; private set; }
        public eMainStatType mainStatType { get; private set; }
        //private int _addedStat;

        //  ���ν����� �÷��ִ� ���꽺�ݵ�
        public List<SubStat> ChildSubstats 
        { 
            get
            {
                if (_childSubStats == null)
                {
                    _childSubStats = new List<SubStat>();
                }
                return _childSubStats;
            }
        }

        private List<SubStat> _childSubStats;
        public int LevelAddedStats { get; private set; } // ������ ����Ʈ�� ���� ���ν��� �߰�����Ʈ


        [HideInInspector] public UnityEvent OnValueChanged;
        public float Value => FinalValue;


        public int FinalValue
        {
            get
            {
                return BaseStat + LevelAddedStats;
            }
        }
        public MainStat(string statName, int value)
        {
            this.StatName = statName;
            this.BaseStat = value;
            this.LevelAddedStats = 0;
            this._childSubStats = new List<SubStat>();

        }

        public MainStat(string statName, int value, eMainStatType mainStatType)
        {
            this.StatName = statName;
            this.BaseStat = value;
            this.LevelAddedStats = 0;
            this._childSubStats = new List<SubStat>();
            this.mainStatType = mainStatType;

        }

        // Increases main stat by 1
        public void IncreaseStat(int addedVal=1)
        {
            if (addedVal <= 0) return;
            LevelAddedStats += addedVal;
            Debug.Log(StatName + ": " + LevelAddedStats);

            UpdateValue();
        }

        
        

        // apply mainstat changes to substat, 
        // each substat will apply changes to each mainstat that belongs to
        private void UpdateValue()
        {
            foreach (SubStat stat in ChildSubstats)
            {
                stat.ApplyChange(this, FinalValue);
            }
        }

        public void AddSubStatAsChild(SubStat ss)
        {
            if (ChildSubstats.Contains(ss)) return;
            ChildSubstats.Add(ss);
        }

        public int GetFinalValue()
        {
            return FinalValue;
        }

        public override string ToString()
        {
            return StatName;
        }

        public bool Equals(MainStat other)
        {
            return this.StatName == other.StatName;
        }

        public void DisplayAllSubstats()
        {
            string s = "";
            foreach (SubStat ss in _childSubStats)
            {
                s+= $"{ss.ToString()}: {ss.FinalValue}\n";
            }
            Debug.Log(s);
        }


        /// <summary>
        /// ���꽺���ϳ��� ���ν�������Ʈ��ŭ�� �ö󰡴°��� ����
        /// </summary>
        /// <param name="subStatType"></param>
        /// <param name="mainStatPoint"></param>
        /// <returns></returns>
        public float GetFutureStat(eSubStatType subStatType, int mainStatPoint)
        {
            for (int i = 0; i < ChildSubstats.Count; i++)
            {
                if (ChildSubstats[i].statType == subStatType) return ChildSubstats[i].GetFutureStat(this, mainStatPoint);
            }
            return -1f;
        } 

        public void ClearStat()
        {
            this.LevelAddedStats = 0;
            UpdateValue();
        }
    }

    [System.Serializable]
    public class SubStat
    {
        // ���ν��� Influencer���� ���꽺�ݵ鿡 ���ϴ� ����

        //public float Value => _finalValue;
        public string DisplayName => displayName;

        private string displayName;

        public eSubStatType statType;

        private float _finalValue;
        
        public float FinalValue
        {
            get
            {
                UpdateFinalValue();
                if (minValue < 0f || maxValue < 0f) return this._finalValue;
                else return Mathf.Clamp(this._finalValue, this.minValue, this.maxValue);
            }
            //set
            //{
            //    UpdateFinalValue();
            //    float origin = _finalValue;
            //    if (origin != value)
            //        OnValueChanged?.Invoke(origin);
            //}
        }


        public bool IsPercentage { private set; get; } // is substat shown as percantage

        private string shownFinalValue => IsPercentage ? _finalValue.ToString() + percantage : _finalValue.ToString();

        public List<Influencer> Influencers => influencers;
        private List<Influencer> influencers; // mainstats that affect substat

        string percantage = "%";

        // �ʱ� ĳ���� ���� ��
        public float BaseStat { get; private set; }

        private float minValue = -1; // ������ �ּҰ�
        private float maxValue = -1; // ������ �ִ밪

        private float uiMaxValue = -1; // UI������ ����
        public float UIMaxValue => uiMaxValue;
        // ������ ���Է� ���� ���� ��
        // ( ���(+) + ����(+) + ����(+) )
        private float _plusValue
        {
            get
            {
                return _plusStatValue + _plusEquipValue + _plusBuffValue;
            }
        } 
        // ������ ���� ���� ���� ��
        // ( ���(*) + ����(*) + ����(*) )
        private float _multipliedValue
        {
            get
            {
                return _multipliedStatValue + _multipliedEquipValue + _multipliedBuffValue;
            }
        } 

        // ����, ��� 
        private List<StatModifier> buffStats;
        public Dictionary<string, List<StatModifier>> equipStats { get; private set; }

        private float _plusStatValue; // ���� �������� �߰��� + ����
        private float _plusEquipValue; // ��� �������� �߰��� + ����
        private float _plusBuffValue; // ������������ �߰��� + ����

        private float _multipliedStatValue; // ���� �������� �߰��� * ����
        private float _multipliedEquipValue; // ��� �������� �߰��� * ����
        private float _multipliedBuffValue; // ���� �������� �߰��� * ����

        [HideInInspector] public UnityEvent<float> OnValueChanged = new();

        #region constructors
        // Constructor that has initial value
        public SubStat(string displayName, float initValue, eSubStatType statType, bool isPercantage = false)
        {

            this.IsPercentage = isPercantage;
            this.BaseStat = initValue;
            this._finalValue = initValue;

            this._plusStatValue = 0;
            this._plusEquipValue = 0;
            this._plusBuffValue = 0;

            this._multipliedStatValue = 0;
            this._multipliedEquipValue = 0;
            this._multipliedBuffValue = 0;

            this.displayName = displayName;
            this.statType = statType;
            this.equipStats = new Dictionary<string, List<StatModifier>>();
            influencers = new List<Influencer>();

            buffStats = new List<StatModifier>();
            //equipStats = new List<StatModifier>();

        }

     
        public SubStat(string displayName, float initValue, eSubStatType statType, float minVal, float maxVal, bool isPercantage = false)
        {

            this.IsPercentage = isPercantage;
            this.BaseStat = initValue;
            this._finalValue = initValue;

            this._plusStatValue = 0;
            this._plusEquipValue = 0;
            this._plusBuffValue = 0;

            this._multipliedStatValue = 0;
            this._multipliedEquipValue = 0;
            this._multipliedBuffValue = 0;

            this.minValue = minVal;
            this.maxValue = maxVal;

            this.displayName = displayName;
            this.statType = statType;
            this.equipStats = new Dictionary<string, List<StatModifier>>();
            influencers = new List<Influencer>();

            buffStats = new List<StatModifier>();
            //equipStats = new List<StatModifier>();
        }

        #endregion

        /// <summary>
        /// �ϳ��� ������ �����Ų��, ������ ������ �� �������� ������ �д�
        /// </summary>
        /// <param name="equipmentID">��� ���� ���̵�</param>
        /// <param name="stat">����� ���� �ϳ�</param>
        public void AddEquipValue(string equipmentID, StatModifier stat)
        {
            if (!equipStats.ContainsKey(equipmentID))
            {
                equipStats[equipmentID] = new List<StatModifier>();
            }

            equipStats[equipmentID].Add(stat);

            if (stat.oper == OperatorType.plus) _plusEquipValue += stat.value;
            else if (stat.oper == OperatorType.subtract) _plusEquipValue -= stat.value;
            else if (stat.oper == OperatorType.multiply) _multipliedEquipValue += stat.value;
            else if (stat.oper == OperatorType.divide) _multipliedEquipValue -= stat.value;

            UpdateFinalValue();
        }
        

        // �� ����� ��� ������ �����ϱ�
        public void UnEquipValue(string equipmentID, StatModifier stat)
        {
            if (equipStats[equipmentID].Remove(stat))
            {
                if (stat.oper == OperatorType.plus) _plusEquipValue -= stat.value;
                else if (stat.oper == OperatorType.subtract) _plusEquipValue += stat.value;
                else if (stat.oper == OperatorType.multiply) _multipliedEquipValue -= stat.value;
                else if (stat.oper == OperatorType.divide) _multipliedEquipValue += stat.value;
            }
            UpdateFinalValue();
        }


        public void AddBuffValue(StatModifier statModifier)
        {
            buffStats.Add(statModifier);

            UpdateFinalValue();
        }
        
        public void RemoveBuffValue(StatModifier statModifier)
        {
            buffStats.Remove(statModifier);

            UpdateFinalValue();
        }
        // Required to use when creating substats
        public void AddAsInfluencer(StatPerValue spv)
        {
            influencers.Add(new Influencer(
                spv.baseMainStat, 
                spv.baseMainStat.GetFinalValue(), 
                spv.perStat, 
                spv.increasedStat));
            UpdateFinalValue();
        }

        // ���� ���÷�𼭰� ������ 
        // if one of the main stat values changed, find that main stat and apply changes to that influencer
        public void ApplyChange(MainStat mainStat, int changedMainStatValue)
        {
            // + ���ݰ��� ����
            _plusStatValue = 0;
            List<Influencer> relatedInfluencers = influencers.Where(inf => inf.IsEqual(mainStat)).ToList();

            for (int i = 0; i < relatedInfluencers.Count; i++) relatedInfluencers[i].ApplyChange(changedMainStatValue);

            for (int i = 0; i < influencers.Count; i++) _plusStatValue += influencers[i].GetFinalValue();

            // update final value based on change
            UpdateFinalValue();
        }

        public void AddStatValue(float value)
        {
            _plusStatValue += value;
            UpdateFinalValue();
        }

        private float UpdateFinalValue()
        {
            float origin = _finalValue;
            // �ʱ� ����
            _finalValue = this.BaseStat;

            // ���� ���� ��
            //foreach(Influencer inf in influencers)
            //{
            //    _plusStatValue += inf.GetFinalValue();
            //}

            _finalValue *= (1f + _multipliedValue);
            _finalValue += _plusValue;
            if (origin != _finalValue) OnValueChanged?.Invoke(_finalValue);
            return _finalValue;
        }

        public float GetAddedStatValue()
        {
            float val = 0;
            foreach (Influencer inf in influencers)
            {
                val += inf.GetFinalValue();
            }
            return val;
        }

        // ���ݷ� ���� ����
        // �⺻ ���ݷ� * (1+ ���� ���ݷ�) + ���� ���ݷ�
        // ���� ���ݷ� = (���� �߰� ���ݷ� + ��� ���ݷ�)

        public override string ToString()
        {
            return displayName;
        }

        // ���ν��� x�� �ش� ���꽺���� �߰����� �����´�
        public float GetFutureStat(MainStat mainStat, int addedMainStatValue)
        {
            float addedValue = 0f;
            // substat can't have repetitive main stat in influencer
            Influencer inf = influencers.Find((Influencer inf) => inf._mainStat.ToString() == mainStat.ToString());
            // apply the changed substat value
            addedValue = inf.GetPreviewValue(addedMainStatValue);
            // update final value based on change
            return addedValue;
        }

        /// <summary>
        /// �ش� ���ν��ݿ� ���ϴ� Influencer�� ����
        /// </summary>
        /// <param name="mainStat"></param>
        /// <returns></returns>
        public Influencer GetInfluencerOf(MainStat mainStat)
        {
            for (int i = 0; i < influencers.Count; i++)
            {
                if (influencers[i]._mainStat.Equals(mainStat)) return influencers[i];
            }
            Debug.LogError("There is no such influencer of " + mainStat.StatName);
            return null;
        }
        public SubStat SetMaxUIValue(float val)
        {
            uiMaxValue = val;
            return this;
        }
    }
    
    public enum eRarity
    {
        F, FF, FFF, E, EE, EEE, D, DD, DDD, C, CC, CCC, B, BB, BBB, A, AA, AAA, S, SS, SSS, EX   
    }


    // class used for substat to apply changes to mainstats
    [System.Serializable]
    public class Influencer
    {
        public MainStat _mainStat { get; }
        public int PerMainStat => _perMainStat;
        private int _perMainStat;

        public float IncreaseValue => _increaseValue;
        private float _increaseValue; // increase value per increased main stat of substat, ex) VIT 1 �� 100 ü��

        private int currentMainStat; // actual value of main stat assigned

        private float finalValue; // value used to apply changes to mainStat 
        public Influencer(MainStat mainStat, int currentMainStatValue, int perMainStat, float increaseValue)
        {
            _mainStat = mainStat;
            _perMainStat = perMainStat;
            _increaseValue = increaseValue;
            currentMainStat = currentMainStatValue;
            finalValue = 0f;
        }

        /// <summary>
        /// �ش� ������ ��� ������ �� ���ݰ��� ����
        /// </summary>
        /// <param name="mainStatValue"></param>
        /// <returns></returns>
        public float ApplyChange(int mainStatValue)
        {
            currentMainStat = mainStatValue;
            finalValue = (currentMainStat / _perMainStat) * _increaseValue;
            return finalValue;
        }

        
        /// <summary>
        /// �߰��� ���ν�������Ʈ�� ���� ����
        /// </summary>
        /// <param name="addedMainStatValue"></param>
        /// <returns></returns>
        public float GetPreviewValue(int addedMainStatValue)
        {
            float result = (addedMainStatValue / _perMainStat) * _increaseValue;
            return result;
        }

        public float GetFinalValue()
        {
            finalValue = (currentMainStat / _perMainStat) * _increaseValue;
            return finalValue;
        }
        public bool IsEqual(MainStat mainStat)
        {
            return string.Equals(this._mainStat.StatName, mainStat.StatName);
        }

    }



    public struct StatPerValue
    {
        public MainStat baseMainStat;
        public int perStat;
        public float increasedStat;
        public StatPerValue(MainStat baseMainStat, int perStat, float increasedStat)
        {
            this.baseMainStat = baseMainStat;
            this.perStat = perStat;
            this.increasedStat = increasedStat;
        }
    }

    public class StatUtility
    {
        
        public static StatPerValue StatPerValue(MainStat mainStat, int perStat, float increaseValue)
        {
            return new StatPerValue(mainStat, perStat, increaseValue);
        }

        public static eSubStatType GetRandomStat()
        {
            int totalLength = System.Enum.GetValues((typeof(eSubStatType))).Length;

            int randNum = Random.Range(0, totalLength);
            return (eSubStatType)randNum;
        }
    }

    [System.Serializable]
    public class StatModifier
    {
        public eSubStatType statType;
        public float value;
        public OperatorType oper;

        public bool Compare(StatModifier stat)
        {
            return stat.statType == this.statType
                && stat.value == this.value
                && stat.oper == this.oper;
        }

        public bool IsStatType(eSubStatType statType)
        {
            return this.statType == statType;
        }

        public bool IsStatType(SubStat subStat)
        {
            return this.statType == subStat.statType;
        }
    }

}
public enum eMainStatType
{
    �ٷ�,
    ����,
    ��ø,
    ����,
    ����,
};

public enum eSubStatType
{
    health,
    mana,
    healthRegen,
    manaRegen,
    attack,
    magicAttack,
    critChance,
    defense,
    magicDefense,
    attackSpeed,
    cc_Resistance,
    moveSpeed,
    attackRange,
    critDamage,
    ��������,
    ��������,
    ���������,
    ���������,
    ����, 
    ȸ��,
    �ִ���������,
    �ִ����ذ���,
    �޴���������,
    �޴����ذ���,
    �߰����,
    �߰�����ġ
};
