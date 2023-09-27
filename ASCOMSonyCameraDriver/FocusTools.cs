using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.SonyMirrorless
{
    public partial class FocusTools : Form
    {
        public FocusTools()
        {
            InitializeComponent();

            InitUI();
        }

        private void InitUI()
        {
            bool connectedSomehow = DriverCommon.CameraConnected || DriverCommon.FocuserConnected;
            bool lensSelected = DriverCommon.Settings.LensId != "";

            labelFocusPosition.Enabled = connectedSomehow && lensSelected;
            editFocusPosition.Enabled = connectedSomehow && lensSelected;
            buttonSet.Enabled = connectedSomehow && lensSelected;

            buttonLearn.Visible = false;
//            buttonLearn.Enabled = connectedSomehow;

            // Get Focus Info from Camera Driver
            if (connectedSomehow)
            {
                UpdateFocusPositionInfo();
            }
        }

        private void UpdateFocusPositionInfo()
        {
            int focusLimit = DriverCommon.Camera.GetFocusLimit();
            int focusPos = DriverCommon.Camera.GetFocus();

            editFocusPosition.Minimum = 0;
            editFocusPosition.Maximum = focusLimit;

            if (focusPos >= 0 && focusPos <= focusLimit)
            {
                focusPosition.Position = (double)focusPos / (double)focusLimit;
                editFocusPosition.Value = focusPos;
            }
            else
            {
                focusPosition.Position = 0;
            }
        }

        private void linkLabelFlatiron_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void VisitLink()
        {
            // Change the color of the link text by setting LinkVisited
            // to true.
            linkLabelFlatiron.LinkVisited = true;
            //Call the Process.Start method to open the default browser
            //with a URL:
            System.Diagnostics.Process.Start("https://www.flaticon.com/authors/icongeek26");
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            DriverCommon.Camera.SetFocus((int)editFocusPosition.Value);
            UpdateFocusPositionInfo();
        }
    }
}
