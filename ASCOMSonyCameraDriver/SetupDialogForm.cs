using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.SonyMirrorless;
using System.Collections;

namespace ASCOM.SonyMirrorless
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        internal bool InInit = false;

        public SetupDialogForm()
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            Camera.deviceId = (string)comboBoxCamera.SelectedItem;
            Camera.tl.Enabled = chkTrace.Checked;
            Camera.defaultReadoutMode = (short)comboBoxOutputFormat.SelectedValue;
            Camera.SaveRawImageData = checkBoxEnableSaveLocation.Checked;
            Camera.SaveRawImageFolder = textBoxSaveLocation.Text;
            Camera.UseLiveview = checkBoxUseLiveview.Checked;
            Camera.AutoLiveview = checkBoxAutoLiveview.Checked;
            Camera.Personality = (int)comboBoxPersonality.SelectedValue;
            Camera.BulbModeEnable = checkBoxBulbMode.Checked;
            Camera.BulbModeTime = short.Parse(textBoxBulbMode.Text.Trim());
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            InInit = true;
            chkTrace.Checked = Camera.tl.Enabled;
            SonyCameraEnumerator enumerator = new SonyCameraEnumerator();
            String selected = "";

            comboBoxCamera.Items.Clear();

            foreach (SonyCamera candidate in enumerator.Cameras)
            {
                comboBoxCamera.Items.Add(candidate.DisplayName);

                if (candidate.DisplayName == Camera.deviceId)
                {
                    selected = candidate.DisplayName;
                }
            }

            if (selected.Length > 0)
            {
                comboBoxCamera.SelectedItem = selected;
            }

            checkBoxEnableSaveLocation.Checked = Camera.SaveRawImageData;
            textBoxSaveLocation.Enabled = Camera.SaveRawImageData;
            textBoxSaveLocation.Text = Camera.SaveRawImageFolder;
            buttonSelectFolder.Enabled = Camera.SaveRawImageData;
            checkBoxUseLiveview.Checked = Camera.UseLiveview;
            checkBoxAutoLiveview.Checked = Camera.AutoLiveview;

            Dictionary<int, string> personalities = new Dictionary<int, string>();

            personalities.Add(SonyCommon.PERSONALITY_APT, "APT");
            personalities.Add(SonyCommon.PERSONALITY_NINA, "N.I.N.A");
            personalities.Add(SonyCommon.PERSONALITY_SHARPCAP, "SharpCap");

            comboBoxPersonality.DataSource = new BindingSource(personalities, null);
            comboBoxPersonality.DisplayMember = "Value";
            comboBoxPersonality.ValueMember = "Key";

            comboBoxPersonality.SelectedValue = Camera.Personality;

            checkBoxBulbMode.Checked = Camera.BulbModeEnable;
            textBoxBulbMode.Text = Camera.BulbModeTime.ToString();
            textBoxBulbMode.Enabled = checkBoxBulbMode.Checked;

            PopulateOutputFormats();

            comboBoxOutputFormat.SelectedValue = Camera.defaultReadoutMode;

            InInit = false;
            timer1.Tick += showCameraStatus;
            timer1.Start();
        }

        private void showCameraStatus(Object o, EventArgs eventArgs)
        {
            SonyCamera camera = Camera.camera;

            if (camera != null)
            {
                textBoxCameraConnected.Text = camera.Connected ? "Connected" : "Disconnected";

                if (camera.Connected)
                {
                    camera.RefreshProperties();
                    textBoxCameraMode.Text = camera.GetPropertyValue(0x500e).Text;
                    textBoxCameraCompressionMode.Text = camera.GetPropertyValue(0x5004).Text;
                    textBoxCameraExposureTime.Text = camera.GetPropertyValue(0xd20d).Text;
                    textBoxCameraISO.Text = camera.GetPropertyValue(0xd21e).Text;
                    textBoxCameraBatteryLevel.Text = camera.GetPropertyValue(0xd218).Text;
                    modeWarning.Visible = textBoxCameraMode.Text != "M";
                }
                else
                {
                    textBoxCameraMode.Text = "-";
                    textBoxCameraCompressionMode.Text = "-";
                    textBoxCameraExposureTime.Text = "-";
                    textBoxCameraISO.Text = "-";
                    textBoxCameraBatteryLevel.Text = "-";
                    modeWarning.Visible = false;
                }
            }
            else
            {
                textBoxCameraConnected.Text = "Not Initialized";
            }
        }

        private void checkBoxEnableSaveLocation_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSaveLocation.Enabled = ((CheckBox)sender).Checked;
            buttonSelectFolder.Enabled = ((CheckBox)sender).Checked;
        }

        private void selectFolder_Click(object sender, EventArgs e)
        {
            selectFolderDialog.SelectedPath = textBoxSaveLocation.Text;

            if (selectFolderDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxSaveLocation.Text = selectFolderDialog.SelectedPath;
            }
        }

        private void comboBoxPersonality_SelectedIndexChanged(object sender, EventArgs e)
        {
            int personality = (int)comboBoxPersonality.SelectedValue;

            short currentOutputFormat = comboBoxOutputFormat.SelectedValue != null ? (short)comboBoxOutputFormat.SelectedValue : SonyCommon.OUTPUTFORMAT_RGGB;

            switch (personality)
            {
                case SonyCommon.PERSONALITY_APT:
                    // APT allows image output to be selected (though BGR will give interesting results)
                    // APT also supports RGB output so liveview can be enabled
                    comboBoxOutputFormat.Enabled = true;
                    checkBoxUseLiveview.Enabled = true;

                    PopulateOutputFormats();

                    comboBoxOutputFormat.SelectedValue = currentOutputFormat > 0 ? currentOutputFormat : SonyCommon.OUTPUTFORMAT_RGGB;
                    break;

                case SonyCommon.PERSONALITY_NINA:
                    // NINA only supports RGGB, so we need to preset format and disable liveview
                    PopulateOutputFormats();
                    comboBoxOutputFormat.SelectedValue = SonyCommon.OUTPUTFORMAT_RGGB;
                    comboBoxOutputFormat.Enabled = false;
                    checkBoxUseLiveview.Enabled = false;
                    checkBoxUseLiveview.Checked = false;
                    break;

                case SonyCommon.PERSONALITY_SHARPCAP:
                    // Sharpcap supports format specification, but wants BGR, not RGB
                    // Doesn't support Liveview selection
                    comboBoxOutputFormat.Enabled = true;

                    PopulateOutputFormats();

                    if (currentOutputFormat == SonyCommon.OUTPUTFORMAT_RGB)
                    {
                        currentOutputFormat = SonyCommon.OUTPUTFORMAT_BGR;
                    }

                    comboBoxOutputFormat.SelectedValue = currentOutputFormat;
                    checkBoxUseLiveview.Enabled = false;
                    checkBoxUseLiveview.Checked = false;
                    break;
            }
        }

        private void PopulateOutputFormats()
        {
            Dictionary<short, string> outputFormats = new Dictionary<short, string>();

            outputFormats.Add(SonyCommon.OUTPUTFORMAT_RGGB, "RAW/RGGB (Unprocessed)");

            switch ((int)comboBoxPersonality.SelectedValue)
            {
                case SonyCommon.PERSONALITY_APT:
                    outputFormats.Add(SonyCommon.OUTPUTFORMAT_RGB, "RGB (Processed)");
                    break;

                case SonyCommon.PERSONALITY_NINA:
                    break;

                case SonyCommon.PERSONALITY_SHARPCAP:
                    outputFormats.Add(SonyCommon.OUTPUTFORMAT_BGR, "BGR (Processed)");
                    break;
            }

            comboBoxOutputFormat.DataSource = new BindingSource(outputFormats, null);
            comboBoxOutputFormat.DisplayMember = "Value";
            comboBoxOutputFormat.ValueMember = "Key";
        }

        private void checkBoxAutoLiveview_CheckedChanged(object sender, EventArgs e)
        {
            if (!InInit && checkBoxAutoLiveview.Checked)
            {
                MessageBox.Show("Please note that this feature is experimental.\n\nThis will automatically take a LiveView image instead of a normal exposure if:\n  - The camera supports it\n  - The exposure time is set to less than\n    or equal to 0.00001s (in APT this is\n    represented as 0.000)");
            }
        }

        private void textBoxBulbMode_Validating(object sender, CancelEventArgs e)
        {
            // Lowest possible value is 1, highest is 30
            int value = -1;

            try
            {
                value = short.Parse(textBoxBulbMode.Text.Trim());
            }
            catch
            {
                // Value already invalid
            }

            if (value < 1 || value > 30)
            {
                e.Cancel = true;
                MessageBox.Show("Value for Bulb Mode must be a number from 1 to 30");
            }
        }

        private void checkBoxBulbMode_CheckedChanged(object sender, EventArgs e)
        {
            textBoxBulbMode.Enabled = checkBoxBulbMode.Checked;
        }
    }
}