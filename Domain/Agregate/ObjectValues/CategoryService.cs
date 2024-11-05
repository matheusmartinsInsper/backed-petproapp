namespace app.Domain.Agregate.ObjectValues
{
    public class CategoryService
    {
        private Dictionary<string,object> _categorys = new Dictionary<string, object>();
        private void setCategorys()
        { 
            _categorys.Clear();
            _categorys.Add("C01", "Higiene");
            _categorys.Add("C02", "Estética");
            _categorys.Add("C03", "Consulta");
            _categorys.Add("C04", "Vacinação");
            _categorys.Add("C05", "Castração");
            _categorys.Add("C06", "Exame");
            _categorys.Add("C07", "Cirurgia");
            _categorys.Add("C08", "Vermifugação");
        }
        public bool validCategory(string category)
        {
            setCategorys();
            return _categorys.ContainsKey(category);
        }
        public object GetCategory(string category)
        {
            if (validCategory(category))
            {
                return _categorys.GetValueOrDefault(category);
            }
            throw new Exception("this code not existe in the categorys");
        }
    }
}
