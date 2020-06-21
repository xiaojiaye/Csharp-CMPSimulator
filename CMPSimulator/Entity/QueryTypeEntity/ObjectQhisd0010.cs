using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMPSimulator.Entity.QueryTypeEntity
{
    /// <summary>
    /// QPD 0.0.1.0 提交包的 实体类，用于初始化时，作为对象参数传输
    /// </summary>
    public class ObjectQhisd0010 : PubHeader
    {
        public string AccNo { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public string MinAmt { get; set; }

        public string MaxAmt { get; set; }

        public string BankType { get; set; }

        public string NextTag { get; set; }

        public string CurrType { get; set; }

        public string DueBillNo { get; set; }

        public string AcctSeq { get; set; }

        public string ComplementFlag { get; set; }

        public string CardAccNoDef { get; set; }

        public string DesByTime { get; set; }
    }
}
