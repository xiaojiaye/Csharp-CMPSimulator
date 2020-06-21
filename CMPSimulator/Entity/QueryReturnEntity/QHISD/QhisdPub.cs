using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMPSimulator.Entity.QueryReturnEntity.QHISD
{
    [XmlRootAttribute("pub")]
    public class QhisdPub
    {
        [XmlElementAttribute("TransCode")]
        public string TransCode { get; set; }
        [XmlElementAttribute("CIS")]
        public string CIS { get; set; }
        [XmlElementAttribute("BankCode")]
        public string BankCode { get; set; }
        [XmlElementAttribute("ID")]
        public string ID { get; set; }
        [XmlElementAttribute("TranDate")]
        public string TranDate { get; set; }
        [XmlElementAttribute("TranTime")]
        public string TranTime { get; set; }
        [XmlElementAttribute("fSeqno")]
        public string fSeqno { get; set; }
        [XmlElementAttribute("RetCode")]
        public string RetCode { get; set; }
        [XmlElementAttribute("RetMsg")]
        public string RetMsg { get; set; }
    }
}
