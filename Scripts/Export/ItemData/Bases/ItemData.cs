using Litkey.Interface;
using UnityEngine;


namespace Litkey.InventorySystem
{
    public enum EquipmentRarity
    {
        �븻 = 0,
        ���� = 1,
        ���� = 2,
        ����ũ = 3,
        ������ = 4,
        ��ȭ = 5,
        �ʿ� = 6


    };

    public enum eEquipmentParts
    {
        helmet,
        body,
        pants,
        shoe,
        Weapon,
        Subweapon,
        Accessory
    }
    /*
        [��� ����]
        ItemData(abstract)
            - CountableItemData(abstract)
                - PortionItemData
            - EquipmentItemData(abstract)
                - WeaponItemData
                - ArmorItemData
    */
    [System.Serializable]
    public abstract class ItemData : ScriptableObject, IRewardable<ItemData>
    {
        public int intID;
        
        public string Name => _name;
        public string Tooltip => _tooltip;
        public Sprite IconSprite => _iconSprite;
        [HideInInspector]
        public string itemType;
        public EquipmentRarity rarity => _rarity;
        public int Weight => _weight;

        /// <summary> �ִ� ������ </summary>
        [SerializeField] private EquipmentRarity _rarity = EquipmentRarity.�븻;
        [SerializeField] private int _weight = 0;
        //protected string _id;
        [SerializeField] private string _name;    // ������ �̸�
        [Multiline]
        [SerializeField] private string _tooltip; // ������ ����
        [SerializeField] private Sprite _iconSprite; // ������ ������

        /// <summary> Ÿ�Կ� �´� ���ο� ������ ���� </summary>
        public abstract Item CreateItem();

        public bool IsEquipment()
        {
            return (this is ArmorItemData) ||
                (this is WeaponItemData) ||
                (this is AccessoryItemData);
        }

        public virtual ItemData GetReward() { return this; }

    }

 
}