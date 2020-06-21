using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMPSimulator.Entity.QueryReturnEntity.DIBPSBC
{
    [XmlRootAttribute("out")]
    public class DibpsbcOut
    {
        [XmlElementAttribute("BnkCode")]
        public string BnkCode { get; set; }
        [XmlElementAttribute("NextTag")]
        public string NextTag { get; set; }
        [XmlElementAttribute("RepReserved1")]
        public string RepReserved1 { get; set; }
        [XmlElementAttribute("RepReserved2")]
        public string RepReserved2 { get; set; }

        [XmlArrayAttribute("rds")]
        public List<rd> listDibpsbcRd { get; set; }  // 注意，xml中的标签rd一定要和 rd 类名称相符，否则不能转换为对象，层在此吃亏。
    }
}
