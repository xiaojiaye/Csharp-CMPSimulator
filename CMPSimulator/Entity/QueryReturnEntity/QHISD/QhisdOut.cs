using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMPSimulator.Entity.QueryReturnEntity.QHISD
{
    [XmlRootAttribute("out")]
    public class QhisdOut
    {
        [XmlElementAttribute("AccNo")]
        public string AccNo { get; set; }
        [XmlElementAttribute("AccName")]
        public string AccName { get; set; }
        [XmlElementAttribute("CurrType")]
        public string CurrType { get; set; }
        [XmlElementAttribute("BeginDate")]
        public string BeginDate { get; set; }
        [XmlElementAttribute("EndDate")]
        public string EndDate { get; set; }
        [XmlElementAttribute("MinAmt")]
        public string MinAmt { get; set; }
        [XmlElementAttribute("MaxAmt")]
        public string MaxAmt { get; set; }
        [XmlElementAttribute("BankType")]
        public string BankType { get; set; }
        [XmlElementAttribute("NextTag")]
        public string NextTag { get; set; }
        [XmlElementAttribute("TotalNum")]
        public string TotalNum { get; set; }
        [XmlElementAttribute("DueBillNo")]
        public string DueBillNo { get; set; }
        [XmlElementAttribute("AcctSeq")]
        public string AcctSeq { get; set; }

        [XmlArrayAttribute("rds")]
        public List<rd> listQhisdRd { get; set; }  // 注意，xml中的标签rd一定要和 rd 类名称相符，否则不能转换为对象，层在此吃亏。
    }
}
