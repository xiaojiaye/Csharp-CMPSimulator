using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMPSimulator.Entity.QueryReturnEntity.QRYGJDTL
{
    [XmlRootAttribute("out")]
    public class QrygjdtlOut
    {
        [XmlElementAttribute("AcctName")]
        public string AcctName { get; set; }
        [XmlElementAttribute("AcctNo")]
        public string AcctNo { get; set; }
        [XmlElementAttribute("CurrType")]
        public string CurrType { get; set; }
        [XmlElementAttribute("StartDate")]
        public string StartDate { get; set; }
        [XmlElementAttribute("EndDate")]
        public string EndDate { get; set; }
        [XmlElementAttribute("NextTag")]
        public string NextTag { get; set; }
        [XmlElementAttribute("AcctSeq")]
        public string AcctSeq { get; set; }
        [XmlArrayAttribute("rds")]
        public List<rd> listQrygjtlRd { get; set; }
    }
}
