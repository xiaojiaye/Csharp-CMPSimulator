using System;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using CMPSimulator.tools;
using CMPSimulator.Entity;
using System.Collections.Generic;
using System.Web;
using CMPSimulator.UI;
using CMPSimulator.Entity.QueryReturnEntity;


namespace CMPSimulator
{
    public partial class Frm_main : Form
    {
        public Frm_main()
        {
            InitializeComponent();
            this.txtCIS.Text = this.Cis;
            this.txtID.Text = this.Id;
            this.txtPayAccNo.Text = this.PayAccNo;
            this.txtPayAccName.Text = this.PayAccName;
            this.txtRecAccNo.Text = this.RecAccNo;
            this.txtRecAccName.Text = this.RecAccName;
            this.txtPostAddress.Text = this.PostAddress;
            this.txtHttpPort.Text = this.HttpPort;
            this.txtSignPort.Text = this.SignPort;
            this.txtTestEnvironmentDate.Text = this.TestEnvironmentDate;
            this.txtContrNo.Text = this.ContrNo;
            this.txtContractNo.Text = this.ContractNo;
            this.txtPersonNo.Text = this.PersonNo;

            // 将DateTimePicker的初始时间，设置成测试环境的时间
            string dt1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string yyyy = this.txtTestEnvironmentDate.Text.Substring(0,4);
            string mm = this.txtTestEnvironmentDate.Text.Substring(4,2);
            string dd = this.txtTestEnvironmentDate.Text.Substring(6,2);
            string eTime = yyyy + "-" + mm + "-" + dd;
            dt1 = dt1.Remove(0, 10);
            dt1 = dt1.Insert(0, eTime);
            this.dtpBeginDate.Value = DateTime.Parse(dt1);
            this.dtpEndDate.Value = DateTime.Parse(dt1);
            this.Refresh();

        }

        private List<BankNoAndBankName> listBankNoAndBankName = null;
        private BankNoAndBankName objBankNoAndBankName = null;
        private string Cis = ConfigurationSettings.AppSettings["CIS"];
        private string Id = ConfigurationSettings.AppSettings["ID"];
        private string PayAccNo = ConfigurationSettings.AppSettings["PayAccNo"];
        private string PayAccName = ConfigurationSettings.AppSettings["PayAccName"];
        private string RecAccNo = ConfigurationSettings.AppSettings["RecAccNo"];
        private string RecAccName = ConfigurationSettings.AppSettings["RecAccName"];
        private string PostAddress = ConfigurationSettings.AppSettings["PostAddress"];
        private string HttpPort = ConfigurationSettings.AppSettings["HpptPort"];
        private string SignPort = ConfigurationSettings.AppSettings["SignPort"];
        private string TestEnvironmentDate = ConfigurationSettings.AppSettings["TestEnvironmentDate"];
        private string ContrNo = ConfigurationSettings.AppSettings["ContrNo"];
        private string PersonNo = ConfigurationSettings.AppSettings["PersonNo"];
        private string ContractNo = ConfigurationSettings.AppSettings["ContractNo"];

        private string Make448Url(string sendTime,string packageID)
        {            
            string url448 = "/servlet/ICBCCMPAPIReqServlet?userID=" + this.txtID.Text.Trim() + "&PackageID=" + packageID + "&SendTime=" + sendTime;
            return "http://" + this.txtPostAddress.Text.Trim() + ":" + this.txtHttpPort.Text.Trim() + url448;
            
        }

        private string makeJoint(string version,string transCode,string packageId,string reqData )
        {
            string strJoint = string.Empty;
            strJoint = "Version=" + version + "&TransCode=" + transCode + "&BankCode=102&GroupCIS=" + this.txtCIS.Text.Trim() + "&ID=" + this.txtID.Text.Trim() + "&PackageID=" + packageId + "&Cert=&reqData=" + reqData;
            return strJoint;
        }
        // 结算类
        private void btnSign_Click(object sender, EventArgs e)
        {
            string version = string.Empty;
            string joint = string.Empty;
            string transCode = string.Empty;
            string xmlPackageSrc = string.Empty;
            if (this.radbtn_0010.Checked)
            {
                version = "0.0.1.0";
            }
            if (this.radbtn_0001.Checked)
            {
                version = "0.0.0.1";
            }
            //先验证测试环境日期是否设置了
            if (this.txtTestEnvironmentDate.Text.Trim().Length==0 || this.txtTestEnvironmentDate.Text.Trim().Length < 8)
            {
                MessageBox.Show("请输入测试环境日期，格式yyyyMMdd");
                return;
            }
            string testEnvironmentDate = this.txtTestEnvironmentDate.Text.Trim();
            string sendTime = DateTime.Now.ToString("yyyymmddHHmmssfff");
            sendTime = sendTime.Remove(0,8);
            sendTime = sendTime.Insert(0,testEnvironmentDate);
            string packageID = sendTime;
            string signUrl = "http://" + this.txtPostAddress.Text.Trim() + ":" + this.txtSignPort.Text.Trim();
            this.txtShowResult.Text = "【往449发的URL：】\r\n" + signUrl + "\r\n";

            if (this.radPayentAndZip.Checked)
            {
                // 如果选择了，则是用压缩报文方式组包
                xmlPackageSrc = CreatePAYENTWithZip0010(sendTime, packageID);
                transCode = "PAYENT";

            } else if (this.radGjkPayent.Checked)
            {
                // 如果选择了，则是管家卡单笔支付
                xmlPackageSrc = CreateGjkPAYENT0010(sendTime, packageID);
                transCode = "PAYENT";
            }
            // 批扣企业汇总记账  ENTDISCOL0010   支持zip
            else if (this.radENTDISCOL0010.Checked) {
                xmlPackageSrc = CreateENTDISCOL0010(sendTime, packageID);
                transCode = "ENTDISCOL";
            }
            // 批扣个人汇总记账   PERDISCOL0001    支持zip
            else if (this.radPERDISCOL0001.Checked)
            {
                xmlPackageSrc = CreatePERDISCOL0001(sendTime, packageID);
                transCode = "PERDISCOL";
            }
            // 普通PAYENT付款，自定义汇款笔数  
            else if (this.radPayent0010CustomizeCount.Checked) {
                xmlPackageSrc = CreatePayent0010CustomizeCount(sendTime, packageID);
                if (this.cbxOtherBank.Checked)
                {
                    // 对他行汇款组包
                    xmlPackageSrc = CreatePayent0010CustomizeCountOtherBank(sendTime, packageID);
                }
                transCode = "PAYENT";
            } else if (this.radPAYPERCOL0010.Checked) {
                // 选择了企业财务室汇总记账
                xmlPackageSrc = CreatePAYPERCOL0010(sendTime, packageID);
                transCode = "PAYPERCOL";
            } else if (this.radPAYPER0010.Checked) {
                // 企业财务室，普通记账  PAYPER
                xmlPackageSrc = CreatePAYPER0010(sendTime, packageID);
                transCode = "PAYPER";

            }

            else
            {
                xmlPackageSrc = CreatePAYENT0010(sendTime, packageID);
                transCode = "PAYENT";
            }
            
            this.txtShowResult.Text = this.txtShowResult.Text + "【往449发的原包：】\r\n" + xmlPackageSrc + "\r\n";
            PostToNc postTonc = new PostToNc();
            string signResult = string.Empty;
            signResult = postTonc.SignOrEncrypt(signUrl,xmlPackageSrc,0);
            this.txtShowResult.Text = this.txtShowResult.Text + "【原XML报文：】\r\n" + xmlPackageSrc + "\r\n";
            this.txtShowResult.Text = this.txtShowResult.Text + "【签名结果：】\r\n" + signResult +"\r\n";



            // string joint = "Version=0.0.1.0&TransCode=PAYENT&BankCode=102&GroupCIS=" + this.txtCIS.Text.Trim() + "&ID=" + this.txtID.Text.Trim() + "&PackageID=" + sendTime + "&Cert=&reqData=" + signResult;
            joint = makeJoint(version, transCode, packageID, signResult);

            if (this.radPayentAndZip.Checked)
            {
                // 使用压缩
                
                joint = joint + "&zipFlag=1";
            }
            string EncryptResult = postTonc.SignOrEncrypt(Make448Url(sendTime,packageID),joint,1);
            this.txtShowResult.Text = this.txtShowResult.Text + "【往448发的URL：】\r\n" + Make448Url(sendTime, packageID) + "\r\n";
            this.txtShowResult.Text = this.txtShowResult.Text + "【往448发的postData：】\r\n" + joint + "\r\n";
            byte[] b = Convert.FromBase64String(EncryptResult);
            string t = Encoding.GetEncoding(936).GetString(b);
            this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回1：】\r\n" + EncryptResult + "\r\n";
            this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回2：】\r\n" + t + "\r\n";
        }   

        private void btnDecodeBase64_Click(object sender, EventArgs e)
        {
            string str_SrcBased64 = this.txtShowResult.Text;
            byte[] b = Convert.FromBase64String(str_SrcBased64);
            string t = Encoding.GetEncoding(936).GetString(b);
            this.txtShowResult.Text = this.txtShowResult.Text + "\r\n【解码后：】\r\n" + t;
        }

        private void btnQueryAll_Click(object sender, EventArgs e)
        {
            
            bool isSelectQueryType = this.radQACCBAL.Checked || this.radQHISD.Checked || this.radQPAYPER.Checked || this.radQPAYENT.Checked || this.radQPD.Checked || this.radQHISBAL.Checked || this.radDIBPSBC.Checked || this.radQRYGJDTL.Checked || this.radQENTDIS.Checked|| this.radQPERDIS0001.Checked|| this.radDIBPSBC0001.Checked;
            //先验证测试环境日期是否设置了
            if (this.txtTestEnvironmentDate.Text.Trim().Length == 0 || this.txtTestEnvironmentDate.Text.Trim().Length < 8)
            {
                MessageBox.Show("请输入测试环境日期，格式yyyyMMdd");
                return;
            }
            //验证是否选择了查询的业务类型
            if (!isSelectQueryType)
            {
                MessageBox.Show("请先选择要查询的业务类型！");
                return;
            }
            this.btnQueryAll.Enabled = false;
            string testEnvironmentDate = this.txtTestEnvironmentDate.Text.Trim();
            string sendTime = DateTime.Now.ToString("yyyymmddHHmmssfff");
            sendTime = sendTime.Remove(0, 8);
            sendTime = sendTime.Insert(0, testEnvironmentDate);
            string packageID = sendTime;
            PostToNc postTonc = new PostToNc();
            string fromICBCResult = string.Empty;

            //【1】生成发查询的URL http://127.0.0.1:448/servlet/ICBCCMPAPIReqServlet?userID=&PackageID=&SendTime=
            string posturl = QueryActionUrl(packageID, sendTime);
            //【2】根据选择，生成XML报文明文
            string queryStr = string.Empty;
            string postData = string.Empty;
            if (this.radQACCBAL.Checked)
            {
                queryStr = CreateQACCBAL0010(sendTime, packageID);
                postData = QueryRequestData("QACCBAL", packageID, queryStr);
                fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                this.txtShowResult.Text = "【post的URL：】\r\n" + posturl + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【post的数据：】\r\n" + postData + "\r\n";
                byte[] b = Convert.FromBase64String(fromICBCResult);
                string t = Encoding.GetEncoding(936).GetString(b);                
                this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回：】\r\n" + fromICBCResult + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【base64解码后：】\r\n" + t + "\r\n";
            }

            if (this.radQPD.Checked)
            {
                string nextTag = "";
                int i = 0;
                do
                {
                    queryStr = CreateQPD0010(sendTime, packageID, nextTag);
                    postData = QueryRequestData("QPD", packageID, queryStr);
                    fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                    this.txtShowResult.Text = this.txtShowResult.Text + "【原XML报文：】\r\n" + queryStr + "\r\n";
                    byte[] b = Convert.FromBase64String(fromICBCResult);
                    string t = Encoding.GetEncoding(936).GetString(b);
                    //GC.Collect();
                    this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回1：】\r\n" + fromICBCResult + "\r\n";
                    this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回2：】\r\n" + t + "\r\n";
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(t);
                    xml.Save("c:\\test\\" + "【" + packageID + "】" + "第" + (i + 1).ToString() + "次QPD.xml");//保存
                    XmlNodeList nodelist_pub = xml.GetElementsByTagName("NextTag");
                    nextTag = nodelist_pub.Item(0).InnerText;
                    //nextTag.Replace("&","&amp;");             
                    //this.lblShowTag.Text = this.lblShowTag.Text + "分割" + nextTag;
                    i++;
                } while (!nextTag.Equals(string.Empty));
                this.txtShowResult.Text = this.txtShowResult.Text + "【总共发查询包的个数是：】\r\n" + i.ToString() + "\r\n";
            }
            /*
            if (this.radQHISD.Checked)
            {
                if (this.dtpBeginDate.Value > this.dtpEndDate.Value)
                {
                    MessageBox.Show("起始日期不能大于结束日期,请重新选择");
                    return;
                }               
                string nextTag = "";
                int i = 0;                
                do
                {
                    queryStr = CreateQHISD0010(sendTime, packageID, nextTag);
                    postData = QueryRequestData("QHISD", packageID, queryStr);
                    fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                    this.txtShowResult.Text = this.txtShowResult.Text + "【原XML报文：】\r\n" + queryStr + "\r\n";
                    byte[] b = Convert.FromBase64String(fromICBCResult);
                    string t = Encoding.GetEncoding(936).GetString(b);
                    //GC.Collect();
                    this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回1：】\r\n" + fromICBCResult + "\r\n";
                    this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回2：】\r\n" + t + "\r\n";
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(t);
                    xml.Save("f:\\test\\" + "第" + (i+1).ToString() + "次文件.xml");//保存
                    XmlNodeList nodelist_pub = xml.GetElementsByTagName("NextTag");
                    nextTag = nodelist_pub.Item(0).InnerText;
                    //nextTag.Replace("&","&amp;");             
                    //this.lblShowTag.Text = this.lblShowTag.Text + "分割" + nextTag;
                    i++;
                } while (!nextTag.Equals(string.Empty));
                this.txtShowResult.Text = this.txtShowResult.Text + "【总共发查询包的个数是：】\r\n" + i.ToString() + "\r\n";
            }
            */
            if (this.radQHISD.Checked)
            {
                if (this.dtpBeginDate.Value > this.dtpEndDate.Value)
                {
                    MessageBox.Show("起始日期不能大于结束日期,请重新选择");
                    return;
                }
                string nextTag = "";
                int i = 0;
                StringBuilder sbQHISD = new StringBuilder();
                do
                {
                    queryStr = CreateQHISD0010(sendTime, packageID, nextTag);
                    postData = QueryRequestData("QHISD", packageID, queryStr);
                    fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                    sbQHISD.Append("【原XML报文：】\r\n" + queryStr + "\r\n");                   
                    byte[] b = Convert.FromBase64String(fromICBCResult);
                    string t = Encoding.GetEncoding(936).GetString(b);
                    //GC.Collect();
                    //sbQHISD.Append("【银行返回1：】\r\n" + fromICBCResult + "\r\n");
                    sbQHISD.Append("【银行返回2：】\r\n" + t + "\r\n");                   
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(t);
                    xml.Save("f:\\test\\" + "第" + (i + 1).ToString() + "次文件.xml");//保存
                    XmlNodeList nodelist_pub = xml.GetElementsByTagName("NextTag");
                    nextTag = nodelist_pub.Item(0).InnerText;
                    //nextTag.Replace("&","&amp;");             
                    //this.lblShowTag.Text = this.lblShowTag.Text + "分割" + nextTag;
                    i++;
                } while (!nextTag.Equals(string.Empty));
                this.txtShowResult.Text = sbQHISD.ToString()+ "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【总共发查询包的个数是：】\r\n" + i.ToString() + "\r\n";
            }

            // 查询批扣企业 QENTDIS
            if (this.radQENTDIS.Checked)
            {
                string sourceId = this.txtShowResult.Text.Trim();
                queryStr = CreateQENTDIS0010(sendTime, packageID, sourceId);
                postData = QueryRequestData("QENTDIS", packageID, queryStr);
                fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                this.txtShowResult.Text = "【post的URL：】\r\n" + posturl + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【post的数据：】\r\n" + postData + "\r\n";
                byte[] b = Convert.FromBase64String(fromICBCResult);
                string t = Encoding.GetEncoding(936).GetString(b);
                this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回：】\r\n" + fromICBCResult + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【base64解码后：】\r\n" + t + "\r\n";
            }

            // 查询批扣个人 QPERDIS
            if (this.radQPERDIS0001.Checked)
            {
                string sourceId = this.txtShowResult.Text.Trim();
                queryStr = CreateQPERDIS0001(sendTime, packageID, sourceId);
                postData = QueryRequestData("QPERDIS", packageID, queryStr);
                fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                this.txtShowResult.Text = "【post的URL：】\r\n" + posturl + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【post的数据：】\r\n" + postData + "\r\n";
                byte[] b = Convert.FromBase64String(fromICBCResult);
                string t = Encoding.GetEncoding(936).GetString(b);
                this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回：】\r\n" + fromICBCResult + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【base64解码后：】\r\n" + t + "\r\n";
            }

            // 查询管家卡明细 QRYGJDTL
            if (this.radQRYGJDTL.Checked)
            {
                if (this.dtpBeginDate.Value > this.dtpEndDate.Value)
                {
                    MessageBox.Show("起始日期不能大于结束日期,请重新选择");
                    return;
                }
                string nextTag = "";
                int i = 0;
                do
                {
                    queryStr = CreateQRYGJDTL0010(sendTime, packageID, nextTag);
                    postData = QueryRequestData("QRYGJDTL", packageID, queryStr);
                    fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                    this.txtShowResult.Text = this.txtShowResult.Text + "【原XML报文：】\r\n" + queryStr + "\r\n";
                    byte[] b = Convert.FromBase64String(fromICBCResult);
                    string t = Encoding.GetEncoding(936).GetString(b);
                    //GC.Collect();
                    this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回1：】\r\n" + fromICBCResult + "\r\n";
                    this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回2：】\r\n" + t + "\r\n";
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(t);
                    xml.Save("f:\\test\\" + "第" + (i + 1).ToString() + "次QRYGJDTL文件.xml");//保存
                    XmlNodeList nodelist_pub = xml.GetElementsByTagName("NextTag");
                    nextTag = nodelist_pub.Item(0).InnerText;
                    //nextTag.Replace("&","&amp;");             
                    //this.lblShowTag.Text = this.lblShowTag.Text + "分割" + nextTag;
                    i++;
                } while (!nextTag.Equals(string.Empty));
                this.txtShowResult.Text = this.txtShowResult.Text + "【总共发查询包的个数是：】\r\n" + i.ToString() + "\r\n";
            }

            // 查询历史余额
            if (this.radQHISBAL.Checked)
            {
                queryStr = CreateQHISBAL0010(sendTime, packageID);
                postData = QueryRequestData("QHISBAL", packageID, queryStr);
                fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                this.txtShowResult.Text = "【post的URL：】\r\n" + posturl + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【post的数据：】\r\n" + postData + "\r\n";
                byte[] b = Convert.FromBase64String(fromICBCResult);
                string t = Encoding.GetEncoding(936).GetString(b);
                this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回：】\r\n" + fromICBCResult + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【base64解码后：】\r\n" + t + "\r\n";
            }

            // 查询付款指令
            if (this.radQPAYENT.Checked)
            {
                string sourcePayId = this.txtShowResult.Text.Trim();
                queryStr = CreateQPAYENT0010(sendTime, packageID, sourcePayId);
                postData = QueryRequestData("QPAYENT", packageID, queryStr);
                fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                this.txtShowResult.Text = "【post的URL：】\r\n" + posturl + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【post的数据：】\r\n" + postData + "\r\n";
                byte[] b = Convert.FromBase64String(fromICBCResult);
                string t = Encoding.GetEncoding(936).GetString(b);
                this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回：】\r\n" + fromICBCResult + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【base64解码后：】\r\n" + t + "\r\n";

            }

            // 查询企业财务室付款指令0010
            if (this.radQPAYPER.Checked)
            {
                string sourcePayId = this.txtShowResult.Text.Trim();
                queryStr = CreateQPAYPER0010(sendTime, packageID, sourcePayId);
                postData = QueryRequestData("QPAYPER", packageID, queryStr);
                fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                this.txtShowResult.Text = "【post的URL：】\r\n" + posturl + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【post的数据：】\r\n" + postData + "\r\n";
                byte[] b = Convert.FromBase64String(fromICBCResult);
                string t = Encoding.GetEncoding(936).GetString(b);
                this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回：】\r\n" + fromICBCResult + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【base64解码后：】\r\n" + t + "\r\n";

            }
            

            // 行名行号下载
            if (this.radDIBPSBC.Checked)
            {
                queryStr = CreateDIBPSBC0001(sendTime, packageID,"");
                postData = QueryRequestData("DIBPSBC", packageID, queryStr);
                fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                this.txtShowResult.Text = "【post的URL：】\r\n" + posturl + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【post的数据：】\r\n" + postData + "\r\n";
                byte[] b = Convert.FromBase64String(fromICBCResult);
                string t = Encoding.GetEncoding(936).GetString(b);
                this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回：】\r\n" + fromICBCResult + "\r\n";
                this.txtShowResult.Text = this.txtShowResult.Text + "【base64解码后：】\r\n" + t + "\r\n";
            }

            // 行名行号下载  radDIBPSBC0001
            if (this.radDIBPSBC0001.Checked)
            {
                StringBuilder sbCount = new StringBuilder();
                StringBuilder sbBankNoAndBankName = new StringBuilder();
                string nextTag = "";
                int i = 0;
                listBankNoAndBankName = new List<BankNoAndBankName>();
                do
                {
                    queryStr = CreateDIBPSBC0001(sendTime, packageID, nextTag);
                    postData = QueryRequestData("DIBPSBC", packageID, queryStr);
                    fromICBCResult = postTonc.SignOrEncrypt(posturl, postData, 1);
                    //this.txtShowResult.Text = this.txtShowResult.Text + "【原XML报文：】\r\n" + queryStr + "\r\n";
                    byte[] b = Convert.FromBase64String(fromICBCResult);
                    string t = Encoding.GetEncoding(936).GetString(b);
                    //GC.Collect();
                    //this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回1：】\r\n" + fromICBCResult + "\r\n";
                    //this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回2：】\r\n" + t + "\r\n";
                    
                    XmlDocument xml = new XmlDocument();  // 初始化一个 xml 实例对象
                    xml.LoadXml(t);   // 载入字符串   如果载入文件，可以用xml.Load("SkillinfoList.txt");
                    XmlNodeList nodelist_pub = xml.GetElementsByTagName("NextTag");
                    nextTag = nodelist_pub.Item(0).InnerText;
                 
                    //得到out下根节点的所有子节点
                    XmlNodeList nodeList = xml.SelectNodes("/CMS/eb/out/rd");
                    // 遍历
                    
                    foreach (XmlNode node in nodeList)
                    {
                        objBankNoAndBankName = new BankNoAndBankName();
                        //InnerText节点包含的所有文本内容                        
                        string PaySysBnkCode = node.ChildNodes[1].InnerText;
                        string BnkName = node.ChildNodes[4].InnerText;
                        objBankNoAndBankName.BnkName1 = BnkName;
                        objBankNoAndBankName.PaySysBnkCode1 = PaySysBnkCode;
                        listBankNoAndBankName.Add(objBankNoAndBankName);
                        objBankNoAndBankName = null;
                    }

                    // 统计并展示包的个数   
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\2.txt"))
                    {     
                            file.Write("第【" + (i + 1).ToString() + "】个包加载到list完毕.xml");                       
                    }

                    //xml.Save("f:\\test\\" + "第" + (i + 1).ToString() + "次QRYGJDTL文件.xml");//保存

                    //nextTag.Replace("&","&amp;");             
                    //this.lblShowTag.Text = this.lblShowTag.Text + "分割" + nextTag;
                    i++;
                    //if (i>=3)
                    //{
                    //    break;
                    //}
                } while (!nextTag.Equals(string.Empty));
                listBankNoAndBankName.ToString();

                // 将list 写入到文件中
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\1.txt"))
                {
                    foreach (BankNoAndBankName obj in listBankNoAndBankName)
                    {
                          file.Write(obj);                       
                    }

                }
                this.txtShowResult.Text = this.txtShowResult.Text + "【总共发查询包的个数是：】\r\n" + i.ToString() + "\r\n";

            }



            this.btnQueryAll.Enabled = true;

        }

  

        /// <summary>
        /// 生成查询类交易的Action的URL
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="sendTime"></param>
        /// <returns></returns>
        private string QueryActionUrl(string packageId,string sendTime)
        {
            StringBuilder sbQueryActionUrl = new StringBuilder();
            sbQueryActionUrl.Append("http://" + this.txtPostAddress.Text.Trim() + ":" + this.txtHttpPort.Text.Trim());
            sbQueryActionUrl.Append("/servlet/ICBCCMPAPIReqServlet?userID=");
            sbQueryActionUrl.Append(this.txtID.Text.Trim());
            sbQueryActionUrl.Append("&PackageID=" + packageId);
            sbQueryActionUrl.Append("&SendTime=" + sendTime);
            return sbQueryActionUrl.ToString();
         }

        /// <summary>
        /// 生成查询类交易的请求数据格式（post方式）
        /// </summary>
        /// <param name="交易代码"></param>
        /// <param name="包ID"></param>
        /// <param name="查询类交易XML"></param>
        /// <returns>返回查询类交易请求数据格式</returns>
        private string QueryRequestData(string qTransCode, string packageId,string querySrcXml)
        {            
            StringBuilder strQueryRequestData = new StringBuilder();
            string interfaceVersion = string.Empty;
            if (this.radbtn_0010.Checked)
            {
                interfaceVersion = "0.0.1.0";
            }
            else
            {
                interfaceVersion = "0.0.0.1";
            }
            strQueryRequestData.Append("Version=" + interfaceVersion);
            strQueryRequestData.Append("&TransCode=" + qTransCode);
            strQueryRequestData.Append("&BankCode=102&GroupCIS=" + this.txtCIS.Text.Trim());
            strQueryRequestData.Append("&ID=" + this.txtID.Text.Trim());
            strQueryRequestData.Append("&PackageID=" + packageId);
            strQueryRequestData.Append("&Cert=&reqData=" + querySrcXml);
            return strQueryRequestData.ToString();
        }

        #region 生成XML包

        /// <summary>
        /// 生成查询余额QACCBAL的包，Version=0.0.1.0
        /// </summary>
        /// <param name="sendTime"></param>
        /// <param name="packageId"></param>
        /// <returns></returns>
        private string CreateQACCBAL0010(string sendTime, string packageId)
        {
            StringBuilder strCreateQACCBAL0010 = new StringBuilder();
            strCreateQACCBAL0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>");
            strCreateQACCBAL0010.Append("QACCBAL</TransCode><CIS>" + this.txtCIS.Text.Trim());
            strCreateQACCBAL0010.Append("</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID>");
            strCreateQACCBAL0010.Append("<TranDate>" + sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno>");
            strCreateQACCBAL0010.Append("</pub><in><TotalNum>1</TotalNum><BLFlag>0</BLFlag><SynFlag>1</SynFlag><rd>");
            strCreateQACCBAL0010.Append("<iSeqno>00001</iSeqno><AccNo>" + this.txtPayAccNo.Text.Trim() + "</AccNo><CurrType></CurrType><ReqReserved3></ReqReserved3><AcctSeq></AcctSeq><MainAcctNo></MainAcctNo></rd></in></eb></CMS>");
            return strCreateQACCBAL0010.ToString();
        }

        private string CreateQPAYENT0010(string sendTime, string packageID, string sourcePayId)
        {
            StringBuilder strCreateQPAYENT0010 = new StringBuilder();
            strCreateQPAYENT0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>");
            strCreateQPAYENT0010.Append("QPAYENT</TransCode><CIS>" + this.txtCIS.Text.Trim());
            strCreateQPAYENT0010.Append("</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID>");
            strCreateQPAYENT0010.Append("<TranDate>" + sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageID + "</fSeqno>");
            strCreateQPAYENT0010.Append("</pub><in><QryfSeqno>"+sourcePayId+"</QryfSeqno>");
            strCreateQPAYENT0010.Append("<QrySerialNo></QrySerialNo></in></eb></CMS>");
            return strCreateQPAYENT0010.ToString();
        }
        /// <summary>
        /// 企业财务室指令查询0010
        /// </summary>
        /// <param name="sendTime"></param>
        /// <param name="packageID"></param>
        /// <param name="sourcePayId"></param>
        /// <returns></returns>
        private string CreateQPAYPER0010(string sendTime, string packageID, string sourcePayId)
        {
            StringBuilder strCreateQPAYPER0010 = new StringBuilder();
            strCreateQPAYPER0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>");
            strCreateQPAYPER0010.Append("QPAYPER</TransCode><CIS>" + this.txtCIS.Text.Trim());
            strCreateQPAYPER0010.Append("</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID>");
            strCreateQPAYPER0010.Append("<TranDate>" + sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageID + "</fSeqno>");
            strCreateQPAYPER0010.Append("</pub><in><QryfSeqno>" + sourcePayId + "</QryfSeqno>");
            strCreateQPAYPER0010.Append("<QrySerialNo></QrySerialNo></in></eb></CMS>");
            return strCreateQPAYPER0010.ToString();
        }
        
        // 查批扣企业0010
        private string CreateQENTDIS0010(string sendTime, string packageID, string sourceId)
        {
            StringBuilder strCreateQENTDIS0010 = new StringBuilder();
            strCreateQENTDIS0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>");
            strCreateQENTDIS0010.Append("QENTDIS</TransCode><CIS>" + this.txtCIS.Text.Trim());
            strCreateQENTDIS0010.Append("</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID>");
            strCreateQENTDIS0010.Append("<TranDate>" + sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageID + "</fSeqno>");
            strCreateQENTDIS0010.Append("</pub><in><QryfSeqno>" + sourceId + "</QryfSeqno>");
            strCreateQENTDIS0010.Append("<QrySerialNo></QrySerialNo></in></eb></CMS>");
            return strCreateQENTDIS0010.ToString();
        }

        // 查批扣个人 0001
        private string CreateQPERDIS0001(string sendTime, string packageID, string sourceId)
        {
            StringBuilder strCreateQPERDIS0001 = new StringBuilder();
            strCreateQPERDIS0001.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>");
            strCreateQPERDIS0001.Append("QPERDIS</TransCode><CIS>" + this.txtCIS.Text.Trim());
            strCreateQPERDIS0001.Append("</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID>");
            strCreateQPERDIS0001.Append("<TranDate>" + sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageID + "</fSeqno>");
            strCreateQPERDIS0001.Append("</pub><in><QryfSeqno>" + sourceId + "</QryfSeqno>");
            strCreateQPERDIS0001.Append("<QrySerialNo></QrySerialNo><ReqReserved1></ReqReserved1><ReqReserved2></ReqReserved2>");

            // strCreateQPERDIS0001.Append("<rd><iSeqno>1</iSeqno><QryiSeqno>1</QryiSeqno><QryOrderNo></QryOrderNo><ReqReserved3></ReqReserved3>");
            // strCreateQPERDIS0001.Append("<ReqReserved4></ReqReserved4></rd></in></eb></CMS>");
            // 循环去不上送任何字段，查整个包           
            strCreateQPERDIS0001.Append("</in></eb></CMS>");

            return strCreateQPERDIS0001.ToString();
        }

        /// <summary>
        /// 查询新历史明细
        /// </summary>
        /// <param name="sendTime"></param>
        /// <param name="BeginDate">格式YYYYMMDD，起始日期必须小于等于截止日期，查询起始日期和截止日期之间的跨度不能超过30天，查询起始日期最早为当前日期的前90天。</param>
        /// <param name="EndDate">格式YYYYMMDD，起始日期必须小于等于截止日期，查询起始日期和截止日期之间的跨度不能超过30天，查询起始日期最早为当前日期的前90天。</param>
        /// <param name="packageId"></param>
        /// <returns></returns>
        private string CreateQHISBAL0010(string sendTime, string packageId)
        {
            string strBeginDate = this.dtpBeginDate.Value.ToString("yyyyMMddHHmmss").Substring(0, 8);
            string strEndDate = this.dtpEndDate.Value.ToString("yyyyMMddHHmmss").Substring(0, 8);
            StringBuilder strCreateQHISBAL0010 = new StringBuilder();
            strCreateQHISBAL0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>");
            strCreateQHISBAL0010.Append("QHISBAL</TransCode><CIS>" + this.txtCIS.Text.Trim());
            strCreateQHISBAL0010.Append("</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID>");
            strCreateQHISBAL0010.Append("<TranDate>" + sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno>");
            strCreateQHISBAL0010.Append("</pub><in><AccNo>" + this.txtPayAccNo.Text.Trim() + "</AccNo><CurrType></CurrType>");
            strCreateQHISBAL0010.Append("<BeginDate>" + strBeginDate + "</BeginDate><EndDate>" + strEndDate + "</EndDate><AcctSeq></AcctSeq></in></eb></CMS>");
            return strCreateQHISBAL0010.ToString();
        }

        /// <summary>
        /// 行名行号信息下载
        /// </summary>
        /// <param name="sendTime"></param>
        /// <param name="packageId"></param>
        /// <returns></returns>
        private string CreateDIBPSBC0001(string sendTime, string packageId, string nextFlag)
        {
            StringBuilder strCreateDIBPSBC0001 = new StringBuilder();
            strCreateDIBPSBC0001.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>");
            strCreateDIBPSBC0001.Append("DIBPSBC</TransCode><CIS>" + this.txtCIS.Text.Trim());
            strCreateDIBPSBC0001.Append("</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID>");
            strCreateDIBPSBC0001.Append("<TranDate>" + sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno>");
            strCreateDIBPSBC0001.Append("</pub><in><BnkCode></BnkCode><NextTag>" + nextFlag + "</NextTag><ReqReserved1></ReqReserved1><ReqReserved2></ReqReserved2></in></eb></CMS>");

            return strCreateDIBPSBC0001.ToString();
        }

        // 管家卡明细查询 QRYGJDTL0010  组包  管家卡卡号放在收款账号那个文本框
        private string CreateQRYGJDTL0010(string sendTime,string packageId, string nextFlag)
        {
            string strBeginDate = this.dtpBeginDate.Value.ToString("yyyyMMddHHmmss").Substring(0, 8);
            string strEndDate = this.dtpEndDate.Value.ToString("yyyyMMddHHmmss").Substring(0, 8);
            StringBuilder strCreateQRYGJDTL0010 = new StringBuilder();
            strCreateQRYGJDTL0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>");
            strCreateQRYGJDTL0010.Append("QRYGJDTL</TransCode><CIS>" + this.txtCIS.Text.Trim());
            strCreateQRYGJDTL0010.Append("</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID>");
            strCreateQRYGJDTL0010.Append("<TranDate>" + sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno>");
            strCreateQRYGJDTL0010.Append("</pub><in>");
            strCreateQRYGJDTL0010.Append("<AcctNo>" + this.txtPayAccNo.Text.Trim() + "</AcctNo><StartDate>" + strBeginDate + "</StartDate><EndDate>" + strEndDate + "</EndDate>");
            strCreateQRYGJDTL0010.Append("<NextTag>" + nextFlag + "</NextTag><AcctSeq></AcctSeq><CardNo>"+this.txtRecAccNo.Text.Trim()+"</CardNo><CurrType>001</CurrType>");
            strCreateQRYGJDTL0010.Append("</in></eb></CMS>");
            return strCreateQRYGJDTL0010.ToString();
        }


        private string CreateQHISD0010(string sendTime,string packageId,string nextFlag)
        {
            string strBeginDate = this.dtpBeginDate.Value.ToString("yyyyMMddHHmmss").Substring(0,8);
            string strEndDate = this.dtpEndDate.Value.ToString("yyyyMMddHHmmss").Substring(0,8);
            StringBuilder strCreateQHISD0010 = new StringBuilder();
            strCreateQHISD0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub><TransCode>");
            strCreateQHISD0010.Append("QHISD</TransCode><CIS>" + this.txtCIS.Text.Trim());
            strCreateQHISD0010.Append("</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID>");
            strCreateQHISD0010.Append("<TranDate>" + sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno>");
            strCreateQHISD0010.Append("</pub><in>");
            strCreateQHISD0010.Append("<AccNo>" + this.txtPayAccNo.Text.Trim() + "</AccNo><BeginDate>" + strBeginDate + "</BeginDate><EndDate>" + strEndDate + "</EndDate>");
            strCreateQHISD0010.Append("<MinAmt>0</MinAmt><MaxAmt>99999999999999999</MaxAmt><BankType>102</BankType>");
            strCreateQHISD0010.Append("<NextTag>" + nextFlag + "</NextTag><CurrType>001</CurrType><DueBillNo></DueBillNo>");
            strCreateQHISD0010.Append("<AcctSeq></AcctSeq><ComplementFlag></ComplementFlag><CardAccNoDef></CardAccNoDef>");
            strCreateQHISD0010.Append("<DesByTime></DesByTime></in></eb></CMS>");
            return strCreateQHISD0010.ToString();
        }

        private string CreateQPD0010(string sendTime, string packageId, string nextFlag)
        {
            StringBuilder strCreateQPD0010 = new StringBuilder();
            strCreateQPD0010.Append("<?xml version='1.0' encoding='GBK'?><CMS><eb><pub>");
            strCreateQPD0010.Append("<TransCode>QPD</TransCode><CIS>" + this.txtCIS.Text.Trim() + "</CIS>");
            strCreateQPD0010.Append("<BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID>");
            strCreateQPD0010.Append("<TranDate>" + sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno>");
            strCreateQPD0010.Append("</pub><in><AccNo>" + this.txtPayAccNo.Text.Trim() + "</AccNo>");
            strCreateQPD0010.Append("<AreaCode></AreaCode><MinAmt>0</MinAmt><MaxAmt>99999999999999999</MaxAmt>");
            strCreateQPD0010.Append("<BeginTime></BeginTime><EndTime></EndTime>");
            strCreateQPD0010.Append("<NextTag>" + nextFlag + "</NextTag><ReqReserved1></ReqReserved1><ReqReserved2></ReqReserved2><CurrType></CurrType><AcctSeq></AcctSeq></in></eb></CMS>");        
            return strCreateQPD0010.ToString();
        }

        private string CreatePAYENT0010(string sendTime, string packageId)
        {
            StringBuilder sbCreatePAYENT0010 = new StringBuilder();
            sbCreatePAYENT0010.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>PAYENT</TransCode><CIS>");
            sbCreatePAYENT0010.Append(this.txtCIS.Text.Trim() + "</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID><TranDate>");
            sbCreatePAYENT0010.Append(sendTime.Substring(0,8) + "</TranDate><TranTime>" + sendTime.Substring(8,9) + "</TranTime><fSeqno>" + packageId + "</fSeqno></pub><in>");
            sbCreatePAYENT0010.Append("<OnlBatF>1</OnlBatF><SettleMode>0</SettleMode><TotalNum>1</TotalNum><TotalAmt>5</TotalAmt>");
            sbCreatePAYENT0010.Append("<SignTime>" + sendTime + "</SignTime><ReqReserved1></ReqReserved1><ReqReserved2></ReqReserved2>");
            sbCreatePAYENT0010.Append("<AlertFlag>1</AlertFlag><rd><iSeqno>00001</iSeqno><ReimburseNo>00001</ReimburseNo>");
            sbCreatePAYENT0010.Append("<ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime><PayType>1</PayType>");
            sbCreatePAYENT0010.Append("<PayAccNo>" + this.txtPayAccNo.Text.Trim() + "</PayAccNo><PayAccNameCN>" + this.txtPayAccName.Text.Trim() + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
            sbCreatePAYENT0010.Append("<RecAccNo>" + this.txtRecAccNo.Text.Trim() + "</RecAccNo><RecAccNameCN>" + this.txtRecAccName.Text.Trim() + "</RecAccNameCN><RecAccNameEN></RecAccNameEN>");
            sbCreatePAYENT0010.Append("<SysIOFlg>1</SysIOFlg><IsSameCity></IsSameCity><Prop></Prop><RecICBCCode></RecICBCCode>");
            sbCreatePAYENT0010.Append("<RecCityName>长沙</RecCityName><RecBankNo></RecBankNo><RecBankName>工商银行长沙测试支行</RecBankName><CurrType>001</CurrType>");
            sbCreatePAYENT0010.Append("<PayAmt>5</PayAmt><UseCode></UseCode><UseCN>测试支付款</UseCN><EnSummary></EnSummary>");
            sbCreatePAYENT0010.Append("<PostScript>附言</PostScript><Summary>Summary摘要</Summary><Ref>RefNo</Ref><Oref>OrefNo</Oref>");
            sbCreatePAYENT0010.Append("<ERPSqn></ERPSqn><BusCode></BusCode><ERPcheckno></ERPcheckno><CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo><BankType></BankType>");
            sbCreatePAYENT0010.Append("<FileNames></FileNames><Indexs></Indexs><PaySubNo></PaySubNo><RecSubNo></RecSubNo><MCardNo></MCardNo><MCardName></MCardName></rd></in></eb></CMS>");

            return sbCreatePAYENT0010.ToString();
        }

        private string CreateGjkPAYENT0010(string sendTime,string packageId)
        {
            StringBuilder sbCreateGjkPAYENT0010 = new StringBuilder();
            sbCreateGjkPAYENT0010.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>PAYENT</TransCode><CIS>");
            sbCreateGjkPAYENT0010.Append(this.txtCIS.Text.Trim() + "</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID><TranDate>");
            sbCreateGjkPAYENT0010.Append(sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno></pub><in>");
            sbCreateGjkPAYENT0010.Append("<OnlBatF>1</OnlBatF><SettleMode>0</SettleMode><TotalNum>1</TotalNum><TotalAmt>2</TotalAmt>");
            sbCreateGjkPAYENT0010.Append("<SignTime>" + sendTime + "</SignTime><ReqReserved1></ReqReserved1><ReqReserved2></ReqReserved2>");
            sbCreateGjkPAYENT0010.Append("<AlertFlag>1</AlertFlag><rd><iSeqno>00001</iSeqno><ReimburseNo>00001</ReimburseNo>");
            sbCreateGjkPAYENT0010.Append("<ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime><PayType>1</PayType>");
            sbCreateGjkPAYENT0010.Append("<PayAccNo>" + this.txtPayAccNo.Text.Trim() + "</PayAccNo><PayAccNameCN>" + this.txtPayAccName.Text.Trim() + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
            sbCreateGjkPAYENT0010.Append("<RecAccNo>" + this.txtRecAccNo.Text.Trim() + "</RecAccNo><RecAccNameCN>" + this.txtRecAccName.Text.Trim() + "</RecAccNameCN><RecAccNameEN></RecAccNameEN>");
            sbCreateGjkPAYENT0010.Append("<SysIOFlg>1</SysIOFlg><IsSameCity></IsSameCity><Prop></Prop><RecICBCCode></RecICBCCode>");
            sbCreateGjkPAYENT0010.Append("<RecCityName>长沙</RecCityName><RecBankNo></RecBankNo><RecBankName>工商银行长沙测试支行</RecBankName><CurrType>001</CurrType>");
            sbCreateGjkPAYENT0010.Append("<PayAmt>2</PayAmt><UseCode></UseCode><UseCN>测试管家卡付款</UseCN><EnSummary></EnSummary>");
            sbCreateGjkPAYENT0010.Append("<PostScript>2020年2月20日管家卡付款</PostScript><Summary>Summary管家卡</Summary><Ref>RefNo</Ref><Oref>OrefNo</Oref>");
            sbCreateGjkPAYENT0010.Append("<ERPSqn></ERPSqn><BusCode></BusCode><ERPcheckno></ERPcheckno><CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo><BankType></BankType>");
            sbCreateGjkPAYENT0010.Append("<FileNames></FileNames><Indexs></Indexs><PaySubNo></PaySubNo><RecSubNo></RecSubNo><MCardNo>9558851901000246063</MCardNo><MCardName>烟草公司常德</MCardName></rd></in></eb></CMS>");

            return sbCreateGjkPAYENT0010.ToString();
        }
        // 自定义付款笔数 PAYENT
        private string CreatePayent0010CustomizeCount(string sendTime, string packageId)
        {
            StringBuilder sbCreatePayent0010CustomizeCount = new StringBuilder();
            string strCustomizeCount = this.txtCustomizeCount.Text.Trim();
            int intCustomizeCount = Int32.Parse(strCustomizeCount);
            sbCreatePayent0010CustomizeCount.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>PAYENT</TransCode><CIS>");
            sbCreatePayent0010CustomizeCount.Append(this.txtCIS.Text.Trim() + "</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID><TranDate>");
            sbCreatePayent0010CustomizeCount.Append(sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno></pub><in>");
            // <OnlBatF>表示联机标志，只能选择1；  <SettleMode>  入账方式  2表示并笔记账，0表示逐笔记账
            sbCreatePayent0010CustomizeCount.Append("<OnlBatF>1</OnlBatF><SettleMode>2</SettleMode><TotalNum>" + strCustomizeCount + "</TotalNum><TotalAmt>" + strCustomizeCount + "</TotalAmt>");
            sbCreatePayent0010CustomizeCount.Append("<SignTime>" + sendTime + "</SignTime><ReqReserved1></ReqReserved1><ReqReserved2></ReqReserved2>");
            string srcRds = CreateRdArea0010(intCustomizeCount);
            sbCreatePayent0010CustomizeCount.Append(srcRds + "</in></eb></CMS>");
            return sbCreatePayent0010CustomizeCount.ToString();
        }

        private string CreatePayent0010CustomizeCountOtherBank(string sendTime, string packageId)
        {
            StringBuilder sbCreatePayent0010CustomizeCountOtherBank = new StringBuilder();
            string strCustomizeCount = this.txtCustomizeCount.Text.Trim();
            int intCustomizeCount = Int32.Parse(strCustomizeCount);
            sbCreatePayent0010CustomizeCountOtherBank.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>PAYENT</TransCode><CIS>");
            sbCreatePayent0010CustomizeCountOtherBank.Append(this.txtCIS.Text.Trim() + "</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID><TranDate>");
            sbCreatePayent0010CustomizeCountOtherBank.Append(sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno></pub><in>");
            // <OnlBatF>表示联机标志，只能选择1；  <SettleMode>  入账方式  2表示并笔记账，0表示逐笔记账
            sbCreatePayent0010CustomizeCountOtherBank.Append("<OnlBatF>1</OnlBatF><SettleMode>0</SettleMode><TotalNum>" + strCustomizeCount + "</TotalNum><TotalAmt>" + strCustomizeCount + "</TotalAmt>");
            sbCreatePayent0010CustomizeCountOtherBank.Append("<SignTime>" + sendTime + "</SignTime><ReqReserved1></ReqReserved1><ReqReserved2></ReqReserved2>");
            string srcRdsOherBank = CreateRdArea0010OtherBank(intCustomizeCount);
            sbCreatePayent0010CustomizeCountOtherBank.Append(srcRdsOherBank + "</in></eb></CMS>");
            return sbCreatePayent0010CustomizeCountOtherBank.ToString();
        }

        private string CreateENTDISCOL0010(string sendTime, string packageId)
        {
            StringBuilder sbCreateENTDISCOL0010 = new StringBuilder();
            sbCreateENTDISCOL0010.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>ENTDISCOL</TransCode><CIS>");
            sbCreateENTDISCOL0010.Append(this.txtCIS.Text.Trim() + "</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID><TranDate>");
            sbCreateENTDISCOL0010.Append(sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno></pub><in>");
            sbCreateENTDISCOL0010.Append("<OnlBatF>1</OnlBatF><BusType>0</BusType><ContrNo>" + this.txtContrNo.Text + "</ContrNo>");
            sbCreateENTDISCOL0010.Append("<RecAccNo>" + this.txtPayAccNo.Text + "</RecAccNo><RecAccNameCN>" + this.txtPayAccName.Text + "</RecAccNameCN>");
            sbCreateENTDISCOL0010.Append("<RecAccNameEN></RecAccNameEN><PayType>1</PayType><TotalNum>1</TotalNum><TotalAmt>5000000</TotalAmt>"); // paytype1表示加急，目前只支持1
            sbCreateENTDISCOL0010.Append("<TotalSummary>火鸟批扣企业测试</TotalSummary><SignTime>" + sendTime + "</SignTime>");  // TotalSummary 20个字符
            sbCreateENTDISCOL0010.Append("<rd><iSeqno>1</iSeqno><PayAccNo>" + this.txtRecAccNo.Text + "</PayAccNo><PayAccNameCN>" + this.txtRecAccName.Text + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
            sbCreateENTDISCOL0010.Append("<PayBranch>工行长沙测试专用支行</PayBranch><CurrType>001</CurrType><PayAmt>5000000</PayAmt><UseCode>000</UseCode>");
            sbCreateENTDISCOL0010.Append("<UseCN>UseCn火鸟测试批扣企业</UseCN><EnSummary>EnSummary</EnSummary><PostScript>PostScript附言火鸟批扣企业测试</PostScript><Summary>Summary摘要</Summary>");
            sbCreateENTDISCOL0010.Append("<Ref>RefNo</Ref><Oref>OrefNo</Oref><ERPSqn>ERPSqn</ERPSqn><BusCode>BusNo</BusCode>");
            sbCreateENTDISCOL0010.Append("<ERPcheckno>ERPckno</ERPcheckno><CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo></rd></in></eb></CMS>");

            return sbCreateENTDISCOL0010.ToString();

        }
        /// <summary>
        /// 企业财务室汇总记账PAYPERCOL  0010
        /// </summary>
        /// <param name="sendTime"></param>
        /// <param name="packageID"></param>
        /// <returns></returns>
        private string CreatePAYPERCOL0010(string sendTime,string packageId)
        {
            StringBuilder sbCreatePAYPERCOL0010 = new StringBuilder();
            string strCustomizeCount = this.txtCustomizeCount.Text.Trim();
            int intCustomizeCount = Int32.Parse(strCustomizeCount);

            sbCreatePAYPERCOL0010.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>PAYPERCOL</TransCode><CIS>");
            sbCreatePAYPERCOL0010.Append(this.txtCIS.Text.Trim() + "</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID><TranDate>");
            sbCreatePAYPERCOL0010.Append(sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno></pub><in>");
            // OnlBatF  联机批量标志  1：联机  2:批量        ；BusType   业务种类编号    10年8月份从1-99改为支持1-999
            sbCreatePAYPERCOL0010.Append("<OnlBatF>2</OnlBatF><BusType>999</BusType>");
            sbCreatePAYPERCOL0010.Append("<TotalNum>" + intCustomizeCount.ToString() + "</TotalNum><TotalAmt>" + intCustomizeCount.ToString() + "</TotalAmt><SignTime>" + sendTime + "</SignTime>");
            sbCreatePAYPERCOL0010.Append("<TotalSummary>PAYPERCOL汇总摘要</TotalSummary><THBaseAccFlag></THBaseAccFlag><RegSerialNO></RegSerialNO><PackageName>批量包别名小J</PackageName>"); // paytype1表示加急，目前只支持1
            string strPaypercolRD = CreatePaypercolRdArea0010(intCustomizeCount);
            sbCreatePAYPERCOL0010.Append(strPaypercolRD + "</in></eb></CMS>");

            return sbCreatePAYPERCOL0010.ToString();

        }
        /// <summary>
        /// 企业财务室提交PAYPER  0010
        /// </summary>
        /// <param name="sendTime"></param>
        /// <param name="packageID"></param>
        /// <returns></returns>
        private string CreatePAYPER0010(string sendTime, string packageId)
        {
            StringBuilder sbCreatePAYPER0010 = new StringBuilder();
            string strCustomizeCount = this.txtCustomizeCount.Text.Trim();
            int intCustomizeCount = Int32.Parse(strCustomizeCount);

            sbCreatePAYPER0010.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>PAYPER</TransCode><CIS>");
            sbCreatePAYPER0010.Append(this.txtCIS.Text.Trim() + "</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID><TranDate>");
            sbCreatePAYPER0010.Append(sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno></pub><in>");
            // OnlBatF  联机批量标志  1：联机  2:批量        ；SettleMode  2：并笔记账 0：逐笔记账
            sbCreatePAYPER0010.Append("<OnlBatF>1</OnlBatF><SettleMode>2</SettleMode>");
            sbCreatePAYPER0010.Append("<TotalNum>" + intCustomizeCount.ToString() + "</TotalNum><TotalAmt>" + intCustomizeCount.ToString() + "</TotalAmt><SignTime>" + sendTime + "</SignTime>");
            sbCreatePAYPER0010.Append("<THBaseAccFlag>0</THBaseAccFlag><RegSerialNO></RegSerialNO>");
            sbCreatePAYPER0010.Append("<PackageName>批量包别名小J</PackageName>"); 
            string strPayperRD = CreatePayperRdArea0010(intCustomizeCount);
            sbCreatePAYPER0010.Append(strPayperRD + "</in></eb></CMS>");

            return sbCreatePAYPER0010.ToString();
        }

        private string CreatePERDISCOL0001(string sendTime, string packageId)
        {
            StringBuilder sbCreatePERDISCOL0001 = new StringBuilder();
            sbCreatePERDISCOL0001.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>PERDISCOL</TransCode><CIS>");
            sbCreatePERDISCOL0001.Append(this.txtCIS.Text.Trim() + "</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID><TranDate>");
            sbCreatePERDISCOL0001.Append(sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno></pub><in>");
            sbCreatePERDISCOL0001.Append("<OnlBatF>1</OnlBatF><BusType>0</BusType><RecAccNo>" + this.txtPayAccNo.Text + "</RecAccNo>");
            sbCreatePERDISCOL0001.Append("<RecAccNameCN>" + this.txtPayAccName.Text + "</RecAccNameCN><RecAccNameEN></RecAccNameEN><TotalNum>1</TotalNum><TotalAmt>19</TotalAmt><SignTime>" + sendTime + "</SignTime>");
            // ReqReserved1 :09年5月份为了配合深圳平安的业务需求，启用该字段。如果上送9，则实现实时扣个人，批量入企业账方式
            // ReqReserved2 :前20位上送“汇总摘要”，现阶段程序会校验该字段长度不得超过20位
            sbCreatePERDISCOL0001.Append("<ReqReserved1>9</ReqReserved1><ReqReserved2>火鸟扣个人ReqReserv2</ReqReserved2><rd>"); // paytype1表示加急，目前只支持1
            sbCreatePERDISCOL0001.Append("<iSeqno>1</iSeqno><PayAccNo>" + this.txtRecAccNo.Text + "</PayAccNo><PayAccNameCN>" + this.txtRecAccName.Text + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");  // TotalSummary 20个字符
            sbCreatePERDISCOL0001.Append("<PayBranch>工行长沙测试专用支行</PayBranch><Portno>" + this.txtPersonNo.Text + "</Portno><ContractNo>" + this.txtContractNo.Text + "</ContractNo><CurrType>001</CurrType>");
            sbCreatePERDISCOL0001.Append("<PayAmt>19</PayAmt><UseCode></UseCode><UseCN>火鸟扣个人UseCN</UseCN><EnSummary></EnSummary>");
            sbCreatePERDISCOL0001.Append("<PostScript>火鸟扣个人附言</PostScript><Summary>火鸟扣个人摘要</Summary><Ref>RefNo</Ref><Oref>OrefNo</Oref>");
            sbCreatePERDISCOL0001.Append("<ERPSqn>ERPNo</ERPSqn><BusCode>busNo</BusCode><ERPcheckno>ERPchkNo</ERPcheckno>");
            sbCreatePERDISCOL0001.Append("<CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo><ReqReserved3></ReqReserved3>");
            sbCreatePERDISCOL0001.Append("<ReqReserved4></ReqReserved4></rd></in></eb></CMS>");
            return sbCreatePERDISCOL0001.ToString();

        }

        private string CreatePAYENTWithZip0010(string sendTime, string packageId)
        {
            StringBuilder sbCreatePAYENTWithZip0010 = new StringBuilder();
            sbCreatePAYENTWithZip0010.Append("<?xml version='1.0' encoding='GB2312'?><CMS><eb><pub><TransCode>PAYENT</TransCode><CIS>");
            sbCreatePAYENTWithZip0010.Append(this.txtCIS.Text.Trim() + "</CIS><BankCode>102</BankCode><ID>" + this.txtID.Text.Trim() + "</ID><TranDate>");
            sbCreatePAYENTWithZip0010.Append(sendTime.Substring(0, 8) + "</TranDate><TranTime>" + sendTime.Substring(8, 9) + "</TranTime><fSeqno>" + packageId + "</fSeqno></pub><in>");
            // <OnlBatF>表示联机标志，只能选择1；  <SettleMode>  入账方式  2表示并笔记账，0表示逐笔记账
            sbCreatePAYENTWithZip0010.Append("<OnlBatF>1</OnlBatF><SettleMode>2</SettleMode><TotalNum>1000</TotalNum><TotalAmt>1000</TotalAmt>");
            sbCreatePAYENTWithZip0010.Append("<SignTime>" + sendTime + "</SignTime><ReqReserved1></ReqReserved1><ReqReserved2></ReqReserved2><zip>");
            string srcRds = CreateRdArea();
            // 对循环区域压缩并base64编码
            string srcRdsZip = string.Empty;
            string srcRdsZipBase64 = string.Empty;
            //srcRdsZip = tools.ZipAndBase64.Compress(srcRds);
            //srcRdsZipBase64 = tools.Base64AndUnbase64.EncodeBase64("gb2312", srcRdsZip);
            srcRdsZipBase64 = tools.ZipHelper.GZipCompressString(srcRds);
            sbCreatePAYENTWithZip0010.Append(srcRdsZipBase64+ "</zip></in></eb></CMS>");
            return sbCreatePAYENTWithZip0010.ToString();
        }
        // 1000笔  工行内付款   加急
        private string CreateRdArea()
        {
            StringBuilder sbRdArea = new StringBuilder();                 
            for (int i = 0; i < 1000; i++)
            {
                sbRdArea.Append("<rd><iSeqno>" + (i + 1).ToString() + "</iSeqno><ReimburseNo></ReimburseNo><ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime>");
                // 1：加急 2：普通（工行异地人民币转账不再区分普通\加急,统一按加急处理） 3：跨行快汇（当涉及账户管家账户转账时，记账方式不支持普通，工行异地转账不再区分普通\加急,统一按加急处理）
                sbRdArea.Append("<PayType>1</PayType><PayAccNo>" + this.txtPayAccNo.Text.Trim() + "</PayAccNo><PayAccNameCN>" + this.txtPayAccName.Text.Trim() + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
                sbRdArea.Append("<RecAccNo>" + this.txtRecAccNo.Text.Trim() + "</RecAccNo><RecAccNameCN>" + this.txtRecAccName.Text.Trim() + "</RecAccNameCN><RecAccNameEN></RecAccNameEN>");
                //  1：系统内  2：系统外            1：同城  2：异地        跨行必输  0：对公账户  1：个人账户
                sbRdArea.Append("<SysIOFlg>1</SysIOFlg><IsSameCity>1</IsSameCity><Prop>1</Prop>");
                sbRdArea.Append("<RecICBCCode></RecICBCCode><RecCityName>长沙</RecCityName><RecBankNo></RecBankNo>");
                sbRdArea.Append("<RecBankName>中国工商银行长沙某测试支行</RecBankName><CurrType>001</CurrType><PayAmt>1</PayAmt>");
                sbRdArea.Append("<UseCode></UseCode><UseCN>工资</UseCN><EnSummary></EnSummary>");
                sbRdArea.Append("<PostScript>测试附言PostScript</PostScript><Summary>摘要Summary</Summary><Ref>RefNo</Ref><Oref>OrefNo</Oref>");
                sbRdArea.Append("<ERPSqn></ERPSqn><BusCode>001</BusCode><ERPcheckno></ERPcheckno>");
                sbRdArea.Append("<CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo>");
                sbRdArea.Append("<BankType>102</BankType><FileNames></FileNames><Indexs></Indexs><PaySubNo></PaySubNo><RecSubNo></RecSubNo>");
                sbRdArea.Append("<MCardNo>9558851901000246741</MCardNo><MCardName>永州烟草公司</MCardName></rd>");
            }
            return sbRdArea.ToString();
        }

        /// <summary>
        /// 创建PAYENT付款RD区域
        /// </summary>
        /// <param name="intCustomizeCount"></param>
        /// <returns></returns>
        private string CreateRdArea0010(int intCustomizeCount)
        {
            StringBuilder sbRdArea = new StringBuilder();
            for (int i = 0; i < intCustomizeCount; i++)
            {
                sbRdArea.Append("<rd><iSeqno>" + (i + 1).ToString() + "</iSeqno><ReimburseNo></ReimburseNo><ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime>");
                // 1：加急 2：普通（工行异地人民币转账不再区分普通\加急,统一按加急处理） 3：跨行快汇（当涉及账户管家账户转账时，记账方式不支持普通，工行异地转账不再区分普通\加急,统一按加急处理）
                sbRdArea.Append("<PayType>1</PayType><PayAccNo>" + this.txtPayAccNo.Text.Trim() + "</PayAccNo><PayAccNameCN>" + this.txtPayAccName.Text.Trim() + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
                sbRdArea.Append("<RecAccNo>" + this.txtRecAccNo.Text.Trim() + "</RecAccNo><RecAccNameCN>" + this.txtRecAccName.Text.Trim() + "</RecAccNameCN><RecAccNameEN></RecAccNameEN>");
                //  1：系统内  2：系统外            1：同城  2：异地        跨行必输  0：对公账户  1：个人账户
                sbRdArea.Append("<SysIOFlg>1</SysIOFlg><IsSameCity>1</IsSameCity><Prop>1</Prop>");
                sbRdArea.Append("<RecICBCCode></RecICBCCode><RecCityName>长沙</RecCityName><RecBankNo></RecBankNo>");
                sbRdArea.Append("<RecBankName>中国工商银行长沙某测试支行</RecBankName><CurrType>001</CurrType><PayAmt>1</PayAmt>");
                sbRdArea.Append("<UseCode></UseCode><UseCN>工资</UseCN><EnSummary></EnSummary>");
                sbRdArea.Append("<PostScript>测试附言PostScript</PostScript><Summary>摘要Summary</Summary><Ref>RefNo</Ref><Oref>OrefNo</Oref>");
                sbRdArea.Append("<ERPSqn></ERPSqn><BusCode>001</BusCode><ERPcheckno></ERPcheckno>");
                sbRdArea.Append("<CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo>");
                sbRdArea.Append("<BankType>102</BankType><FileNames></FileNames><Indexs></Indexs><PaySubNo></PaySubNo><RecSubNo></RecSubNo>");
                sbRdArea.Append("<MCardNo>9558851901000246741</MCardNo><MCardName>永州烟草公司</MCardName></rd>");
            }
            return sbRdArea.ToString();
        }

        /// <summary>
        /// 创建Paypercol付款RD区域
        /// </summary>
        /// <param name="intCustomizeCount"></param>
        /// <returns></returns>
        private string CreatePaypercolRdArea0010(int intCustomizeCount)
        {
            StringBuilder sbPaypercolRdArea = new StringBuilder();
            for (int i = 0; i < intCustomizeCount; i++)
            {
                sbPaypercolRdArea.Append("<rd><iSeqno>" + (i + 1).ToString() + "</iSeqno><ReimburseNo></ReimburseNo><ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime>");
                // 1：加急 2：普通（工行异地人民币转账不再区分普通\加急,统一按加急处理） 3：跨行快汇（当涉及账户管家账户转账时，记账方式不支持普通，工行异地转账不再区分普通\加急,统一按加急处理）
                sbPaypercolRdArea.Append("<PayType>1</PayType><PayAccNo>" + this.txtPayAccNo.Text.Trim() + "</PayAccNo><PayAccNameCN>" + this.txtPayAccName.Text.Trim() + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
                sbPaypercolRdArea.Append("<RecAccNo>" + this.txtRecAccNo.Text.Trim() + "</RecAccNo><RecAccNameCN>" + this.txtRecAccName.Text.Trim() + "</RecAccNameCN><RecAccNameEN></RecAccNameEN>");
                //  1：系统内  2：系统外            1：同城  2：异地        跨行必输  0：对公账户  1：个人账户
                sbPaypercolRdArea.Append("<SysIOFlg>1</SysIOFlg><IsSameCity>1</IsSameCity>");
                sbPaypercolRdArea.Append("<RecICBCCode></RecICBCCode><RecCityName>长沙</RecCityName><RecBankNo></RecBankNo>");
                sbPaypercolRdArea.Append("<RecBankName>中国工商银行长沙某测试支行</RecBankName><CurrType>001</CurrType><PayAmt>1</PayAmt>");
                sbPaypercolRdArea.Append("<UseCode></UseCode><UseCN>工资</UseCN><EnSummary></EnSummary>");
                sbPaypercolRdArea.Append("<PostScript>PostScript附言</PostScript><Summary>Summary摘要</Summary>");
                sbPaypercolRdArea.Append("<Ref>RefNo</Ref><Oref>OrefNo</Oref>");
                sbPaypercolRdArea.Append("<ERPSqn>ERPSqn</ERPSqn><BusCode></BusCode><ERPcheckno>ERPckNO</ERPcheckno><CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo>CrvouhNo</CrvouhNo>");
                sbPaypercolRdArea.Append("</rd>");
            }
            return sbPaypercolRdArea.ToString();
        }

        /// <summary>
        /// 创建Paypercol付款RD区域
        /// </summary>
        /// <param name="intCustomizeCount"></param>
        /// <returns></returns>
        private string CreatePayperRdArea0010(int intCustomizeCount)
        {
            StringBuilder sbPayperRdArea = new StringBuilder();
            for (int i = 0; i < intCustomizeCount; i++)
            {
                sbPayperRdArea.Append("<rd><iSeqno>" + (i + 1).ToString() + "</iSeqno><ReimburseNo></ReimburseNo><ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime>");
                // 1：加急 2：普通（工行异地人民币转账不再区分普通\加急,统一按加急处理） 3：跨行快汇（当涉及账户管家账户转账时，记账方式不支持普通，工行异地转账不再区分普通\加急,统一按加急处理）
                sbPayperRdArea.Append("<PayType>1</PayType><PayAccNo>" + this.txtPayAccNo.Text.Trim() + "</PayAccNo><PayAccNameCN>" + this.txtPayAccName.Text.Trim() + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
                sbPayperRdArea.Append("<RecAccNo>" + this.txtRecAccNo.Text.Trim() + "</RecAccNo><RecAccNameCN>" + this.txtRecAccName.Text.Trim() + "</RecAccNameCN><RecAccNameEN></RecAccNameEN>");
                //  1：系统内  2：系统外            1：同城  2：异地        跨行必输  0：对公账户  1：个人账户
                sbPayperRdArea.Append("<SysIOFlg>1</SysIOFlg><IsSameCity>1</IsSameCity>");
                sbPayperRdArea.Append("<RecICBCCode></RecICBCCode><RecCityName>长沙</RecCityName><RecBankNo></RecBankNo>");
                sbPayperRdArea.Append("<RecBankName>中国工商银行长沙某测试支行</RecBankName><CurrType>001</CurrType><PayAmt>1</PayAmt>");
                sbPayperRdArea.Append("<UseCode></UseCode><UseCN>工资</UseCN><EnSummary></EnSummary>");
                sbPayperRdArea.Append("<PostScript>PostScript附言</PostScript><Summary>Summary摘要</Summary>");
                sbPayperRdArea.Append("<Ref>RefNo</Ref><Oref>OrefNo</Oref>");
                sbPayperRdArea.Append("<ERPSqn>ERPSqn</ERPSqn><BusCode></BusCode><ERPcheckno>ERPckNO</ERPcheckno><CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo>CrvouhNo</CrvouhNo>");
                sbPayperRdArea.Append("<MCardNo>9558851901000246741</MCardNo><MCardName>永州烟草公司</MCardName></rd>");
            }
            return sbPayperRdArea.ToString();
        }


        private string CreateRdArea0010OtherBank(int intCustomizeCount)
        {
            StringBuilder sbRdArea = new StringBuilder();
            for (int i = 0; i < intCustomizeCount; i++)
            {
                sbRdArea.Append("<rd><iSeqno>" + (i + 1).ToString() + "</iSeqno><ReimburseNo></ReimburseNo><ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime>");
                // 1：加急 2：普通（工行异地人民币转账不再区分普通\加急,统一按加急处理） 3：跨行快汇（当涉及账户管家账户转账时，记账方式不支持普通，工行异地转账不再区分普通\加急,统一按加急处理）
                sbRdArea.Append("<PayType>2</PayType><PayAccNo>" + this.txtPayAccNo.Text.Trim() + "</PayAccNo><PayAccNameCN>" + this.txtPayAccName.Text.Trim() + "</PayAccNameCN><PayAccNameEN></PayAccNameEN>");
                sbRdArea.Append("<RecAccNo>6213662173600339136</RecAccNo><RecAccNameCN>陈勇</RecAccNameCN><RecAccNameEN></RecAccNameEN>");
                //  1：系统内  2：系统外            1：同城  2：异地        跨行必输  0：对公账户  1：个人账户
                sbRdArea.Append("<SysIOFlg>2</SysIOFlg><IsSameCity>1</IsSameCity><Prop>1</Prop>");
                sbRdArea.Append("<RecICBCCode></RecICBCCode><RecCityName>常德市</RecCityName><RecBankNo>313558771020</RecBankNo>");
                sbRdArea.Append("<RecBankName>华融湘江银行股份有限公司津市支行</RecBankName><CurrType>001</CurrType><PayAmt>1</PayAmt>");
                sbRdArea.Append("<UseCode></UseCode><UseCN>工资</UseCN><EnSummary></EnSummary>");
                sbRdArea.Append("<PostScript>测试附言PostScript</PostScript><Summary>摘要Summary</Summary><Ref>RefNo</Ref><Oref>OrefNo</Oref>");
                sbRdArea.Append("<ERPSqn></ERPSqn><BusCode>001</BusCode><ERPcheckno></ERPcheckno>");
                sbRdArea.Append("<CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo>");
                sbRdArea.Append("<BankType>102</BankType><FileNames></FileNames><Indexs></Indexs><PaySubNo></PaySubNo><RecSubNo></RecSubNo>");
                sbRdArea.Append("<MCardNo></MCardNo><MCardName></MCardName></rd>");
            }
            return sbRdArea.ToString();
        }


        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSignToXML_Click(object sender, EventArgs e)
        {
            string xml = "<?xml version='1.0' encoding='gbk'?><CMS><eb><pub><TransCode>PAYENT</TransCode><CIS>190190000087388</CIS><BankCode>102</BankCode><ID>YQHL201811.y.1901</ID><TranDate>20181201</TranDate><TranTime>093808752</TranTime><fSeqno>201812024880961</fSeqno></pub><in><OnlBatF>1</OnlBatF><SettleMode>0</SettleMode><TotalNum>1</TotalNum><TotalAmt>123</TotalAmt><SignTime>20181202113835903</SignTime><ReqReserved1></ReqReserved1><ReqReserved2></ReqReserved2><AlertFlag>1</AlertFlag><rd><iSeqno>00001</iSeqno><ReimburseNo>00001</ReimburseNo><ReimburseNum></ReimburseNum><StartDate></StartDate><StartTime></StartTime><PayType>1</PayType><PayAccNo>1901023009014433939</PayAccNo><PayAccNameCN>莱勘墮邢粱瘸气入词仲沈僧</PayAccNameCN><PayAccNameEN></PayAccNameEN><RecAccNo>1901023319100000113</RecAccNo><RecAccNameCN>莱勘墮邢粱瘸气入词仲沈僧</RecAccNameCN><RecAccNameEN></RecAccNameEN><SysIOFlg>1</SysIOFlg><IsSameCity></IsSameCity><Prop></Prop><RecICBCCode></RecICBCCode><RecCityName>长沙</RecCityName><RecBankNo></RecBankNo><RecBankName>工商银行长沙测试支行</RecBankName><CurrType>001</CurrType><PayAmt>123</PayAmt><UseCode></UseCode><UseCN>测试支付款</UseCN><EnSummary></EnSummary><PostScript>PostScript附言</PostScript><Summary>Summary摘要</Summary><Ref>RefNo</Ref><Oref>OrefNo</Oref><ERPSqn></ERPSqn><BusCode></BusCode><ERPcheckno></ERPcheckno><CrvouhType></CrvouhType><CrvouhName></CrvouhName><CrvouhNo></CrvouhNo><BankType></BankType><FileNames></FileNames><Indexs></Indexs><PaySubNo></PaySubNo><RecSubNo></RecSubNo><MCardNo></MCardNo><MCardName></MCardName></rd></in></eb></CMS>";
            this.txtShowResult.Text = "【签名的XML包原文】：" + xml + "\r\n";
            PostToNc testSign = new PostToNc();
            string signed = testSign.SignOrEncrypt("http://127.0.0.1:449",xml,0);
            this.txtShowResult.Text =this.txtShowResult.Text + "【签名后的Sign串】：" + signed + "\r\n";
            int jLenth = signed.Length;
            this.txtShowResult.Text = this.txtShowResult.Text + "【我的签名长度】：" + jLenth.ToString() + "\r\n";

            string joint = "Version=0.0.1.0&TransCode=PAYENT&BankCode=102&GroupCIS=" + this.txtCIS.Text.Trim() + "&ID=" + this.txtID.Text.Trim() + "&PackageID=" + "201812024880961" + "&Cert=&reqData=" + signed;
            string EncryptResult = testSign.SignOrEncrypt(Make448Url("201812024880961", "201812024880961"), joint, 1);
            this.txtShowResult.Text = this.txtShowResult.Text + "【往448发的URL：】\r\n" + Make448Url("201812024880961", "201812024880961") + "\r\n";
            this.txtShowResult.Text = this.txtShowResult.Text + "【往448发的postData：】\r\n" + joint + "\r\n";
            byte[] b = Convert.FromBase64String(EncryptResult);
            string t = Encoding.GetEncoding(936).GetString(b);
            this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回1：】\r\n" + EncryptResult + "\r\n";
            this.txtShowResult.Text = this.txtShowResult.Text + "【银行返回2：】\r\n" + t + "\r\n";
        }

        private void btnBankRespRestore_Click(object sender, EventArgs e)
        {
            string bankResponse = this.txtShowResult.Text.Trim();
            byte[] b = Convert.FromBase64String(bankResponse);
            string t = Encoding.GetEncoding(936).GetString(b);
            this.txtShowResult.Text = this.txtShowResult.Text + "\r\n" + t;
        }

        private void btnZipAndBase64_Click(object sender, EventArgs e)
        {
            string src = String.Empty;
            src = this.txtShowResult.Text.Trim();
            string afterZipAndBase64 = ZipAndBase64.Compress(src);
            this.txtShowResult.Text = this.txtShowResult.Text + "\r\n" + "【Gzip后：】" + afterZipAndBase64;
            afterZipAndBase64 = tools.Base64AndUnbase64.EncodeBase64("gb2312", afterZipAndBase64);
            this.txtShowResult.Text = this.txtShowResult.Text + "\r\n" + "【zip并base64后：】" + afterZipAndBase64;
        }

        private void btnUnbase64AndUnzip_Click(object sender, EventArgs e)
        {
            string src = this.txtShowResult.Text.Trim();
            string strUnbase64AndUnzip = tools.ZipHelper.GZipDecompressString(src);
            this.txtShowResult.Text = this.txtShowResult.Text + "\r\n【Base64解码且解压之后的报文是：】" + strUnbase64AndUnzip;
        }

        private void Frm_main_Load(object sender, EventArgs e)
        {

        }

        private void btnUrlEncode_Click(object sender, EventArgs e)
        {
            string src = this.txtShowResult.Text.Trim();
            // string srcUrlEncoded = HttpUtility.UrlEncode(src, System.Text.Encoding.GetEncoding("GB2312"));
            string srcUrlEncoded = HttpUtility.UrlEncode(src);
            this.txtShowResult.Text = this.txtShowResult.Text + "\r\n【URL编码后：】" + "\r\n" + srcUrlEncoded;
        }

        private void btnUrlDecode_Click(object sender, EventArgs e)
        {
            string strBeforeDecode = this.txtShowResult.Text.Trim();
            //System.Web.HttpUtility.HtmlEncode(str);
            //System.Web.HttpUtility.HtmlDecode(str);
            //System.Web.HttpUtility.UrlEncode(str);
            //System.Web.HttpUtility.UrlDecode(str);
            string src = HttpUtility.UrlDecode(strBeforeDecode);
            this.txtShowResult.Text = this.txtShowResult.Text + "\r\n【URL解码后：】" + "\r\n" + src;

        }

        private void btnGoPayentUi_Click(object sender, EventArgs e)
        {
            FrmPayent objFrmPayent = new FrmPayent();
            objFrmPayent.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Entity.QueryReturnEntity.QpdRetrun0010 obj = new Entity.QueryReturnEntity.QpdRetrun0010();
            //QpdRd obj1 = new QpdRd();
            //QpdRd obj2 = new QpdRd();
            //QpdRd obj3 = new QpdRd();
            //QpdRd obj4 = new QpdRd();
            //obj.ListQpdRd.Add(obj1);
            //obj.ListQpdRd.Add(obj2);
            //obj.ListQpdRd.Add(obj3);
            //obj.ListQpdRd.Add(obj4);
            //this.txtShowResult.Text = obj.ToString();


            // QPD返回的原串，包括pub、out、rd 区域
            string strQpdReturnSrc = @"<?xml  version='1.0' encoding='GBK' ?><CMS><eb><pub><TransCode>QPD</TransCode><CIS>190190001325933</CIS><BankCode>102</BankCode><ID>jsj202006.y.1901</ID><TranDate>20200615</TranDate><TranTime>003918562</TranTime><fSeqno>20200615003918562</fSeqno><RetCode>0</RetCode><RetMsg></RetMsg></pub><out><AccNo>1901008019200101142</AccNo><AccName>袋两裹煮佩拦挺担个撒厘魏输移氯</AccName><CurrType>001</CurrType><AreaCode>1901</AreaCode><NextTag></NextTag><TotalNum>57</TotalNum><RepReserved1></RepReserved1><RepReserved2></RepReserved2><AcctSeq></AcctSeq><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.609697</TimeStamp><RepReserved3>1901|00080|20177|000057</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000057</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-5&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                             &lt;流水号&gt;071650317起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000057</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.587087</TimeStamp><RepReserved3>1901|00080|20177|000056</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000056</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-14&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650731起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000056</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.565145</TimeStamp><RepReserved3>1901|00080|20177|000055</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000055</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-74&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650313起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000055</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.542831</TimeStamp><RepReserved3>1901|00080|20177|000054</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000054</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-92&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650733起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000054</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.520257</TimeStamp><RepReserved3>1901|00080|20177|000053</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000053</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-43&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650311起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000053</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.495949</TimeStamp><RepReserved3>1901|00080|20177|000052</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000052</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-132&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650309起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000052</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.471931</TimeStamp><RepReserved3>1901|00080|20177|000051</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000051</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-98&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650307起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000051</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.450837</TimeStamp><RepReserved3>1901|00080|20177|000050</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000050</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-56&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650729起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000050</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.429180</TimeStamp><RepReserved3>1901|00080|20177|000049</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000049</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-140&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650305起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000049</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.407480</TimeStamp><RepReserved3>1901|00080|20177|000048</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000048</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-29&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650727起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000048</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.386138</TimeStamp><RepReserved3>1901|00080|20177|000047</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000047</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-138&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650303起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000047</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.362774</TimeStamp><RepReserved3>1901|00080|20177|000046</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000046</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-79&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650301起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000046</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.339296</TimeStamp><RepReserved3>1901|00080|20177|000045</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000045</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-149&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650299起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000045</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.307473</TimeStamp><RepReserved3>1901|00080|20177|000044</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000044</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-77&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650725起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000044</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.286901</TimeStamp><RepReserved3>1901|00080|20177|000043</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000043</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-57&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650297起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000043</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.261720</TimeStamp><RepReserved3>1901|00080|20177|000042</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000042</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-73&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650723起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000042</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.236897</TimeStamp><RepReserved3>1901|00080|20177|000041</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000041</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-110&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650295起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000041</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.213220</TimeStamp><RepReserved3>1901|00080|20177|000040</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000040</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-129&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650721起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000040</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.189740</TimeStamp><RepReserved3>1901|00080|20177|000039</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000039</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-70&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650293起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000039</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.167301</TimeStamp><RepReserved3>1901|00080|20177|000038</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000038</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-125&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650719起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000038</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.133685</TimeStamp><RepReserved3>1901|00080|20177|000037</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000037</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-96&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650291起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000037</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.096981</TimeStamp><RepReserved3>1901|00080|20177|000036</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000036</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-97&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650717起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000036</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.075909</TimeStamp><RepReserved3>1901|00080|20177|000035</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000035</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-75&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650285起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000035</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.053538</TimeStamp><RepReserved3>1901|00080|20177|000034</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000034</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-143&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650715起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000034</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.031368</TimeStamp><RepReserved3>1901|00080|20177|000033</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000033</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-28&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650289起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000033</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.48.006575</TimeStamp><RepReserved3>1901|00080|20177|000032</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000032</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-135&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650713起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000032</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.986240</TimeStamp><RepReserved3>1901|00080|20177|000031</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000031</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-10&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650283起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000031</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.963443</TimeStamp><RepReserved3>1901|00080|20177|000030</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000030</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-148&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650711起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000030</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.940159</TimeStamp><RepReserved3>1901|00080|20177|000029</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000029</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-1&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                             &lt;流水号&gt;071650287起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000029</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.882800</TimeStamp><RepReserved3>1901|00080|20177|000028</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000028</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-142&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650709起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000028</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.861110</TimeStamp><RepReserved3>1901|00080|20177|000027</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000027</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-117&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650281起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000027</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.836552</TimeStamp><RepReserved3>1901|00080|20177|000026</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000026</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-108&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650707起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000026</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.809721</TimeStamp><RepReserved3>1901|00080|20177|000025</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000025</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-84&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650279起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000025</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.784470</TimeStamp><RepReserved3>1901|00080|20177|000024</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000024</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-104&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650705起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000024</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.763375</TimeStamp><RepReserved3>1901|00080|20177|000023</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000023</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-134&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650277起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000023</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.741965</TimeStamp><RepReserved3>1901|00080|20177|000022</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000022</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-41&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650275起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000022</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.719763</TimeStamp><RepReserved3>1901|00080|20177|000021</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000021</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-8&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                             &lt;流水号&gt;071650273起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000021</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.693655</TimeStamp><RepReserved3>1901|00080|20177|000020</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000020</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-60&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650703起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000020</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.536600</TimeStamp><RepReserved3>1901|00080|20177|000019</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000019</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-24&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650269起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000019</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.277227</TimeStamp><RepReserved3>1901|00080|20177|000018</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000018</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-95&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650701起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000018</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.255492</TimeStamp><RepReserved3>1901|00080|20177|000017</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000017</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-36&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650271起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000017</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.222873</TimeStamp><RepReserved3>1901|00080|20177|000016</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000016</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-59&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650699起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000016</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.200841</TimeStamp><RepReserved3>1901|00080|20177|000015</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000015</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-69&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650257起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000015</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.142270</TimeStamp><RepReserved3>1901|00080|20177|000014</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000014</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-76&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650697起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000014</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.120306</TimeStamp><RepReserved3>1901|00080|20177|000013</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000013</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-15&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650265起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000013</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.084869</TimeStamp><RepReserved3>1901|00080|20177|000012</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000012</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-107&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650689起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000012</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.061072</TimeStamp><RepReserved3>1901|00080|20177|000011</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000011</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-55&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650253起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000011</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.47.020081</TimeStamp><RepReserved3>1901|00080|20177|000010</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000010</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-145&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650685起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000010</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.46.995017</TimeStamp><RepReserved3>1901|00080|20177|000009</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000009</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-17&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650259起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000009</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.46.962966</TimeStamp><RepReserved3>1901|00080|20177|000008</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000008</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-87&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650687起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000008</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.46.940426</TimeStamp><RepReserved3>1901|00080|20177|000007</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000007</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-119&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650261起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000007</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.46.915351</TimeStamp><RepReserved3>1901|00080|20177|000006</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000006</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-16&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650695起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000006</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.46.892401</TimeStamp><RepReserved3>1901|00080|20177|000005</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000005</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-139&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650267起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000005</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.46.865694</TimeStamp><RepReserved3>1901|00080|20177|000004</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000004</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-32&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650693起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000004</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.46.803909</TimeStamp><RepReserved3>1901|00080|20177|000003</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000003</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-89&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                            &lt;流水号&gt;071650263起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000003</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.45.290253</TimeStamp><RepReserved3>1901|00080|20177|000002</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000002</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-150&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650691起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000002</OnlySequence></rd><rd><Drcrf>1</Drcrf><VouhNo>0</VouhNo><Amount>1</Amount><RecipBkNo>0</RecipBkNo><RecipAccNo>9558800200147745380</RecipAccNo><RecipName>疆娇</RecipName><Summary>Summary摘要</Summary><UseCN>用途UseCN</UseCN><PostScript>PostScript附言</PostScript><Ref>RefNO111</Ref><BusCode>001</BusCode><Oref>OrefNo222</Oref><EnSummary></EnSummary><BusType>0</BusType><CvouhType>000</CvouhType><AddInfo></AddInfo><TimeStamp>2020-06-14-23.26.43.211041</TimeStamp><RepReserved3>1901|00080|20177|000001</RepReserved3><RepReserved4></RepReserved4><UpDtranf>0</UpDtranf><ValueDate>20200625</ValueDate><TrxCode>41248</TrxCode><SequenceNo>20177000001</SequenceNo><Cashf>0</Cashf><SubAcctSeq></SubAcctSeq><THCurrency></THCurrency><ReceiptInfo>&lt;指令编号&gt;CMM1357456724-124&lt;/指令编号&gt;&lt;提交人&gt;jsj202006.y.1901&lt;/提交人&gt;&lt;最终授权人&gt;&lt;/最终授权人&gt;                                           &lt;流水号&gt;071650255起息日:2020-06-25汇款币种/金额:人民币(本位币) / 0.01 附言:PostScript附言 ERP业务编号:RefNO111 ERP相关业务编号:OrefNo222 &lt;/流水号&gt;</ReceiptInfo><OnlySequence>20177000001</OnlySequence></rd></out></eb></CMS>";
            // QPD返回的原串的<pub>节点部分
            string strQpdOfPubSrc = string.Empty;
            int posStartPub = strQpdReturnSrc.IndexOf(@"<pub>");
            int posEndPub = strQpdReturnSrc.LastIndexOf(@"</pub>");
             strQpdOfPubSrc = strQpdReturnSrc.Substring(posStartPub, posEndPub - posStartPub + 6);
            // 将 strQpdOfPubSrc 转换为pub 对象
            QpdPub objPub = XmlSerializeHelper.DESerializer<QpdPub>(strQpdOfPubSrc);


            // QPD返回的<out>节点部分
            string strQpdOfOutSrc = string.Empty;
            int posStartOut = strQpdReturnSrc.IndexOf(@"<out>");
            int posEndOut = strQpdReturnSrc.LastIndexOf(@"</out>");
            strQpdOfOutSrc = strQpdReturnSrc.Substring(posStartOut,posEndOut - posStartOut + 6);

            // 得到<out>节点部分后，进一步处理，在<rd>前加<rds>，在</rd>加</rds>
            int posFirsIndexOfRdInOut = strQpdOfOutSrc.IndexOf(@"<rd>");           
            strQpdOfOutSrc = strQpdOfOutSrc.Insert(posFirsIndexOfRdInOut,@"<rds>");
            int posLastIndexOfRdInOut = strQpdOfOutSrc.LastIndexOf(@"</rd>");
            strQpdOfOutSrc = strQpdOfOutSrc.Insert(posLastIndexOfRdInOut + 5, @"</rds>");

           // 将xml out 部分 转换为 对象
            QpdOut objOut = XmlSerializeHelper.DESerializer<QpdOut>(strQpdOfOutSrc);

        }
    }
}
