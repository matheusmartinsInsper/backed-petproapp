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
        private List<valueInstanceForm> _valueinstances;
        public string iduser { get { return _iduser; } }
        public string idform { get { return _idform; } }
        public string nameform { get { return _nameform; } }
        public string color { get { return _color; } }
        public List<attribute> attributes { get { return _attribute; } }
        public List<valueInstanceForm> valueInstanceForms { get { return _valueinstances; } }
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
        public static Form restore(FormDbDTO formdto, List<valueInstanceForm> instances)
        {
            List<valueInstanceForm> values = new List<valueInstanceForm>();
            values = instances.ToList();
            Form form = new Form();
            form._idform = formdto.idform;
            form._nameform = formdto.nameform;
            form._attribute = formdto.attributes;
            form._color = formdto.color;
            form._valueinstances = values;
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
        public void createInstanceForm(List<valueInstanceFormCreate> valuesformcreate)
        {
            _valueinstances = new List<valueInstanceForm>();
            foreach (valueInstanceFormCreate valueform in valuesformcreate)
            {
                if (valueform.formid != _idform)
                    throw new Exception("Esse campo não pertence a esse formulario");
                valueInstanceForm instance = new valueInstanceForm();
                instance.idattendance = valueform.idattendance;
                instance.valueid = Guid.NewGuid().ToString("N");
                instance.formid = valueform.formid;
                if (_attribute.Where((attribute) => attribute.idattribute == valueform.attributeid).Count() == 0)
                    throw new Exception("Atributo não encontrado no formulario base");
                foreach(attribute attribute in _attribute)
                {
                    if (attribute.typeattribute == "checkbox" || attribute.typeattribute == "select")
                    {
                        //if (attribute.options.Where((option) => option.idoption == valueform.optionid).Count() == 0)
                        //    throw new Exception("option nao valido");
                        instance.optionid = valueform.optionid;
                    }
                }
                instance.attributeid = valueform.attributeid;
                instance.optionid = valueform.optionid;
                instance.valuenumber = valueform.valuenumber;  
                instance.valuetext = valueform.valuetext;
                instance.isselected = valueform.isselected;
                _valueinstances.Add(instance);
            }
        }
        public List<valueInstanceForm> getFormInstanceByAttendance(string idattendance)
        {
            return _valueinstances.Where((instance)=>instance.idattendance==idattendance).ToList();
        }
        public List<attribute> MapValuesToAttributes(string idattendance)
        {
            List<valueInstanceForm> filteredValues = getFormInstanceByAttendance(idattendance);
            List<attribute> attributes = new List<attribute>();

            foreach (valueInstanceForm value in filteredValues)
            {
                attribute existingAttribute = attributes.FirstOrDefault(a => a.idattribute == value.attributeid);

                if (existingAttribute == null)
                {
                    attribute newAttribute = new attribute();
                    attribute originalAttribute = _attribute.First(at => at.idattribute == value.attributeid);

                    newAttribute.label = originalAttribute.label;
                    newAttribute.idattribute = value.attributeid;
                    newAttribute.idform = value.formid;
                    newAttribute.typeattribute = originalAttribute.typeattribute;
                    newAttribute.options = new List<Option>();

                    if (originalAttribute.typeattribute == "text" || originalAttribute.typeattribute == "textarea")
                    {
                        newAttribute.value = value.valuetext;
                    }
                    else if (originalAttribute.typeattribute == "number")
                    {
                        newAttribute.value = value.valuenumber;
                    }
                    attributes.Add(newAttribute);
                    existingAttribute = newAttribute;  
                }

                if (existingAttribute.typeattribute == "checkbox" || existingAttribute.typeattribute == "select")
                {
                    Option option = _attribute
                        .First(at => at.idattribute == value.attributeid)
                        .options.FirstOrDefault(opt => opt.idoption == value.optionid);

                    if (option != null)
                    {
                        // Clonar a opção e configurar a seleção
                        Option optionClone = new Option
                        {
                            idoption = option.idoption,
                            value = option.value,
                            idattribute = option.idattribute,
                            isselected = value.isselected
                        };

                        existingAttribute.options.Add(optionClone);
                    }
                }
            }

            return attributes;
        }


    }
    public class attribute
    {
        public string label { get; set; }
        public string idattribute { get; set; }
        public string idform { get; set; }
        public string typeattribute { get; set; }
        public object value { get; set; }
        public List<Option> options { get; set; }
    }
    public class Option
    {
        public string value { get; set; }
        public string idoption { get; set; }
        public string idattribute { get; set; }
        public bool isselected { get; set; }    
    }
    public class valueInstanceForm
    {
        public string valueid { get; set; }
        public string formid { get; set; }
        public string attributeid { get; set; }
        public string optionid { get; set; }
        public string valuetext { get; set; }
        public float valuenumber { get; set; }
        public bool isselected { get; set; }
        public string idattendance { get; set; }
    }
    public class valueInstanceFormCreate
    {
        public string idattendance { get; set; }
        public string formid { get; set; }
        public string attributeid { get; set; }
        public string optionid { get; set; }
        public string valuetext { get; set; }
        public float valuenumber { get; set; }
        public bool isselected { get; set; }
       
    }
}
