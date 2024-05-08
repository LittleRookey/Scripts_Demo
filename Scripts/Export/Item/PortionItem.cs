namespace Litkey.InventorySystem
{
    /// <summary> ���� ������ - ���� ������ </summary>
    public class PortionItem : CountableItem, IUsableItem
    {
        public PortionItemData PortionItemData { get; private set; }

        public PortionItem(PortionItemData data, string id, int amount = 1) : base(data, id, amount) 
        {
            PortionItemData = data;
        }

        public bool Use()
        {
            if (Amount <= 0) return false;
            // �ӽ� : ���� �ϳ� ����
            Amount -= 1;

            return true;
        }

        protected override CountableItem Clone(int amount)
        {
            return new PortionItem(CountableData as PortionItemData, ID, amount);
        }


    }
}