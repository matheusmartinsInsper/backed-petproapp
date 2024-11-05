namespace app.Domain.Agregate.ObjectValues
{
    public class VaccineService
    {
        private Dictionary<string, string> _categorys = new Dictionary<string, string>();
        private void setVaccinesCode()
        {
            _categorys.Clear();
            _categorys.Add("V01", "Vacina antiraiva");
            _categorys.Add("V02", "Vacina tantrica");
            _categorys.Add("V03", "Vacina contra raiva");
            _categorys.Add("V04", "Vacina contra queda de cabelo");
            _categorys.Add("V05", "Vacina da gripe");
        }
        public bool validCategory(string category)
        {
            setVaccinesCode();
            return _categorys.ContainsKey(category);
        }
        public string GetVaccine(string category)
        {
            if (validCategory(category))
            {
                return _categorys.GetValueOrDefault(category);
            }
            throw new Exception("this code not existe in the categorys");
        }
    }
}
