using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMPSimulator.Entity;


namespace CMPSimulator.tools
{
    /// <summary>
    /// 银企互联通用工具类
    /// </summary>
    public static class CmpCommonTools
    {
        /// <summary>
        /// 生成HTTP加密服务的 post请求 ACTION URL
        /// </summary>
        /// <param name="objActionUrl"></param>
        /// <returns></returns>

        public static string makeActionUrl(ActionUrl objActionUrl)
        {
            string strActionUrl = string.Empty;
            strActionUrl = @"http://" + objActionUrl.strPostAddress + ":" + objActionUrl.strHttpPort + @"/servlet/ICBCCMPAPIReqServlet?userID=";
            strActionUrl = strActionUrl + objActionUrl.strId + "&PackageID=" + objActionUrl.strPackageID + "&SendTime=" + objActionUrl.strSendTime;
            return strActionUrl;
        }
        /// <summary>
        /// 生成发往http加密端口的 POST数据
        /// </summary>
        /// <param name="objActionUrl">objActionUrl对象中，包含了CIS\TRANSCODE\ID</param>
        /// <param name="zipFlag">是否压缩报文节点</param>
        /// <param name="signData">签名数据</param>
        /// <returns></returns>
        public static string createPostData(ActionUrl objActionUrl , bool zipFlag,string signData) {
            string strPostData = string.Empty;
            strPostData = "Version=" + objActionUrl.strInterfaceVersion +  "&TransCode=" + objActionUrl.strTransCode +  "&BankCode=102&GroupCIS=" + objActionUrl.strCis + "&ID=" + objActionUrl.strId + "&PackageID=" + objActionUrl.strPackageID + "&Cert=&reqData=" + signData;
            if (zipFlag)
            {
                strPostData = strPostData + "&zipFlag=1";
            }
            return strPostData;
        }
    }
}
