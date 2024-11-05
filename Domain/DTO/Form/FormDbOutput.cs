namespace app.Domain.DTO.Form
{
    public class FormDbOutput
    {
            public string formid { get; set; }  
            public string color { get; set; }
            public string formname { get; set; }     // Correspondente a f.formname
            public string attributeid { get; set; }     // Correspondente a a.attributeid
            public string attributename { get; set; } // Correspondente a a.attributename
            public string attributetype { get; set; } // Correspondente a a.attributetype
            public string optionid { get; set; }        // Correspondente a o.optionid
            public string optionvalue { get; set; }  
    }
}
