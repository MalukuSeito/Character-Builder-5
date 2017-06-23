using System;
using System.Xml.Serialization;

namespace Character_Builder
{
    //public enum FieldType
    // {
    //    Textfield, Box
    //}
    public class PDFField
    {
        [XmlAttribute]
        public String Name { get; set; }
        [XmlAttribute]
        public String Field { get; set; }
        //[XmlAttribute]
        //FieldType Type { get; set; }
        public PDFField() { }
        public PDFField(string name, string field) 
        {
            Name = name;
            Field = field;
        }
    }
}
