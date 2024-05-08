

namespace Litkey.InventorySystem
{
    /*
        [��� ����]
        Item : �⺻ ������
            - EquipmentItem : ��� ������
            - CountableItem : ������ �����ϴ� ������
    */
    [System.Serializable]
    public abstract class Item
    {
        public ItemData Data { get; private set; }
        public string ID => _id; // ����ũ ���̵�

        private string _id;
        public Item(ItemData data, string id)
        {
            Data = data;
            this._id = id; 
        }


    }


}