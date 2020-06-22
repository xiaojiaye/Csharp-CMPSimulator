using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMPSimulator.Entity
{
    public class BankNoAndBankName
    {
        private string BnkCode;  // 行别代码
        private string NextTag;  // 下页标识
        private string RepReserved1;  // 响应备用字段1
        private string RepReserved2;  // 响应备用字段2
        private string ECFlag;  // 跨行电联标志
        private string PaySysBnkCode;  // 支付系统行号
        private string EISBnkCode;  // 电子联行行号
        private string EISSiteCode;  // EIS小站号
        private string BnkName; // 银行名称
        private string RepReserved3;  // 响应备用字段3,    备用，目前无意义
        private string RepReserved4;  // 响应备用字段4,    格式:行别代码|行别名称（该字段仅针对电子票据）

        public override string ToString()
        {
            return this.BnkName + "`" + this.PaySysBnkCode + "`" + "\r\n";

        }

        public string BnkCode1
        {
            get
            {
                return BnkCode;
            }

            set
            {
                BnkCode = value;
            }
        }

        public string NextTag1
        {
            get
            {
                return NextTag;
            }

            set
            {
                NextTag = value;
            }
        }

        public string RepReserved11
        {
            get
            {
                return RepReserved1;
            }

            set
            {
                RepReserved1 = value;
            }
        }

        public string RepReserved21
        {
            get
            {
                return RepReserved2;
            }

            set
            {
                RepReserved2 = value;
            }
        }

        public string ECFlag1
        {
            get
            {
                return ECFlag;
            }

            set
            {
                ECFlag = value;
            }
        }

        public string PaySysBnkCode1
        {
            get
            {
                return PaySysBnkCode;
            }

            set
            {
                PaySysBnkCode = value;
            }
        }

        public string EISBnkCode1
        {
            get
            {
                return EISBnkCode;
            }

            set
            {
                EISBnkCode = value;
            }
        }

        public string EISSiteCode1
        {
            get
            {
                return EISSiteCode;
            }

            set
            {
                EISSiteCode = value;
            }
        }

        public string BnkName1
        {
            get
            {
                return BnkName;
            }

            set
            {
                BnkName = value;
            }
        }

        public string RepReserved31
        {
            get
            {
                return RepReserved3;
            }

            set
            {
                RepReserved3 = value;
            }
        }

        public string RepReserved41
        {
            get
            {
                return RepReserved4;
            }

            set
            {
                RepReserved4 = value;
            }
        }
        

    }
}
