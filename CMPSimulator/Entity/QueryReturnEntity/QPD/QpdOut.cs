using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMPSimulator.Entity.QueryReturnEntity
{
    [XmlRootAttribute("out")]
    public class QpdOut
    {
        [XmlElementAttribute("AccNo")]
        public string AccNo { get; set; }
        [XmlElementAttribute("AccName")]
        public string AccName { get; set; }
        [XmlElementAttribute("CurrType")]
        public string CurrType { get; set; }
        [XmlElementAttribute("AreaCode")]
        public string AreaCode { get; set; }
        [XmlElementAttribute("NextTag")]
        public string NextTag { get; set; }
        [XmlElementAttribute("TotalNum")]
        public string TotalNum { get; set; }
        [XmlElementAttribute("RepReserved1")]
        public string RepReserved1 { get; set; }
        [XmlElementAttribute("RepReserved2")]
        public string RepReserved2 { get; set; }
        [XmlElementAttribute("AcctSeq")]
        public string AcctSeq { get; set; }

        [XmlArrayAttribute("rds")]
        public List<rd> listQpdRd { get; set; }  // 注意，xml中的标签rd一定要和 rd 类名称相符，否则不能转换为对象，层在此吃亏。
        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<AccNo>"+AccNo+"</AccNo><AccName>"+AccName+"</AccName><CurrType>"+CurrType+"</CurrType><AreaCode>"+AreaCode+"</AreaCode>");
        //    sb.Append("<NextTag>"+NextTag+"</NextTag><TotalNum>"+TotalNum+"</TotalNum><RepReserved1>"+RepReserved1+"</RepReserved1><RepReserved2>"+RepReserved2+"</RepReserved2><AcctSeq>"+AcctSeq+"</AcctSeq>");
        //    return sb.ToString();
        //}
    }
}
