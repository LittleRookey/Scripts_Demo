using UnityEngine;


namespace Litkey.InventorySystem
{
    /// <summary> ��� - �� ������ </summary>
    [CreateAssetMenu(fileName = "Item_Accessory_", menuName = "Inventory System/Item Data/Accessory")]
    public class AccessoryItemData : EquipmentItemData
    {
        /// <summary> ���� </summary>
        public override Item CreateItem()
        {
            //string _id = UniqueIDGenerator.GenerateUnqiueIDDateTime(Name);
            var item = new AccessoryItem(this, intID.ToString());
            //ResourceManager.Instance.MakeRandomStats(item);
            return item;
        }
    }
}
