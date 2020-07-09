using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMPSimulator.Entity.QueryReturnEntity.QRYGJDTL
{
    // srcXml 中的字段个数可以比rd中的属性多，没有的就不会解析。
    // 属性类型都要是string，都要有[XmlElementAttribute("Drcrf")]说明
    // 其中有一个int或者没有xml注解，就好string 转 classObject 失败
    [XmlRootAttribute("rd", IsNullable = false)]
    public class rd
    {
        [XmlElementAttribute("GCardNo")]
        public string GCardNo { get; set; }
        [XmlElementAttribute("GCardName")]
        public string GCardName { get; set; }
        [XmlElementAttribute("RciAcctNo")]
        public string RciAcctNo { get; set; }
        [XmlElementAttribute("RciAcctName")]
        public string RciAcctName { get; set; }
        [XmlElementAttribute("Amt")]
        public string Amt { get; set; }
        [XmlElementAttribute("WordDate")]
        public string WordDate { get; set; }
        [XmlElementAttribute("GContractNo")]
        public string GContractNo { get; set; }
        [XmlElementAttribute("GCardType")]
        public string GCardType { get; set; }
        [XmlElementAttribute("CurrType")]
        public string CurrType { get; set; }
        [XmlElementAttribute("CardLvl")]
        public string CardLvl { get; set; }
        [XmlElementAttribute("SuperiorAcct")]
        public string SuperiorAcct { get; set; }
        [XmlElementAttribute("SuperiorName")]
        public string SuperiorName { get; set; }
        [XmlElementAttribute("TranSerialno")]
        public string TranSerialno { get; set; }
        [XmlElementAttribute("TimeStmp")]
        public string TimeStmp { get; set; }
        [XmlElementAttribute("Summary")]
        public string Summary { get; set; }
        [XmlElementAttribute("CardBal")]
        public string CardBal { get; set; }
        [XmlElementAttribute("PostScript")]
        public string PostScript { get; set; }
    }
}
