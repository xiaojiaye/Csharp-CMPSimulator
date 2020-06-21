



using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace CMPSimulator.tools
{
    public static class SignOrEncrypt
    {
        public static HttpWebRequest myHttpWebRequest;
        public static HttpWebResponse myHttpWebResponse;
        public static XmlDocument doc_1 = new XmlDocument();

        /// <summary>
        /// 发送信息到NC服务器，根据参数选择是签名还是加密
        /// </summary>
        /// <param name="Url">post的目的地URL及端口号</param>
        /// <param name="PackageSrc">xml格式的包源文件</param>
        /// <param name="lnsign">0表示签名，1表示加密</param>
        /// <returns>返回加密或者签名的字符串</returns>
		public static string EncryptOrSign(string Url, string PackageSrc, int lnsign)
        {
            myHttpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            string str_temp = "", str_SignOrEncrypt = "", str_ReceivePackage = "";
            int j_length;
            Encoding encoding = Encoding.GetEncoding(936);
            if (lnsign == 0)
            {
                byte[] byte1 = encoding.GetBytes(PackageSrc);
                myHttpWebRequest.Method = "post";
                myHttpWebRequest.ContentType = "INFOSEC_SIGN/1.0";
                myHttpWebRequest.ContentLength = encoding.GetByteCount(PackageSrc);
                int countl = encoding.GetByteCount(PackageSrc);
                try
                {
                    Stream newStream = myHttpWebRequest.GetRequestStream();
                    newStream.Write(byte1, 0, byte1.Length);
                    newStream.Close();
                }
                catch
                {
                    return "1不能形成文件流或NC签名未启动";
                }
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                try
                {
                    if (myHttpWebRequest.HaveResponse)
                    {
                        Stream streamResponse = myHttpWebResponse.GetResponseStream();
                        StreamReader streamRead = new StreamReader(streamResponse, Encoding.GetEncoding(936));
                        Char[] readBuff = new Char[256];
                        int count = streamRead.Read(readBuff, 0, 256);
                        while (count > 0)
                        {
                            String outputData = new String(readBuff, 0, count);
                            str_temp = str_temp + outputData;
                            count = streamRead.Read(readBuff, 0, 256);
                        }
                        int hh = str_temp.Length;
                        string str_temp1 = str_temp.Substring(84, (hh - 113));
                        doc_1.LoadXml(str_temp);

                        XmlNodeList nodelist_pub = doc_1.GetElementsByTagName("sign");
                        //XmlNode elem = doc.DocumentElement.GetElementsByTagName("sign");
                        str_SignOrEncrypt = nodelist_pub.Item(0).InnerText;
                        streamResponse.Close();
                        streamRead.Close();
                        myHttpWebResponse.Close();
                    }
                }
                catch
                {
                    return "1不能形成文件流或NC签名响应流未启动";
                }
            }
            else
            {
                myHttpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                byte[] byte2 = encoding.GetBytes(PackageSrc);
                myHttpWebRequest.Method = "post";
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.ServicePoint.Expect100Continue = false;  // 非常关键，曾经在此处踩坑，请求头中Expect: 100-continue，NC3.1 服务器 不响应
                                                                          //            “Expect: 100 -continue”的来龙去脉：

                //HTTP / 1.1 协议里设计 100(Continue) HTTP 状态码的的目的是，在客户端发送 Request Message 之前，HTTP / 1.1 协议允许客户端先判定服务器是否愿意接受客户端发来的消息主体（基于 Request Headers）。
                //即， Client 和 Server 在 Post （较大）数据之前，允许双方“握手”，如果匹配上了，Client 才开始发送（较大）数据。
                //这么做的原因是，如果客户端直接发送请求数据，但是服务器又将该请求拒绝的话，这种行为将带来很大的资源开销。

                //协议对 HTTP/ 1.1 clients 的要求是：
                //如果 client 预期等待“100 -continue”的应答，那么它发的请求必须包含一个 " Expect: 100-continue"  的头域！
                // 此处采用了简单的处理方法，就是请求时，不询问服务器是否接受，直接发。
                // 一句代码搞定：myHttpWebRequest.ServicePoint.Expect100Continue = false;


                try
                {
                    Stream newStream = myHttpWebRequest.GetRequestStream();
                    newStream.Write(byte2, 0, byte2.Length);
                    newStream.Close();
                }
                catch
                {
                    return "1不能形成文件流或NC加密未启动";
                }
                try
                {
                    myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    if (myHttpWebRequest.HaveResponse)
                    {
                        Stream streamResponse1 = myHttpWebResponse.GetResponseStream();
                        StreamReader streamRead1 = new StreamReader(streamResponse1, Encoding.GetEncoding(936));
                        Char[] readBuff = new Char[256];
                        int count = streamRead1.Read(readBuff, 0, 256);
                        while (count > 0)
                        {
                            String outputData = new String(readBuff, 0, count);
                            str_ReceivePackage = str_ReceivePackage + outputData;
                            count = streamRead1.Read(readBuff, 0, 256);
                        }
                        streamResponse1.Close();
                        streamRead1.Close();
                        myHttpWebResponse.Close();
                        j_length = str_ReceivePackage.Length - 8;
                        str_SignOrEncrypt = str_ReceivePackage.Substring(8, j_length);
                    }
                }
                catch (Exception ex)
                {
                    return "1不能形成文件流或NC加密响应流未启动" + ex.Message;
                }
            }
            return str_SignOrEncrypt;
        }
    }
}
