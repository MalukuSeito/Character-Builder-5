using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Character_Builder_5
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
