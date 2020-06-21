namespace CMPSimulator
{
    partial class Frm_main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtShowResult = new System.Windows.Forms.TextBox();
            this.panelInterface = new System.Windows.Forms.Panel();
            this.radbtn_0010 = new System.Windows.Forms.RadioButton();
            this.radbtn_0001 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCIS = new System.Windows.Forms.TextBox();
            this.btnSign = new System.Windows.Forms.Button();
            this.btnDecodeBase64 = new System.Windows.Forms.Button();
            this.btnQueryAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPayAccNo = new System.Windows.Forms.TextBox();
            this.txtRecAccNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPayAccName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRecAccName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTestEnvironmentDate = new System.Windows.Forms.TextBox();
            this.btnSignToXML = new System.Windows.Forms.Button();
            this.gbxChooseQueryType = new System.Windows.Forms.GroupBox();
            this.radPAYPER0010 = new System.Windows.Forms.RadioButton();
            this.radPAYPERCOL0010 = new System.Windows.Forms.RadioButton();
            this.cbxOtherBank = new System.Windows.Forms.CheckBox();
            this.txtCustomizeCount = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.radPayent0010CustomizeCount = new System.Windows.Forms.RadioButton();
            this.radDIBPSBC0001 = new System.Windows.Forms.RadioButton();
            this.radQPERDIS0001 = new System.Windows.Forms.RadioButton();
            this.radPERDISCOL0001 = new System.Windows.Forms.RadioButton();
            this.radQENTDIS = new System.Windows.Forms.RadioButton();
            this.radENTDISCOL0010 = new System.Windows.Forms.RadioButton();
            this.radQRYGJDTL = new System.Windows.Forms.RadioButton();
            this.radGjkPayent = new System.Windows.Forms.RadioButton();
            this.radPayentAndZip = new System.Windows.Forms.RadioButton();
            this.radDIBPSBC = new System.Windows.Forms.RadioButton();
            this.radQHISBAL = new System.Windows.Forms.RadioButton();
            this.radQPAYPER = new System.Windows.Forms.RadioButton();
            this.radQPAYENT = new System.Windows.Forms.RadioButton();
            this.radQHISD = new System.Windows.Forms.RadioButton();
            this.radQPD = new System.Windows.Forms.RadioButton();
            this.radQACCBAL = new System.Windows.Forms.RadioButton();
            this.dtpBeginDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPostAddress = new System.Windows.Forms.TextBox();
            this.txtHttpPort = new System.Windows.Forms.TextBox();
            this.txtSignPort = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnBankRespRestore = new System.Windows.Forms.Button();
            this.btnZipAndBase64 = new System.Windows.Forms.Button();
            this.btnUnbase64AndUnzip = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtContrNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtContractNo = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPersonNo = new System.Windows.Forms.TextBox();
            this.btnUrlEncode = new System.Windows.Forms.Button();
            this.btnUrlDecode = new System.Windows.Forms.Button();
            this.btnGoPayentUi = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panelInterface.SuspendLayout();
            this.gbxChooseQueryType.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtShowResult
            // 
            this.txtShowResult.HideSelection = false;
            this.txtShowResult.Location = new System.Drawing.Point(12, 253);
            this.txtShowResult.Multiline = true;
            this.txtShowResult.Name = "txtShowResult";
            this.txtShowResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtShowResult.Size = new System.Drawing.Size(768, 346);
            this.txtShowResult.TabIndex = 0;
            // 
            // panelInterface
            // 
            this.panelInterface.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelInterface.Controls.Add(this.radbtn_0010);
            this.panelInterface.Controls.Add(this.radbtn_0001);
            this.panelInterface.Location = new System.Drawing.Point(12, 12);
            this.panelInterface.Name = "panelInterface";
            this.panelInterface.Size = new System.Drawing.Size(147, 29);
            this.panelInterface.TabIndex = 1;
            // 
            // radbtn_0010
            // 
            this.radbtn_0010.AutoSize = true;
            this.radbtn_0010.Checked = true;
            this.radbtn_0010.Location = new System.Drawing.Point(74, 6);
            this.radbtn_0010.Name = "radbtn_0010";
            this.radbtn_0010.Size = new System.Drawing.Size(65, 16);
            this.radbtn_0010.TabIndex = 1;
            this.radbtn_0010.TabStop = true;
            this.radbtn_0010.Text = "0.0.1.0";
            this.radbtn_0010.UseVisualStyleBackColor = true;
            // 
            // radbtn_0001
            // 
            this.radbtn_0001.AutoSize = true;
            this.radbtn_0001.Location = new System.Drawing.Point(3, 6);
            this.radbtn_0001.Name = "radbtn_0001";
            this.radbtn_0001.Size = new System.Drawing.Size(65, 16);
            this.radbtn_0001.TabIndex = 0;
            this.radbtn_0001.Text = "0.0.0.1";
            this.radbtn_0001.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(165, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "证书ID：";
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(224, 15);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(120, 21);
            this.txtID.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(360, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "CIS：";
            // 
            // txtCIS
            // 
            this.txtCIS.Location = new System.Drawing.Point(401, 14);
            this.txtCIS.Name = "txtCIS";
            this.txtCIS.Size = new System.Drawing.Size(139, 21);
            this.txtCIS.TabIndex = 3;
            // 
            // btnSign
            // 
            this.btnSign.Location = new System.Drawing.Point(12, 605);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(75, 23);
            this.btnSign.TabIndex = 4;
            this.btnSign.Text = "结算类";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // btnDecodeBase64
            // 
            this.btnDecodeBase64.Location = new System.Drawing.Point(174, 605);
            this.btnDecodeBase64.Name = "btnDecodeBase64";
            this.btnDecodeBase64.Size = new System.Drawing.Size(75, 23);
            this.btnDecodeBase64.TabIndex = 5;
            this.btnDecodeBase64.Text = "Base64解码";
            this.btnDecodeBase64.UseVisualStyleBackColor = true;
            this.btnDecodeBase64.Click += new System.EventHandler(this.btnDecodeBase64_Click);
            // 
            // btnQueryAll
            // 
            this.btnQueryAll.Location = new System.Drawing.Point(93, 605);
            this.btnQueryAll.Name = "btnQueryAll";
            this.btnQueryAll.Size = new System.Drawing.Size(75, 23);
            this.btnQueryAll.TabIndex = 6;
            this.btnQueryAll.Text = "查询类";
            this.btnQueryAll.UseVisualStyleBackColor = true;
            this.btnQueryAll.Click += new System.EventHandler(this.btnQueryAll_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "付号：";
            // 
            // txtPayAccNo
            // 
            this.txtPayAccNo.Location = new System.Drawing.Point(52, 47);
            this.txtPayAccNo.Name = "txtPayAccNo";
            this.txtPayAccNo.Size = new System.Drawing.Size(139, 21);
            this.txtPayAccNo.TabIndex = 3;
            // 
            // txtRecAccNo
            // 
            this.txtRecAccNo.Location = new System.Drawing.Point(52, 74);
            this.txtRecAccNo.Name = "txtRecAccNo";
            this.txtRecAccNo.Size = new System.Drawing.Size(139, 21);
            this.txtRecAccNo.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "收号：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(215, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "付户：";
            // 
            // txtPayAccName
            // 
            this.txtPayAccName.Location = new System.Drawing.Point(256, 50);
            this.txtPayAccName.Name = "txtPayAccName";
            this.txtPayAccName.Size = new System.Drawing.Size(139, 21);
            this.txtPayAccName.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(215, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "收户：";
            // 
            // txtRecAccName
            // 
            this.txtRecAccName.Location = new System.Drawing.Point(256, 77);
            this.txtRecAccName.Name = "txtRecAccName";
            this.txtRecAccName.Size = new System.Drawing.Size(139, 21);
            this.txtRecAccName.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(555, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "测试环境日期yyyyMMdd：";
            // 
            // txtTestEnvironmentDate
            // 
            this.txtTestEnvironmentDate.Location = new System.Drawing.Point(692, 11);
            this.txtTestEnvironmentDate.Name = "txtTestEnvironmentDate";
            this.txtTestEnvironmentDate.Size = new System.Drawing.Size(88, 21);
            this.txtTestEnvironmentDate.TabIndex = 3;
            this.txtTestEnvironmentDate.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnSignToXML
            // 
            this.btnSignToXML.Location = new System.Drawing.Point(255, 605);
            this.btnSignToXML.Name = "btnSignToXML";
            this.btnSignToXML.Size = new System.Drawing.Size(75, 23);
            this.btnSignToXML.TabIndex = 9;
            this.btnSignToXML.Text = "仅签名";
            this.btnSignToXML.UseVisualStyleBackColor = true;
            this.btnSignToXML.Click += new System.EventHandler(this.btnSignToXML_Click);
            // 
            // gbxChooseQueryType
            // 
            this.gbxChooseQueryType.Controls.Add(this.radPAYPER0010);
            this.gbxChooseQueryType.Controls.Add(this.radPAYPERCOL0010);
            this.gbxChooseQueryType.Controls.Add(this.cbxOtherBank);
            this.gbxChooseQueryType.Controls.Add(this.txtCustomizeCount);
            this.gbxChooseQueryType.Controls.Add(this.label14);
            this.gbxChooseQueryType.Controls.Add(this.radPayent0010CustomizeCount);
            this.gbxChooseQueryType.Controls.Add(this.radDIBPSBC0001);
            this.gbxChooseQueryType.Controls.Add(this.radQPERDIS0001);
            this.gbxChooseQueryType.Controls.Add(this.radPERDISCOL0001);
            this.gbxChooseQueryType.Controls.Add(this.radQENTDIS);
            this.gbxChooseQueryType.Controls.Add(this.radENTDISCOL0010);
            this.gbxChooseQueryType.Controls.Add(this.radQRYGJDTL);
            this.gbxChooseQueryType.Controls.Add(this.radGjkPayent);
            this.gbxChooseQueryType.Controls.Add(this.radPayentAndZip);
            this.gbxChooseQueryType.Controls.Add(this.radDIBPSBC);
            this.gbxChooseQueryType.Controls.Add(this.radQHISBAL);
            this.gbxChooseQueryType.Controls.Add(this.radQPAYPER);
            this.gbxChooseQueryType.Controls.Add(this.radQPAYENT);
            this.gbxChooseQueryType.Controls.Add(this.radQHISD);
            this.gbxChooseQueryType.Controls.Add(this.radQPD);
            this.gbxChooseQueryType.Controls.Add(this.radQACCBAL);
            this.gbxChooseQueryType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbxChooseQueryType.Location = new System.Drawing.Point(401, 41);
            this.gbxChooseQueryType.Name = "gbxChooseQueryType";
            this.gbxChooseQueryType.Size = new System.Drawing.Size(379, 189);
            this.gbxChooseQueryType.TabIndex = 10;
            this.gbxChooseQueryType.TabStop = false;
            this.gbxChooseQueryType.Text = "请选择业务类型：";
            // 
            // radPAYPER0010
            // 
            this.radPAYPER0010.AutoSize = true;
            this.radPAYPER0010.Location = new System.Drawing.Point(119, 56);
            this.radPAYPER0010.Name = "radPAYPER0010";
            this.radPAYPER0010.Size = new System.Drawing.Size(101, 16);
            this.radPAYPER0010.TabIndex = 20;
            this.radPAYPER0010.TabStop = true;
            this.radPAYPER0010.Text = "radPAYPER0010";
            this.radPAYPER0010.UseVisualStyleBackColor = true;
            // 
            // radPAYPERCOL0010
            // 
            this.radPAYPERCOL0010.AutoSize = true;
            this.radPAYPERCOL0010.Location = new System.Drawing.Point(6, 56);
            this.radPAYPERCOL0010.Name = "radPAYPERCOL0010";
            this.radPAYPERCOL0010.Size = new System.Drawing.Size(107, 16);
            this.radPAYPERCOL0010.TabIndex = 19;
            this.radPAYPERCOL0010.TabStop = true;
            this.radPAYPERCOL0010.Text = "企业财务室汇总";
            this.radPAYPERCOL0010.UseVisualStyleBackColor = true;
            // 
            // cbxOtherBank
            // 
            this.cbxOtherBank.AutoSize = true;
            this.cbxOtherBank.Location = new System.Drawing.Point(255, 133);
            this.cbxOtherBank.Name = "cbxOtherBank";
            this.cbxOtherBank.Size = new System.Drawing.Size(72, 16);
            this.cbxOtherBank.TabIndex = 18;
            this.cbxOtherBank.Text = "跨行汇款";
            this.cbxOtherBank.UseVisualStyleBackColor = true;
            // 
            // txtCustomizeCount
            // 
            this.txtCustomizeCount.Location = new System.Drawing.Point(197, 130);
            this.txtCustomizeCount.Name = "txtCustomizeCount";
            this.txtCustomizeCount.Size = new System.Drawing.Size(36, 21);
            this.txtCustomizeCount.TabIndex = 17;
            this.txtCustomizeCount.Text = "1";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(131, 134);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 16;
            this.label14.Text = "自定义笔数";
            // 
            // radPayent0010CustomizeCount
            // 
            this.radPayent0010CustomizeCount.AutoSize = true;
            this.radPayent0010CustomizeCount.Location = new System.Drawing.Point(6, 133);
            this.radPayent0010CustomizeCount.Name = "radPayent0010CustomizeCount";
            this.radPayent0010CustomizeCount.Size = new System.Drawing.Size(119, 16);
            this.radPayent0010CustomizeCount.TabIndex = 15;
            this.radPayent0010CustomizeCount.TabStop = true;
            this.radPayent0010CustomizeCount.Text = "自定义笔数PAYENT";
            this.radPayent0010CustomizeCount.UseVisualStyleBackColor = true;
            // 
            // radDIBPSBC0001
            // 
            this.radDIBPSBC0001.AutoSize = true;
            this.radDIBPSBC0001.Location = new System.Drawing.Point(214, 71);
            this.radDIBPSBC0001.Name = "radDIBPSBC0001";
            this.radDIBPSBC0001.Size = new System.Drawing.Size(119, 16);
            this.radDIBPSBC0001.TabIndex = 14;
            this.radDIBPSBC0001.TabStop = true;
            this.radDIBPSBC0001.Text = "行名行号下载0001";
            this.radDIBPSBC0001.UseVisualStyleBackColor = true;
            // 
            // radQPERDIS0001
            // 
            this.radQPERDIS0001.AutoSize = true;
            this.radQPERDIS0001.Location = new System.Drawing.Point(178, 115);
            this.radQPERDIS0001.Name = "radQPERDIS0001";
            this.radQPERDIS0001.Size = new System.Drawing.Size(119, 16);
            this.radQPERDIS0001.TabIndex = 13;
            this.radQPERDIS0001.TabStop = true;
            this.radQPERDIS0001.Text = "批扣个人查询0001";
            this.radQPERDIS0001.UseVisualStyleBackColor = true;
            // 
            // radPERDISCOL0001
            // 
            this.radPERDISCOL0001.AutoSize = true;
            this.radPERDISCOL0001.Location = new System.Drawing.Point(6, 115);
            this.radPERDISCOL0001.Name = "radPERDISCOL0001";
            this.radPERDISCOL0001.Size = new System.Drawing.Size(143, 16);
            this.radPERDISCOL0001.TabIndex = 12;
            this.radPERDISCOL0001.TabStop = true;
            this.radPERDISCOL0001.Text = "批扣个人汇总记账0001";
            this.radPERDISCOL0001.UseVisualStyleBackColor = true;
            // 
            // radQENTDIS
            // 
            this.radQENTDIS.AutoSize = true;
            this.radQENTDIS.Location = new System.Drawing.Point(178, 93);
            this.radQENTDIS.Name = "radQENTDIS";
            this.radQENTDIS.Size = new System.Drawing.Size(119, 16);
            this.radQENTDIS.TabIndex = 11;
            this.radQENTDIS.TabStop = true;
            this.radQENTDIS.Text = "批扣企业查询0010";
            this.radQENTDIS.UseVisualStyleBackColor = true;
            // 
            // radENTDISCOL0010
            // 
            this.radENTDISCOL0010.AutoSize = true;
            this.radENTDISCOL0010.Location = new System.Drawing.Point(6, 93);
            this.radENTDISCOL0010.Name = "radENTDISCOL0010";
            this.radENTDISCOL0010.Size = new System.Drawing.Size(155, 16);
            this.radENTDISCOL0010.TabIndex = 10;
            this.radENTDISCOL0010.TabStop = true;
            this.radENTDISCOL0010.Text = "扣企业汇总记账提交0010";
            this.radENTDISCOL0010.UseVisualStyleBackColor = true;
            // 
            // radQRYGJDTL
            // 
            this.radQRYGJDTL.AutoSize = true;
            this.radQRYGJDTL.Location = new System.Drawing.Point(201, 39);
            this.radQRYGJDTL.Name = "radQRYGJDTL";
            this.radQRYGJDTL.Size = new System.Drawing.Size(95, 16);
            this.radQRYGJDTL.TabIndex = 9;
            this.radQRYGJDTL.TabStop = true;
            this.radQRYGJDTL.Text = "查管家卡明细";
            this.radQRYGJDTL.UseVisualStyleBackColor = true;
            // 
            // radGjkPayent
            // 
            this.radGjkPayent.AutoSize = true;
            this.radGjkPayent.Location = new System.Drawing.Point(101, 71);
            this.radGjkPayent.Name = "radGjkPayent";
            this.radGjkPayent.Size = new System.Drawing.Size(107, 16);
            this.radGjkPayent.TabIndex = 8;
            this.radGjkPayent.TabStop = true;
            this.radGjkPayent.Text = "管家卡单笔支付";
            this.radGjkPayent.UseVisualStyleBackColor = true;
            // 
            // radPayentAndZip
            // 
            this.radPayentAndZip.AutoSize = true;
            this.radPayentAndZip.Location = new System.Drawing.Point(6, 71);
            this.radPayentAndZip.Name = "radPayentAndZip";
            this.radPayentAndZip.Size = new System.Drawing.Size(89, 16);
            this.radPayentAndZip.TabIndex = 7;
            this.radPayentAndZip.TabStop = true;
            this.radPayentAndZip.Text = "PAYENT加Zip";
            this.radPayentAndZip.UseVisualStyleBackColor = true;
            // 
            // radDIBPSBC
            // 
            this.radDIBPSBC.AutoSize = true;
            this.radDIBPSBC.Location = new System.Drawing.Point(119, 39);
            this.radDIBPSBC.Name = "radDIBPSBC";
            this.radDIBPSBC.Size = new System.Drawing.Size(83, 16);
            this.radDIBPSBC.TabIndex = 6;
            this.radDIBPSBC.TabStop = true;
            this.radDIBPSBC.Text = "查行名行号";
            this.radDIBPSBC.UseVisualStyleBackColor = true;
            // 
            // radQHISBAL
            // 
            this.radQHISBAL.AutoSize = true;
            this.radQHISBAL.Location = new System.Drawing.Point(6, 39);
            this.radQHISBAL.Name = "radQHISBAL";
            this.radQHISBAL.Size = new System.Drawing.Size(107, 16);
            this.radQHISBAL.TabIndex = 5;
            this.radQHISBAL.TabStop = true;
            this.radQHISBAL.Text = "新历史余额查询";
            this.radQHISBAL.UseVisualStyleBackColor = true;
            // 
            // radQPAYPER
            // 
            this.radQPAYPER.AutoSize = true;
            this.radQPAYPER.Location = new System.Drawing.Point(302, 15);
            this.radQPAYPER.Name = "radQPAYPER";
            this.radQPAYPER.Size = new System.Drawing.Size(71, 16);
            this.radQPAYPER.TabIndex = 4;
            this.radQPAYPER.TabStop = true;
            this.radQPAYPER.Text = "财务室查";
            this.radQPAYPER.UseVisualStyleBackColor = true;
            // 
            // radQPAYENT
            // 
            this.radQPAYENT.AutoSize = true;
            this.radQPAYENT.Location = new System.Drawing.Point(225, 15);
            this.radQPAYENT.Name = "radQPAYENT";
            this.radQPAYENT.Size = new System.Drawing.Size(71, 16);
            this.radQPAYENT.TabIndex = 3;
            this.radQPAYENT.TabStop = true;
            this.radQPAYENT.Text = "付款查询";
            this.radQPAYENT.UseVisualStyleBackColor = true;
            // 
            // radQHISD
            // 
            this.radQHISD.AutoSize = true;
            this.radQHISD.Location = new System.Drawing.Point(148, 15);
            this.radQHISD.Name = "radQHISD";
            this.radQHISD.Size = new System.Drawing.Size(71, 16);
            this.radQHISD.TabIndex = 2;
            this.radQHISD.TabStop = true;
            this.radQHISD.Text = "历史明细";
            this.radQHISD.UseVisualStyleBackColor = true;
            // 
            // radQPD
            // 
            this.radQPD.AutoSize = true;
            this.radQPD.Location = new System.Drawing.Point(71, 15);
            this.radQPD.Name = "radQPD";
            this.radQPD.Size = new System.Drawing.Size(71, 16);
            this.radQPD.TabIndex = 1;
            this.radQPD.TabStop = true;
            this.radQPD.Text = "今日明细";
            this.radQPD.UseVisualStyleBackColor = true;
            // 
            // radQACCBAL
            // 
            this.radQACCBAL.AutoSize = true;
            this.radQACCBAL.Location = new System.Drawing.Point(6, 15);
            this.radQACCBAL.Name = "radQACCBAL";
            this.radQACCBAL.Size = new System.Drawing.Size(59, 16);
            this.radQACCBAL.TabIndex = 0;
            this.radQACCBAL.TabStop = true;
            this.radQACCBAL.Text = "查余额";
            this.radQACCBAL.UseVisualStyleBackColor = true;
            // 
            // dtpBeginDate
            // 
            this.dtpBeginDate.Location = new System.Drawing.Point(17, 105);
            this.dtpBeginDate.Name = "dtpBeginDate";
            this.dtpBeginDate.Size = new System.Drawing.Size(105, 21);
            this.dtpBeginDate.TabIndex = 5;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(128, 105);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(103, 21);
            this.dtpEndDate.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 188);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "POST地址：";
            // 
            // txtPostAddress
            // 
            this.txtPostAddress.Location = new System.Drawing.Point(76, 185);
            this.txtPostAddress.Name = "txtPostAddress";
            this.txtPostAddress.Size = new System.Drawing.Size(100, 21);
            this.txtPostAddress.TabIndex = 14;
            // 
            // txtHttpPort
            // 
            this.txtHttpPort.Location = new System.Drawing.Point(244, 185);
            this.txtHttpPort.Name = "txtHttpPort";
            this.txtHttpPort.Size = new System.Drawing.Size(42, 21);
            this.txtHttpPort.TabIndex = 15;
            // 
            // txtSignPort
            // 
            this.txtSignPort.Location = new System.Drawing.Point(353, 185);
            this.txtSignPort.Name = "txtSignPort";
            this.txtSignPort.Size = new System.Drawing.Size(40, 21);
            this.txtSignPort.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(179, 188);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "加密端口：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(287, 188);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "签名端口：";
            // 
            // btnBankRespRestore
            // 
            this.btnBankRespRestore.Location = new System.Drawing.Point(336, 605);
            this.btnBankRespRestore.Name = "btnBankRespRestore";
            this.btnBankRespRestore.Size = new System.Drawing.Size(95, 23);
            this.btnBankRespRestore.TabIndex = 19;
            this.btnBankRespRestore.Text = "银行返回解析";
            this.btnBankRespRestore.UseVisualStyleBackColor = true;
            this.btnBankRespRestore.Click += new System.EventHandler(this.btnBankRespRestore_Click);
            // 
            // btnZipAndBase64
            // 
            this.btnZipAndBase64.Location = new System.Drawing.Point(436, 605);
            this.btnZipAndBase64.Name = "btnZipAndBase64";
            this.btnZipAndBase64.Size = new System.Drawing.Size(90, 23);
            this.btnZipAndBase64.TabIndex = 20;
            this.btnZipAndBase64.Text = "zipAndBase64";
            this.btnZipAndBase64.UseVisualStyleBackColor = true;
            this.btnZipAndBase64.Click += new System.EventHandler(this.btnZipAndBase64_Click);
            // 
            // btnUnbase64AndUnzip
            // 
            this.btnUnbase64AndUnzip.Location = new System.Drawing.Point(527, 605);
            this.btnUnbase64AndUnzip.Name = "btnUnbase64AndUnzip";
            this.btnUnbase64AndUnzip.Size = new System.Drawing.Size(123, 23);
            this.btnUnbase64AndUnzip.TabIndex = 21;
            this.btnUnbase64AndUnzip.Text = "Unbase64AndUnzip";
            this.btnUnbase64AndUnzip.UseVisualStyleBackColor = true;
            this.btnUnbase64AndUnzip.Click += new System.EventHandler(this.btnUnbase64AndUnzip_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 139);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 22;
            this.label11.Text = "扣企业协议编号";
            // 
            // txtContrNo
            // 
            this.txtContrNo.Location = new System.Drawing.Point(110, 132);
            this.txtContrNo.Name = "txtContrNo";
            this.txtContrNo.Size = new System.Drawing.Size(100, 21);
            this.txtContrNo.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 164);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 12);
            this.label12.TabIndex = 24;
            this.label12.Text = "扣个人协议编号(总)";
            // 
            // txtContractNo
            // 
            this.txtContractNo.Location = new System.Drawing.Point(128, 159);
            this.txtContractNo.Name = "txtContractNo";
            this.txtContractNo.Size = new System.Drawing.Size(100, 21);
            this.txtContractNo.TabIndex = 25;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(234, 162);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 26;
            this.label13.Text = "个人编号";
            // 
            // txtPersonNo
            // 
            this.txtPersonNo.Location = new System.Drawing.Point(293, 159);
            this.txtPersonNo.Name = "txtPersonNo";
            this.txtPersonNo.Size = new System.Drawing.Size(100, 21);
            this.txtPersonNo.TabIndex = 27;
            // 
            // btnUrlEncode
            // 
            this.btnUrlEncode.Location = new System.Drawing.Point(656, 605);
            this.btnUrlEncode.Name = "btnUrlEncode";
            this.btnUrlEncode.Size = new System.Drawing.Size(75, 23);
            this.btnUrlEncode.TabIndex = 28;
            this.btnUrlEncode.Text = "urlEncode";
            this.btnUrlEncode.UseVisualStyleBackColor = true;
            this.btnUrlEncode.Click += new System.EventHandler(this.btnUrlEncode_Click);
            // 
            // btnUrlDecode
            // 
            this.btnUrlDecode.Location = new System.Drawing.Point(739, 605);
            this.btnUrlDecode.Name = "btnUrlDecode";
            this.btnUrlDecode.Size = new System.Drawing.Size(75, 23);
            this.btnUrlDecode.TabIndex = 29;
            this.btnUrlDecode.Text = "urlDecode";
            this.btnUrlDecode.UseVisualStyleBackColor = true;
            this.btnUrlDecode.Click += new System.EventHandler(this.btnUrlDecode_Click);
            // 
            // btnGoPayentUi
            // 
            this.btnGoPayentUi.Location = new System.Drawing.Point(51, 224);
            this.btnGoPayentUi.Name = "btnGoPayentUi";
            this.btnGoPayentUi.Size = new System.Drawing.Size(75, 23);
            this.btnGoPayentUi.TabIndex = 30;
            this.btnGoPayentUi.Text = "去支付";
            this.btnGoPayentUi.UseVisualStyleBackColor = true;
            this.btnGoPayentUi.Click += new System.EventHandler(this.btnGoPayentUi_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(143, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 31;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 629);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGoPayentUi);
            this.Controls.Add(this.btnUrlDecode);
            this.Controls.Add(this.btnUrlEncode);
            this.Controls.Add(this.txtPersonNo);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtContractNo);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtContrNo);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnUnbase64AndUnzip);
            this.Controls.Add(this.btnZipAndBase64);
            this.Controls.Add(this.btnBankRespRestore);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtSignPort);
            this.Controls.Add(this.txtHttpPort);
            this.Controls.Add(this.txtPostAddress);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.dtpBeginDate);
            this.Controls.Add(this.gbxChooseQueryType);
            this.Controls.Add(this.btnSignToXML);
            this.Controls.Add(this.txtRecAccName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtRecAccNo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnQueryAll);
            this.Controls.Add(this.btnDecodeBase64);
            this.Controls.Add(this.btnSign);
            this.Controls.Add(this.txtPayAccName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPayAccNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCIS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTestEnvironmentDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelInterface);
            this.Controls.Add(this.txtShowResult);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Frm_main";
            this.Text = "CMPSimulator";
            this.Load += new System.EventHandler(this.Frm_main_Load);
            this.panelInterface.ResumeLayout(false);
            this.panelInterface.PerformLayout();
            this.gbxChooseQueryType.ResumeLayout(false);
            this.gbxChooseQueryType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtShowResult;
        private System.Windows.Forms.Panel panelInterface;
        private System.Windows.Forms.RadioButton radbtn_0010;
        private System.Windows.Forms.RadioButton radbtn_0001;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCIS;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.Button btnDecodeBase64;
        private System.Windows.Forms.Button btnQueryAll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPayAccNo;
        private System.Windows.Forms.TextBox txtRecAccNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPayAccName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRecAccName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTestEnvironmentDate;
        private System.Windows.Forms.Button btnSignToXML;
        private System.Windows.Forms.GroupBox gbxChooseQueryType;
        private System.Windows.Forms.RadioButton radQPAYPER;
        private System.Windows.Forms.RadioButton radQPAYENT;
        private System.Windows.Forms.RadioButton radQHISD;
        private System.Windows.Forms.RadioButton radQPD;
        private System.Windows.Forms.RadioButton radQACCBAL;
        private System.Windows.Forms.DateTimePicker dtpBeginDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPostAddress;
        private System.Windows.Forms.TextBox txtHttpPort;
        private System.Windows.Forms.TextBox txtSignPort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnBankRespRestore;
        private System.Windows.Forms.RadioButton radQHISBAL;
        private System.Windows.Forms.RadioButton radDIBPSBC;
        private System.Windows.Forms.Button btnZipAndBase64;
        private System.Windows.Forms.Button btnUnbase64AndUnzip;
        private System.Windows.Forms.RadioButton radPayentAndZip;
        private System.Windows.Forms.RadioButton radGjkPayent;
        private System.Windows.Forms.RadioButton radQRYGJDTL;
        private System.Windows.Forms.RadioButton radENTDISCOL0010;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtContrNo;
        private System.Windows.Forms.RadioButton radQENTDIS;
        private System.Windows.Forms.RadioButton radPERDISCOL0001;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtContractNo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPersonNo;
        private System.Windows.Forms.RadioButton radQPERDIS0001;
        private System.Windows.Forms.RadioButton radDIBPSBC0001;
        private System.Windows.Forms.TextBox txtCustomizeCount;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton radPayent0010CustomizeCount;
        private System.Windows.Forms.CheckBox cbxOtherBank;
        private System.Windows.Forms.Button btnUrlEncode;
        private System.Windows.Forms.Button btnUrlDecode;
        private System.Windows.Forms.RadioButton radPAYPERCOL0010;
        private System.Windows.Forms.RadioButton radPAYPER0010;
        private System.Windows.Forms.Button btnGoPayentUi;
        private System.Windows.Forms.Button button1;
    }
}

