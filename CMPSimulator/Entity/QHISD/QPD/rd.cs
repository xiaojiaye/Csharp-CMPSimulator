using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CMPSimulator.Entity.QueryReturnEntity
{
    [XmlRootAttribute("rd", IsNullable = false)]
    public class rd
    {
        [XmlElementAttribute("Drcrf")]
        public string Drcrf { get; set; }
        [XmlElementAttribute("VouhNo")]
        public string VouhNo { get; set; }
        [XmlElementAttribute("Amount")]
        public string Amount { get; set; }
        [XmlElementAttribute("RecipBkNo")]
        public string RecipBkNo { get; set; }
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
        [XmlElementAttribute("Ref")]
        public string Ref { get; set; }
        [XmlElementAttribute("BusCode")]
        public string BusCode { get; set; }
        [XmlElementAttribute("Oref")]
        public string Oref { get; set; }
        [XmlElementAttribute("EnSummary")]
        public string EnSummary { get; set; }
        [XmlElementAttribute("BusType")]
        public string BusType { get; set; }
        [XmlElementAttribute("CvouhType")]
        public string CvouhType { get; set; }
        [XmlElementAttribute("AddInfo")]
        public string AddInfo { get; set; }
        [XmlElementAttribute("TimeStamp")]
        public string TimeStamp { get; set; }
        [XmlElementAttribute("RepReserved3")]
        public string RepReserved3 { get; set; }
        [XmlElementAttribute("RepReserved4")]
        public string RepReserved4 { get; set; }
        [XmlElementAttribute("UpDtranf")]
        public string UpDtranf { get; set; }
        [XmlElementAttribute("ValueDate")]
        public string ValueDate { get; set; }
        [XmlElementAttribute("TrxCode")]
        public string TrxCode { get; set; }
        [XmlElementAttribute("SequenceNo")]
        public string SequenceNo { get; set; }
        [XmlElementAttribute("Cashf")]
        public string Cashf { get; set; }
        [XmlElementAttribute("CASHEXF")]
        public string CASHEXF { get; set; }
        [XmlElementAttribute("Remark")]
        public string Remark { get; set; }
        [XmlElementAttribute("TradeDate")]
        public string TradeDate { get; set; }
        [XmlElementAttribute("TradeTime")]
        public string TradeTime { get; set; }
        [XmlElementAttribute("TradeLocation")]
        public string TradeLocation { get; set; }
        [XmlElementAttribute("TradeFee")]
        public string TradeFee { get; set; }
        [XmlElementAttribute("ExRate")]
        public string ExRate { get; set; }
        [XmlElementAttribute("ForCurrtype")]
        public string ForCurrtype { get; set; }
        [XmlElementAttribute("EnAbstract")]
        public string EnAbstract { get; set; }
        [XmlElementAttribute("RecBankName")]
        public string RecBankName { get; set; }
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
        [XmlElementAttribute("ReceiptInfo")]
        public string ReceiptInfo { get; set; }
        [XmlElementAttribute("OnlySequence")]
        public string OnlySequence { get; set; }
 
        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<Drcrf>"+Drcrf+"</Drcrf><VouhNo>"+VouhNo+"</VouhNo><Amount>"+Amount+"</Amount><RecipBkNo>"+RecipBkNo+"</RecipBkNo><RecipAccNo>"+RecipAccNo+"</RecipAccNo>");
        //    sb.Append("<RecipName>"+RecipName+"</RecipName><Summary>"+Summary+"</Summary><UseCN>"+UseCN+"</UseCN><PostScript>"+PostScript+"</PostScript><Ref>"+Ref+"</Ref><BusCode>"+BusCode+"</BusCode>");
        //    sb.Append("<Oref>"+Oref+"</Oref><EnSummary>"+EnSummary+"</EnSummary><BusType>"+BusType+"</BusType><CvouhType>"+CvouhType+"</CvouhType><AddInfo>"+AddInfo+"</AddInfo><TimeStamp>"+TimeStamp+"</TimeStamp>");
        //    sb.Append("<RepReserved3>"+RepReserved3+"</RepReserved3><RepReserved4>"+RepReserved4+"</RepReserved4><UpDtranf>"+UpDtranf+"</UpDtranf><ValueDate>"+ValueDate+"</ValueDate><TrxCode>"+TrxCode+"</TrxCode>");
        //    sb.Append("<SequenceNo>"+SequenceNo+"</SequenceNo><Cashf>"+Cashf+"</Cashf><CASHEXF>"+CASHEXF+"</CASHEXF><Remark>"+Remark+"</Remark><TradeDate>"+TradeDate+"</TradeDate><TradeTime>"+TradeTime+"</TradeTime>");
        //    sb.Append("<TradeLocation>"+TradeLocation+"</TradeLocation><TradeFee>"+TradeFee+"</TradeFee><ExRate>"+ExRate+"</ExRate><ForCurrtype>"+ForCurrtype+"</ForCurrtype><EnAbstract>"+EnAbstract+"</EnAbstract><RecBankName>"+RecBankName+"</RecBankName>");
        //    sb.Append("<OpenBankNo>"+OpenBankNo+"</OpenBankNo><OpenBankBIC>"+OpenBankBIC+"</OpenBankBIC><OpenBankName>"+OpenBankName+"</OpenBankName><SubAcctSeq>"+SubAcctSeq+"</SubAcctSeq><THCurrency>"+THCurrency+"</THCurrency><ReceiptInfo>"+ReceiptInfo+"</ReceiptInfo><OnlySequence>"+OnlySequence+"</OnlySequence>");
        //    return sb.ToString();
        //}
    }
}
