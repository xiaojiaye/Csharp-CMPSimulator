using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMPSimulator.Entity.QueryReturnEntity.QHISD
{
    // srcXml 中的字段个数可以比rd中的属性多，没有的就不会解析。
    // 属性类型都要是string，都要有[XmlElementAttribute("Drcrf")]说明
    // 其中有一个int或者没有xml注解，就好string 转 classObject 失败
    [XmlRootAttribute("rd", IsNullable = false)]
    public class rd
    {
        [XmlElementAttribute("Drcrf")]
        public string Drcrf { get; set; }
        [XmlElementAttribute("VouhNo")]
        public string VouhNo { get; set; }
        [XmlElementAttribute("DebitAmount")]
        public string DebitAmount { get; set; }
        [XmlElementAttribute("CreditAmount")]
        public string CreditAmount { get; set; }
        [XmlElementAttribute("Balance")]
        public string Balance { get; set; }
        [XmlElementAttribute("RecipBkNo")]
        public string RecipBkNo { get; set; }
        [XmlElementAttribute("RecipBkName")]
        public string RecipBkName { get; set; }
        [XmlElementAttribute("RecipAccNo")]
        public string RecipAccNo { get; set; }
        [XmlElementAttribute("RecipName")]
        public string RecipName { get; set; }
        [XmlElementAttribute("Summary")]
        public string Summary { get; set; }
        [XmlElementAttribute("UseCN")]
        public string UseCN { get; set; }
        [XmlElementAttribute("PostScript")]
        public string PostScript { get; set; }
        [XmlElementAttribute("BusCode")]
        public string BusCode { get; set; }
        [XmlElementAttribute("Date")]
        public string Date { get; set; }
        [XmlElementAttribute("Time")]
        public string Time { get; set; }
        [XmlElementAttribute("Ref")]
        public string Ref { get; set; }
        [XmlElementAttribute("Oref")]
        public string Oref { get; set; }
        [XmlElementAttribute("EnSummary")]
        public string EnSummary { get; set; }
        [XmlElementAttribute("BusType")]
        public string BusType { get; set; }
        [XmlElementAttribute("VouhType")]
        public string VouhType { get; set; }
        [XmlElementAttribute("AddInfo")]
        public string AddInfo { get; set; }
        [XmlElementAttribute("Toutfo")]
        public string Toutfo { get; set; }
        [XmlElementAttribute("OnlySequence")]
        public string OnlySequence { get; set; }
        [XmlElementAttribute("AgentAcctName")]
        public string AgentAcctName { get; set; }
        [XmlElementAttribute("AgentAcctNo")]
        public string AgentAcctNo { get; set; }
        [XmlElementAttribute("UpDtranf")]
        public string UpDtranf { get; set; }
        [XmlElementAttribute("ValueDate")]
        public string ValueDate { get; set; }

        [XmlElementAttribute("TrxCode")]
        public string TrxCode { get; set; }
        [XmlElementAttribute("Ref1")]
        public string Ref1 { get; set; }
        [XmlElementAttribute("Oref1")]
        public string Oref1 { get; set; }
        [XmlElementAttribute("CASHF")]
        public string CASHF { get; set; }
        [XmlElementAttribute("BusiDate")]
        public string BusiDate { get; set; }
        [XmlElementAttribute("BusiTime")]
        public string BusiTime { get; set; }
        //[XmlElementAttribute("SeqNo")]
        public string SeqNo { get; set; }
        [XmlElementAttribute("MgNo")]
        public string MgNo { get; set; }
        [XmlElementAttribute("MgAccNo")]
        public string MgAccNo { get; set; }
        [XmlElementAttribute("MgCurrType")]
        public string MgCurrType { get; set; }
        [XmlElementAttribute("CashExf")]
        public string CashExf { get; set; }
        [XmlElementAttribute("DetailNo")]
        public string DetailNo { get; set; }
        [XmlElementAttribute("Remark")]
        public string Remark { get; set; }
        [XmlElementAttribute("TradeTime")]
        public string TradeTime { get; set; }
        [XmlElementAttribute("TradeFee")]
        public string TradeFee { get; set; }
        [XmlElementAttribute("TradeLocation")]
        public string TradeLocation { get; set; }
        [XmlElementAttribute("ExRate")]
        public string ExRate { get; set; }
        [XmlElementAttribute("ForCurrtype")]
        public string ForCurrtype { get; set; }
        [XmlElementAttribute("EnAbstract")]
        public string EnAbstract { get; set; }
        [XmlElementAttribute("OpenBankNo")]
        public string OpenBankNo { get; set; }
        [XmlElementAttribute("OpenBankBIC")]
        public string OpenBankBIC { get; set; }
        [XmlElementAttribute("OpenBankName")]
        public string OpenBankName { get; set; }
        [XmlElementAttribute("SubAcctSeq")]
        public string SubAcctSeq { get; set; }
        [XmlElementAttribute("THCurrency")]
        public string THCurrency { get; set; }
        [XmlElementAttribute("RecipBkName1")]
        public string RecipBkName1 { get; set; }
        [XmlElementAttribute("RecipBkNo1")]
        public string RecipBkNo1 { get; set; }
        [XmlElementAttribute("TInfoNew")]
        public string TInfoNew { get; set; }
        [XmlElementAttribute("RefundMsg")]
        public string RefundMsg { get; set; }
        [XmlElementAttribute("BusTypNo")]
        public string BusTypNo { get; set; }
        [XmlElementAttribute("ReceiptInfo")]
        public string ReceiptInfo { get; set; }
        [XmlElementAttribute("TelNo")]
        public string TelNo { get; set; }
        [XmlElementAttribute("MDCARDNO")]
        public string MDCARDNO { get; set; }
        [XmlElementAttribute("TrxAmt")]
        public string TrxAmt { get; set; }
        [XmlElementAttribute("TrxCurr")]
        public string TrxCurr { get; set; }
        [XmlElementAttribute("CurrType")]
        public string CurrType { get; set; }
    }
}
