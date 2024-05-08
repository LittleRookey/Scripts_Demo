using UnityEngine;


namespace Litkey.InventorySystem
{
    /// <summary> ��� - �� ������ </summary>
    [CreateAssetMenu(fileName = "Item_Armor_", menuName = "Inventory System/Item Data/Armor")]
    public class ArmorItemData : EquipmentItemData
    {
        /// <summary> ���� </summary>
        public override Item CreateItem()
        {
            //string _id = UniqueIDGenerator.GenerateUnqiueIDDateTime(Name);
            itemType = "��";
            var item = new ArmorItem(this, intID.ToString());
            //ResourceManager.Instance.MakeRandomStats(item);
            return item;
        }

    }
}