using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMPSimulator.Entity.QueryTypeEntity
{
    public class ObjectQpd0010:PubHeader
    {
        public string AccNo { get; set; }

        public string AreaCode { get; set; }

        public string MinAmt { get; set; }

        public string MaxAmt { get; set; }

        public string BeginTime { get; set; }

        public string EndTime { get; set; }

        public string NextTag { get; set; }

        public string ReqReserved1 { get; set; }

        public string ReqReserved2 { get; set; }

        public string CurrType { get; set; }

        public string AcctSeq { get; set; }
    }
}
