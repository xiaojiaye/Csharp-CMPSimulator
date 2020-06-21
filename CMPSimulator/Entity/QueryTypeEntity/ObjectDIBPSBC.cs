using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMPSimulator.Entity.QueryTypeEntity
{
    public class ObjectDIBPSBC : PubHeader
    {
        public string BnkCode { get; set; }  // 行别代码 , 3

        public string NextTag { get; set; }  // 查询下页标识， 60

        public string ReqReserved1 { get; set; }  // 是否查询跨行快汇行名行号，输入1为查询跨行快汇行名行号，不输入或输入0为非查询跨行快汇行名行号，
                                                  // 输入其他则报错B1612（是否查询跨行快汇行名行号上送有误） , 100

        public string ReqReserved2 { get; set; } // 备用，目前无意义  ,  100
    }
}
