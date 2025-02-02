

namespace Litkey.InventorySystem
{
    public interface ICraftable
    {

    }
    /// <summary> 수량 아이템 - 포션 아이템 </summary>
    public class CraftItem : CountableItem
    {
        public CraftItem(CraftItemData data, string id, int amount = 1) : base(data, id, amount) { }

        public bool Use()
        {
            // 임시 : 개수 하나 감소
            //Amount--;

            return true;
        }

        protected override CountableItem Clone(int amount)
        {
            return new CraftItem(CountableData as CraftItemData, ID, amount);
        }

    }
}
