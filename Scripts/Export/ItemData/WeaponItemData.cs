using UnityEngine;

namespace Litkey.InventorySystem
{
    /// <summary> ��� - ���� ������ </summary>
    [CreateAssetMenu(fileName = "Item_Weapon_", menuName = "Inventory System/Item Data/Weaopn")]
    public class WeaponItemData : EquipmentItemData
    {
        /// <summary> ���ݷ� </summary>

        public override Item CreateItem()
        {
            //string _id = UniqueIDGenerator.GenerateUnqiueIDDateTime(Name);
            var item = new WeaponItem(this, intID.ToString());

            return item;
        }

        
    }
}