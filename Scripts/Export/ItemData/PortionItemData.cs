using UnityEngine;
using Litkey.Stat;

namespace Litkey.InventorySystem
{
    /// <summary> �Һ� ������ ���� </summary>
    [CreateAssetMenu(fileName = "Item_Portion_", menuName = "Inventory System/Item Data/Portion")]
    public class PortionItemData : CountableItemData
    {
        /// <summary> ȿ����(ȸ���� ��) </summary>
        [SerializeField] protected StatModifier[] stats;

        public override Item CreateItem()
        {
            //string _id = UniqueIDGenerator.GenerateUnqiueIDDateTime(Name);
            return new PortionItem(this, intID.ToString());
        }

        public override ItemData GetReward()
        {
            return this;
        }

        public StatModifier[] GetStats()
        {
            return stats;
        }

    }
}