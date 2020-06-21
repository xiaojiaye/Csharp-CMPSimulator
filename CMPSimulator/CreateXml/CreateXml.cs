using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMPSimulator.Entity;
using CMPSimulator.Entity.QueryTypeEntity;

namespace CMPSimulator.CreateXml
{
    public static class CreateXml
    {
        #region PAYENT相关XML生成方法
        /// <summary>
        /// 产生PAYENT0010且带zip节点的报文
        /// </summary>
        /// <param name="objPayent">objPayent</param>
        /// <returns></returns>
        public static string CreatePAYENTWithZip0010(ObjectPayent objPayent)
        {
            StringBuilder sbCreatePAYENTWithZip0010 = new StringBuilder();
            sbCreatePAYENTWithZip0010.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>" + objPayent.TransCode + "</TransCode><CIS>");
            sbCreatePAYENTWithZip0010.Append(objPayent.CIS + "</CIS><BankCode>102</BankCode><ID>" + objPayent.ID + "</ID><TranDate>");
            sbCreatePAYENTWithZip0010.Append(objPayent.TranDate + "</TranDate><TranTime>" + objPayent.TranTime + "</TranTime><fSeqno>" + objPayent.fSeqno + "</fSeqno></pub><in>");
            // <OnlBatF>表示联机标志，只能选择1；  <SettleMode>  入账方式  2表示并笔记账，0表示逐笔记账
            sbCreatePAYENTWithZip0010.Append("<OnlBatF>" + objPayent.OnlBatF + "</OnlBatF><SettleMode>" + objPayent.SettleMode + "</SettleMode><TotalNum>" + objPayent.TotalNum + "</TotalNum><TotalAmt>" + objPayent.TotalAmt + "</TotalAmt>");
            sbCreatePAYENTWithZip0010.Append("<SignTime>" + objPayent.SignTime + "</SignTime><ReqReserved1>" + objPayent.ReqReserved1 + "</ReqReserved1><ReqReserved2>" + objPayent.ReqReserved2 + "</ReqReserved2><zip>");
            string srcRds = CreateRdArea(objPayent);
            // 对循环区域压缩并base64编码
            string srcRdsZip = string.Empty;
            string srcRdsZipBase64 = string.Empty;
            //srcRdsZip = tools.ZipAndBase64.Compress(srcRds);
            //srcRdsZipBase64 = tools.Base64AndUnbase64.EncodeBase64("gb2312", srcRdsZip);
            srcRdsZipBase64 = tools.ZipHelper.GZipCompressString(srcRds);
            sbCreatePAYENTWithZip0010.Append(srcRdsZipBase64 + "</zip></in></eb></CMS>");
            return sbCreatePAYENTWithZip0010.ToString();
        }
        /// <summary>
        /// 产生PAYENT中循环区域的报文
        /// </summary>
        /// <param name="objPayent">objPayent</param>
        /// <returns></returns>
        public static string CreateRdArea(ObjectPayent objPayent)
        {
            StringBuilder sbRdArea = new StringBuilder();
            for (int i = 0; i < Int32.Parse(objPayent.TotalNum); i++)
            {
                sbRdArea.Append("<rd><iSeqno>" + (i + 1).ToString() + "</iSeqno><ReimburseNo></ReimburseNo><ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime>");
                // 1：加急 2：普通（工行异地人民币转账不再区分普通\加急,统一按加急处理） 3：跨行快汇（当涉及账户管家账户转账时，记账方式不支持普通，工行异地转账不再区分普通\加急,统一按加急处理）
                sbRdArea.Append("<PayType>" + objPayent.PayType + "</PayType><PayAccNo>" + objPayent.PayAccNo + "</PayAccNo><PayAccNameCN>" + objPayent.PayAccNameCN + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
                sbRdArea.Append("<RecAccNo>" + objPayent.RecAccNo + "</RecAccNo><RecAccNameCN>" + objPayent.RecAccNameCN + "</RecAccNameCN><RecAccNameEN></RecAccNameEN>");
                //  1：系统内  2：系统外            1：同城  2：异地        跨行必输  0：对公账户  1：个人账户
                sbRdArea.Append("<SysIOFlg>" + objPayent.SysIOFlg + "</SysIOFlg><IsSameCity>" + objPayent.IsSameCity + "</IsSameCity><Prop>" + objPayent.Prop + "</Prop>");
                sbRdArea.Append("<RecICBCCode></RecICBCCode><RecCityName>长沙</RecCityName><RecBankNo></RecBankNo>");
                sbRdArea.Append("<RecBankName>中国工商银行长沙某测试支行</RecBankName><CurrType>" + objPayent.CurrType + "</CurrType><PayAmt>1</PayAmt>");
                sbRdArea.Append("<UseCode></UseCode><UseCN>" + objPayent.UseCN + "</UseCN><EnSummary></EnSummary>");
                sbRdArea.Append("<PostScript>" + objPayent.PostScript + "</PostScript><Summary>" + objPayent.Summary + "</Summary><Ref>" + objPayent.Ref + "</Ref><Oref>" + objPayent.Oref + "</Oref>");
                sbRdArea.Append("<ERPSqn></ERPSqn><BusCode>001</BusCode><ERPcheckno></ERPcheckno>");
                sbRdArea.Append("<CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo>");
                sbRdArea.Append("<BankType>102</BankType><FileNames></FileNames><Indexs></Indexs><PaySubNo></PaySubNo><RecSubNo></RecSubNo>");
                sbRdArea.Append("<MCardNo>" + objPayent.MCardNo + "</MCardNo><MCardName>" + objPayent.MCardName + "</MCardName></rd>");
            }
            return sbRdArea.ToString();
        }
        /// <summary>
        /// 产生产生PAYENT0010报文，不带zip节点
        /// </summary>
        /// <param name="objPayent"></param>
        /// <returns></returns>
        public static string CreatePAYENT0010(ObjectPayent objPayent)
        {
            StringBuilder sbCreatePayent0010CustomizeCount = new StringBuilder();
            sbCreatePayent0010CustomizeCount.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>" + objPayent.TransCode + "</TransCode><CIS>");
            sbCreatePayent0010CustomizeCount.Append(objPayent.CIS + "</CIS><BankCode>102</BankCode><ID>" + objPayent.ID + "</ID><TranDate>");
            sbCreatePayent0010CustomizeCount.Append(objPayent.TranDate + "</TranDate><TranTime>" + objPayent.TranTime + "</TranTime><fSeqno>" + objPayent.fSeqno + "</fSeqno></pub><in>");
            // <OnlBatF>表示联机标志，只能选择1；  <SettleMode>  入账方式  2表示并笔记账，0表示逐笔记账
            sbCreatePayent0010CustomizeCount.Append("<OnlBatF>" + objPayent.OnlBatF + "</OnlBatF><SettleMode>" + objPayent.SettleMode + "</SettleMode><TotalNum>" + objPayent.TotalNum + "</TotalNum><TotalAmt>" + objPayent.TotalAmt + "</TotalAmt>");
            sbCreatePayent0010CustomizeCount.Append("<SignTime>" + objPayent.SignTime + "</SignTime><ReqReserved1>" + objPayent.ReqReserved1 + "</ReqReserved1><ReqReserved2>" + objPayent.ReqReserved2 + "</ReqReserved2>");
            string srcRds = CreateRdArea(objPayent);
            sbCreatePayent0010CustomizeCount.Append(srcRds + "</in></eb></CMS>");
            return sbCreatePayent0010CustomizeCount.ToString();
        }

        #endregion

        #region PAYPERCOL  0.0.1.0  报文生成方法
        public static string CreatePAYPERCOL0010(ObjectPaypercol0010 objPaypercol0010)
        {
            StringBuilder sbCreatePAYPERCOL0010 = new StringBuilder();
            sbCreatePAYPERCOL0010.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>" + objPaypercol0010.TransCode + "</TransCode><CIS>");
            sbCreatePAYPERCOL0010.Append(objPaypercol0010.CIS + "</CIS><BankCode>102</BankCode><ID>" + objPaypercol0010.ID + "</ID><TranDate>");
            sbCreatePAYPERCOL0010.Append(objPaypercol0010.TranDate + "</TranDate><TranTime>" + objPaypercol0010.TranTime + "</TranTime><fSeqno>" + objPaypercol0010.fSeqno + "</fSeqno></pub><in>");
            // <OnlBatF>表示联机标志，只能选择1；  <SettleMode>  入账方式  2表示并笔记账，0表示逐笔记账
            sbCreatePAYPERCOL0010.Append("<OnlBatF>" + objPaypercol0010.OnlBatF + "</OnlBatF><BusType>"+ objPaypercol0010.BusType + "</BusType><TotalNum>" + objPaypercol0010.TotalNum + "</TotalNum><TotalAmt>" + objPaypercol0010.TotalAmt + "</TotalAmt>");
            sbCreatePAYPERCOL0010.Append("<SignTime>" + objPaypercol0010.SignTime + "</SignTime><TotalSummary>"+ objPaypercol0010.TotalSummary + "</TotalSummary>");
            sbCreatePAYPERCOL0010.Append("<THBaseAccFlag>"+objPaypercol0010.THBaseAccFlag+"</THBaseAccFlag><RegSerialNO>"+objPaypercol0010.RegSerialNO+"</RegSerialNO><PackageName>"+objPaypercol0010.PackageName+"</PackageName>");

            string srcRds = CreatePaypercolRdArea(objPaypercol0010);
            sbCreatePAYPERCOL0010.Append(srcRds + "</in></eb></CMS>");
            return sbCreatePAYPERCOL0010.ToString();

        }
        /// <summary>
        /// 产生PAYPERCOL 中 RD 区域的报文体
        /// </summary>
        /// <param name="objPaypercol0010"></param>
        /// <returns></returns>
        private static string CreatePaypercolRdArea(ObjectPaypercol0010 objPaypercol0010)
        {
            StringBuilder sbRdArea = new StringBuilder();
            for (int i = 0; i < Int32.Parse(objPaypercol0010.TotalNum); i++)
            {
                sbRdArea.Append("<rd><iSeqno>" + (i + 1).ToString() + "</iSeqno><ReimburseNo></ReimburseNo><ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime>");
                // 1：加急 2：普通（工行异地人民币转账不再区分普通\加急,统一按加急处理） 3：跨行快汇（当涉及账户管家账户转账时，记账方式不支持普通，工行异地转账不再区分普通\加急,统一按加急处理）
                sbRdArea.Append("<PayType>" + objPaypercol0010.PayType + "</PayType><PayAccNo>" + objPaypercol0010.PayAccNo + "</PayAccNo><PayAccNameCN>" + objPaypercol0010.PayAccNameCN + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
                sbRdArea.Append("<RecAccNo>" + objPaypercol0010.RecAccNo + "</RecAccNo><RecAccNameCN>" + objPaypercol0010.RecAccNameCN + "</RecAccNameCN><RecAccNameEN></RecAccNameEN>");
                //  1：系统内  2：系统外            1：同城  2：异地        跨行必输  0：对公账户  1：个人账户
                sbRdArea.Append("<SysIOFlg>" + objPaypercol0010.SysIOFlg + "</SysIOFlg><IsSameCity>" + objPaypercol0010.IsSameCity + "</IsSameCity>");
                sbRdArea.Append("<RecICBCCode></RecICBCCode><RecCityName>长沙</RecCityName><RecBankNo></RecBankNo>");
                sbRdArea.Append("<RecBankName>中国工商银行长沙某测试支行</RecBankName><CurrType>" + objPaypercol0010.CurrType + "</CurrType><PayAmt>1</PayAmt>");
                sbRdArea.Append("<UseCode></UseCode><UseCN>" + objPaypercol0010.UseCN + "</UseCN><EnSummary></EnSummary>");
                sbRdArea.Append("<PostScript>" + objPaypercol0010.PostScript + "</PostScript><Summary>" + objPaypercol0010.Summary + "</Summary><Ref>" + objPaypercol0010.Ref + "</Ref><Oref>" + objPaypercol0010.Oref + "</Oref>");
                sbRdArea.Append("<ERPSqn></ERPSqn><BusCode>001</BusCode><ERPcheckno></ERPcheckno>");
                sbRdArea.Append("<CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo>");
                //sbRdArea.Append("<BankType>102</BankType><FileNames></FileNames><Indexs></Indexs><PaySubNo></PaySubNo><RecSubNo></RecSubNo>");
                sbRdArea.Append("</rd>");
            }
            return sbRdArea.ToString();

        }
        #endregion

        #region PAYPER相关XML生成方法
        public static string CreatePAYPER0010(ObjectPayper0010 objPayper0010)
        {
            StringBuilder sbCreatePAYPER0010CustomizeCount = new StringBuilder();
            sbCreatePAYPER0010CustomizeCount.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>" + objPayper0010.TransCode + "</TransCode><CIS>");
            sbCreatePAYPER0010CustomizeCount.Append(objPayper0010.CIS + "</CIS><BankCode>102</BankCode><ID>" + objPayper0010.ID + "</ID><TranDate>");
            sbCreatePAYPER0010CustomizeCount.Append(objPayper0010.TranDate + "</TranDate><TranTime>" + objPayper0010.TranTime + "</TranTime><fSeqno>" + objPayper0010.fSeqno + "</fSeqno></pub><in>");
            // <OnlBatF>表示联机标志，只能选择1；  <SettleMode>  入账方式  2表示并笔记账，0表示逐笔记账
            sbCreatePAYPER0010CustomizeCount.Append("<OnlBatF>" + objPayper0010.OnlBatF + "</OnlBatF><SettleMode>" + objPayper0010.SettleMode + "</SettleMode><TotalNum>" + objPayper0010.TotalNum + "</TotalNum><TotalAmt>" + objPayper0010.TotalAmt + "</TotalAmt>");
            sbCreatePAYPER0010CustomizeCount.Append("<SignTime>" + objPayper0010.SignTime + "</SignTime><THBaseAccFlag>" + objPayper0010.THBaseAccFlag + "</THBaseAccFlag><RegSerialNO>" + objPayper0010.RegSerialNO + "</RegSerialNO>");
            sbCreatePAYPER0010CustomizeCount.Append("<PackageName>" + objPayper0010.PackageName + "</PackageName>" + "<TotalSummary>" + objPayper0010.TotalSummary+ "</TotalSummary>");
            sbCreatePAYPER0010CustomizeCount.Append("<BatchSumFlag>" + objPayper0010.BatchSumFlag+ "</BatchSumFlag>");
            string srcRds = CreatePayperRdArea(objPayper0010);
            sbCreatePAYPER0010CustomizeCount.Append(srcRds + "</in></eb></CMS>");
            return sbCreatePAYPER0010CustomizeCount.ToString();
        }
        /// <summary>
        /// 产生PAYPER中循环区域的报文
        /// </summary>
        /// <param name="objPayent">objPayent</param>
        /// <returns></returns>
        public static string CreatePayperRdArea(ObjectPayper0010 objPayper0010)
        {
            StringBuilder sbRdArea = new StringBuilder();
            for (int i = 0; i < Int32.Parse(objPayper0010.TotalNum); i++)
            {
                sbRdArea.Append("<rd><iSeqno>" + (i + 1).ToString() + "</iSeqno><ReimburseNo></ReimburseNo><ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime>");
                // 1：加急 2：普通（工行异地人民币转账不再区分普通\加急,统一按加急处理） 3：跨行快汇（当涉及账户管家账户转账时，记账方式不支持普通，工行异地转账不再区分普通\加急,统一按加急处理）
                sbRdArea.Append("<PayType>" + objPayper0010.PayType + "</PayType><PayAccNo>" + objPayper0010.PayAccNo + "</PayAccNo><PayAccNameCN>" + objPayper0010.PayAccNameCN + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
                sbRdArea.Append("<RecAccNo>" + objPayper0010.RecAccNo + "</RecAccNo><RecAccNameCN>" + objPayper0010.RecAccNameCN + "</RecAccNameCN><RecAccNameEN></RecAccNameEN>");
                //  1：系统内  2：系统外            1：同城  2：异地        跨行必输  0：对公账户  1：个人账户
                sbRdArea.Append("<SysIOFlg>" + objPayper0010.SysIOFlg + "</SysIOFlg><IsSameCity>" + objPayper0010.IsSameCity + "</IsSameCity>");
                sbRdArea.Append("<RecICBCCode></RecICBCCode><RecCityName>长沙</RecCityName><RecBankNo></RecBankNo>");
                sbRdArea.Append("<RecBankName>中国工商银行长沙某测试支行</RecBankName><CurrType>" + objPayper0010.CurrType + "</CurrType><PayAmt>1</PayAmt>");
                sbRdArea.Append("<UseCode></UseCode><UseCN>" + objPayper0010.UseCN + "</UseCN><EnSummary></EnSummary>");
                sbRdArea.Append("<PostScript>" + objPayper0010.PostScript + "</PostScript><Summary>" + objPayper0010.Summary + "</Summary><Ref>" + objPayper0010.Ref + "</Ref><Oref>" + objPayper0010.Oref + "</Oref>");
                sbRdArea.Append("<ERPSqn></ERPSqn><BusCode>001</BusCode><ERPcheckno></ERPcheckno>");
                sbRdArea.Append("<CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo>");
                //sbRdArea.Append("<BankType>102</BankType><FileNames></FileNames><Indexs></Indexs><PaySubNo></PaySubNo><RecSubNo></RecSubNo>");
                sbRdArea.Append("<MCardNo>" + objPayper0010.MCardNo + "</MCardNo><MCardName>" + objPayper0010.MCardName + "</MCardName></rd>");
            }
            return sbRdArea.ToString();
        }

        public static string CreateQACCBAL0010(ObjectQaccbal0010 objQueryAccbal0010)
        {
            StringBuilder strCreateQACCBAL0010 = new StringBuilder();
            strCreateQACCBAL0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>");
            strCreateQACCBAL0010.Append("QACCBAL</TransCode><CIS>" + objQueryAccbal0010.CIS);
            strCreateQACCBAL0010.Append("</CIS><BankCode>102</BankCode><ID>" + objQueryAccbal0010.ID+ "</ID>");
            strCreateQACCBAL0010.Append("<TranDate>" + objQueryAccbal0010.TranDate + "</TranDate><TranTime>" + objQueryAccbal0010.TranTime + "</TranTime><fSeqno>" + objQueryAccbal0010.fSeqno + "</fSeqno>");
            strCreateQACCBAL0010.Append("</pub><in><TotalNum>"+ objQueryAccbal0010.TotalNum+ "</TotalNum><BLFlag>"+ objQueryAccbal0010.BLFlag + "</BLFlag><SynFlag>"+ objQueryAccbal0010.SynFlag + "</SynFlag><rd>");
            strCreateQACCBAL0010.Append("<iSeqno>00001</iSeqno><AccNo>" + objQueryAccbal0010.AccNo + "</AccNo><CurrType>"+ objQueryAccbal0010.CurrType + "</CurrType><ReqReserved3>"+ objQueryAccbal0010.ReqReserved3 + "</ReqReserved3><AcctSeq>"+ objQueryAccbal0010.AcctSeq + "</AcctSeq><MainAcctNo>" + objQueryAccbal0010.MainAcctNo + "</MainAcctNo></rd></in></eb></CMS>");
            return strCreateQACCBAL0010.ToString();
        }

        public static string CreateQPD0010(ObjectQpd0010 objQpd0010)
        {
            StringBuilder strCreateQPD0010 = new StringBuilder();
            strCreateQPD0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub>");
            strCreateQPD0010.Append("<TransCode>QPD</TransCode><CIS>" + objQpd0010.CIS + "</CIS>");
            strCreateQPD0010.Append("<BankCode>"+ objQpd0010.BankCode + "</BankCode><ID>" + objQpd0010.ID + "</ID>");
            strCreateQPD0010.Append("<TranDate>" + objQpd0010.TranDate + "</TranDate><TranTime>" + objQpd0010.TranTime + "</TranTime><fSeqno>" + objQpd0010.fSeqno + "</fSeqno>");
            strCreateQPD0010.Append("</pub><in><AccNo>" + objQpd0010.AccNo + "</AccNo>");
            strCreateQPD0010.Append("<AreaCode>"+ objQpd0010.AreaCode + "</AreaCode><MinAmt>"+ objQpd0010.MinAmt + "</MinAmt><MaxAmt>"+ objQpd0010.MaxAmt + "</MaxAmt>");
            strCreateQPD0010.Append("<BeginTime></BeginTime><EndTime></EndTime>");
            strCreateQPD0010.Append("<NextTag>" + objQpd0010.NextTag + "</NextTag><ReqReserved1>"+ objQpd0010.ReqReserved1 + "</ReqReserved1><ReqReserved2>"+ objQpd0010.ReqReserved2 + "</ReqReserved2><CurrType>"+ objQpd0010.CurrType + "</CurrType><AcctSeq>"+ objQpd0010.AcctSeq + "</AcctSeq></in></eb></CMS>");
            return strCreateQPD0010.ToString();
        }

        public static string CreateQHISD0010(ObjectQhisd0010 objQhisd0010)
        {
            StringBuilder strCreateQHISD0010 = new StringBuilder();
            strCreateQHISD0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub>");
            strCreateQHISD0010.Append("<TransCode>"+ objQhisd0010.TransCode + "</TransCode><CIS>" + objQhisd0010.CIS + "</CIS>");
            strCreateQHISD0010.Append("<BankCode>" + objQhisd0010.BankCode + "</BankCode><ID>" + objQhisd0010.ID + "</ID>");
            strCreateQHISD0010.Append("<TranDate>" + objQhisd0010.TranDate + "</TranDate><TranTime>" + objQhisd0010.TranTime + "</TranTime><fSeqno>" + objQhisd0010.fSeqno + "</fSeqno>");
            strCreateQHISD0010.Append("</pub><in><AccNo>" + objQhisd0010.AccNo + "</AccNo>");
            strCreateQHISD0010.Append("<BeginDate>"+objQhisd0010.BeginDate+"</BeginDate><EndDate>"+objQhisd0010.EndDate+"</EndDate><MinAmt>"+objQhisd0010.MinAmt+"</MinAmt>");
            strCreateQHISD0010.Append("<MaxAmt>"+objQhisd0010.MaxAmt+"</MaxAmt><BankType>"+objQhisd0010.BankType+"</BankType><NextTag>"+objQhisd0010.NextTag+"</NextTag><CurrType>"+objQhisd0010.CurrType+"</CurrType>");
            strCreateQHISD0010.Append("<DueBillNo>"+objQhisd0010.DueBillNo+"</DueBillNo><AcctSeq>"+objQhisd0010.AcctSeq+"</AcctSeq><ComplementFlag>"+objQhisd0010.ComplementFlag+"</ComplementFlag><CardAccNoDef>"+objQhisd0010.CardAccNoDef+"</CardAccNoDef><DesByTime>"+objQhisd0010.DesByTime+"</DesByTime></in></eb></CMS>");
            return strCreateQHISD0010.ToString();

        }

        public static string CreateQRYGJDTL0010(ObjectQRYGJDTL0010 objQRYGJDTL0010)
        {
            StringBuilder strCreateQRYGJDTL0010 = new StringBuilder();
            strCreateQRYGJDTL0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub>");
            strCreateQRYGJDTL0010.Append("<TransCode>" + objQRYGJDTL0010.TransCode + "</TransCode><CIS>" + objQRYGJDTL0010.CIS + "</CIS>");
            strCreateQRYGJDTL0010.Append("<BankCode>" + objQRYGJDTL0010.BankCode + "</BankCode><ID>" + objQRYGJDTL0010.ID + "</ID>");
            strCreateQRYGJDTL0010.Append("<TranDate>" + objQRYGJDTL0010.TranDate + "</TranDate><TranTime>" + objQRYGJDTL0010.TranTime + "</TranTime><fSeqno>" + objQRYGJDTL0010.fSeqno + "</fSeqno>");
            strCreateQRYGJDTL0010.Append("</pub><in><AcctNo>" + objQRYGJDTL0010.AcctNo + "</AcctNo>");
            strCreateQRYGJDTL0010.Append("<StartDate>"+objQRYGJDTL0010.StartDate+"</StartDate><EndDate>"+objQRYGJDTL0010.EndDate+ "</EndDate><NextTag>"+objQRYGJDTL0010.NextTag+"</NextTag><AcctSeq>"+objQRYGJDTL0010.AcctSeq+"</AcctSeq>");
            strCreateQRYGJDTL0010.Append("<CardNo>"+objQRYGJDTL0010.CardNo+"</CardNo><CurrType>"+objQRYGJDTL0010.CurrType+"</CurrType></in></eb></CMS>");
            return strCreateQRYGJDTL0010.ToString();
        }

        public static string CreateQPAYENT0010(ObjectQpayent0010 objQPAYENT0010)
        {
            StringBuilder strCreateQPAYENT0010 = new StringBuilder();
            strCreateQPAYENT0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>"+objQPAYENT0010.TransCode);
            strCreateQPAYENT0010.Append("</TransCode><CIS>" + objQPAYENT0010.CIS);
            strCreateQPAYENT0010.Append("</CIS><BankCode>102</BankCode><ID>" + objQPAYENT0010.ID + "</ID>");
            strCreateQPAYENT0010.Append("<TranDate>" + objQPAYENT0010.TranDate + "</TranDate><TranTime>" + objQPAYENT0010.TranTime + "</TranTime><fSeqno>" + objQPAYENT0010.fSeqno + "</fSeqno>");
            strCreateQPAYENT0010.Append("</pub><in><QryfSeqno>"+objQPAYENT0010.QryfSeqno+"</QryfSeqno><QrySerialNo>"+objQPAYENT0010.QrySerialNo+"</QrySerialNo></in></eb></CMS>");
            return strCreateQPAYENT0010.ToString();
        }

        public static string CreateQpayper0010(ObjectQpayper0010 objQpayper0010)
        {
            StringBuilder strCreateQpayper0010 = new StringBuilder();
            strCreateQpayper0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>" + objQpayper0010.TransCode);
            strCreateQpayper0010.Append("</TransCode><CIS>" + objQpayper0010.CIS);
            strCreateQpayper0010.Append("</CIS><BankCode>102</BankCode><ID>" + objQpayper0010.ID + "</ID>");
            strCreateQpayper0010.Append("<TranDate>" + objQpayper0010.TranDate + "</TranDate><TranTime>" + objQpayper0010.TranTime + "</TranTime><fSeqno>" + objQpayper0010.fSeqno + "</fSeqno>");
            strCreateQpayper0010.Append("</pub><in><QryfSeqno>" + objQpayper0010.QryfSeqno + "</QryfSeqno><QrySerialNo>" + objQpayper0010.QrySerialNo + "</QrySerialNo></in></eb></CMS>");
            return strCreateQpayper0010.ToString();

        }

        public static string CreateDIBPSBC0001(ObjectDIBPSBC objDIBPSBC0001)
        {
            StringBuilder strCreateDIBPSBC0001 = new StringBuilder();
            strCreateDIBPSBC0001.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>" + objDIBPSBC0001.TransCode);
            strCreateDIBPSBC0001.Append("</TransCode><CIS>" + objDIBPSBC0001.CIS);
            strCreateDIBPSBC0001.Append("</CIS><BankCode>102</BankCode><ID>" + objDIBPSBC0001.ID + "</ID>");
            strCreateDIBPSBC0001.Append("<TranDate>" + objDIBPSBC0001.TranDate + "</TranDate><TranTime>" + objDIBPSBC0001.TranTime + "</TranTime><fSeqno>" + objDIBPSBC0001.fSeqno + "</fSeqno>");
            strCreateDIBPSBC0001.Append("</pub><in><BnkCode>"+objDIBPSBC0001.BnkCode+"</BnkCode><NextTag>"+objDIBPSBC0001.NextTag+"</NextTag><ReqReserved1>"+objDIBPSBC0001.ReqReserved1+"</ReqReserved1><ReqReserved2>"+objDIBPSBC0001.ReqReserved2+"</ReqReserved2></in></eb></CMS>");
            return strCreateDIBPSBC0001.ToString();

        }

        #endregion

    }
}
