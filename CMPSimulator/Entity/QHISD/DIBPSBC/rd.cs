using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMPSimulator.Entity.QueryReturnEntity.DIBPSBC
{
    [XmlRootAttribute("rd", IsNullable = false)]
    public class rd
    {
        [XmlElementAttribute("ECFlag")]
        public string ECFlag { get; set; }
        [XmlElementAttribute("PaySysBnkCode")]
        public string PaySysBnkCode { get; set; }
        [XmlElementAttribute("EISBnkCode")]
        public string EISBnkCode { get; set; }
        [XmlElementAttribute("EISSiteCode")]
        public string EISSiteCode { get; set; }
        [XmlElementAttribute("BnkName")]
        public string BnkName { get; set; }
        [XmlElementAttribute("RepReserved3")]
        public string RepReserved3 { get; set; }
        [XmlElementAttribute("RepReserved4")]
        public string RepReserved4 { get; set; }
    }
}
