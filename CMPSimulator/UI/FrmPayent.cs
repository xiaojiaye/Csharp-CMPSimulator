using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMPSimulator.Entity;
using CMPSimulator.Entity.QueryTypeEntity;
using CMPSimulator.tools;
using System.Xml;
using CMPSimulator.Entity.QueryReturnEntity;
using CMPSimulator.Entity.QueryReturnEntity.DIBPSBC;
using CMPSimulator.Entity.QueryReturnEntity.QHISD;
using CMPSimulater.tools;
using System.Web;
using System.IO;
using CMPSimulator.Entity.QueryReturnEntity.QRYGJDTL;

namespace CMPSimulator.UI
{

    public partial class FrmPayent : Form
    {
        #region 成员变量 及 构造方法
        private string strSignTime = null;  //格式是yyyyMMddHHmmssSSS
        private string strActionUrl = null;  // 发送POST请求的URL
        private string strCis = ConfigurationSettings.AppSettings["CIS"];
        private string strId = ConfigurationSettings.AppSettings["ID"];
        private string strPayAccNo = ConfigurationSettings.AppSettings["PayAccNo"];
        private string strPayAccName = ConfigurationSettings.AppSettings["PayAccName"];
        private string strRecAccNo = ConfigurationSettings.AppSettings["RecAccNo"];
        private string strRecAccName = ConfigurationSettings.AppSettings["RecAccName"];
        private string strPostAddress = ConfigurationSettings.AppSettings["PostAddress"];
        private string strHttpPort = ConfigurationSettings.AppSettings["HpptPort"];
        private string strSignPort = ConfigurationSettings.AppSettings["SignPort"];
        private string strTestEnvironmentDate = ConfigurationSettings.AppSettings["TestEnvironmentDate"];
        private string strMCardNo = ConfigurationSettings.AppSettings["MCardNo"];
        private string strMCardName = ConfigurationSettings.AppSettings["MCardName"];

        private string strInterfaceVersion = string.Empty;  // 接口版本号
        private string strSignUrl = string.Empty;   // 签名服务的POST请求  URL
        private string strSignResult = string.Empty;  // 签名结果
        private string strScr = string.Empty;  // xml原报文明文
        private string strHttpSrvUrl = string.Empty;  // http加密服务的POST ACTION url
        private string strPostData = string.Empty;   // 往加密服务端口POST的DATA
        private bool zipFlag = false;   // 报文部分是否压缩
        private string strTransCode = string.Empty;   // 交易代码
        private string EncryptResult = string.Empty;   // NC返回的加密服务结果
        private string transCodeVersionZipFlag = string.Empty;  //  一个联合字段，依据此字段产生不同交易参数的XML明文
        private string queryCodeVersion = string.Empty; // 一个联合字段，查询交易代码+接口版本号 eg：QACCBAL0.0.1.0
        private ObjectPayent objPayent = null;   
        private ObjectPayper0010 objPayper0010 = null;
        private ObjectPaypercol0010 objPaypercol0010 = null;
        private ObjectQaccbal0010 objQaccbal0010 = null;
        private ObjectQpd0010 objQpd0010 = null;
        private ObjectQhisd0010 objQhisd0010 = null;
        private ObjectQRYGJDTL0010 objQRYGJDTL0010 = null;
        private ObjectQpayent0010 objQPAYENT0010 = null;
        private ObjectQpayper0010 objQpayper0010 = null;
        private ObjectDIBPSBC objDIBPSBC0001 = null;

        public FrmPayent()
        {
            InitializeComponent();
            this.txtPayAccNo.Text = strPayAccNo;
            this.txtPayAccNameCN.Text = strPayAccName;
            this.txtRecAccNo.Text = strRecAccNo;
            this.txtRecAccNameCN.Text = strRecAccName;
            this.txtMCardNo.Text = strMCardNo;
            this.txtMCardName.Text = strMCardName;
            this.txtCertId.Text = strId;
            this.txtCis.Text = strCis;
            this.txtServerAddress.Text = strPostAddress;
            this.txtHttpPort.Text = strHttpPort;
            this.txtSignPort.Text = strSignPort;
            this.txtEnvironmentTime.Text = strTestEnvironmentDate;
            // 判断D盘是否有test文件夹
            string path = @"D:\test\";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }
        #endregion

        // 结算类提交
        private void btnPayentSubmit_Click(object sender, EventArgs e)
        {
            this.dgvShowQpdRd.DataSource = null;
            this.txtResultShow.Text = string.Empty;
            string strBeijingTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");   // PC实际提交实际
            strSignTime = strTestEnvironmentDate + strBeijingTime.Substring(8, 9);  // 根据测试环境时间改变签名时间

            #region 验证选择项
            // 账号非空验证。。。
            // 账号的数字验证
            // 户名长度验证。。。
            // 用途长度验证。。。      

            // 判断是否选择了业务类型
            if (returnTransCodeByChoose().Length == 0 || returnTransCodeByChoose()== null)
            {
                MessageBox.Show("请选择业务类型！");
                return;
            } 
            #endregion

            #region 结算提交类业务参数初始化
            // 确定接口版本
            if (this.radInterface0001.Checked)
            {
                strInterfaceVersion = "0.0.0.1";
            }else
            {
                strInterfaceVersion = "0.0.1.0";
            }

            // 确定 报文部分是否压缩
            if (this.ckBoxZipFlag.Checked)
            {
                zipFlag = true;
            }else
            {
                zipFlag = false;
            }

            // 确定交易代码 , 根据选择的单选按钮返回transCode
            strTransCode = returnTransCodeByChoose();
            transCodeVersionZipFlag = strTransCode + strInterfaceVersion + zipFlag.ToString();



            // 【1】确定签名URL地址
            strSignUrl = @"http://" + strPostAddress + ":" + strSignPort;
            // 【2】根据业务参数组包
            // 根据交易代码transCode 创建不同对象，进而创建不同报文
            switch (transCodeVersionZipFlag)
            {
                case "PAYENT0.0.1.0False" :
                    //根据交易代码，创建并封装实体类对象
                    objPayent = createObjectPayent0010(strBeijingTime);
                    // 作为对象传输，进而生成报文
                    strScr = CreateXml.CreateXml.CreatePAYENT0010(objPayent);
                    break;
                case "PAYENT0.0.1.0True" :
                    objPayent = createObjectPayent0010(strBeijingTime);
                    strScr = CreateXml.CreateXml.CreatePAYENTWithZip0010(objPayent);
                    break;
                case "PAYPER0.0.1.0False":
                    objPayper0010 = createObjectPayper0010(strBeijingTime);
                    strScr = CreateXml.CreateXml.CreatePAYPER0010(objPayper0010);
                    break;
                case "PAYPERCOL0.0.1.0False":
                    objPaypercol0010 = createObjectPaypercol0010(strBeijingTime);
                    strScr = CreateXml.CreateXml.CreatePAYPERCOL0010(objPaypercol0010);
                    break;
                default:
                    MessageBox.Show("该版本的接口不支持，或者作者未开发此版本接口！");
                    return;
                    break;
            }
            
            #endregion

            #region 签名服务
            // 【3】向签名端口发送
            strSignResult = SignOrEncrypt.EncryptOrSign(strSignUrl, strScr,0);
            this.txtResultShow.Text = "【签名URL地址：】\r\n" + strSignUrl + "\r\n";
            this.txtResultShow.Text = this.txtResultShow.Text + "【签名前的原包：】\r\n" + strScr + "\r\n";
            this.txtResultShow.Text = this.txtResultShow.Text + "【签名后的数据：】\r\n" + strSignResult + "\r\n";
            #endregion

            #region Http加密服务
            // 【1】确定HTTP加密服务的 ACTION  URL地址
            // 封装ActionUrl对象
            ActionUrl objActionUrl = new ActionUrl()
            {
                strHttpPort = strHttpPort,
                strPostAddress = strPostAddress,
                strId = strId,
                strPackageID = strBeijingTime,
                strSendTime = strBeijingTime,
                strInterfaceVersion = strInterfaceVersion,
                strCis = strCis,
                strTransCode = strTransCode

            };
            string strActionUrl = CmpCommonTools.makeActionUrl(objActionUrl);
            // 【2】确定POST 数据
            strPostData = CmpCommonTools.createPostData(objActionUrl, zipFlag, strSignResult);
            // 【3】向加密端口发送
            EncryptResult = SignOrEncrypt.EncryptOrSign(strActionUrl, strPostData, 1);
            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的URL：】\r\n" + strActionUrl + "\r\n";
            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的postData：】\r\n" + strPostData + "\r\n";
            byte[] b = Convert.FromBase64String(EncryptResult);
            string t = Encoding.GetEncoding(936).GetString(b);
            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回1：】\r\n" + EncryptResult + "\r\n";
            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回2：】\r\n" + t + "\r\n";
            // 【5】记录日志
            WriteLog.writeLog(this.txtResultShow.Text, strTransCode, strBeijingTime);
            #endregion
        }


        #region 检查及验证方法、load方法
        private void FrmPayent_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 检查gboxBusType控件中的  业务单选按钮是否已经选择
        /// </summary>
        /// <returns></returns>
        private bool checkBusTypeChoosed()
        {
            bool checkedFlag = false;
            foreach (Control item in gboxBusType.Controls)
            {
                if (item is RadioButton)
                {
                    RadioButton rdoCtl = item as RadioButton;
                    if (rdoCtl.Checked)
                    {
                        checkedFlag = true;
                        return checkedFlag;
                    }
                }
            }
            return checkedFlag;
        }

        /// <summary>
        /// 根据按钮的选择，返回transCode
        /// </summary>
        /// <returns></returns>
        private string returnTransCodeByChoose()
        {
            string transCode = string.Empty;
            foreach (Control item in gboxBusType.Controls)
            {
                if (item is RadioButton)
                {
                    RadioButton rdoCtl = item as RadioButton;
                    if (rdoCtl.Checked)
                    {
                        transCode = rdoCtl.Name.Substring(3);
                        return transCode;
                    }
                }
            }
            return transCode;
        }
        #endregion

        #region 根据业务类型和用户选择的参数，将实体类对象初始化,提交类型封装
        /// <summary>
        /// 返回objPayent对象
        /// </summary>
        /// <param name="strBeijingTime"></param>
        /// <returns></returns>
        private ObjectPayent createObjectPayent0010(string strBeijingTime)
        {
            // 封装支付对象
            ObjectPayent objPayent = new ObjectPayent()
            {
                TransCode = strTransCode,
                CIS = strCis,
                BankCode = "102",
                ID = strId,
                TranDate = strBeijingTime.Substring(0, 8), // ERP系统产生的交易日期，格式是yyyyMMdd
                TranTime = strBeijingTime.Substring(8, 9), // ERP系统产生的交易时间，格式如HHmmssSSS，精确到毫秒；
                fSeqno = strBeijingTime,
                OnlBatF = this.radJoinToMachine.Checked ? "1" : "2",
                SettleMode = this.radBingbiJiZhang.Checked ? "2" : "0",
                TotalNum = this.txtTotalNum.Text.Trim(),
                TotalAmt = this.txtTotalNum.Text.Trim(),
                SignTime = strSignTime,
                ReqReserved1 = this.txtReqReserved1.Text.Trim(),
                ReqReserved2 = this.txtReqReserved2.Text.Trim(),
                AlertFlag = "0",
                PayType = this.radUrgent.Checked ? "1" : "2",
                PayAccNo = this.txtPayAccNo.Text.Trim(),
                PayAccNameCN = this.txtPayAccNameCN.Text.Trim(),
                RecAccNo = this.txtRecAccNo.Text.Trim(),
                RecAccNameCN = this.txtRecAccNameCN.Text.Trim(),
                SysIOFlg = this.radSystemIn.Checked ? "1" : "2",
                IsSameCity = this.radIsSameCity.Checked ? "1" : "2",
                Prop = this.radToCompany.Checked ? "0" : "1",
                CurrType = "001",
                PayAmt = this.txtTotalNum.Text.Trim(),
                UseCN = this.txtUseCN.Text.Trim(),
                PostScript = this.txtPostScript.Text.Trim(),
                Summary = this.txtSummary.Text.Trim(),
                Ref = this.txtRef.Text.Trim(),
                Oref = this.txtOref.Text.Trim(),
                MCardNo = this.txtMCardNo.Text.Trim(),
                MCardName = this.txtMCardName.Text.Trim()
            };
            return objPayent;
        }

        private ObjectPayper0010 createObjectPayper0010(string strBeijingTime)
        {
            // 封装支付对象
            ObjectPayper0010 objPayper0010 = new ObjectPayper0010()
            {
                TransCode = strTransCode,
                CIS = strCis,
                BankCode = "102",
                ID = strId,
                TranDate = strBeijingTime.Substring(0, 8), // ERP系统产生的交易日期，格式是yyyyMMdd
                TranTime = strBeijingTime.Substring(8, 9), // ERP系统产生的交易时间，格式如HHmmssSSS，精确到毫秒；
                fSeqno = strBeijingTime,
                OnlBatF = this.radJoinToMachine.Checked ? "1" : "2",
                SettleMode = this.radBingbiJiZhang.Checked ? "2" : "0",
                TotalNum = this.txtTotalNum.Text.Trim(),
                TotalAmt = this.txtTotalNum.Text.Trim(),
                SignTime = strSignTime,
                THBaseAccFlag = "0",
                RegSerialNO = "",
                PackageName = this.txtReqReserved1.Text.Trim(),
                TotalSummary = this.txtTotalSummary.Text.Trim(),
                BatchSumFlag = this.radHuiZongHuiChong.Checked ? "0" : "2",

                PayType = this.radUrgent.Checked ? "1" : "2",
                PayAccNo = this.txtPayAccNo.Text.Trim(),
                PayAccNameCN = this.txtPayAccNameCN.Text.Trim(),
                RecAccNo = this.txtRecAccNo.Text.Trim(),
                RecAccNameCN = this.txtRecAccNameCN.Text.Trim(),
                SysIOFlg = this.radSystemIn.Checked ? "1" : "2",
                IsSameCity = this.radIsSameCity.Checked ? "1" : "2",
                CurrType = "001",
                PayAmt = this.txtTotalNum.Text.Trim(),
                UseCN = this.txtUseCN.Text.Trim(),
                PostScript = this.txtPostScript.Text.Trim(),
                Summary = this.txtSummary.Text.Trim(),
                Ref = this.txtRef.Text.Trim(),
                Oref = this.txtOref.Text.Trim(),
                MCardNo = this.txtMCardNo.Text.Trim(),
                MCardName = this.txtMCardName.Text.Trim()
            };
            return objPayper0010;
        }

        private ObjectPaypercol0010 createObjectPaypercol0010(string strBeijingTime)
        {
            // 封装支付对象
            ObjectPaypercol0010 objPaypercol0010 = new ObjectPaypercol0010()
            {
                TransCode = strTransCode,
                CIS = strCis,
                BankCode = "102",
                ID = strId,
                TranDate = strBeijingTime.Substring(0, 8), // ERP系统产生的交易日期，格式是yyyyMMdd
                TranTime = strBeijingTime.Substring(8, 9), // ERP系统产生的交易时间，格式如HHmmssSSS，精确到毫秒；
                fSeqno = strBeijingTime,
                OnlBatF = this.radJoinToMachine.Checked ? "1" : "2",
                BusType = this.txtBusType.Text.Trim(),        
                TotalNum = this.txtTotalNum.Text.Trim(),
                TotalAmt = this.txtTotalNum.Text.Trim(),
                SignTime = strSignTime,
                TotalSummary = this.txtTotalSummary.Text.Trim(),
                THBaseAccFlag = "0",
                RegSerialNO = "",
                PackageName = this.txtReqReserved1.Text.Trim(),

                PayType = this.radUrgent.Checked ? "1" : "2",
                PayAccNo = this.txtPayAccNo.Text.Trim(),
                PayAccNameCN = this.txtPayAccNameCN.Text.Trim(),
                RecAccNo = this.txtRecAccNo.Text.Trim(),
                RecAccNameCN = this.txtRecAccNameCN.Text.Trim(),
                SysIOFlg = this.radSystemIn.Checked ? "1" : "2",
                IsSameCity = this.radIsSameCity.Checked ? "1" : "2",
                CurrType = "001",
                PayAmt = "1",
                UseCN = this.txtUseCN.Text.Trim(),
                PostScript = this.txtPostScript.Text.Trim(),
                Summary = this.txtSummary.Text.Trim(),
                Ref = this.txtRef.Text.Trim(),
                Oref = this.txtOref.Text.Trim()
            };
            return objPaypercol0010;
        }

        private ObjectQaccbal0010 createObjectQaccbal0010(string strBeijingTime)
        {
            // 封装QACCBAL查询对象
            ObjectQaccbal0010 objQaccbal0010 = new ObjectQaccbal0010()
            {
                TransCode = strTransCode,
                CIS = strCis,
                BankCode = "102",
                ID = strId,
                TranDate = strBeijingTime.Substring(0, 8), // ERP系统产生的交易日期，格式是yyyyMMdd
                TranTime = strBeijingTime.Substring(8, 9), // ERP系统产生的交易时间，格式如HHmmssSSS，精确到毫秒；
                fSeqno = strBeijingTime,
                TotalNum = "1",
                BLFlag = "0",
                SynFlag = "0", // 选送标签，0否；1是,如果上送值为1,则只允许上送不超过10个账号。不送标签默认为0
                iSeqno = "1",
                AccNo = this.txtPayAccNo.Text.Trim(),
                CurrType = this.txtCurrType.Text.Trim(),
                ReqReserved3 = "102", // 行别,集团有他行账号时为必输项（输入102或不输入为工行，其他为他行）
                AcctSeq = "", // 对于注册了电子银行渠道的账户管家账户，该字段若上送，则长度必须为9,对于本行FT类账户,忽略该字段上送值
                MainAcctNo = "" //安心托管子账号查询时选输安心托管主账户               
            };
            return objQaccbal0010;
        }

        private ObjectQpayent0010 createObjectQpayent0010(string strBeijingTime)
        {
            // 封装QPAYENT查询对象
            ObjectQpayent0010 objQPayent0010 = new ObjectQpayent0010()
            {
                TransCode = strTransCode,
                CIS = strCis,
                BankCode = "102",
                ID = strId,
                TranDate = strBeijingTime.Substring(0, 8), // ERP系统产生的交易日期，格式是yyyyMMdd
                TranTime = strBeijingTime.Substring(8, 9), // ERP系统产生的交易时间，格式如HHmmssSSS，精确到毫秒；
                fSeqno = strBeijingTime,
                QryfSeqno = this.txtQueryCondition.Text.Trim(),
                QrySerialNo = ""            
            };
            return objQPayent0010;
        }

        private ObjectQpayper0010 createObjectQpayper0010(string strBeijingTime)
        {
            // 封装QPAYPER查询对象
            ObjectQpayper0010 objQpayper0010 = new ObjectQpayper0010()
            {
                TransCode = strTransCode,
                CIS = strCis,
                BankCode = "102",
                ID = strId,
                TranDate = strBeijingTime.Substring(0, 8), // ERP系统产生的交易日期，格式是yyyyMMdd
                TranTime = strBeijingTime.Substring(8, 9), // ERP系统产生的交易时间，格式如HHmmssSSS，精确到毫秒；
                fSeqno = strBeijingTime,
                QryfSeqno = this.txtQueryCondition.Text.Trim(),
                QrySerialNo = ""
            };
            return objQpayper0010;
        }

        private ObjectQpd0010 createObjectQpd0010(string strBeijingTime)
        {
            // 封装QPD查询对象
            ObjectQpd0010 objQpd0010 = new ObjectQpd0010()
            {
                TransCode = strTransCode,
                CIS = strCis,
                BankCode = "102",
                ID = strId,
                TranDate = strBeijingTime.Substring(0, 8), // ERP系统产生的交易日期，格式是yyyyMMdd
                TranTime = strBeijingTime.Substring(8, 9), // ERP系统产生的交易时间，格式如HHmmssSSS，精确到毫秒；
                fSeqno = strBeijingTime,
                AccNo = this.txtPayAccNo.Text.Trim(),
                AreaCode = this.txtPayAccNo.Text.Trim().Substring(0,4),
                MinAmt = "0",
                MaxAmt = "0", // 若输入则为整数，单位为分.发生额上下限都有值时则下限必须小于等于上限（“发生额下限”及“发生额上限”若都输入0，则默认查询全部记录）
                BeginTime = "", //预留，目前无意义（“开始时间”不能晚于“终止时间” 格式为HHmmss）
                EndTime = "", // 预留，目前无意义（“开始时间”不能晚于“终止时间” 格式为HHmmss）
                NextTag = "",
                ReqReserved1 = "", // 行别,在集团有他行账号，且查询他行往来账号的情况下为必输
                ReqReserved2 = "", // 备用，目前无意义
                CurrType = this.txtCurrType.Text.Trim(),             
                AcctSeq = "" // 对于注册了电子银行渠道的账户管家账户，该字段若上送，则长度必须为9,对于本行FT类账户,忽略该字段上送值
             };
            return objQpd0010;
        }
        
        private ObjectQhisd0010 createObjectQhisd0010(string strBeijingTime)
        {
            string strBeginDate = this.dtpBeginDate.Value.ToString("yyyyMMddHHmmss").Substring(0, 8);
            string strEndDate = this.dtpEndDate.Value.ToString("yyyyMMddHHmmss").Substring(0, 8);
            // 封装QHISD查询对象
            ObjectQhisd0010 objQhisd0010 = new ObjectQhisd0010()
            {
                TransCode = strTransCode,
                CIS = strCis,
                BankCode = "102",
                ID = strId,
                TranDate = strBeijingTime.Substring(0, 8), // ERP系统产生的交易日期，格式是yyyyMMdd
                TranTime = strBeijingTime.Substring(8, 9), // ERP系统产生的交易时间，格式如HHmmssSSS，精确到毫秒；
                fSeqno = strBeijingTime,
                AccNo = this.txtPayAccNo.Text.Trim(),
                BeginDate = strBeginDate,
                EndDate = strEndDate,
                MinAmt = "0",   // 若输入则为正数; 贷款户历史明细、定期户历史明细、保证金户历史明细无需输入，如输入，则忽略   ;其余账户类型需要输入
                MaxAmt = "99999999999999999", // 若输入则为正数，单位为分.发生额上下限都有值时则下限必须小于等于上限;  贷款户历史明细、定期户历史明细、保证金户历史明细无需输入，如输入，则忽略;  其余账户类型需要输入
                BankType = "",
                NextTag = "",
                CurrType = this.txtCurrType.Text.Trim(),
                DueBillNo = "",  // 借据编号 ;查询贷款户可上送该字段;   其余账户类型无需输入，若输入，则忽略
                AcctSeq = "",
                ComplementFlag = "0",
                CardAccNoDef = "",
                DesByTime = "" //明细是否按时间降序排序 只对往来户历史明细查询生效；0或者空 - 否：明细升序返回；
                               //1 - 是：明细降序返回；  兼容存量客户不上送此字段，此时返回明细顺序为：先当日(降序)，后历史(升序)；
            };
            return objQhisd0010;
        }

        private ObjectQRYGJDTL0010 createObjectQRYGJDTL0010(string strBeijingTime)
        {
            string strBeginDate = this.dtpBeginDate.Value.ToString("yyyyMMddHHmmss").Substring(0, 8);
            string strEndDate = this.dtpEndDate.Value.ToString("yyyyMMddHHmmss").Substring(0, 8);
            // 封装QHISD查询对象
            ObjectQRYGJDTL0010 objQRYGJDTL0010 = new ObjectQRYGJDTL0010()
            {
                TransCode = strTransCode,
                CIS = strCis,
                BankCode = "102",
                ID = strId,
                TranDate = strBeijingTime.Substring(0, 8), // ERP系统产生的交易日期，格式是yyyyMMdd
                TranTime = strBeijingTime.Substring(8, 9), // ERP系统产生的交易时间，格式如HHmmssSSS，精确到毫秒；
                fSeqno = strBeijingTime,
                AcctNo = this.txtPayAccNo.Text.Trim(),
                StartDate = strBeginDate,
                EndDate = strEndDate,
                NextTag = "",
                AcctSeq = "",
                CardNo = this.txtMCardNo.Text.Trim(),
                CurrType = this.txtCurrType.Text.Trim()
            };
            return objQRYGJDTL0010;
        }

        private ObjectDIBPSBC createObjectDIBPSBC0001(string strBeijingTime)
        {
            // 封装DIBPSBC查询对象
            ObjectDIBPSBC objDibpsbc0001 = new ObjectDIBPSBC()
            {
                TransCode = strTransCode,
                CIS = strCis,
                BankCode = "102",
                ID = strId,
                TranDate = strBeijingTime.Substring(0, 8), // ERP系统产生的交易日期，格式是yyyyMMdd
                TranTime = strBeijingTime.Substring(8, 9), // ERP系统产生的交易时间，格式如HHmmssSSS，精确到毫秒；
                fSeqno = strBeijingTime,
                BnkCode = this.txtBankTypeCode.Text.Trim(),
                NextTag = "",
                ReqReserved1 = this.ckBoxQueryKuaHangKuaiHuiBankNo.Checked ? "1" : "0",
                ReqReserved2 = ""
            };
            return objDibpsbc0001;
        }
        #endregion

        //  查询类提交
        private void btnQuerySubmit_Click(object sender, EventArgs e)
        {
            this.dgvShowQpdRd.DataSource = null;
            this.txtResultShow.Text = string.Empty;
            string strBeijingTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");   // PC实际提交实际
            
            #region 验证选择项
            // 账号非空验证。。。
            // 账号的数字验证
            // 户名长度验证。。。
            // 用途长度验证。。。      

            // 判断是否选择了业务类型
            if (returnTransCodeByChoose().Length == 0 || returnTransCodeByChoose() == null)
            {
                MessageBox.Show("请选择业务类型！");
                return;
            }
            #endregion

            #region 提交类业务参数初始化
            // 确定接口版本
            if (this.radInterface0001.Checked)
            {
                strInterfaceVersion = "0.0.0.1";
            }
            else
            {
                strInterfaceVersion = "0.0.1.0";
            }

            // 确定交易代码 , 根据选择的单选按钮返回transCode
            strTransCode = returnTransCodeByChoose();
            queryCodeVersion = strTransCode + strInterfaceVersion ;

            // 【1】确定HTTP 448 加密服务的 ACTION  URL地址
            // 封装ActionUrl对象
            ActionUrl objActionUrl = new ActionUrl()
            {
                strHttpPort = strHttpPort,
                strPostAddress = strPostAddress,
                strId = strId,
                strPackageID = strBeijingTime,
                strSendTime = strBeijingTime,
                strInterfaceVersion = strInterfaceVersion,
                strCis = strCis,
                strTransCode = strTransCode

            };
            string strActionUrl = CmpCommonTools.makeActionUrl(objActionUrl);

            // 【2】根据业务参数组包
            // 根据交易代码transCode 创建不同对象，进而创建不同报文
            switch (queryCodeVersion)
            {
                case "QACCBAL0.0.1.0":
                    //根据交易代码，创建并封装实体类对象
                    objQaccbal0010 = createObjectQaccbal0010(strBeijingTime);
                    // 作为对象传输，进而生成报文
                    strScr = CreateXml.CreateXml.CreateQACCBAL0010(objQaccbal0010);
                    break;
                #region case "QPD0.0.1.0":
                case "QPD0.0.1.0":
                    {
                        int iCountQpd = 0;
                        QpdOut objtemp = new QpdOut();
                        QpdOut objOut = new QpdOut();
                        // 封装QPD对象
                        objQpd0010 = createObjectQpd0010(strBeijingTime);
                        do
                        {
                            // 产生QPD的XML报文
                            strScr = CreateXml.CreateXml.CreateQPD0010(objQpd0010);
                            // 【3】确定POST 数据 查询类的ZIP标识都是false，结算类根据实际情况送
                            strPostData = CmpCommonTools.createPostData(objActionUrl, false, strScr);
                            // 【4】向加密端口发送
                            EncryptResult = SignOrEncrypt.EncryptOrSign(strActionUrl, strPostData, 1);   // 第三个参数：   0  签名； 1  加密
                            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的URL：】\r\n" + strActionUrl + "\r\n";
                            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的postData：】\r\n" + strPostData + "\r\n";
                            byte[] bb = Convert.FromBase64String(EncryptResult);
                            string tt = Encoding.GetEncoding(936).GetString(bb);
                            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回1：】\r\n" + EncryptResult + "\r\n";
                            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回2：】\r\n" + tt + "\r\n";
                            // 【5】解析xml返回报文中的nextTag字段
                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(tt);

                            // 【如果 RetCode ！= 0 】 ，则不进行 指令包处理
                            XmlNodeList nodelist_RetCode = xml.GetElementsByTagName("RetCode");
                            string strRetCode = nodelist_RetCode.Item(0).InnerText;
                            XmlNodeList nodelist_RetMsg = xml.GetElementsByTagName("RetMsg");
                            string strRetMsg = nodelist_RetMsg.Item(0).InnerText;
                            if (!("0".Equals(strRetCode)))
                            {
                                this.txtResultShow.Text = this.txtResultShow.Text + "【RetCode：】" + strRetCode + "\r\n";
                                this.txtResultShow.Text = this.txtResultShow.Text + "【RetMsg：】" + strRetMsg + "\r\n";
                                // 退出方法前，要写日志
                                WriteLog.writeLog(this.txtResultShow.Text, queryCodeVersion, strBeijingTime);
                                return;
                            }

                            // 【6】将xml字符串，序列化为对象
                            string strQpdReturnSrc = tt;
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
                            strQpdOfOutSrc = strQpdReturnSrc.Substring(posStartOut, posEndOut - posStartOut + 6);

                            // 得到<out>节点部分后，进一步处理，在<rd>前加<rds>，在</rd>加</rds>
                            // ...此处还需要加业务逻辑，如果 posFirsIndexOfRdInOut < 0 ，则没有 rd 循环体返回
                            int posFirsIndexOfRdInOut = strQpdOfOutSrc.IndexOf(@"<rd>");
                            strQpdOfOutSrc = strQpdOfOutSrc.Insert(posFirsIndexOfRdInOut, @"<rds>");
                            int posLastIndexOfRdInOut = strQpdOfOutSrc.LastIndexOf(@"</rd>");
                            strQpdOfOutSrc = strQpdOfOutSrc.Insert(posLastIndexOfRdInOut + 5, @"</rds>");

                            // 将xml out 部分 转换为 对象
                            objtemp = XmlSerializeHelper.DESerializer<QpdOut>(strQpdOfOutSrc);
                            if (iCountQpd == 0)
                            {
                                objOut.listQpdRd = objtemp.listQpdRd;
                            }
                            else
                            {
                                objOut.listQpdRd.AddRange(objtemp.listQpdRd);
                            }

                            xml.Save("d:\\test\\" + "【" + objQpd0010.fSeqno + "】" + "第" + (iCountQpd + 1).ToString() + "次QPD.xml");//保存
                            XmlNodeList nodelist_pub = xml.GetElementsByTagName("NextTag");
                            objQpd0010.NextTag = nodelist_pub.Item(0).InnerText;
                            iCountQpd++;

                        } while (objQpd0010.NextTag.Length != 0);

                        this.txtResultShow.Text = this.txtResultShow.Text + "【总共发查询包的个数是：】\r\n" + iCountQpd.ToString() + "\r\n";
                        this.txtResultShow.Text = this.txtResultShow.Text + "【总共有当日明细：】" + objOut.listQpdRd.Count.ToString() + "条" + "\r\n";
                        this.dgvShowQpdRd.DataSource = objOut.listQpdRd;
                        tools.DataGridViewStyle.DgvStyle1(this.dgvShowQpdRd);
                    }
                    return;
                    break;
                #endregion

                #region case "QHISD0.0.1.0":
                case "QHISD0.0.1.0":
                    {
                        if (this.dtpBeginDate.Value > this.dtpEndDate.Value)
                        {
                            MessageBox.Show("起始日期不能大于结束日期,请重新选择");
                            return;
                        }

                        int iCountQHISD = 0;   // 包的个数    
                        QhisdOut objtemp = new QhisdOut();
                        QhisdOut objOut = new QhisdOut();
                        // 封装QHISD对象
                        objQhisd0010 = createObjectQhisd0010(strBeijingTime);
                        do
                        {
                            // 产生QHISD的XML报文
                            strScr = CreateXml.CreateXml.CreateQHISD0010(objQhisd0010);
                            // 【3】确定POST 数据 查询类的ZIP标识都是false，结算类根据实际情况送
                            strPostData = CmpCommonTools.createPostData(objActionUrl, false, strScr);
                            // 【4】向加密端口发送
                            EncryptResult = SignOrEncrypt.EncryptOrSign(strActionUrl, strPostData, 1);   // 第三个参数：   0  签名； 1  加密
                            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的URL：】\r\n" + strActionUrl + "\r\n";
                            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的postData：】\r\n" + strPostData + "\r\n";
                            byte[] bb = Convert.FromBase64String(EncryptResult);
                            string tt = Encoding.GetEncoding(936).GetString(bb);
                            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回1：】\r\n" + EncryptResult + "\r\n";
                            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回2：】\r\n" + tt + "\r\n";
                            // 【5】解析xml返回报文中的nextTag字段
                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(tt);

                            // 【如果 RetCode ！= 0 】 ，则不进行 指令包处理
                            XmlNodeList nodelist_RetCode = xml.GetElementsByTagName("RetCode");
                            string strRetCode = nodelist_RetCode.Item(0).InnerText;
                            XmlNodeList nodelist_RetMsg = xml.GetElementsByTagName("RetMsg");
                            string strRetMsg = nodelist_RetMsg.Item(0).InnerText;
                            if (!("0".Equals(strRetCode)))
                            {
                                this.txtResultShow.Text = this.txtResultShow.Text + "【RetCode：】" + strRetCode + "\r\n";
                                this.txtResultShow.Text = this.txtResultShow.Text + "【RetMsg：】" + strRetMsg + "\r\n";
                                // 退出方法前，要写日志
                                WriteLog.writeLog(this.txtResultShow.Text, queryCodeVersion, strBeijingTime);
                                return;
                            }


                            // 【6】将xml字符串，序列化为对象
                            string strQhisdReturnSrc = tt;
                            // QHISD返回的原串的<pub>节点部分
                            string strQhisdOfPubSrc = string.Empty;
                            int posStartPub = strQhisdReturnSrc.IndexOf(@"<pub>");
                            int posEndPub = strQhisdReturnSrc.LastIndexOf(@"</pub>");
                            strQhisdOfPubSrc = strQhisdReturnSrc.Substring(posStartPub, posEndPub - posStartPub + 6);
                            // 将 strQhisdOfPubSrc 转换为pub 对象
                            QhisdPub objPub = XmlSerializeHelper.DESerializer<QhisdPub>(strQhisdOfPubSrc);


                            // QHISD返回的<out>节点部分
                            string strQhisdOfOutSrc = string.Empty;
                            int posStartOut = strQhisdReturnSrc.IndexOf(@"<out>");
                            int posEndOut = strQhisdReturnSrc.LastIndexOf(@"</out>");
                            strQhisdOfOutSrc = strQhisdReturnSrc.Substring(posStartOut, posEndOut - posStartOut + 6);

                            // 得到<out>节点部分后，进一步处理，在<rd>前加<rds>，在</rd>加</rds>
                            // ...此处还需要加业务逻辑，如果 posFirsIndexOfRdInOut < 0 ，则没有 rd 循环体返回
                            int posFirsIndexOfRdInOut = strQhisdOfOutSrc.IndexOf(@"<rd>");
                            strQhisdOfOutSrc = strQhisdOfOutSrc.Insert(posFirsIndexOfRdInOut, @"<rds>");
                            int posLastIndexOfRdInOut = strQhisdOfOutSrc.LastIndexOf(@"</rd>");
                            strQhisdOfOutSrc = strQhisdOfOutSrc.Insert(posLastIndexOfRdInOut + 5, @"</rds>");

                            // 将xml out 部分 转换为 对象
                            objtemp = XmlSerializeHelper.DESerializer<QhisdOut>(strQhisdOfOutSrc);
                            if (iCountQHISD == 0)
                            {
                                objOut.listQhisdRd = objtemp.listQhisdRd;
                            }
                            else
                            {
                                objOut.listQhisdRd.AddRange(objtemp.listQhisdRd);
                            }

                            xml.Save("d:\\test\\" + "【" + objQhisd0010.fSeqno + "】" + "第" + (iCountQHISD + 1).ToString() + "次QHISD.xml");//保存
                            XmlNodeList nodelist_pub = xml.GetElementsByTagName("NextTag");
                            objQhisd0010.NextTag = nodelist_pub.Item(0).InnerText;
                            iCountQHISD++;

                        } while (objQhisd0010.NextTag.Length != 0);

                        this.txtResultShow.Text = this.txtResultShow.Text + "【总共发查询包的个数是：】\r\n" + iCountQHISD.ToString() + "\r\n";
                        this.txtResultShow.Text = this.txtResultShow.Text + "【总共有当日明细：】" + objOut.listQhisdRd.Count.ToString() + "条" + "\r\n";
                        this.dgvShowQpdRd.DataSource = objOut.listQhisdRd;
                        tools.DataGridViewStyle.DgvStyle1(this.dgvShowQpdRd);
                    }
                    return;
                    break;
                #endregion

                #region  case "QRYGJDTL0.0.1.0":
                case "QRYGJDTL0.0.1.0":
                    {
                        if (this.dtpBeginDate.Value > this.dtpEndDate.Value)
                        {
                            MessageBox.Show("起始日期不能大于结束日期,请重新选择");
                            return;
                        }

                        int iCountQRYGJDTL = 0;   // 包的个数    
                        QrygjdtlOut objtemp = new QrygjdtlOut();
                        QrygjdtlOut objOut = new QrygjdtlOut();
                        // 封装QHISD对象
                        objQRYGJDTL0010 = createObjectQRYGJDTL0010(strBeijingTime);
                        do
                        {
                            // 产生QRYGJDTL的XML报文
                            strScr = CreateXml.CreateXml.CreateQRYGJDTL0010(objQRYGJDTL0010);
                            // 【3】确定POST 数据 查询类的ZIP标识都是false，结算类根据实际情况送
                            strPostData = CmpCommonTools.createPostData(objActionUrl, false, strScr);
                            // 【4】向加密端口发送
                            EncryptResult = SignOrEncrypt.EncryptOrSign(strActionUrl, strPostData, 1);   // 第三个参数：   0  签名； 1  加密
                            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的URL：】\r\n" + strActionUrl + "\r\n";
                            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的postData：】\r\n" + strPostData + "\r\n";
                            byte[] bb = Convert.FromBase64String(EncryptResult);
                            string tt = Encoding.GetEncoding(936).GetString(bb);
                            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回1：】\r\n" + EncryptResult + "\r\n";
                            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回2：】\r\n" + tt + "\r\n";
                            // 【5】解析xml返回报文中的nextTag字段
                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(tt);

                            // 【如果 RetCode ！= 0 】 ，则不进行 指令包处理
                            XmlNodeList nodelist_RetCode = xml.GetElementsByTagName("RetCode");
                            string strRetCode = nodelist_RetCode.Item(0).InnerText;
                            XmlNodeList nodelist_RetMsg = xml.GetElementsByTagName("RetMsg");
                            string strRetMsg = nodelist_RetMsg.Item(0).InnerText;
                            if (!("0".Equals(strRetCode)))
                            {
                                this.txtResultShow.Text = this.txtResultShow.Text + "【RetCode：】" + strRetCode + "\r\n";
                                this.txtResultShow.Text = this.txtResultShow.Text + "【RetMsg：】" + strRetMsg + "\r\n";
                                // 退出方法前，要写日志
                                WriteLog.writeLog(this.txtResultShow.Text, queryCodeVersion, strBeijingTime);
                                return;
                            }


                            // 【6】将xml字符串，序列化为对象
                            string strQrygjdtlReturnSrc = tt;
                            // QRYGJDTL返回的原串的<pub>节点部分
                            string strQrygjdtlOfPubSrc = string.Empty;
                            int posStartPub = strQrygjdtlReturnSrc.IndexOf(@"<pub>");
                            int posEndPub = strQrygjdtlReturnSrc.LastIndexOf(@"</pub>");
                            strQrygjdtlOfPubSrc = strQrygjdtlReturnSrc.Substring(posStartPub, posEndPub - posStartPub + 6);
                            // 将 strQrygjdtlOfPubSrc 转换为pub 对象
                            QrygjdtlPub objPub = XmlSerializeHelper.DESerializer<QrygjdtlPub>(strQrygjdtlOfPubSrc);


                            // QRYGJDTL返回的<out>节点部分
                            string strQrygjdtlOfOutSrc = string.Empty;
                            int posStartOut = strQrygjdtlReturnSrc.IndexOf(@"<out>");
                            int posEndOut = strQrygjdtlReturnSrc.LastIndexOf(@"</out>");
                            strQrygjdtlOfOutSrc = strQrygjdtlReturnSrc.Substring(posStartOut, posEndOut - posStartOut + 6);

                            // 得到<out>节点部分后，进一步处理，在<rd>前加<rds>，在</rd>加</rds>
                            // ...此处还需要加业务逻辑，如果 posFirsIndexOfRdInOut < 0 ，则没有 rd 循环体返回
                            int posFirsIndexOfRdInOut = strQrygjdtlOfOutSrc.IndexOf(@"<rd>");
                            strQrygjdtlOfOutSrc = strQrygjdtlOfOutSrc.Insert(posFirsIndexOfRdInOut, @"<rds>");
                            int posLastIndexOfRdInOut = strQrygjdtlOfOutSrc.LastIndexOf(@"</rd>");
                            strQrygjdtlOfOutSrc = strQrygjdtlOfOutSrc.Insert(posLastIndexOfRdInOut + 5, @"</rds>");

                            // 将xml out 部分 转换为 对象
                            objtemp = XmlSerializeHelper.DESerializer<QrygjdtlOut>(strQrygjdtlOfOutSrc);
                            if (iCountQRYGJDTL == 0)
                            {
                                objOut.listQrygjtlRd = objtemp.listQrygjtlRd;
                            }
                            else
                            {
                                objOut.listQrygjtlRd.AddRange(objtemp.listQrygjtlRd);
                            }

                            xml.Save("d:\\test\\" + "【" + objQRYGJDTL0010.fSeqno + "】" + "第" + (iCountQRYGJDTL + 1).ToString() + "次QRYGJDTL.xml");//保存
                            XmlNodeList nodelist_pub = xml.GetElementsByTagName("NextTag");
                            objQRYGJDTL0010.NextTag = nodelist_pub.Item(0).InnerText;
                            iCountQRYGJDTL++;

                        } while (objQRYGJDTL0010.NextTag.Length != 0);
                        this.txtResultShow.Text = this.txtResultShow.Text + "【总共发查询包的个数是：】\r\n" + iCountQRYGJDTL.ToString() + "\r\n";
                        this.txtResultShow.Text = this.txtResultShow.Text + "【总共有当日明细：】" + objOut.listQrygjtlRd.Count.ToString() + "条" + "\r\n";
                        this.dgvShowQpdRd.DataSource = objOut.listQrygjtlRd;
                        tools.DataGridViewStyle.DgvStyle1(this.dgvShowQpdRd);
                    }
                    WriteLog.writeLog(this.txtResultShow.Text, queryCodeVersion, strBeijingTime);
                    return;
                    break;

                #endregion

                #region case "QPAYENT0.0.1.0":
                case "QPAYENT0.0.1.0":
                    //根据交易代码，创建并封装实体类对象
                    objQPAYENT0010 = createObjectQpayent0010(strBeijingTime);
                    // 作为对象传输，进而生成报文
                    strScr = CreateXml.CreateXml.CreateQPAYENT0010(objQPAYENT0010);
                    break;
                #endregion

                #region case "QPAYPER0.0.1.0":
                case "QPAYPER0.0.1.0":
                    //根据交易代码，创建并封装实体类对象
                    objQpayper0010 = createObjectQpayper0010(strBeijingTime);
                    // 作为对象传输，进而生成报文
                    strScr = CreateXml.CreateXml.CreateQpayper0010(objQpayper0010);
                    break;
                #endregion

                #region case "DIBPSBC0.0.1.0" :
                case "DIBPSBC0.0.1.0":
                    MessageBox.Show("DIBPSBC行名行号信息下载接口只支持0.0.0.1版本，请选择此版本接口！");
                    return;
                    break;
                    #endregion

                #region  case "DIBPSBC0.0.0.1":                    
                    case "DIBPSBC0.0.0.1":
                    {
                        int iPackageCount = 0;
                        DateTime dtStart = DateTime.Now;
                        DibpsbcOut objtemp = new DibpsbcOut();
                        DibpsbcOut objOut = new DibpsbcOut();
                        // 封装DIBPSBC对象
                        objDIBPSBC0001 = createObjectDIBPSBC0001(strBeijingTime);
                        do
                        {
                            // 产生DIBPSBC的XML报文
                            strScr = CreateXml.CreateXml.CreateDIBPSBC0001(objDIBPSBC0001);
                            // 【3】确定POST 数据 查询类的ZIP标识都是false，结算类根据实际情况送
                            strPostData = CmpCommonTools.createPostData(objActionUrl, false, strScr);
                            // 【4】向加密端口发送
                            EncryptResult = SignOrEncrypt.EncryptOrSign(strActionUrl, strPostData, 1);   // 第三个参数：   0  签名； 1  加密
                            this.txtResultShow.Text = "【往448发的URL：】\r\n" + strActionUrl + "\r\n";
                            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的postData：】\r\n" + strPostData + "\r\n";
                            byte[] bb = Convert.FromBase64String(EncryptResult);
                            string tt = Encoding.GetEncoding(936).GetString(bb);
                            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回1：】\r\n" + EncryptResult + "\r\n";
                            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回2：】\r\n" + tt + "\r\n";
                            // 【5】解析xml返回报文中的nextTag字段
                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(tt);

                            // 【如果 RetCode ！= 0 】 ，则不进行 指令包处理
                            XmlNodeList nodelist_RetCode = xml.GetElementsByTagName("RetCode");
                            string strRetCode = nodelist_RetCode.Item(0).InnerText;
                            XmlNodeList nodelist_RetMsg = xml.GetElementsByTagName("RetMsg");
                            string strRetMsg = nodelist_RetMsg.Item(0).InnerText;
                            if (!("0".Equals(strRetCode)))
                            {
                                this.txtResultShow.Text = this.txtResultShow.Text + "【RetCode：】" + strRetCode + "\r\n";
                                this.txtResultShow.Text = this.txtResultShow.Text + "【RetMsg：】" + strRetMsg + "\r\n";
                                // 退出方法前，要写日志
                                WriteLog.writeLog(this.txtResultShow.Text, queryCodeVersion, strBeijingTime);
                                return;
                            }

                            // 【6】将xml字符串，序列化为对象
                            string strDibpsbcReturnSrc = tt;
                            // DIBPSBC返回的原串的<pub>节点部分
                            string strDibpsbcOfPubSrc = string.Empty;
                            int posStartPub = strDibpsbcReturnSrc.IndexOf(@"<pub>");
                            int posEndPub = strDibpsbcReturnSrc.LastIndexOf(@"</pub>");
                            strDibpsbcOfPubSrc = strDibpsbcReturnSrc.Substring(posStartPub, posEndPub - posStartPub + 6);
                            // 将 strDibpsbcOfPubSrc 转换为pub 对象
                            DibpsbcPub objPub = XmlSerializeHelper.DESerializer<DibpsbcPub>(strDibpsbcOfPubSrc);


                            // DIBPSBC返回的<out>节点部分
                            string strDibpsbcOfOutSrc = string.Empty;
                            int posStartOut = strDibpsbcReturnSrc.IndexOf(@"<out>");
                            int posEndOut = strDibpsbcReturnSrc.LastIndexOf(@"</out>");
                            strDibpsbcOfOutSrc = strDibpsbcReturnSrc.Substring(posStartOut, posEndOut - posStartOut + 6);

                            // 得到<out>节点部分后，进一步处理，在<rd>前加<rds>，在</rd>加</rds>
                            // ...此处还需要加业务逻辑，如果 posFirsIndexOfRdInOut < 0 ，则没有 rd 循环体返回
                            int posFirsIndexOfRdInOut = strDibpsbcOfOutSrc.IndexOf(@"<rd>");
                            strDibpsbcOfOutSrc = strDibpsbcOfOutSrc.Insert(posFirsIndexOfRdInOut, @"<rds>");
                            int posLastIndexOfRdInOut = strDibpsbcOfOutSrc.LastIndexOf(@"</rd>");
                            strDibpsbcOfOutSrc = strDibpsbcOfOutSrc.Insert(posLastIndexOfRdInOut + 5, @"</rds>");

                            // 将xml out 部分 转换为 对象
                            objtemp = XmlSerializeHelper.DESerializer<DibpsbcOut>(strDibpsbcOfOutSrc);
                            if (iPackageCount == 0)
                            {
                                objOut.listDibpsbcRd = objtemp.listDibpsbcRd;
                            }
                            else
                            {
                                objOut.listDibpsbcRd.AddRange(objtemp.listDibpsbcRd);
                            }

                            // xml.Save("d:\\test\\" + "【" + objDIBPSBC0001.fSeqno + "】" + "第" + (iPackageCount + 1).ToString() + "次DIBPSBC.xml");//保存,文件量太大，故没保存
                            XmlNodeList nodelist_pub = xml.GetElementsByTagName("NextTag");
                            objDIBPSBC0001.NextTag = nodelist_pub.Item(0).InnerText;
                            iPackageCount++;

                        } while (objDIBPSBC0001.NextTag.Length != 0);
                        DateTime dtEndtime1 = DateTime.Now;
                        this.txtResultShow.Text = this.txtResultShow.Text + "【总共发查询包的个数是：】\r\n" + iPackageCount.ToString() + "\r\n";
                        this.txtResultShow.Text = this.txtResultShow.Text + "【总共有行号记录：】" + objOut.listDibpsbcRd.Count.ToString() + "条" + "\r\n";
                        this.dgvShowQpdRd.DataSource = objOut.listDibpsbcRd;
                        DateTime dtEndtime2 = DateTime.Now;
                        this.txtResultShow.Text = this.txtResultShow.Text + "【开始时间：】" + dtStart.ToString("yyyyMMddHHmmssSSS")  + "\r\n";
                        this.txtResultShow.Text = this.txtResultShow.Text + "【list对象ADD完毕后的时间：】" + dtEndtime1.ToString("yyyyMMddHHmmssSSS") + "\r\n";
                        this.txtResultShow.Text = this.txtResultShow.Text + "【list<rd>显示在datagridview的时间：】" + dtEndtime2.ToString("yyyyMMddHHmmssSSS") + "\r\n";
                        tools.DataGridViewStyle.DgvStyle1(this.dgvShowQpdRd);
                    }
                    WriteLog.writeLog(this.txtResultShow.Text, queryCodeVersion, strBeijingTime);
                    return;
                    break;
                #endregion
                default:
                    MessageBox.Show("该版本的接口不支持，或者作者未开发此版本接口！");
                    return;
                    break;
            }

            #endregion
            // 【3】确定POST 数据
            strPostData = CmpCommonTools.createPostData(objActionUrl, false, strScr);
            // 【4】向加密端口发送
            EncryptResult = SignOrEncrypt.EncryptOrSign(strActionUrl, strPostData, 1);
            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的URL：】\r\n" + strActionUrl + "\r\n";
            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的postData：】\r\n" + strPostData + "\r\n";
            byte[] b = Convert.FromBase64String(EncryptResult);
            string t = Encoding.GetEncoding(936).GetString(b);
            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回1：】\r\n" + EncryptResult + "\r\n";
            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回2：】\r\n" + t + "\r\n";

            // 【5】记录日志
            WriteLog.writeLog(this.txtResultShow.Text, queryCodeVersion , strBeijingTime);
        }

        #region 其他工具类函数
        /// <summary>
        /// 给DataGridView添加行号
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="e"></param>
        private void dgvShowQpdRd_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            tools.DataGridViewStyle.DgvRowPostPaint(this.dgvShowQpdRd, e);
        }


        /// <summary>
        /// 将结果视图Datagridview导出为excel ,20200621提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            //ExportDvgToExcel objExpertToExcel = new ExportDvgToExcel();
            //objExpertToExcel.OutputAsExcelFile(this.dgvShowQpdRd);

            ExportDgvToExcelByNPOI.ExportExcel("", this.dgvShowQpdRd, "宋体", 11); //默认文件名,DataGridView控件的名称,字体,字号 
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBase64Decode_Click(object sender, EventArgs e)
        {
            string str_SrcBased64 = this.txtResultShow.Text;
            byte[] b = Convert.FromBase64String(str_SrcBased64);
            string t = Encoding.GetEncoding(936).GetString(b);
            this.txtResultShow.Text = this.txtResultShow.Text + "\r\n【解码后：】\r\n" + t;
        }

        /// <summary>
        /// 先压缩，再base64编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZipAndBase64_Click(object sender, EventArgs e)
        {
            string src = String.Empty;
            src = this.txtResultShow.Text.Trim();
            string afterZipAndBase64 = ZipAndBase64.Compress(src);
            this.txtResultShow.Text = this.txtResultShow.Text + "\r\n" + "【Gzip后：】" + afterZipAndBase64;
            afterZipAndBase64 = tools.Base64AndUnbase64.EncodeBase64("gb2312", afterZipAndBase64);
            this.txtResultShow.Text = this.txtResultShow.Text + "\r\n" + "【zip并base64后：】" + afterZipAndBase64;
        }

        private void btnUnbase64AndUnzip_Click(object sender, EventArgs e)
        {
            string src = this.txtResultShow.Text.Trim();
            string strUnbase64AndUnzip = tools.ZipHelper.GZipDecompressString(src);
            this.txtResultShow.Text = this.txtResultShow.Text + "\r\n【Base64解码且解压之后的报文是：】" + strUnbase64AndUnzip;
        }

        private void btnUrlEncode_Click(object sender, EventArgs e)
        {
            string src = this.txtResultShow.Text.Trim();
            // string srcUrlEncoded = HttpUtility.UrlEncode(src, System.Text.Encoding.GetEncoding("GB2312"));
            string srcUrlEncoded = HttpUtility.UrlEncode(src);
            this.txtResultShow.Text = this.txtResultShow.Text + "\r\n【URL编码后：】" + "\r\n" + srcUrlEncoded;
        }

        private void btnUrlDecode_Click(object sender, EventArgs e)
        {
            string strBeforeDecode = this.txtResultShow.Text.Trim();
            string src = HttpUtility.UrlDecode(strBeforeDecode);
            this.txtResultShow.Text = this.txtResultShow.Text + "\r\n【URL解码后：】" + "\r\n" + src;
        }

        private void btnSaveToTxt_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (this.txtResultShow.Text.Length == 0)
            {
                MessageBox.Show("没有任何内容，就不保存了。。。");
                return;
            }
        }
        #endregion

        private void btnXmlToClass_Click(object sender, EventArgs e)
        {
            Frm_XmlToClass objFrm_XmlToClass = new Frm_XmlToClass();
            objFrm_XmlToClass.Show();
        }
        /// <summary>
        /// 烟草测试支付专用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTabaccoTestOnly_Click(object sender, EventArgs e)
        {

            this.dgvShowQpdRd.DataSource = null;
            this.txtResultShow.Text = string.Empty;
            string strBeijingTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");   // PC实际提交实际
            strSignTime = strTestEnvironmentDate + strBeijingTime.Substring(8, 9);  // 根据测试环境时间改变签名时间

            #region 验证选择项
            // 账号非空验证。。。
            // 账号的数字验证
            // 户名长度验证。。。
            // 用途长度验证。。。      

            // 判断是否选择了业务类型
            if (returnTransCodeByChoose().Length == 0 || returnTransCodeByChoose() == null)
            {
                MessageBox.Show("请选择业务类型！");
                return;
            }
            #endregion

            #region 结算提交类业务参数初始化
            // 确定接口版本
            if (this.radInterface0001.Checked)
            {
                strInterfaceVersion = "0.0.0.1";
            }
            else
            {
                strInterfaceVersion = "0.0.1.0";
            }

            // 确定 报文部分是否压缩
            if (this.ckBoxZipFlag.Checked)
            {
                zipFlag = true;
            }
            else
            {
                zipFlag = false;
            }

            // 确定交易代码 , 根据选择的单选按钮返回transCode
            strTransCode = returnTransCodeByChoose();
            transCodeVersionZipFlag = strTransCode + strInterfaceVersion + zipFlag.ToString();



            // 【1】确定签名URL地址
            strSignUrl = @"http://" + strPostAddress + ":" + strSignPort;
            // 【2】根据业务参数组包
            // 根据交易代码transCode 创建不同对象，进而创建不同报文
            switch (transCodeVersionZipFlag)
            {
                case "PAYENT0.0.1.0False":
                    //根据交易代码，创建并封装实体类对象
                    objPayent = createObjectPayent0010(strBeijingTime);
                    // 作为对象传输，进而生成报文
                    strScr = CreateXml.CreateXml.CreatePAYENT0010(objPayent);
                    break;
                case "PAYENT0.0.1.0True":
                    objPayent = createObjectPayent0010(strBeijingTime);
                    strScr = CreateXml.CreateXml.CreatePAYENTWithZip0010(objPayent);
                    break;
                //case "PAYPER0.0.1.0False":
                //    objPayper0010 = createObjectPayper0010(strBeijingTime);
                //    strScr = CreateXml.CreateXml.CreatePAYPER0010(objPayper0010);
                //    break;
                //case "PAYPERCOL0.0.1.0False":
                //    objPaypercol0010 = createObjectPaypercol0010(strBeijingTime);
                //    strScr = CreateXml.CreateXml.CreatePAYPERCOL0010(objPaypercol0010);
                //    break;
                default:
                    MessageBox.Show("该版本的接口不支持，或者作者未开发此版本接口！");
                    return;
                    break;
            }

            #endregion

            #region 签名服务
            // 【3】向签名端口发送
            strSignResult = SignOrEncrypt.EncryptOrSign(strSignUrl, strScr, 0);
            this.txtResultShow.Text = "【签名URL地址：】\r\n" + strSignUrl + "\r\n";
            this.txtResultShow.Text = this.txtResultShow.Text + "【签名前的原包：】\r\n" + strScr + "\r\n";
            this.txtResultShow.Text = this.txtResultShow.Text + "【签名后的数据：】\r\n" + strSignResult + "\r\n";
            #endregion

            #region Http加密服务
            // 【1】确定HTTP加密服务的 ACTION  URL地址
            // 封装ActionUrl对象
            ActionUrl objActionUrl = new ActionUrl()
            {
                strHttpPort = strHttpPort,
                strPostAddress = strPostAddress,
                strId = strId,
                strPackageID = strBeijingTime,
                strSendTime = strBeijingTime,
                strInterfaceVersion = strInterfaceVersion,
                strCis = strCis,
                strTransCode = strTransCode

            };
            string strActionUrl = CmpCommonTools.makeActionUrl(objActionUrl);
            // 【2】确定POST 数据
            strPostData = CmpCommonTools.createPostData(objActionUrl, zipFlag, strSignResult);
            // 【3】向加密端口发送
            EncryptResult = SignOrEncrypt.EncryptOrSign(strActionUrl, strPostData, 1);
            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的URL：】\r\n" + strActionUrl + "\r\n";
            this.txtResultShow.Text = this.txtResultShow.Text + "【往448发的postData：】\r\n" + strPostData + "\r\n";
            byte[] b = Convert.FromBase64String(EncryptResult);
            string t = Encoding.GetEncoding(936).GetString(b);
            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回1：】\r\n" + EncryptResult + "\r\n";
            this.txtResultShow.Text = this.txtResultShow.Text + "【银行返回2：】\r\n" + t + "\r\n";
            // 【5】记录日志
            WriteLog.writeLog(this.txtResultShow.Text, strTransCode, strBeijingTime);
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WriteLog.writeLogByOne(this.txtResultShow.Text, "原支付包号" + this.txtQueryCondition.Text.Trim());
            MessageBox.Show("手动保存成功");
        }
    }
}
