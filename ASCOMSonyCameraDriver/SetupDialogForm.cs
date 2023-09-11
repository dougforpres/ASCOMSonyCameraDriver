using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
//        internal Camera ascomCamera;

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
            DriverCommon.Settings.DeviceId = (string)comboBoxCamera.SelectedItem;
            DriverCommon.Settings.EnableLogging = chkTrace.Checked;
            DriverCommon.Settings.DefaultReadoutMode = (short)comboBoxOutputFormat.SelectedValue;
            DriverCommon.Settings.ARWAutosave = checkBoxEnableSaveLocation.Checked;
            DriverCommon.Settings.ARWAutosaveFolder = textBoxSaveLocation.Text;
            DriverCommon.Settings.ARWAutosaveWithDate = checkBoxAppendDate.Checked;
            DriverCommon.Settings.ARWAutosaveAlwaysCreateEmptyFolder = checkBoxCreateMultipleDirectories.Checked;
            DriverCommon.Settings.UseLiveview = checkBoxUseLiveview.Checked;
            DriverCommon.Settings.AutoLiveview = checkBoxAutoLiveview.Checked;
            DriverCommon.Settings.Personality = (int)comboBoxPersonality.SelectedValue;
            DriverCommon.Settings.BulbModeEnable = checkBoxBulbMode.Checked;
            DriverCommon.Settings.BulbModeTime = short.Parse(textBoxBulbMode.Text.Trim());
            DriverCommon.Settings.AllowISOAdjust = checkBoxAllowISOAdjust.Checked;
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
            chkTrace.Checked = DriverCommon.Settings.EnableLogging;
            SonyCameraEnumerator enumerator = new SonyCameraEnumerator();
            String selected = "";

            comboBoxCamera.Items.Clear();

            foreach (SonyCamera candidate in enumerator.Cameras)
            {
                comboBoxCamera.Items.Add(candidate.DisplayName);

                if (candidate.DisplayName == DriverCommon.Settings.DeviceId)
                {
                    selected = candidate.DisplayName;
                }
            }

            if (selected.Length > 0)
            {
                comboBoxCamera.SelectedItem = selected;
            }

            checkBoxEnableSaveLocation.Checked = DriverCommon.Settings.ARWAutosave;
            textBoxSaveLocation.Enabled = DriverCommon.Settings.ARWAutosave;
            textBoxSaveLocation.Text = DriverCommon.Settings.ARWAutosaveFolder;
            checkBoxAppendDate.Enabled = textBoxSaveLocation.Enabled;
            checkBoxAppendDate.Checked = DriverCommon.Settings.ARWAutosaveWithDate;
            checkBoxCreateMultipleDirectories.Enabled = textBoxSaveLocation.Enabled;
            checkBoxCreateMultipleDirectories.Checked = DriverCommon.Settings.ARWAutosaveAlwaysCreateEmptyFolder;

            buttonSelectFolder.Enabled = DriverCommon.Settings.ARWAutosave;
            checkBoxUseLiveview.Checked = DriverCommon.Settings.UseLiveview;
            checkBoxAutoLiveview.Checked = DriverCommon.Settings.AutoLiveview;

            Dictionary<int, string> personalities = new Dictionary<int, string>();

            personalities.Add(SonyCommon.PERSONALITY_APT, "APT");
            personalities.Add(SonyCommon.PERSONALITY_NINA, "N.I.N.A");
            personalities.Add(SonyCommon.PERSONALITY_SHARPCAP, "SharpCap");

            comboBoxPersonality.DataSource = new BindingSource(personalities, null);
            comboBoxPersonality.DisplayMember = "Value";
            comboBoxPersonality.ValueMember = "Key";

            comboBoxPersonality.SelectedValue = DriverCommon.Settings.Personality;

            checkBoxBulbMode.Checked = DriverCommon.Settings.BulbModeEnable;
            textBoxBulbMode.Text = DriverCommon.Settings.BulbModeTime.ToString();
            textBoxBulbMode.Enabled = checkBoxBulbMode.Checked;

            checkBoxAllowISOAdjust.Checked = DriverCommon.Settings.AllowISOAdjust;

            PopulateOutputFormats();

            comboBoxOutputFormat.SelectedValue = DriverCommon.Settings.DefaultReadoutMode;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);

            textBoxVersion.Text = fileVersion.FileVersion;

            InInit = false;
//            timer1.Tick += showCameraStatus;
        }

/*        private void showCameraStatus(Object o, EventArgs eventArgs)
        {
            SonyCamera camera = Camera.camera;

            if (camera != null)
            {
                textBoxCameraConnected.Text = camera.Connected ? "Connected" : "Disconnected";

                if (camera.Connected)
                {
                    using (new Camera.SerializedAccess(ascomCamera, "setupDialog"))
                    {
                        Camera.LogMessage("setup", "Refresh Properties");
                        camera.RefreshProperties();
                        Camera.LogMessage("setup", "500e");
                        textBoxCameraMode.Text = camera.GetPropertyValue(0x500e).Text;
                        textBoxCameraCompressionMode.Text = camera.GetPropertyValue(0x5004).Text;
                        textBoxCameraExposureTime.Text = camera.GetPropertyValue(0xd20d).Text;
                        textBoxCameraISO.Text = camera.GetPropertyValue(0xd21e).Text;
                        textBoxCameraBatteryLevel.Text = camera.GetPropertyValue(0xd218).Text;
                        modeWarning.Visible = textBoxCameraMode.Text != "M";
                        Camera.LogMessage("setup", "All Props updated");
                    }
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
        }*/

        private void checkBoxEnableSaveLocation_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSaveLocation.Enabled = ((CheckBox)sender).Checked;
            buttonSelectFolder.Enabled = ((CheckBox)sender).Checked;
            checkBoxAppendDate.Enabled = ((CheckBox)sender).Checked;
            checkBoxCreateMultipleDirectories.Enabled = ((CheckBox)sender).Checked;
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
            if (!InInit)
            {
                textBoxBulbMode.Enabled = checkBoxBulbMode.Checked;
                MessageBox.Show("Note that this option will only take effect if your camera's list of supported Exposure times is known.  See the wiki page at the bottom of the settings page for more info.");
            }
        }

        private void checkBoxAllowISOAdjust_CheckedChanged(object sender, EventArgs e)
        {
            if (!InInit)
            {
                MessageBox.Show("Note that this option will only take effect if your camera's list of supported ISO values is known.  See the wiki page at the bottom of the settings page for more info.");
            }
        }

        private void linkWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Change the color of the link text by setting LinkVisited
                // to true.
                linkExposureAndISO.LinkVisited = true;

                //Call the Process.Start method to open the default browser
                //with a URL:
                System.Diagnostics.Process.Start("https://github.com/dougforpres/ASCOMSonyCameraDriver/wiki/Controlling-the-Exposure-Time-and-ISO-(Gain)");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Change the color of the link text by setting LinkVisited
                // to true.
                linkWiki.LinkVisited = true;

                //Call the Process.Start method to open the default browser
                //with a URL:
                System.Diagnostics.Process.Start("https://github.com/dougforpres/ASCOMSonyCameraDriver/wiki/");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        /*        private void tabControl1_Deselecting(object sender, TabControlCancelEventArgs e)
                {
                    if (tabControl1.SelectedTab == cameraInfoTab) // Index == 3)
                    {
                        // Stop the timer
                        timer1.Stop();
                    }
                }

                private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
                {
                    if (tabControl1.SelectedTab == cameraInfoTab) // Index == 3)
                    {
                        // Stop the timer
                        timer1.Start();
                    }
                }*/
    }
}