

namespace Litkey.InventorySystem
{
    public interface ICraftable
    {

    }
    /// <summary> ���� ������ - ���� ������ </summary>
    public class CraftItem : CountableItem
    {
        public CraftItem(CraftItemData data, string id, int amount = 1) : base(data, id, amount) { }

        public bool Use()
        {
            // �ӽ� : ���� �ϳ� ����
            //Amount--;

            return true;
        }

        protected override CountableItem Clone(int amount)
        {
            return new CraftItem(CountableData as CraftItemData, ID, amount);
        }

    }
}
