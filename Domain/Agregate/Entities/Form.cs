using app.Domain.DTO.Form;
using app.WebUI.DTO;

namespace app.Domain.Agregate.Entities
{
    public class Form
    {
        private string _iduser;
        private string _nameform;
        private string _idform;
        private string _color;
        private List<attribute> _attribute;
        public string iduser { get { return _iduser; } }
        public string idform { get { return _idform; } }
        public string nameform { get { return _nameform; } }
        public string color { get { return _color; } }
        public List<attribute> attributes { get { return _attribute; } }
        private Form() { }
        public static Form create(FormDTO formdto,string iduser)
        {
            Form form = new Form();
            form._idform = Guid.NewGuid().ToString("N");
            form._iduser = iduser;
            form._nameform = formdto.nameform;
            form._color = formdto.color;
            form._attribute = form.generateIdForAttribute(formdto.fields,form);
            return form;
        }
        public static Form restore(FormDbDTO formdto)
        {
            Form form = new Form();
            form._idform = formdto.idform;
            form._nameform = formdto.nameform;
            form._attribute = formdto.attributes;
            form._color = formdto.color;
            return form;
        }
        private List<attribute> generateIdForAttribute(List<Fields> fields, Form form)
        {
            List<attribute> attributes = new List<attribute>(); 
            foreach(Fields field in fields)
            {
                attribute att = new attribute();
                att.idform = _idform;
                att.idattribute = Guid.NewGuid().ToString("N");
                att.label = field.label;
                att.typeattribute = field.type;
                att.options = form.generateIdForOption(field.options,att.idattribute);
                attributes.Add(att);
            }
            return attributes;
        }
        private List<Option> generateIdForOption(List<string> options,string idattribute)
        {
            List<Option> opp = new List<Option>();
            foreach(string option in options)
            {
                Option optionofatt = new Option();
                optionofatt.idattribute = idattribute;
                optionofatt.value = option;
                optionofatt.idoption = Guid.NewGuid().ToString("N");
                opp.Add(optionofatt);
            }
            return opp;
        }
    }
    public class attribute
    {
        public string label { get; set; }
        public string idattribute { get; set; }
        public string idform { get; set; }
        public string typeattribute { get; set; }
        public List<Option> options { get; set; }
    }
    public class Option
    {
        public string value { get; set; }
        public string idoption { get; set; }
        public string idattribute { get; set; }
    }
}
