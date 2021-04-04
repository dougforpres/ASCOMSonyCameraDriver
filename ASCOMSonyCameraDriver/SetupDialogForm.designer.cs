namespace ASCOM.SonyMirrorless
{
    partial class SetupDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.selectFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.checkBoxUseLiveview = new System.Windows.Forms.CheckBox();
            this.textBoxCameraBatteryLevel = new System.Windows.Forms.TextBox();
            this.labelCameraBatteryLevel = new System.Windows.Forms.Label();
            this.textBoxCameraISO = new System.Windows.Forms.TextBox();
            this.labelCameraISO = new System.Windows.Forms.Label();
            this.textBoxCameraExposureTime = new System.Windows.Forms.TextBox();
            this.textBoxCameraCompressionMode = new System.Windows.Forms.TextBox();
            this.textBoxCameraMode = new System.Windows.Forms.TextBox();
            this.textBoxCameraConnected = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBoxOutputFormat = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxPersonality = new System.Windows.Forms.ComboBox();
            this.checkBoxAutoLiveview = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.selectCameraTab = new System.Windows.Forms.TabPage();
            this.cameraPersonalityTab = new System.Windows.Forms.TabPage();
            this.driverSettingsTab = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBoxBulbMode = new System.Windows.Forms.CheckBox();
            this.cameraInfoTab = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.modeWarning = new System.Windows.Forms.PictureBox();
            this.extrasTab = new System.Windows.Forms.TabPage();
            this.buttonSelectFolder = new System.Windows.Forms.Button();
            this.textBoxSaveLocation = new System.Windows.Forms.TextBox();
            this.checkBoxEnableSaveLocation = new System.Windows.Forms.CheckBox();
            this.textBoxBulbMode = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.selectCameraTab.SuspendLayout();
            this.cameraPersonalityTab.SuspendLayout();
            this.driverSettingsTab.SuspendLayout();
            this.cameraInfoTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modeWarning)).BeginInit();
            this.extrasTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(956, 497);
            this.cmdOK.Margin = new System.Windows.Forms.Padding(6);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(118, 46);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(826, 497);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(6);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(118, 48);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1009, 67);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please select the camera you\'d like to work with. (Only devices currently connect" +
    "ed and recognized by Windows are listed)";
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.SonyMirrorless.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(32, 487);
            this.picASCOM.Margin = new System.Windows.Forms.Padding(6);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 142);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Camera";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(16, 325);
            this.chkTrace.Margin = new System.Windows.Forms.Padding(6);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(355, 29);
            this.chkTrace.TabIndex = 4;
            this.chkTrace.Text = "Driver Trace on (Write to log file)";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.AccessibleName = "Sony Camera Selection";
            this.comboBoxCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(146, 134);
            this.comboBoxCamera.Margin = new System.Windows.Forms.Padding(6);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(418, 33);
            this.comboBoxCamera.TabIndex = 1;
            // 
            // selectFolderDialog
            // 
            this.selectFolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // checkBoxUseLiveview
            // 
            this.checkBoxUseLiveview.AutoSize = true;
            this.checkBoxUseLiveview.Location = new System.Drawing.Point(21, 123);
            this.checkBoxUseLiveview.Name = "checkBoxUseLiveview";
            this.checkBoxUseLiveview.Size = new System.Drawing.Size(341, 29);
            this.checkBoxUseLiveview.TabIndex = 15;
            this.checkBoxUseLiveview.Text = "Use Liveview for preview mode";
            this.checkBoxUseLiveview.UseVisualStyleBackColor = true;
            // 
            // textBoxCameraBatteryLevel
            // 
            this.textBoxCameraBatteryLevel.Location = new System.Drawing.Point(256, 68);
            this.textBoxCameraBatteryLevel.Name = "textBoxCameraBatteryLevel";
            this.textBoxCameraBatteryLevel.ReadOnly = true;
            this.textBoxCameraBatteryLevel.Size = new System.Drawing.Size(209, 31);
            this.textBoxCameraBatteryLevel.TabIndex = 11;
            // 
            // labelCameraBatteryLevel
            // 
            this.labelCameraBatteryLevel.AutoSize = true;
            this.labelCameraBatteryLevel.Location = new System.Drawing.Point(16, 71);
            this.labelCameraBatteryLevel.Name = "labelCameraBatteryLevel";
            this.labelCameraBatteryLevel.Size = new System.Drawing.Size(138, 25);
            this.labelCameraBatteryLevel.TabIndex = 10;
            this.labelCameraBatteryLevel.Text = "Battery Level";
            // 
            // textBoxCameraISO
            // 
            this.textBoxCameraISO.Location = new System.Drawing.Point(256, 246);
            this.textBoxCameraISO.Name = "textBoxCameraISO";
            this.textBoxCameraISO.ReadOnly = true;
            this.textBoxCameraISO.Size = new System.Drawing.Size(209, 31);
            this.textBoxCameraISO.TabIndex = 9;
            // 
            // labelCameraISO
            // 
            this.labelCameraISO.AutoSize = true;
            this.labelCameraISO.Location = new System.Drawing.Point(16, 249);
            this.labelCameraISO.Name = "labelCameraISO";
            this.labelCameraISO.Size = new System.Drawing.Size(47, 25);
            this.labelCameraISO.TabIndex = 8;
            this.labelCameraISO.Text = "ISO";
            // 
            // textBoxCameraExposureTime
            // 
            this.textBoxCameraExposureTime.Location = new System.Drawing.Point(256, 202);
            this.textBoxCameraExposureTime.Name = "textBoxCameraExposureTime";
            this.textBoxCameraExposureTime.ReadOnly = true;
            this.textBoxCameraExposureTime.Size = new System.Drawing.Size(209, 31);
            this.textBoxCameraExposureTime.TabIndex = 7;
            // 
            // textBoxCameraCompressionMode
            // 
            this.textBoxCameraCompressionMode.Location = new System.Drawing.Point(256, 156);
            this.textBoxCameraCompressionMode.Name = "textBoxCameraCompressionMode";
            this.textBoxCameraCompressionMode.ReadOnly = true;
            this.textBoxCameraCompressionMode.Size = new System.Drawing.Size(209, 31);
            this.textBoxCameraCompressionMode.TabIndex = 6;
            // 
            // textBoxCameraMode
            // 
            this.textBoxCameraMode.Location = new System.Drawing.Point(256, 112);
            this.textBoxCameraMode.Name = "textBoxCameraMode";
            this.textBoxCameraMode.ReadOnly = true;
            this.textBoxCameraMode.Size = new System.Drawing.Size(209, 31);
            this.textBoxCameraMode.TabIndex = 5;
            // 
            // textBoxCameraConnected
            // 
            this.textBoxCameraConnected.Location = new System.Drawing.Point(256, 24);
            this.textBoxCameraConnected.Name = "textBoxCameraConnected";
            this.textBoxCameraConnected.ReadOnly = true;
            this.textBoxCameraConnected.Size = new System.Drawing.Size(209, 31);
            this.textBoxCameraConnected.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 25);
            this.label7.TabIndex = 3;
            this.label7.Text = "Mode";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(156, 25);
            this.label6.TabIndex = 2;
            this.label6.Text = "Exposure Time";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(167, 25);
            this.label5.TabIndex = 1;
            this.label5.Text = "Save Images As";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Connected";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // comboBoxOutputFormat
            // 
            this.comboBoxOutputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOutputFormat.FormattingEnabled = true;
            this.comboBoxOutputFormat.Items.AddRange(new object[] {
            "RGB (Processed)",
            "RAW/RGGB (Unprocessed)"});
            this.comboBoxOutputFormat.Location = new System.Drawing.Point(262, 72);
            this.comboBoxOutputFormat.Name = "comboBoxOutputFormat";
            this.comboBoxOutputFormat.Size = new System.Drawing.Size(267, 33);
            this.comboBoxOutputFormat.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(213, 25);
            this.label9.TabIndex = 8;
            this.label9.Text = "Image Output Format";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(186, 25);
            this.label8.TabIndex = 1;
            this.label8.Text = "Target Application";
            // 
            // comboBoxPersonality
            // 
            this.comboBoxPersonality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPersonality.FormattingEnabled = true;
            this.comboBoxPersonality.Location = new System.Drawing.Point(262, 24);
            this.comboBoxPersonality.Name = "comboBoxPersonality";
            this.comboBoxPersonality.Size = new System.Drawing.Size(267, 33);
            this.comboBoxPersonality.TabIndex = 0;
            this.comboBoxPersonality.SelectedIndexChanged += new System.EventHandler(this.comboBoxPersonality_SelectedIndexChanged);
            // 
            // checkBoxAutoLiveview
            // 
            this.checkBoxAutoLiveview.AutoSize = true;
            this.checkBoxAutoLiveview.Location = new System.Drawing.Point(16, 27);
            this.checkBoxAutoLiveview.Name = "checkBoxAutoLiveview";
            this.checkBoxAutoLiveview.Size = new System.Drawing.Size(358, 29);
            this.checkBoxAutoLiveview.TabIndex = 0;
            this.checkBoxAutoLiveview.Text = "Use LiveView for 0.0s exposures";
            this.checkBoxAutoLiveview.UseVisualStyleBackColor = true;
            this.checkBoxAutoLiveview.CheckedChanged += new System.EventHandler(this.checkBoxAutoLiveview_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.selectCameraTab);
            this.tabControl1.Controls.Add(this.cameraPersonalityTab);
            this.tabControl1.Controls.Add(this.driverSettingsTab);
            this.tabControl1.Controls.Add(this.cameraInfoTab);
            this.tabControl1.Controls.Add(this.extrasTab);
            this.tabControl1.Location = new System.Drawing.Point(24, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1050, 439);
            this.tabControl1.TabIndex = 21;
            // 
            // selectCameraTab
            // 
            this.selectCameraTab.Controls.Add(this.comboBoxCamera);
            this.selectCameraTab.Controls.Add(this.label1);
            this.selectCameraTab.Controls.Add(this.label2);
            this.selectCameraTab.Location = new System.Drawing.Point(8, 39);
            this.selectCameraTab.Name = "selectCameraTab";
            this.selectCameraTab.Padding = new System.Windows.Forms.Padding(3);
            this.selectCameraTab.Size = new System.Drawing.Size(1034, 392);
            this.selectCameraTab.TabIndex = 0;
            this.selectCameraTab.Text = "Select Camera";
            this.selectCameraTab.UseVisualStyleBackColor = true;
            // 
            // cameraPersonalityTab
            // 
            this.cameraPersonalityTab.Controls.Add(this.comboBoxOutputFormat);
            this.cameraPersonalityTab.Controls.Add(this.checkBoxUseLiveview);
            this.cameraPersonalityTab.Controls.Add(this.label9);
            this.cameraPersonalityTab.Controls.Add(this.comboBoxPersonality);
            this.cameraPersonalityTab.Controls.Add(this.label8);
            this.cameraPersonalityTab.Location = new System.Drawing.Point(8, 39);
            this.cameraPersonalityTab.Name = "cameraPersonalityTab";
            this.cameraPersonalityTab.Padding = new System.Windows.Forms.Padding(3);
            this.cameraPersonalityTab.Size = new System.Drawing.Size(1034, 392);
            this.cameraPersonalityTab.TabIndex = 1;
            this.cameraPersonalityTab.Text = "App Settings";
            this.cameraPersonalityTab.UseVisualStyleBackColor = true;
            // 
            // driverSettingsTab
            // 
            this.driverSettingsTab.Controls.Add(this.textBoxBulbMode);
            this.driverSettingsTab.Controls.Add(this.label10);
            this.driverSettingsTab.Controls.Add(this.checkBoxBulbMode);
            this.driverSettingsTab.Controls.Add(this.checkBoxAutoLiveview);
            this.driverSettingsTab.Location = new System.Drawing.Point(8, 39);
            this.driverSettingsTab.Name = "driverSettingsTab";
            this.driverSettingsTab.Size = new System.Drawing.Size(1034, 392);
            this.driverSettingsTab.TabIndex = 4;
            this.driverSettingsTab.Text = "Driver Settings";
            this.driverSettingsTab.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(566, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 25);
            this.label10.TabIndex = 2;
            this.label10.Text = "seconds";
            // 
            // checkBoxBulbMode
            // 
            this.checkBoxBulbMode.AutoSize = true;
            this.checkBoxBulbMode.Location = new System.Drawing.Point(16, 73);
            this.checkBoxBulbMode.Name = "checkBoxBulbMode";
            this.checkBoxBulbMode.Size = new System.Drawing.Size(494, 29);
            this.checkBoxBulbMode.TabIndex = 1;
            this.checkBoxBulbMode.Text = "Use BULB mode for any exposures longer than";
            this.checkBoxBulbMode.UseVisualStyleBackColor = true;
            this.checkBoxBulbMode.CheckedChanged += new System.EventHandler(this.checkBoxBulbMode_CheckedChanged);
            // 
            // cameraInfoTab
            // 
            this.cameraInfoTab.Controls.Add(this.label3);
            this.cameraInfoTab.Controls.Add(this.modeWarning);
            this.cameraInfoTab.Controls.Add(this.textBoxCameraBatteryLevel);
            this.cameraInfoTab.Controls.Add(this.textBoxCameraISO);
            this.cameraInfoTab.Controls.Add(this.labelCameraBatteryLevel);
            this.cameraInfoTab.Controls.Add(this.label4);
            this.cameraInfoTab.Controls.Add(this.label5);
            this.cameraInfoTab.Controls.Add(this.labelCameraISO);
            this.cameraInfoTab.Controls.Add(this.label6);
            this.cameraInfoTab.Controls.Add(this.textBoxCameraExposureTime);
            this.cameraInfoTab.Controls.Add(this.label7);
            this.cameraInfoTab.Controls.Add(this.textBoxCameraCompressionMode);
            this.cameraInfoTab.Controls.Add(this.textBoxCameraConnected);
            this.cameraInfoTab.Controls.Add(this.textBoxCameraMode);
            this.cameraInfoTab.Location = new System.Drawing.Point(8, 39);
            this.cameraInfoTab.Name = "cameraInfoTab";
            this.cameraInfoTab.Size = new System.Drawing.Size(1034, 392);
            this.cameraInfoTab.TabIndex = 3;
            this.cameraInfoTab.Text = "Camera Info";
            this.cameraInfoTab.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 350);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(877, 25);
            this.label3.TabIndex = 13;
            this.label3.Text = "* The information above is only populated when the application is connected to th" +
    "e camera";
            // 
            // modeWarning
            // 
            this.modeWarning.Image = ((System.Drawing.Image)(resources.GetObject("modeWarning.Image")));
            this.modeWarning.Location = new System.Drawing.Point(484, 108);
            this.modeWarning.Name = "modeWarning";
            this.modeWarning.Size = new System.Drawing.Size(40, 40);
            this.modeWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.modeWarning.TabIndex = 12;
            this.modeWarning.TabStop = false;
            this.modeWarning.Visible = false;
            // 
            // extrasTab
            // 
            this.extrasTab.Controls.Add(this.chkTrace);
            this.extrasTab.Controls.Add(this.buttonSelectFolder);
            this.extrasTab.Controls.Add(this.textBoxSaveLocation);
            this.extrasTab.Controls.Add(this.checkBoxEnableSaveLocation);
            this.extrasTab.Location = new System.Drawing.Point(8, 39);
            this.extrasTab.Name = "extrasTab";
            this.extrasTab.Size = new System.Drawing.Size(1034, 392);
            this.extrasTab.TabIndex = 2;
            this.extrasTab.Text = "Extras";
            this.extrasTab.UseVisualStyleBackColor = true;
            // 
            // buttonSelectFolder
            // 
            this.buttonSelectFolder.Location = new System.Drawing.Point(860, 19);
            this.buttonSelectFolder.Name = "buttonSelectFolder";
            this.buttonSelectFolder.Size = new System.Drawing.Size(52, 46);
            this.buttonSelectFolder.TabIndex = 4;
            this.buttonSelectFolder.Text = "...";
            this.buttonSelectFolder.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSelectFolder.UseVisualStyleBackColor = true;
            this.buttonSelectFolder.Click += new System.EventHandler(this.selectFolder_Click);
            // 
            // textBoxSaveLocation
            // 
            this.textBoxSaveLocation.Location = new System.Drawing.Point(410, 27);
            this.textBoxSaveLocation.Name = "textBoxSaveLocation";
            this.textBoxSaveLocation.Size = new System.Drawing.Size(444, 31);
            this.textBoxSaveLocation.TabIndex = 14;
            // 
            // checkBoxEnableSaveLocation
            // 
            this.checkBoxEnableSaveLocation.AutoSize = true;
            this.checkBoxEnableSaveLocation.Location = new System.Drawing.Point(16, 27);
            this.checkBoxEnableSaveLocation.Name = "checkBoxEnableSaveLocation";
            this.checkBoxEnableSaveLocation.Size = new System.Drawing.Size(234, 29);
            this.checkBoxEnableSaveLocation.TabIndex = 10;
            this.checkBoxEnableSaveLocation.Text = "Save Raw Captures";
            this.checkBoxEnableSaveLocation.UseVisualStyleBackColor = true;
            this.checkBoxEnableSaveLocation.CheckedChanged += new System.EventHandler(this.checkBoxEnableSaveLocation_CheckedChanged);
            // 
            // textBoxBulbMode
            // 
            this.textBoxBulbMode.Location = new System.Drawing.Point(516, 71);
            this.textBoxBulbMode.MaxLength = 2;
            this.textBoxBulbMode.Name = "textBoxBulbMode";
            this.textBoxBulbMode.Size = new System.Drawing.Size(44, 31);
            this.textBoxBulbMode.TabIndex = 3;
            this.textBoxBulbMode.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxBulbMode_Validating);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 560);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sony Mirrorless Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.selectCameraTab.ResumeLayout(false);
            this.selectCameraTab.PerformLayout();
            this.cameraPersonalityTab.ResumeLayout(false);
            this.cameraPersonalityTab.PerformLayout();
            this.driverSettingsTab.ResumeLayout(false);
            this.driverSettingsTab.PerformLayout();
            this.cameraInfoTab.ResumeLayout(false);
            this.cameraInfoTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modeWarning)).EndInit();
            this.extrasTab.ResumeLayout(false);
            this.extrasTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.FolderBrowserDialog selectFolderDialog;
        private System.Windows.Forms.CheckBox checkBoxUseLiveview;
        private System.Windows.Forms.TextBox textBoxCameraExposureTime;
        private System.Windows.Forms.TextBox textBoxCameraCompressionMode;
        private System.Windows.Forms.TextBox textBoxCameraMode;
        private System.Windows.Forms.TextBox textBoxCameraConnected;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBoxOutputFormat;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxPersonality;
        private System.Windows.Forms.Label labelCameraISO;
        private System.Windows.Forms.TextBox textBoxCameraISO;
        private System.Windows.Forms.TextBox textBoxCameraBatteryLevel;
        private System.Windows.Forms.Label labelCameraBatteryLevel;
        private System.Windows.Forms.CheckBox checkBoxAutoLiveview;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage selectCameraTab;
        private System.Windows.Forms.TabPage cameraPersonalityTab;
        private System.Windows.Forms.TabPage cameraInfoTab;
        private System.Windows.Forms.TabPage extrasTab;
        private System.Windows.Forms.Button buttonSelectFolder;
        private System.Windows.Forms.TextBox textBoxSaveLocation;
        private System.Windows.Forms.CheckBox checkBoxEnableSaveLocation;
        private System.Windows.Forms.PictureBox modeWarning;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage driverSettingsTab;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBoxBulbMode;
        private System.Windows.Forms.TextBox textBoxBulbMode;
    }
}