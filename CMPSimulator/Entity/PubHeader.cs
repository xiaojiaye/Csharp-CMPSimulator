using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMPSimulator.Entity
{
    /// <summary>
    /// pub 部分
    /// </summary>
    public class PubHeader
    {
        public string TransCode { get; set; }
        public string CIS { get; set; }
        public string BankCode { get; set; }
        public string ID { get; set; }
        public string TranDate { get; set; }
        public string TranTime { get; set; }
        public string fSeqno { get; set; }
    }
}
