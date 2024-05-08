using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Litkey.Stat;
using Litkey.Utility;
using UnityEngine.Events;

public class StatContainer : MonoBehaviour
{
    [SerializeField] protected BaseStat baseStat;

    public int MonsterLevel;

    #region Stats
    public MainStat Strength { private set; get; } // �ٷ�
    public MainStat Vit { private set; get; } // ����
    public MainStat Avi { private set; get; } // ��ø
    public MainStat Sensation { private set; get; } // ����
    public MainStat Int { private set; get; } // ����

    public SubStat HP; // ü��

    public SubStat Attack { private set; get; } // ���ݷ�
    public SubStat MagicAttack { private set; get; } // ���� ���ݷ�

    public SubStat Defense { private set; get; } // ����
    public SubStat MagicDefense { private set; get; } // ���� ����

    public SubStat AttackSpeed { private set; get; } // ���ݼӵ�
    public SubStat MoveSpeed { private set; get; } // �̵� �ӵ�

    public SubStat CritChance { private set; get; } // ũ��Ƽ�� Ȯ��
    public SubStat CritDamage { private set; get; } // ũ��Ƽ�� ������

    public SubStat Precision { private set; get; } // ����
    public SubStat Evasion { private set; get; } // ȸ��

    public SubStat p_resist { private set; get; } // ���� ���� %
    public SubStat m_resist { private set; get; } // ���� ���� %

    public SubStat p_penetration { private set; get; } // ���� ����� %
    public SubStat m_penetration { private set; get; } // ���� ����� %

    // Ư�� ����
    //public SubStat receiveAdditionalDamage; // �޴� ���� ����
    //public SubStat giveAdditionalDamage; // �ִ� ���� ����

    //public SubStat receiveLessDamage; // �޴� ���� ����
    //public SubStat giveLessDamage; // �ִ� ���� ����

    public SubStat ExtraGold; // �߰� ���
    public SubStat ExtraExp; // �߰� ����ġ 
    #endregion

    //public UnitLevel unitLevel;
    [SerializeField] protected Alias alias;

    public int AbilityPoint { get; protected set; }

    public int addedStat { 
        get
        {
            if (statGiven == null)
            {
                statGiven = new Dictionary<eMainStatType, int>() {
                    { eMainStatType.�ٷ�, 0 },
                    { eMainStatType.����, 0 },
                    { eMainStatType.��ø, 0 },
                    { eMainStatType.����, 0 },
                    { eMainStatType.����, 0 },
                };
            }
            _addedStat = 0;
            foreach (var statplus in statGiven.Values)
            {
                _addedStat += statplus;
            }
            return _addedStat;
        }
    }
    protected int _addedStat = 0;

    // ���� ���� ���� ���������� ���ν��� + ���̽� ���� + �̸� ����(����)�� ���� ���� �ɰ��̴�
    public Dictionary<eMainStatType, MainStat> mainStats
    {
        get
        {
            if (_mainStats == null)
            {
                _mainStats = new Dictionary<eMainStatType, MainStat>() {
                    { eMainStatType.�ٷ�, this.Strength },
                    { eMainStatType.����, this.Vit },
                    { eMainStatType.��ø, this.Avi },
                    { eMainStatType.����, this.Sensation },
                    { eMainStatType.����, this.Int },
                };
            }
            return _mainStats;
        }
    }
    protected Dictionary<eMainStatType, MainStat> _mainStats;

    public Dictionary<eSubStatType, SubStat> subStats
    {
        get
        {
            if (_subStats == null || _subStats.Count == 0)
            {
                _subStats = new Dictionary<eSubStatType, SubStat>() {
                    { eSubStatType.health, this.HP },
                    { eSubStatType.attack, this.Attack },
                    { eSubStatType.magicAttack, this.MagicAttack },
                    { eSubStatType.defense, this.Defense },
                    { eSubStatType.magicDefense, this.MagicDefense },
                    //{ eSubStatType.attackRange, this.},
                    //{ eSubStatType.cc_Resistance, this. },
                    { eSubStatType.critChance, this.CritChance },
                    { eSubStatType.critDamage, this.CritDamage },
                    { eSubStatType.attackSpeed, this.AttackSpeed },
                    //{ eSubStatType.healthRegen, this. },
                    //{ eSubStatType.mana, this. },
                    //{ eSubStatType.manaRegen, this. },
                    { eSubStatType.moveSpeed, this.MoveSpeed },
                    { eSubStatType.���������, this.p_penetration },
                    { eSubStatType.���������, this.m_penetration },
                    { eSubStatType.����, this.Precision },
                    { eSubStatType.ȸ��, this.Evasion },
                    { eSubStatType.��������, this.p_resist },
                    { eSubStatType.��������, this.m_resist },
                    //{ eSubStatType.�޴����ذ���, this. },
                    //{ eSubStatType.�޴���������, this.Int },
                    //{ eSubStatType.�ִ����ذ���, this.Int },
                    //{ eSubStatType.�ִ���������, this.Int },
                    { eSubStatType.�߰�����ġ, this.ExtraExp },
                    { eSubStatType.�߰����, this.ExtraGold },
                };
            }
            return _subStats;
        }
    }
    protected Dictionary<eSubStatType, SubStat> _subStats;


    [HideInInspector] public UnityEvent<eMainStatType> OnIncreaseStat = new();
    [HideInInspector] public UnityEvent<eMainStatType, int> OnTryIncreaseStat = new();
    [HideInInspector] public UnityEvent OnApplyStat;
    [HideInInspector] public UnityEvent OnCancelStat;

    protected Dictionary<eMainStatType, int> statGiven;

    protected virtual void Awake()
    {
        this.MonsterLevel = baseStat.MonsterLevel;
        SetupStats();

        if (statGiven == null)
        {
            statGiven = new Dictionary<eMainStatType, int>() {
                { eMainStatType.�ٷ�, 0 },
                { eMainStatType.����, 0 },
                { eMainStatType.��ø, 0 },
                { eMainStatType.����, 0 },
                { eMainStatType.����, 0 },
            };
        }


        _mainStats = new Dictionary<eMainStatType, MainStat>() {
            { eMainStatType.�ٷ�, this.Strength },
            { eMainStatType.����, this.Vit },
            { eMainStatType.��ø, this.Avi },
            { eMainStatType.����, this.Sensation },
            { eMainStatType.����, this.Int },
        };

        if (_subStats == null)
        {
            _subStats = new Dictionary<eSubStatType, SubStat>() {
                    { eSubStatType.health, this.HP },
                    { eSubStatType.attack, this.Attack },
                    { eSubStatType.magicAttack, this.MagicAttack },
                    { eSubStatType.defense, this.Defense },
                    { eSubStatType.magicDefense, this.MagicDefense },
                    //{ eSubStatType.attackRange, this.},
                    //{ eSubStatType.cc_Resistance, this. },
                    { eSubStatType.critChance, this.CritChance },
                    { eSubStatType.critDamage, this.CritDamage },
                    { eSubStatType.attackSpeed, this.AttackSpeed },
                    //{ eSubStatType.healthRegen, this. },
                    //{ eSubStatType.mana, this. },
                    //{ eSubStatType.manaRegen, this. },
                    { eSubStatType.moveSpeed, this.MoveSpeed },
                    { eSubStatType.���������, this.p_penetration },
                    { eSubStatType.���������, this.m_penetration },
                    { eSubStatType.����, this.Precision },
                    { eSubStatType.ȸ��, this.Evasion },
                    { eSubStatType.��������, this.p_resist },
                    { eSubStatType.��������, this.m_resist },
                    //{ eSubStatType.�޴����ذ���, this. },
                    //{ eSubStatType.�޴���������, this.Int },
                    //{ eSubStatType.�ִ����ذ���, this.Int },
                    //{ eSubStatType.�ִ���������, this.Int },
                    { eSubStatType.�߰�����ġ, this.ExtraExp },
                    { eSubStatType.�߰����, this.ExtraGold },
                };
        }
        
        //Evasion = new SubStat("ȸ��", baseStat.Evasion, eSubStatType.ȸ��);

        //receiveAdditionalDamage = new SubStat("�޴� ���� ����", 0f, eSubStatType.�޴���������, true);
        //giveAdditionalDamage = new SubStat("�ִ� ���� ����", 0f, eSubStatType.�ִ���������, true);
        //receiveLessDamage = new SubStat("�޴� ���� ����", 0f, eSubStatType.�޴����ذ���, true);
        //giveLessDamage = new SubStat("�ִ� ���� ����", 0f, eSubStatType.�޴���������, true);

    }

    public void ClearMainStats()
    {
        Strength.ClearStat();
        Vit.ClearStat();
        Avi.ClearStat();
        Sensation.ClearStat();
        Int.ClearStat();
    }

    protected void SetupStats()
    {
        Strength = new MainStat("�ٷ�", 0, eMainStatType.�ٷ�);
        Vit = new MainStat("����", 0, eMainStatType.����);
        Avi = new MainStat("��ø", 0, eMainStatType.��ø);
        Sensation = new MainStat("����", 0, eMainStatType.����);
        Int = new MainStat("����", 0, eMainStatType.����);

        HP = new SubStat("ü��", baseStat.MaxHP, eSubStatType.health).SetMaxUIValue(1000f);
        Attack = new SubStat("���ݷ�", baseStat.Attack, eSubStatType.attack).SetMaxUIValue(100f);
        MagicAttack = new SubStat("���� ���ݷ�", baseStat.MagicAttack, eSubStatType.magicAttack).SetMaxUIValue(100f);

        Defense = new SubStat("����", baseStat.Defense, eSubStatType.defense).SetMaxUIValue(100f);
        MagicDefense = new SubStat("���� ����", baseStat.MagicDefense, eSubStatType.magicDefense).SetMaxUIValue(100f);

        AttackSpeed = new SubStat("���ݼӵ�", baseStat.AttackSpeed, eSubStatType.attackSpeed, true).SetMaxUIValue(0.3f);
        MoveSpeed = new SubStat("�̵��ӵ�", baseStat.MoveSpeed, eSubStatType.moveSpeed, true).SetMaxUIValue(0.3f);

        CritChance = new SubStat("ũ��Ƽ�� Ȯ��", baseStat.CritChance, eSubStatType.critChance, true).SetMaxUIValue(1f);
        CritDamage = new SubStat("ũ��Ƽ�� ������", baseStat.CritDamage, eSubStatType.critDamage, true).SetMaxUIValue(1f);

        ExtraGold = new SubStat("��� �߰�ŉ�淮", baseStat.ExtraGold, eSubStatType.�߰����, true).SetMaxUIValue(1f);
        ExtraExp = new SubStat("����ġ �߰�ŉ�淮", baseStat.ExtraExp, eSubStatType.�߰�����ġ, true).SetMaxUIValue(1f);

        Precision = new SubStat("����", baseStat.Precision, eSubStatType.����).SetMaxUIValue(100f);
        Evasion = new SubStat("ȸ��", baseStat.Evasion, eSubStatType.ȸ��).SetMaxUIValue(100f);

        p_resist = new SubStat("���� ���׷�", baseStat.p_resist, eSubStatType.��������, 0f, 100f).SetMaxUIValue(100f);
        m_resist = new SubStat("���� ���׷�", baseStat.magic_resist, eSubStatType.��������, 0f, 100f).SetMaxUIValue(100f);
        p_penetration = new SubStat("���� �����", baseStat.p_penetration, eSubStatType.���������, 0f, 100f).SetMaxUIValue(100f);
        m_penetration = new SubStat("���� �����", baseStat.magic_penetration, eSubStatType.���������, 0f, 100f).SetMaxUIValue(100f);

        //receiveAdditionalDamage = new SubStat("�޴� ���� ����", 0f, eSubStatType.�޴���������, true);
        //giveAdditionalDamage = new SubStat("�ִ� ���� ����", 0f, eSubStatType.�ִ���������, true);
        //receiveLessDamage = new SubStat("�޴� ���� ����", 0f, eSubStatType.�޴����ذ���, true);
        //giveLessDamage = new SubStat("�ִ� ���� ����", 0f, eSubStatType.�޴���������, true);


        Strength.AddSubStatAsChild(Attack);
        Strength.AddSubStatAsChild(HP);

        Vit.AddSubStatAsChild(HP);
        Vit.AddSubStatAsChild(Defense);
        Vit.AddSubStatAsChild(MagicDefense);

        Avi.AddSubStatAsChild(AttackSpeed);
        Avi.AddSubStatAsChild(Evasion);
        Avi.AddSubStatAsChild(CritChance);

        Sensation.AddSubStatAsChild(Precision);
        Sensation.AddSubStatAsChild(CritDamage);

        Int.AddSubStatAsChild(MagicAttack);

        // ���꽺�ݿ��� ���ν��ݰ� ����α�
        HP.AddAsInfluencer(StatUtility.StatPerValue(Strength, 1, 13f));
        HP.AddAsInfluencer(StatUtility.StatPerValue(Vit, 1, 25f));

        Attack.AddAsInfluencer(StatUtility.StatPerValue(Strength, 1, 9f));
        Attack.AddAsInfluencer(StatUtility.StatPerValue(Avi, 1, 3f));

        MagicAttack.AddAsInfluencer(StatUtility.StatPerValue(Int, 1, 15f));

        Defense.AddAsInfluencer(StatUtility.StatPerValue(Vit, 1, 4f));

        MagicDefense.AddAsInfluencer(StatUtility.StatPerValue(Vit, 1, 3f));

        AttackSpeed.AddAsInfluencer(StatUtility.StatPerValue(Avi, 5, 0.01f));

        CritChance.AddAsInfluencer(StatUtility.StatPerValue(Avi, 5, 0.005f));
        CritDamage.AddAsInfluencer(StatUtility.StatPerValue(Sensation, 3, 0.01f));

        //ExtraGold = new SubStat("��� �߰�ŉ�淮", baseStat.ExtraGold, eSubStatType.�߰����, true);
        //ExtraExp = new SubStat("����ġ �߰�ŉ�淮", baseStat.ExtraExp, eSubStatType.�߰�����ġ, true);

        Precision.AddAsInfluencer(StatUtility.StatPerValue(Sensation, 2, 1f));
        Evasion.AddAsInfluencer(StatUtility.StatPerValue(Avi, 5, 1f));
    }

    /// <summary>
    /// ���ν��� ����Ʈ������ �ش� ���꽺���� �� �������� ���� 
    /// </summary>
    /// <param name="subStatType"></param>
    /// <returns></returns>
    public float GetTotalPreviewOf(eSubStatType subStatType)
    {
        float total = 0f;
        var influencers = subStats[subStatType].Influencers;
        for (int i = 0; i< influencers.Count; i++)
        {
            // statGiven[mainStatType] != 0 => 
            var mainStatUsed = statGiven[influencers[i]._mainStat.mainStatType];
            if (mainStatUsed > 0)
            {
                // 
                total += influencers[i].GetPreviewValue(mainStatUsed);
            }
        }
        return total;
    }


    
    // addedstat = 0, 1, 5
    // 1, 1, 4
    // 2, 1, 3
    // 3, 1, 2
    public void TryAddMainStat(eMainStatType mainStat, int val=1)
    {
        if (this.addedStat + val > this.AbilityPoint) return;
        if (this.addedStat + val < 0) return;
        if (statGiven[mainStat] + val < 0) return;
        statGiven[mainStat] += val;

        OnTryIncreaseStat?.Invoke(mainStat, statGiven[mainStat]);
    }

    public void ApplyStat()
    {
        foreach (var stat in mainStats.Keys)
        {
            var increaseStat = statGiven[stat];
            if (increaseStat == 0) continue;
            mainStats[stat].IncreaseStat(increaseStat);
            OnIncreaseStat?.Invoke(stat);
        }

        this.AbilityPoint -= this.addedStat;
        ClearStatGivenPoints();
        OnApplyStat?.Invoke();
    }

    protected void ClearStatGivenPoints()
    {
        foreach (var stat in mainStats.Keys)
        {
            statGiven[stat] = 0;
        }
    }

    public void CancelStatChange()
    {
        foreach (var stat in mainStats.Keys)
        {
            statGiven[stat] = 0;
        }
        OnCancelStat?.Invoke();
    }

    #region Damage 
    // ü�� - (�� ���ݷ� - ���� (>=0) ) * (100 - (�Ʊ� �������� - �� ��������� >= 0) / 100 or (�Ʊ��������� - �� ���������) / 100)
    // => ��ü�� - ((�ư� - ����) * (100 - (�������� - �Ʊ������ / 100)) 
    // ������ * ( 
    // �����ڰ� �θ�,
    public Damage GetDamageAgainst(StatContainer enemyStat)
    {

        float dmg;
        var m_AttackVal = GetFinalDamage();

        if (m_AttackVal.isPhysicalDmg)
        {
            float attackDmg = (m_AttackVal.damage * (1f + (p_penetration.FinalValue - enemyStat.p_resist.FinalValue))) 
                - (enemyStat.Defense.FinalValue * 1f + (enemyStat.p_resist.FinalValue - p_penetration.FinalValue));


            dmg = (Mathf.Clamp(attackDmg, 1f, 999999999));

        }
        else
        {
            // magic dmg
            //float attackDmg = m_AttackVal.damage - enemyStat.MagicDefense.FinalValue;
            //dmg = (Mathf.Clamp(attackDmg, 1f, 999999999) * (1f + (m_penetration.FinalValue - enemyStat.m_resist.FinalValue) / 100f));
            float attackDmg = (m_AttackVal.damage * (1f + (m_penetration.FinalValue - enemyStat.m_resist.FinalValue)))
                - (enemyStat.MagicDefense.FinalValue * 1f + (enemyStat.m_resist.FinalValue - m_penetration.FinalValue));
            dmg = (Mathf.Clamp(attackDmg, 1f, 999999999));
        }
        return new Damage(dmg, m_AttackVal.isCrit, m_AttackVal.isPhysicalDmg);
    }

    public Damage GetFinalDamage()
    {
        bool isPhysic = Attack.FinalValue >= MagicAttack.FinalValue;
        if (isPhysic)
        {
            if (ProbabilityCheck.GetThisChanceResult(CritChance.FinalValue))
            {
                return new Damage(Attack.FinalValue * (1 + CritDamage.FinalValue), true, isPhysic);
            }
            return new Damage(Attack.FinalValue, false, isPhysic);
        }
        else
        {
            if (ProbabilityCheck.GetThisChanceResult(CritChance.FinalValue))
            {
                return new Damage(MagicAttack.FinalValue * (1 + CritDamage.FinalValue), true, false);
            }
            return new Damage(MagicAttack.FinalValue, false, false);
        }
    }

    public Damage GetSkillDamage(float skillDmgPercent, bool isPhysic)
    {
        if (ProbabilityCheck.GetThisChanceResult(CritChance.FinalValue))
        {
            return new Damage((Attack.FinalValue * skillDmgPercent) * (1 + CritDamage.FinalValue), true, isPhysic);
        }
        return new Damage(Attack.FinalValue * skillDmgPercent, false, isPhysic);
    }

    // ���������θ�
    // ���� = ���� 
    // ���߷� = ���� - �� ȸ��
    // True = ���߼��� 
    public bool CalculateHit(StatContainer enemyStat)
    {
        float hitChance = (Precision.FinalValue - enemyStat.Evasion.FinalValue) / (Precision.FinalValue + enemyStat.Evasion.FinalValue);
        // 5/15 = .333 /.
        
        if (TryGetComponent<LevelSystem>(out LevelSystem levelSystem))
        {
            // �� ������ ������ �� ȸ��ġ�� �ö�
            // �Ʊ������� ������ �ƹ��ϵ� ����
            // 5������ �� ȸ�� 5% �ö�
            int allyLevel = levelSystem.GetLevel();
            if (enemyStat.MonsterLevel > allyLevel)
            {
                int quotient = (enemyStat.MonsterLevel - allyLevel) / 5;
                hitChance -= 0.05f * quotient;
            } else if (allyLevel > enemyStat.MonsterLevel)
            {
                int quotient = (allyLevel - enemyStat.MonsterLevel) / 5;
                hitChance += 0.05f * quotient;
            }
        }
        Debug.Log("Hitchance: " + hitChance);
        hitChance = Mathf.Clamp(hitChance, 0.05f, 1f);
        return ProbabilityCheck.GetThisChanceResult(hitChance);
    }

    public float Defend(float inComingDamage)
    {
        return Mathf.Max(1f, inComingDamage - Defense.FinalValue);
    }
    #endregion

    public void AddMaxHealth(float val)
    {
        HP.AddStatValue(val);
    }

    public void GiveAlias(Alias alias)
    {
        this.alias = alias;
    }

    public BaseStat GetBaseStat()
    {
        return this.baseStat;
    }
}
