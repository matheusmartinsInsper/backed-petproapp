using app.Domain.DTO.Item;

namespace app.Domain.Agregate.Entities
{
    public class Item
    {
        private string _itemid;
        private string _iduser;
        private string _category;
        private string _description;
        private string _name;
        private List<ItemSize> _sizes;
        private string _unity;
        private DateTime _datecreate;
        private List<string> categoriesvalid = new List<string>()
        {
            "Medicamento",
            "Brinquedos",
            "Alimentos",
            "Vacina",
            "Insumos",
            "Vestimenta"
        };
        public List<ItemSize> sizes { get { return _sizes; } }
        public string itemid { get { return _itemid; } }
        public string iduser { get { return _iduser; } }
        public string category { get { return _category; } }
        public string description { get { return _description; } }
        public string name { get { return _name; } }
        public string unity { get { return _unity; } }
        public DateTime datecreate { get { return _datecreate; } }
        private Item() { }
        public static Item create(ItemDTO itemdto, string iduser)
        {
            Item item = new Item();
            item._sizes = new List<ItemSize>();
            item._datecreate = DateTime.Now;
            item._category = item.categoriesvalid.Find((x) => x == itemdto.category).First()!=null?itemdto.category:throw new Exception("Categoria invalida");
            item._name = itemdto.name;
            item._description = itemdto.description;   
            item._unity = itemdto.unity;
            item._iduser = iduser;
            item._itemid = Guid.NewGuid().ToString("N");
            foreach(ItemSizeDTO sizedto in itemdto.sizes)
            {
                ItemSize size = ItemSize.create(sizedto, item.itemid);
                item._sizes.Add(size);
            }
            return item;
        }
        public static Item restore(ItemDTODb itemdto,List<ItemSizeDTODb> sizes)
        {
            Item item = new Item();
            item._sizes = new List<ItemSize>();
            item._datecreate = itemdto.datecreate;
            item._category = itemdto.category;
            item._name = itemdto.name;
            item._description = itemdto.description;
            item._unity = itemdto.unity;
            item._iduser = itemdto.iduser;
            item._itemid = itemdto.itemid;
            foreach (ItemSizeDTODb sizedto in sizes)
            {
                ItemSize size = ItemSize.restore(sizedto);
                item._sizes.Add(size);
            }
            return item;
        }
        public void avalaibleItem(string iditemsize)
        {
            ItemSize itemsize = _sizes.FirstOrDefault((x) => x.iditemsize == iditemsize);
            itemsize.Avalaible();
        }
        public void unavalaibleItem(string iditemsize)
        {
            ItemSize itemsize = _sizes.FirstOrDefault((x) => x.iditemsize == iditemsize);
            itemsize.Unavalaible();
        }
    }
    public class ItemSize
    {
        private string _iditem;
        private string _iditemsize;
        private string _size;
        private bool _avalaible;
        private float _price;
        public string iditemsize { get { return _iditemsize; } }
        public string iditem { get { return _iditem; } }
        public string size { get { return _size; } }
        public bool avalaible { get { return _avalaible; } }
        public float price { get { return _price; } }
        private ItemSize() { }
        public static ItemSize create(ItemSizeDTO itemsizedto, string iditem)
        {
            ItemSize itemsize = new ItemSize();
            itemsize._iditem = iditem;
            itemsize._iditemsize = Guid.NewGuid().ToString("N");
            itemsize._size = itemsizedto.size;
            itemsize._price = itemsizedto.price;
            itemsize._avalaible = itemsizedto.avalaible;
            return itemsize;
        }
        public static ItemSize restore(ItemSizeDTODb itemsizedto)
        {
            ItemSize itemsize = new ItemSize();
            itemsize._iditem = itemsizedto.iditem;
            itemsize._iditemsize = itemsizedto.iditemsize;
            itemsize._size = itemsizedto.size;
            itemsize._price = itemsizedto.price;
            itemsize._avalaible = itemsizedto.avalaible;
            return itemsize;
        }
        public void Avalaible()
        {
            _avalaible = true;
        }

        public void Unavalaible()
        {
            _avalaible = false;
        }
    }
}
