namespace ASCOM.SonyMirrorless
{
    partial class FocusTools
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
            this.linkLabelFlatiron = new System.Windows.Forms.LinkLabel();
            this.pictureBoxMAcro = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBoxEstimatedFocus = new System.Windows.Forms.GroupBox();
            this.buttonSet = new System.Windows.Forms.Button();
            this.labelFocusPosition = new System.Windows.Forms.Label();
            this.buttonLearn = new System.Windows.Forms.Button();
            this.editFocusPosition = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.focusPosition = new ASCOM.SonyMirrorless.FocusPosition();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMAcro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBoxEstimatedFocus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editFocusPosition)).BeginInit();
            this.SuspendLayout();
            // 
            // linkLabelFlatiron
            // 
            this.linkLabelFlatiron.AutoSize = true;
            this.linkLabelFlatiron.Font = new System.Drawing.Font("Microsoft Sans Serif", 4.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelFlatiron.LinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabelFlatiron.Location = new System.Drawing.Point(601, 115);
            this.linkLabelFlatiron.Name = "linkLabelFlatiron";
            this.linkLabelFlatiron.Size = new System.Drawing.Size(169, 16);
            this.linkLabelFlatiron.TabIndex = 2;
            this.linkLabelFlatiron.TabStop = true;
            this.linkLabelFlatiron.Text = "Icons made by Icongeek26";
            this.linkLabelFlatiron.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFlatiron_LinkClicked);
            // 
            // pictureBoxMAcro
            // 
            this.pictureBoxMAcro.Image = global::ASCOM.SonyMirrorless.Properties.Resources.Macro;
            this.pictureBoxMAcro.Location = new System.Drawing.Point(43, 60);
            this.pictureBoxMAcro.Name = "pictureBoxMAcro";
            this.pictureBoxMAcro.Size = new System.Drawing.Size(36, 36);
            this.pictureBoxMAcro.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMAcro.TabIndex = 3;
            this.pictureBoxMAcro.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ASCOM.SonyMirrorless.Properties.Resources.Landscape;
            this.pictureBox2.Location = new System.Drawing.Point(690, 60);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(36, 36);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // groupBoxEstimatedFocus
            // 
            this.groupBoxEstimatedFocus.Controls.Add(this.focusPosition);
            this.groupBoxEstimatedFocus.Controls.Add(this.pictureBox2);
            this.groupBoxEstimatedFocus.Controls.Add(this.pictureBoxMAcro);
            this.groupBoxEstimatedFocus.Controls.Add(this.linkLabelFlatiron);
            this.groupBoxEstimatedFocus.Location = new System.Drawing.Point(12, 12);
            this.groupBoxEstimatedFocus.Name = "groupBoxEstimatedFocus";
            this.groupBoxEstimatedFocus.Size = new System.Drawing.Size(776, 145);
            this.groupBoxEstimatedFocus.TabIndex = 5;
            this.groupBoxEstimatedFocus.TabStop = false;
            this.groupBoxEstimatedFocus.Text = "Simulated Camera UI";
            // 
            // buttonSet
            // 
            this.buttonSet.Location = new System.Drawing.Point(327, 190);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(87, 48);
            this.buttonSet.TabIndex = 9;
            this.buttonSet.Text = "Set";
            this.toolTip1.SetToolTip(this.buttonSet, "Set focus position. (requires camera or lens to be in connected state)");
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
            // 
            // labelFocusPosition
            // 
            this.labelFocusPosition.AutoSize = true;
            this.labelFocusPosition.Location = new System.Drawing.Point(12, 202);
            this.labelFocusPosition.Name = "labelFocusPosition";
            this.labelFocusPosition.Size = new System.Drawing.Size(173, 25);
            this.labelFocusPosition.TabIndex = 8;
            this.labelFocusPosition.Text = "Focuser Position";
            // 
            // buttonLearn
            // 
            this.buttonLearn.Location = new System.Drawing.Point(672, 190);
            this.buttonLearn.Name = "buttonLearn";
            this.buttonLearn.Size = new System.Drawing.Size(116, 48);
            this.buttonLearn.TabIndex = 6;
            this.buttonLearn.Text = "Learn";
            this.toolTip1.SetToolTip(this.buttonLearn, "Learn (or Relearn) lens attached to camera. (requires camera or lens to be in con" +
        "nected state)");
            this.buttonLearn.UseVisualStyleBackColor = true;
            // 
            // editFocusPosition
            // 
            this.editFocusPosition.Location = new System.Drawing.Point(191, 200);
            this.editFocusPosition.Name = "editFocusPosition";
            this.editFocusPosition.Size = new System.Drawing.Size(120, 31);
            this.editFocusPosition.TabIndex = 11;
            // 
            // focusPosition
            // 
            this.focusPosition.BackColor = System.Drawing.Color.Black;
            this.focusPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.focusPosition.ForeColor = System.Drawing.Color.White;
            this.focusPosition.Location = new System.Drawing.Point(95, 60);
            this.focusPosition.Name = "focusPosition";
            this.focusPosition.Position = 0.5D;
            this.focusPosition.Size = new System.Drawing.Size(580, 36);
            this.focusPosition.TabIndex = 1;
            // 
            // FocusTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 254);
            this.Controls.Add(this.editFocusPosition);
            this.Controls.Add(this.buttonLearn);
            this.Controls.Add(this.groupBoxEstimatedFocus);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.labelFocusPosition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FocusTools";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FocusTools";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMAcro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBoxEstimatedFocus.ResumeLayout(false);
            this.groupBoxEstimatedFocus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editFocusPosition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FocusPosition focusPosition;
        private System.Windows.Forms.LinkLabel linkLabelFlatiron;
        private System.Windows.Forms.PictureBox pictureBoxMAcro;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox groupBoxEstimatedFocus;
        private System.Windows.Forms.Button buttonLearn;
        private System.Windows.Forms.Label labelFocusPosition;
        private System.Windows.Forms.Button buttonSet;
        private System.Windows.Forms.NumericUpDown editFocusPosition;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}