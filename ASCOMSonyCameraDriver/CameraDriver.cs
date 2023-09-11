//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Camera driver for Sony Mirrorless Camera
//
// Description:	Implements ASCOM driver for Sony Mirrorless camera.
//				Communicates using USB connection.
//
// Implements:	ASCOM Camera interface version: 2
// Author:		(2019) Doug Henderson <retrodotkiwi@gmail.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 10-Dec-2019	XXX	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//
#define Camera

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Threading;
using Microsoft.Win32;
using System.IO;

namespace ASCOM.SonyMirrorless
{
    //
    // Your driver's DeviceID is ASCOM.SonyMirrorless.Camera
    //
    // The Guid attribute sets the CLSID for ASCOM.SonyMirrorless.Camera

    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Camera Driver for Sony Mirrorless Camera.
    /// </summary>
    [Guid("33a95fe9-6775-4291-a16c-d6a2f5e4812c")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Camera : ICameraV2
    {
        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>

        internal static bool LastSetFastReadout = false;
        internal static int RequestedStartX = 0;
        internal static int RequestedStartY = 0;
        internal static int RequestedWidth = -1;
        internal static int RequestedHeight = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="a6400"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Camera()
        {
            DriverCommon.ReadProfile(); // Read device configuration from the ASCOM Profile store

            DriverCommon.LogCameraMessage("Camera", "Starting initialisation");

            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object

            DriverCommon.LogCameraMessage("Camera", "Completed initialisation");
        }

        //
        // PUBLIC COM INTERFACE ICameraV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            //            if (IsConnected)
            //                System.Windows.Forms.MessageBox.Show("Camera is currently connected.  Some options are only available when not connected, these will be disabled.");
            DriverCommon.LogCameraMessage("SetupDialog", "[in]");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    DriverCommon.WriteProfile(); // Persist device configuration values to the ASCOM Profile store

                    // Update connected camera with bulb mode info
                    if (DriverCommon.Camera != null)
                    {
                        DriverCommon.Camera.BulbMode = DriverCommon.Settings.BulbModeEnable;
                        DriverCommon.Camera.BulbModeTime = DriverCommon.Settings.BulbModeTime;
                    }
                }
            }

            DriverCommon.LogCameraMessage("SetupDialog", "[out]");
        }

        public ArrayList SupportedActions
        {
            get
            {
                DriverCommon.LogCameraMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            DriverCommon.LogCameraMessage("", $"Action {actionName}, parameters {actionParameters} not implemented");
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // Call CommandString and return as soon as it finishes
//            this.CommandString(command, raw);
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
            // DO NOT have both these sections!  One or the other
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
//            string ret = CommandString(command, raw);
            // TODO decode the return string and return true or false
            // or
            throw new ASCOM.MethodNotImplementedException("CommandBool");
            // DO NOT have both these sections!  One or the other
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            DriverCommon.LogCameraMessage("Dispose", "Disposing");
 
            Connected = false;

            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public bool Connected
        {
            get
            {
                using (new SerializedAccess(this, "get_Connected"))
                {
                    DriverCommon.LogCameraMessage("Connected", "Get {0}", DriverCommon.CameraConnected.ToString());

                    return DriverCommon.CameraConnected;
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_Connected"))
                {
                    DriverCommon.LogCameraMessage("Connected", "Set {0}", value.ToString());

                    if (value && !DriverCommon.CameraConnected && DriverCommon.Settings.DeviceId == "")
                    {
                        // Need to display setup dialog
                        SetupDialog();
                    }

                    if (DriverCommon.Settings.DeviceId != "")
                    {
                        DriverCommon.CameraConnected = value;
                    }
                    else
                    {
                        DriverCommon.LogCameraMessage("Connected Set", "Camera not yet specified");
                    }
                }
            }
        }

        public string Description
        {
            get
            {
                return DriverCommon.CameraDriverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                return DriverCommon.CameraDriverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                return DriverCommon.DriverVersion;
            }
        }

        public short InterfaceVersion
        {
            get
            {
                return Convert.ToInt16("2");
            }
        }

        public string Name
        {
            get
            {
                return DriverCommon.CameraDriverName;
            }
        }

        #endregion

        #region ICamera Implementation

        public void AbortExposure()
        {
            using (new SerializedAccess(this, "AbortExposure()"))
            {
                DriverCommon.LogCameraMessage("AbortExposure", "Attempting to cancel any in-progress capture");

                DriverCommon.Camera.StopCapture();
            }
        }

        public short BayerOffsetX
        {
            get
            {
                using (new SerializedAccess(this, "get_BayerOffsetX"))
                {
                    DriverCommon.LogCameraMessage("BayerOffsetX Get Get", "");

                    return (short)DriverCommon.Camera.Info.BayerXOffset;
                }
            }
        }

        public short BayerOffsetY
        {
            get
            {
                using (new SerializedAccess(this, "get_BayerOffsetY"))
                {
                    DriverCommon.LogCameraMessage("BayerOffsetY Get Get", "");

                    return (short)DriverCommon.Camera.Info.BayerYOffset;
                }
            }
        }

        public short BinX
        {
            get
            {
                using (new SerializedAccess(this, "get_BinX"))
                {
                    DriverCommon.LogCameraMessage("BinX Get", "1");

                    return 1;
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_BinX"))
                {
                    DriverCommon.LogCameraMessage("BinX Set", value.ToString());

                    if (value != 1) throw new ASCOM.InvalidValueException("BinX", value.ToString(), "1"); // Only 1 is valid in this simple template
                }
            }
        }

        public short BinY
        {
            get
            {
                using (new SerializedAccess(this, "get_BinY"))
                {
                    DriverCommon.LogCameraMessage("BinY Get", "1");

                    return 1;
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_BinY"))
                {
                    DriverCommon.LogCameraMessage("BinY Set", value.ToString());

                    if (value != 1) throw new ASCOM.InvalidValueException("BinY", value.ToString(), "1"); // Only 1 is valid in this simple template
                }
            }
        }

        public double CCDTemperature
        {
            get
            {
                using (new SerializedAccess(this, "get_CCDTemperature"))
                {
//                    DriverCommon.LogCameraMessage("CCDTemperature Get Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("CCDTemperature", false);
                }
            }
        }

        public CameraStates CameraState
        {
            get
            {
                using (new SerializedAccess(this, "get_CameraState", true))
                {
                    DriverCommon.LogCameraMessage("CameraState Get", DriverCommon.Camera.State.ToString());

                    return DriverCommon.Camera.State;
                }
            }
        }

        public int CameraXSize
        {
            get
            {
                using (new SerializedAccess(this, "get_CameraXSize", true))
                {
                    int x = (int)(DriverCommon.Camera.Info.CropMode == 0 ? DriverCommon.Camera.Info.ImageWidthPixels : DriverCommon.Camera.Info.ImageWidthCroppedPixels);
                    DriverCommon.LogCameraMessage("CameraXSize Get", x.ToString());

                    return x;
                }
            }
        }

        public int CameraYSize
        {
            get
            {
                using (new SerializedAccess(this, "get_CameraYSize", true))
                {
                    int y = (int)(DriverCommon.Camera.Info.CropMode == 0 ? DriverCommon.Camera.Info.ImageHeightPixels : DriverCommon.Camera.Info.ImageHeightCroppedPixels);
                    DriverCommon.LogCameraMessage("CameraYSize Get", y.ToString());

                    return y;
                }
            }
        }

        public bool CanAbortExposure
        {
            get
            {
                using (new SerializedAccess(this, "get_CanAbortExposure"))
                {
                    DriverCommon.LogCameraMessage("CanAbortExposure Get", true.ToString());

                    return true;
                }
            }
        }

        public bool CanAsymmetricBin
        {
            get
            {
                using (new SerializedAccess(this, "get_CanAsymmetricBin"))
                {
                    DriverCommon.LogCameraMessage("CanAsymmetricBin Get", false.ToString());

                    return false;
                }
            }
        }

        public bool CanFastReadout
        {
            get
            {
                using (new SerializedAccess(this, "get_CanFastReadout"))
                {
                    bool result = DriverCommon.Settings.Personality != SonyCommon.PERSONALITY_NINA;

                    DriverCommon.LogCameraMessage("CanFastReadout Get", result.ToString());

                    return result;
                }
            }
        }

        public bool CanGetCoolerPower
        {
            get
            {
                using (new SerializedAccess(this, "get_CanGetCoolerPower"))
                {
                    DriverCommon.LogCameraMessage("CanGetCoolerPower Get", false.ToString());

                    return false;
                }
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                using (new SerializedAccess(this, "get_CanPulseGuide"))
                {
                    DriverCommon.LogCameraMessage("CanPulseGuide Get", false.ToString());

                    return false;
                }
            }
        }

        public bool CanSetCCDTemperature
        {
            get
            {
                using (new SerializedAccess(this, "get_CanSetCCDTemperature"))
                {
                    DriverCommon.LogCameraMessage("CanSetCCDTemperature Get", false.ToString());

                    return false;
                }
            }
        }

        public bool CanStopExposure
        {
            get
            {
                using (new SerializedAccess(this, "get_CanStopExposure"))
                {
                    DriverCommon.LogCameraMessage("CanStopExposure Get", true.ToString());

                    return true;
                }
            }
        }

        public bool CoolerOn
        {
            get
            {
                using (new SerializedAccess(this, "get_CoolerOn"))
                {
//                    DriverCommon.LogCameraMessage("CoolerOn Get Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("CoolerOn", false);
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_CoolerOn"))
                {
//                    DriverCommon.LogCameraMessage("CoolerOn Set Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("CoolerOn", true);
                }
            }
        }

        public double CoolerPower
        {
            get
            {
                using (new SerializedAccess(this, "get_CoolerPower"))
                {
//                    DriverCommon.LogCameraMessage("CoolerPower Get Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("CoolerPower", false);
                }
            }
        }

        public double ElectronsPerADU
        {
            get
            {
                using (new SerializedAccess(this, "get_ElectronsPerADU"))
                {
                    DriverCommon.LogCameraMessage("ElectronsPerADU Get Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("ElectronsPerADU", false);
                }
            }
        }

        public double ExposureMax
        {
            get
            {
                using (new SerializedAccess(this, "get_ExposureMax", true))
                {
                    DriverCommon.LogCameraMessage("ExposureMax Get Get", DriverCommon.Camera.Info.ExposureTimeMax.ToString());

                    return DriverCommon.Camera.Info.ExposureTimeMax;
                }
            }
        }

        public double ExposureMin
        {
            get
            {
                using (new SerializedAccess(this, "get_ExposureMin", true))
                {
                    DriverCommon.LogCameraMessage("ExposureMin Get", DriverCommon.Camera.Info.ExposureTimeMin.ToString());

                    return DriverCommon.Camera.Info.ExposureTimeMin;
                }
            }
        }

        public double ExposureResolution
        {
            get
            {
                using (new SerializedAccess(this, "get_ExposureResolution", true))
                {
                    DriverCommon.LogCameraMessage("ExposureResolution Get", DriverCommon.Camera.Info.ExposureTimeStep.ToString());

                    return DriverCommon.Camera.Info.ExposureTimeStep;
                }
            }
        }

        public bool FastReadout
        {
            get
            {
                using (new SerializedAccess(this, "get_FastReadout", true))
                {
                    DriverCommon.LogCameraMessage("FastReadout Get", DriverCommon.Camera.Mode.Preview.ToString());

                    return DriverCommon.Camera.PreviewMode;
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_FastReadout", true))
                {
                    value = value && DriverCommon.Settings.UseLiveview;

                    DriverCommon.LogCameraMessage("FastReadout Set", value.ToString());
                    DriverCommon.Camera.PreviewMode = value;
                    LastSetFastReadout = value;
                }
            }
        }

        public double FullWellCapacity
        {
            get
            {
                using (new SerializedAccess(this, "get_FullWellCapacity"))
                {
                    DriverCommon.LogCameraMessage("FullWellCapacity Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("FullWellCapacity", false);
                }
            }
        }

        public short Gain
        {
            get
            {
                using (new SerializedAccess(this, "get_Gain"))
                {
                    if (Connected)
                    {
                        if (DriverCommon.Settings.AllowISOAdjust && DriverCommon.Camera.Gains.Count > 0)
                        {
                            short gainIndex = DriverCommon.Camera.GainIndex;
                            DriverCommon.LogCameraMessage("Gain Get", gainIndex.ToString());

                            return gainIndex;
                        }
                        else
                        {
                            throw new ASCOM.PropertyNotImplementedException("Gains property is not enabled, see driver settings dialog");
                        }
                    }
                    else
                    {
                        throw new ASCOM.NotConnectedException("Camera must be connected to retrieve gain");
                    }
                }
            }

            set
            {
                using (new SerializedAccess(this, "set_Gain"))
                {
                    if (Connected)
                    {
                        if (DriverCommon.Settings.AllowISOAdjust && DriverCommon.Camera.Gains.Count > 0)
                        {
                            DriverCommon.Camera.GainIndex = value;
                            DriverCommon.LogCameraMessage("Gain Set", value.ToString());
                        }
                        else
                        {
                            throw new ASCOM.PropertyNotImplementedException("Gains property is not enabled, see driver settings dialog");
                        }
                    }
                    else
                    {
                        throw new ASCOM.NotConnectedException("Camera must be connected to retrieve gain");
                    }
                }
            }
        }

        public short GainMax
        {
            get
            {
                using (new SerializedAccess(this, "get_GainMax"))
                {
                    DriverCommon.LogCameraMessage("GainMax Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("GainMax", false);
                }
            }
        }

        public short GainMin
        {
            get
            {
                using (new SerializedAccess(this, "get_GainMin"))
                {
                    DriverCommon.LogCameraMessage("GainMin Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("GainMin", true);
                }
            }
        }

        public ArrayList Gains
        {
            get
            {
                using (new SerializedAccess(this, "get_Gains"))
                {
                    if (Connected)
                    {
                        ArrayList gains = DriverCommon.Camera.Gains;

                        if (DriverCommon.Settings.AllowISOAdjust && gains.Count > 0)
                        {

                            DriverCommon.LogCameraMessage("Gains Get", String.Format("Size = {0}", gains.Count));

                            return gains;
                        }
                        else
                        {
                            throw new ASCOM.PropertyNotImplementedException("Gains property is not enabled, see driver settings dialog");
                        }
                    }
                    else
                    {
                        throw new ASCOM.NotConnectedException("Camera must be connected to get list of available gains");
                    }
                }
            }
        }

        public bool HasShutter
        {
            get
            {
                using (new SerializedAccess(this, "get_HasShutter"))
                {
                    DriverCommon.LogCameraMessage("HasShutter Get", true.ToString());

                    return true;
                }
            }
        }

        public double HeatSinkTemperature
        {
            get
            {
                using (new SerializedAccess(this, "get_HeatSinkTemperature"))
                {
//                    DriverCommon.LogCameraMessage("HeatSinkTemperature Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("HeatSinkTemperature", false);
                }
            }
        }

        private object ImageData()
        {
            object result = null;

            if (DriverCommon.Camera.LastImage.Status != SonyImage.ImageStatus.Ready)
            {
                DriverCommon.LogCameraMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken!");
                throw new ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken!");
            }

            SonyImage image = DriverCommon.Camera.LastImage;

            int requestedWidth = NumX;
            int requestedHeight = NumY;
            DriverCommon.LogCameraMessage("ImageArray", "requestedWidth = {0}, cameraNumX = {1}, camera.Mode.ImageWidth = {2}", (int)Math.Min(DriverCommon.Settings.ImageWidth, DriverCommon.Camera.Mode.ImageWidthPixels), DriverCommon.Settings.ImageWidth, DriverCommon.Camera.Mode.ImageWidthPixels);
            DriverCommon.LogCameraMessage("ImageArray Get", String.Format("(numX = {0}, numY = {1}, image.Width = {2}, image.Height = {3})", requestedWidth, requestedHeight, image.Width, image.Height));

            switch (image.m_info.ImageMode)
            {
                case 1:
                    DriverCommon.LogCameraMessage("BAYER info", String.Format("Dimensions = {0}, {1} x {2}", SonyImage.BAYER.Rank, SonyImage.BAYER.GetLength(0), SonyImage.BAYER.GetLength(1)));

                    result = Resize(SonyImage.BAYER, SonyImage.BAYER.Rank, StartX, StartY, requestedWidth, requestedHeight);
                    break;

                case 2:
                    if (DriverCommon.Settings.Personality != SonyCommon.PERSONALITY_NINA)
                    {
                        DriverCommon.LogCameraMessage("RGB info", String.Format("Dimensions = {0}, {1} x {2} x {3}", SonyImage.RGB.Rank, SonyImage.RGB.GetLength(0), SonyImage.RGB.GetLength(1), SonyImage.RGB.GetLength(2)));

                        result = Resize(SonyImage.RGB, SonyImage.RGB.Rank, StartX, StartY, requestedWidth, requestedHeight);
                    }
                    else
                    {
                        DriverCommon.LogCameraMessage("RGB info as MONO", String.Format("Dimensions = {0}, {1} x {2}", SonyImage.BAYER.Rank, SonyImage.BAYER.GetLength(0), SonyImage.BAYER.GetLength(1)));

                        result = Resize(SonyImage.BAYER, SonyImage.BAYER.Rank, StartX, StartY, requestedWidth, requestedHeight);
                    }
                    break;

                default:
                    DriverCommon.LogCameraMessage("Unknown info", String.Format("{0} - Throwing", image.m_info.ImageMode));

                    throw new ASCOM.InvalidOperationException("Call to ImageArray resulted in invalid image type!");
            }

            return result;
        }

        public object ImageArray
        {
            get
            {
                using (new SerializedAccess(this, "get_ImageArray", true))
                {
                    return ImageData();

                    /*object result = null;

                    if (DriverCommon.Camera.LastImage.Status != SonyImage.ImageStatus.Ready)
                    {
                        DriverCommon.LogCameraMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken!");
                        throw new ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken!");
                    }

                    SonyImage image = DriverCommon.Camera.LastImage;

                    int requestedWidth = NumX;
                    int requestedHeight = NumY;
                    DriverCommon.LogCameraMessage("ImageArray", "requestedWidth = {0}, cameraNumX = {1}, camera.Mode.ImageWidth = {2}", (int)Math.Min(DriverCommon.Settings.ImageWidth, DriverCommon.Camera.Mode.ImageWidthPixels), DriverCommon.Settings.ImageWidth, DriverCommon.Camera.Mode.ImageWidthPixels);
                    DriverCommon.LogCameraMessage("ImageArray Get", String.Format("(numX = {0}, numY = {1}, image.Width = {2}, image.Height = {3})", requestedWidth, requestedHeight, image.Width, image.Height));

                    switch (image.m_info.ImageMode)
                    {
                        case 1:
                            DriverCommon.LogCameraMessage("BAYER info", String.Format("Dimensions = {0}, {1} x {2}", SonyImage.BAYER.Rank, SonyImage.BAYER.GetLength(0), SonyImage.BAYER.GetLength(1)));

                            result = Resize(SonyImage.BAYER, SonyImage.BAYER.Rank, StartX, StartY, requestedWidth, requestedHeight);
                            break;

                        case 2:
                            if (DriverCommon.Settings.Personality != SonyCommon.PERSONALITY_NINA)
                            {
                                DriverCommon.LogCameraMessage("RGB info", String.Format("Dimensions = {0}, {1} x {2} x {3}", SonyImage.RGB.Rank, SonyImage.RGB.GetLength(0), SonyImage.RGB.GetLength(1), SonyImage.RGB.GetLength(2)));

                                result = Resize(SonyImage.RGB, SonyImage.RGB.Rank, StartX, StartY, requestedWidth, requestedHeight);
                            }
                            else
                            {
                                DriverCommon.LogCameraMessage("RGB info as MONO", String.Format("Dimensions = {0}, {1} x {2}", SonyImage.BAYER.Rank, SonyImage.BAYER.GetLength(0), SonyImage.BAYER.GetLength(1)));

                                result = Resize(SonyImage.BAYER, SonyImage.BAYER.Rank, StartX, StartY, requestedWidth, requestedHeight);
                            }
                            break;

                        default:
                             DriverCommon.LogCameraMessage("Unknown info", String.Format("{0} - Throwing", image.m_info.ImageMode));

                            throw new ASCOM.InvalidOperationException("Call to ImageArray resulted in invalid image type!");
                    }

                    return result;*/
                }
            }
        }

        public object ImageArrayVariant
        {
            get
            {
                using (new SerializedAccess(this, "get_ImageArrayVariant", true))
                {
                    SonyImage image = DriverCommon.Camera.LastImage;
                    int x = 0;
                    int y = 0;
                    int c;

                    switch (image.m_info.ImageMode)
                    {
                        case 1:     // RGGB
                            int[,] rggbInput = (int[,])ImageData();
                            x = rggbInput.GetLength(0);
                            y = rggbInput.GetLength(1);
                            object[,] rggbOutput = new object[x, y];

                            for (int xcopy = 0; xcopy < x; xcopy++)
                            {
                                for (int ycopy = 0; ycopy < y; ycopy++)
                                {
                                    rggbOutput[xcopy, ycopy] = rggbInput[xcopy, ycopy];
                                }
                            }

                            return rggbOutput;


                        case 2:     // RGB
                            int[,,] rgbInput = (int[,,])ImageData();
                            x = rgbInput.GetLength(0);
                            y = rgbInput.GetLength(1);
                            c = rgbInput.GetLength(2);
                            object[,,] rgbOutput = new object[x, y, c];

                            for (int xcopy = 0; xcopy < x; xcopy++)
                            {
                                for (int ycopy = 0; ycopy < y; ycopy++)
                                {
                                    for (int ccopy = 0; ccopy < c; ccopy++)
                                    {
                                        rgbOutput[xcopy, ycopy, ccopy] = rgbInput[xcopy, ycopy, ccopy];
                                    }
                                }
                            }

                            return rgbOutput;

                        default:
                            throw new ASCOM.InvalidOperationException("Unable to detect picture format");
                    }
/*                    if (DriverCommon.Camera.LastImage.Status != SonyImage.ImageStatus.Ready)
                    {
                        DriverCommon.LogCameraMessage("ImageArrayVariant Get", "Throwing InvalidOperationException because of a call to ImageArrayVariant before the first image has been taken!");
                        throw new ASCOM.InvalidOperationException("Call to ImageArrayVariant before the first image has been taken!");
                    }
                    object[,,] cameraImageArrayVariant = new object[DriverCommon.Settings.ImageWidth, DriverCommon.Settings.ImageHeight, 3];

                    return cameraImageArrayVariant;*/
                }
            }
        }

        public bool ImageReady
        {
            get
            {
                using (new SerializedAccess(this, "get_ImageReady", true))
                {
                    bool ready = DriverCommon.Camera.ImageReady;

                    DriverCommon.LogCameraMessage("ImageReady Get", ready.ToString());

                    return ready;
                }
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                using (new SerializedAccess(this, "get_IsPulseGuiding"))
                {
//                    DriverCommon.LogCameraMessage("IsPulseGuiding Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("IsPulseGuiding", false);
                }
            }
        }

        public double LastExposureDuration
        {
            get
            {
                using (new SerializedAccess(this, "get_LastExposureDuration", true))
                {
                    if (DriverCommon.Camera.LastImage.Status != SonyImage.ImageStatus.Ready)
                    {
                        DriverCommon.LogCameraMessage("LastExposureDuration Get", "Throwing InvalidOperationException because of a call to LastExposureDuration before the first image has been taken!");
                        throw new ASCOM.InvalidOperationException("Call to LastExposureDuration before the first image has been taken!");
                    }

                    double result = DriverCommon.Camera.LastImage.Duration;
                    DriverCommon.LogCameraMessage("LastExposureDuration Get", result.ToString());

                    return result;
                }
            }
        }

        public string LastExposureStartTime
        {
            get
            {
                using (new SerializedAccess(this, "get_LastExposureStartTime", true))
                {
                    if (DriverCommon.Camera.LastImage.Status != SonyImage.ImageStatus.Ready)
                    {
                        DriverCommon.LogCameraMessage("LastExposureStartTime Get", "Throwing InvalidOperationException because of a call to LastExposureStartTime before the first image has been taken!");
                        throw new ASCOM.InvalidOperationException("Call to LastExposureStartTime before the first image has been taken!");
                    }

                    string exposureStartString = DriverCommon.Camera.LastImage.StartTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");
                    DriverCommon.LogCameraMessage("LastExposureStartTime Get", exposureStartString.ToString());
                    return exposureStartString;
                }
            }
        }

        public int MaxADU
        {
            get
            {
                using (new SerializedAccess(this, "get_MaxADU"))
                {
                    int bpp = (int)DriverCommon.Camera.Info.BitsPerPixel;
                    int maxADU = (1 << bpp) - 1;

                    DriverCommon.LogCameraMessage("MaxADU Get", maxADU.ToString());

                    return maxADU;
                }
            }
        }

        public short MaxBinX
        {
            get
            {
                using (new SerializedAccess(this, "get_MaxBinX"))
                {
//                    DriverCommon.LogCameraMessage("MaxBinX Get", "1");

                    return 1;
                }
            }
        }

        public short MaxBinY
        {
            get
            {
                using (new SerializedAccess(this, "get_MaxBinY"))
                {
//                    DriverCommon.LogCameraMessage("MaxBinY Get", "1");

                    return 1;
                }
            }
        }

        public int NumX
        {
            get
            {
                using (new SerializedAccess(this, "get_NumX", true))
                {
                    int x = (int)DriverCommon.Camera.Mode.ImageWidthPixels;

                    if (DriverCommon.Camera.ImageReady)
                    {
                        x = DriverCommon.Camera.LastImage.Width;
                    }

                    x = (int)Math.Min((RequestedWidth > 0 ? RequestedWidth : DriverCommon.Settings.ImageWidth), x);

                    DriverCommon.LogCameraMessage("NumX Get", x.ToString());
                    return x;
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_NumX", true))
                {
                    if (value < 1 || (value + StartX) > DriverCommon.Settings.ImageWidth)
                    {
                        DriverCommon.LogCameraMessage("NumX", $"Attempt to set value {value} where StartX ({StartX}) + this would be greater than width ({DriverCommon.Settings.ImageWidth})");

                        throw new ASCOM.InvalidValueException("Out of range");
                    }

                    RequestedWidth = value;
//                    DriverCommon.Settings.ImageWidth = value;
                    DriverCommon.LogCameraMessage("NumX set", value.ToString());
                }
            }
        }

        public int NumY
        {
            get
            {
                using (new SerializedAccess(this, "get_NumY", true))
                {
                    int y = (int)DriverCommon.Camera.Mode.ImageHeightPixels;

                    if (DriverCommon.Camera.ImageReady)
                    {
                        y = DriverCommon.Camera.LastImage.Height;
                    }

                    y = (int)Math.Min((RequestedHeight > 0 ? RequestedHeight : DriverCommon.Settings.ImageHeight), y);

                    DriverCommon.LogCameraMessage("NumY Get", y.ToString());
                    return y;
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_NumY", true))
                {
                    if (value < 1 || (value + StartY) > DriverCommon.Settings.ImageHeight)
                    {
                        DriverCommon.LogCameraMessage("NumY", $"Attempt to set value {value} where StartY ({StartY}) + this would be greater than width ({DriverCommon.Settings.ImageHeight})");

                        throw new ASCOM.InvalidValueException("Out of range");
                    }

                    // NINA sets this before it changes readout mode
                    RequestedHeight = value;
//                    DriverCommon.Settings.ImageHeight = value;
                    DriverCommon.LogCameraMessage("NumY set", value.ToString());
                }
            }
        }

        public short PercentCompleted
        {
            get
            {
                using (new SerializedAccess(this, "get_PercentCompleted"))
                {
//                    DriverCommon.LogCameraMessage("PercentCompleted Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("PercentCompleted", false);
                }
            }
        }

        public double PixelSizeX
        {
            get
            {
                using (new SerializedAccess(this, "get_PixelSizeX", true))
                {
                    DriverCommon.LogCameraMessage("PixelSizeX Get", DriverCommon.Camera.Info.PixelWidth.ToString());

                    return DriverCommon.Camera.Info.PixelWidth;
                }
            }
        }

        public double PixelSizeY
        {
            get
            {
                using (new SerializedAccess(this, "get_PixelSizeY", true))
                {
                    DriverCommon.LogCameraMessage("PixelSizeY Get", DriverCommon.Camera.Info.PixelHeight.ToString());

                    return DriverCommon.Camera.Info.PixelHeight;
                }
            }
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            using (new SerializedAccess(this, "PulseGuide()"))
            {
                DriverCommon.LogCameraMessage("PulseGuide", "Not implemented");

                throw new ASCOM.MethodNotImplementedException("PulseGuide");
            }
        }

        public short ReadoutMode
        {
            get
            {
                using (new SerializedAccess(this, "get_ReadoutMode", true))
                {
                    DriverCommon.LogCameraMessage("ReadoutMode Get", DriverCommon.Camera.OutputMode.ToString());

                    return (short)(DriverCommon.Camera.OutputMode - 1);
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_ReadoutMode", true))
                {
                    if (ReadoutModes.Count > value)
                    {
                        DriverCommon.LogCameraMessage("ReadoutMode Set", value.ToString());

                        switch (value)
                        {
                            case 0:
                                DriverCommon.Camera.PreviewMode = false;
                                break;

                            case 1:
                                DriverCommon.Camera.PreviewMode = true;
                                break;
                        }
                    }
                    else
                    {
                        throw new ASCOM.InvalidValueException("ReadoutMode not in allowable values");
                    }
                }
            }
        }

        public ArrayList ReadoutModes
        {
            get
            {
                using (new SerializedAccess(this, "get_ReadoutModes", true))
                {
                    ArrayList modes = new ArrayList();
                    SonyCommon.CameraInfo info = DriverCommon.Camera.Resolutions;
                    bool cropped = DriverCommon.Camera.Info.CropMode == 0;

                    modes.Add(String.Format("Full Resolution ({0} x {1})", DriverCommon.Settings.ImageWidth, DriverCommon.Settings.ImageHeight));

                    if (DriverCommon.Camera.HasLiveView)
                    {
//                        if (DriverCommon.Settings.Personality == SonyCommon.PERSONALITY_NINA)
//                        {
//                            modes.Add(String.Format("LiveView ({0} x {1}) [Mono]", DriverCommon.Camera.Resolutions.PreviewWidthPixels, DriverCommon.Camera.Resolutions.PreviewHeightPixels));
//                        }
//                        else
//                        {
                            modes.Add(String.Format("LiveView ({0} x {1})", DriverCommon.Camera.Resolutions.PreviewWidthPixels, DriverCommon.Camera.Resolutions.PreviewHeightPixels));
//                        }
                    }

                    return modes;
                }
            }
        }

        public string SensorName
        {
            get
            {
                using (new SerializedAccess(this, "get_SensorName", true))
                {
//                    DriverCommon.LogCameraMessage("SensorName Get", DriverCommon.Camera.Info.SensorName);

                    return DriverCommon.Camera.Info.SensorName;
                }
            }
        }

        public SensorType SensorType
        {
            get
            {
                using (new SerializedAccess(this, "get_SensorType", true))
                {
                    SensorType type;

//                    if (DriverCommon.Settings.Personality == SonyCommon.PERSONALITY_NINA)
//                    {
//                        type = DriverCommon.Camera.PreviewMode ? SensorType.Monochrome : SensorType.RGGB;
//                    }
//                    else
//                    {
                        type = DriverCommon.Camera.OutputMode == SonyCamera.ImageMode.RGB ? SensorType.Color : SensorType.RGGB;
//                    }
                    DriverCommon.LogCameraMessage("SensorType Get", type.ToString());

                    return type;
                }
            }
        }

        public double SetCCDTemperature
        {
            get
            {
                using (new SerializedAccess(this, "get_SetCCDTemperature"))
                {
//                    DriverCommon.LogCameraMessage("SetCCDTemperature Get", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("SetCCDTemperature", false);
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_SetCCDTemperature"))
                {
//                    DriverCommon.LogCameraMessage("SetCCDTemperature Set", "Not implemented");

                    throw new ASCOM.PropertyNotImplementedException("SetCCDTemperature", true);
                }
            }
        }

        public void StartExposure(double Duration, bool Light)
        {
            using (new SerializedAccess(this, "StartExposure()", true))
            {
                if (Duration < 0.0)
                {
                    DriverCommon.LogCameraMessage("StartExposure", $"Exposure time must be >= 0 seconds (was passed {Duration})");
                    throw new InvalidValueException("StartExposure", "Duration", ">= 0");
                }

                if (StartX + NumX > DriverCommon.Camera.Mode.ImageWidthPixels)
                {
                    DriverCommon.LogCameraMessage("StartExposure", $"StartX ({StartX}( + NumX ({NumX}) > ImageWidth ({DriverCommon.Camera.Info.ImageWidthPixels})");
                    throw new InvalidValueException("StartExposure", "StartX+NumX", $"<={DriverCommon.Camera.Info.ImageWidthPixels}");
                }

                if (StartY + NumY > DriverCommon.Camera.Mode.ImageHeightPixels)
                {
                    DriverCommon.LogCameraMessage("StartExposure", $"StartY ({StartY}( + NumY ({NumY}) > ImageHeight ({DriverCommon.Camera.Info.ImageHeightPixels})");
                    throw new InvalidValueException("StartExposure", "StartX+NumX", $"<={DriverCommon.Camera.Info.ImageHeightPixels}");
                }

                if (!LastSetFastReadout)
                {
                    if (Duration <= 1.0e-5 && DriverCommon.Camera.HasLiveView && DriverCommon.Settings.AutoLiveview)
                    {
                        DriverCommon.Camera.PreviewMode = true;
                        DriverCommon.LogCameraMessage("StartExposure", "Asked for 0.0s exposure, AutoLiveview enabled and camera supports it - taking image as liveview");
                    }
                    else
                    {
                        DriverCommon.Camera.PreviewMode = false;
                    }
                }

                DriverCommon.LogCameraMessage("StartExposure", String.Format("Duration={0}, Light={1}", Duration.ToString(), Light.ToString()));

                DriverCommon.Camera.StartCapture(Duration, DriverCommon.Settings.Personality, DriverCommon.Settings.DefaultReadoutMode);
            }
        }

        public int StartX
        {
            get
            {
                using (new SerializedAccess(this, "get_StartX"))
                {
                    DriverCommon.LogCameraMessage("StartX Get", RequestedStartX.ToString());

                    return RequestedStartX;
//                    DriverCommon.LogCameraMessage("StartX Get", DriverCommon.Settings.ImageXOffset.ToString());

//                    return DriverCommon.Settings.ImageXOffset;
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_StartX"))
                {
                    int width = DriverCommon.Settings.ImageWidth;

                    if (value < 0 || (value + NumX) > width)
                    {
                        int numX = NumX;

                        DriverCommon.LogCameraMessage("StartX", $"Attempt to set value {value} where numX ({numX}) + this would be greater than width ({width})");
                        throw new ASCOM.InvalidValueException("Out of range");
                    }

                    //                    DriverCommon.Settings.ImageXOffset = value;
                    RequestedStartX = value;

                    DriverCommon.LogCameraMessage("StartX Set", value.ToString());
                }
            }
        }

        public int StartY
        {
            get
            {
                using (new SerializedAccess(this, "get_StartY"))
                {
                    DriverCommon.LogCameraMessage("StartY Get", RequestedStartY.ToString());

                    return RequestedStartY;
//                    DriverCommon.LogCameraMessage("StartY Get", DriverCommon.Settings.ImageYOffset.ToString());

//                    return DriverCommon.Settings.ImageYOffset;
                }
            }
            set
            {
                using (new SerializedAccess(this, "set_StartY"))
                {
                    int height = DriverCommon.Settings.ImageHeight;

                    if (value < 0 || (value + NumY) > height)
                    {
                        int numY = NumY;

                        DriverCommon.LogCameraMessage("StartY", $"Attempt to set value {value} where numY ({numY}) + this would be greater than height ({height})");
                        throw new ASCOM.InvalidValueException("Out of range");
                    }

                    RequestedStartY = value;
//                    DriverCommon.Settings.ImageYOffset = value;

                    DriverCommon.LogCameraMessage("StartY set", value.ToString());
                }
            }
        }

        public void StopExposure()
        {
            using (new SerializedAccess(this, "StopExposure()", true))
            {
                DriverCommon.LogCameraMessage("StopExposure", "Attempting to stop exposure");
                DriverCommon.Camera.StopCapture();
            }
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Camera";
                if (bRegister)
                {
                    P.Register(DriverCommon.CameraDriverId, DriverCommon.CameraDriverDescription);
                }
                else
                {
                    P.Unregister(DriverCommon.CameraDriverId);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                return DriverCommon.CameraConnected;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                DriverCommon.LogCameraMessage("CheckConnected", message);
                throw new ASCOM.NotConnectedException(message);
            }
        }

        #endregion

        internal static object Resize(object array, int rank, int startX, int startY, int width, int height)
        {
            DriverCommon.LogCameraMessage("Resize", "rank={0}, startX={1}, startY={2}, width={3}, height={4}", rank, startX, startY, width, height);

            if (rank == 2)
            {
                int[,] input = (int[,])array;

                if (startX == 0 && startY == 0 && width >= input.GetLength(0) && height >= input.GetLength(1))
                {
                    return input;
                }

                int[,] output = new int[width, height];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        output[x, y] = input[x + startX, y + startY];
                    }
                }

                return output;
            }
            else if (rank == 3)
            {
                int[,,] input = (int[,,])array;

                if (startX == 0 && startY == 0 && width >= input.GetLength(0) && height >= input.GetLength(1))
                {
                    return input;
                }

                int zLen = input.GetLength(2);
                int[,,] output = new int[width, height, zLen];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < zLen; z++)
                        {
                            output[x, y, z] = input[x + startX, y + startY, z];
                        }
                    }
                }

                return output;
            }
            else
            {
                // Ummm
                throw new ASCOM.InvalidValueException();
            }
        }

        public class SerializedAccess : IDisposable
        {
            internal static Mutex m_serialAccess = new Mutex();

            internal String m_method;
            internal Camera m_cam;

            public SerializedAccess(Camera cam, String method, bool checkConnected = false)
            {
                m_cam = cam;
                m_method = method;
//               DriverCommon.LogCameraMessage(m_method, "[enter] {0}", m_serialAccess.ToString());

                if (checkConnected)
                {
                    m_cam.CheckConnected(String.Format("Camera must be connected before '{0}' can be called", method));
                }

                if (!m_serialAccess.WaitOne(1000))
                {
                    DriverCommon.LogCameraMessage(m_method, "Waiting to enter {0}", m_serialAccess.ToString());
                    m_serialAccess.WaitOne();
                }

//               DriverCommon.LogCameraMessage(m_method, "[in] {0}", m_serialAccess.ToString());
            }

            public void Dispose()
            {
//               DriverCommon.LogCameraMessage(m_method, "[out] {0}", m_serialAccess.ToString());
                m_serialAccess.ReleaseMutex();
            }
        }
    }
}
