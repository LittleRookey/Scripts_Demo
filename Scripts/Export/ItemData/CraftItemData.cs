using UnityEngine;


namespace Litkey.InventorySystem
{
    /// <summary> �Һ� ������ ���� </summary>
    [CreateAssetMenu(fileName = "Item_Craft_", menuName = "Inventory System/Item Data/CraftItem")]
    public class CraftItemData : CountableItemData
    {
        /// <summary> ȿ����(ȸ���� ��) </summary>
        public float Value => _value;
        [SerializeField] private float _value;

        public override Item CreateItem()
        {
            //string _id = UniqueIDGenerator.GenerateUnqiueIDDateTime(Name);
            return new CraftItem(this, intID.ToString());
        }
    }
}
