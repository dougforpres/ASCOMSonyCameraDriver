using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using ASCOM.Utilities;
using Microsoft.Win32;

namespace ASCOM.SonyMirrorless
{
    public class SonyProfile
    {
        private static int DefaultImageWidth = 6024; // Constants to define the ccd pixel dimenstions
        private static int DefaultImageHeight = 4024;

        public bool EnableLogging = false;
        public string DeviceId = "";
        public short DefaultReadoutMode = SonyCommon.OUTPUTFORMAT_RGB;
        public bool UseLiveview = false;
        public int Personality = SonyCommon.PERSONALITY_APT;
        public bool AutoLiveview = false;
        public bool BulbModeEnable = false;
        public short BulbModeTime = 1;
        public bool AllowISOAdjust = false;
        public bool ARWAutosave = false;
        public string ARWAutosaveFolder = "";
        public bool ARWAutosaveWithDate = false;
        public bool ARWAutosaveAlwaysCreateEmptyFolder = false;

        // Dynamic values
        public int ImageWidth = DefaultImageWidth; // Initialise variables to hold values required for functionality tested by Conform
        public int ImageHeight = DefaultImageHeight;
//        public int ImageXOffset = 0;
//        public int ImageYOffset = 0;
    }

    class DriverCommon
    {
        private static Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public static string DriverVersion = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

        // IMPORTANT
        // CameraDriverName **cannot** change, the APT software recognizes this name specifically and enables fast-readout
        // for preview mode.
        // "Sony Mirrorless Camera"
        public static string CameraDriverName = "Sony Mirrorless Camera";
        public static string CameraDriverId = "ASCOM.SonyMirrorless.Camera";
        public static string CameraDriverDescription = "Sony Mirrorless Camera";
        public static string CameraDriverInfo = $"Camera control for various Sony DSLR/Mirrorless cameras. Help/Support: <retrodotkiwi@gmail.com>. Version: {DriverVersion}";

        public static string FocuserDriverName = "Sony Lens Focuser";
        public static string FocuserDriverId = "ASCOM.SonyMirrorless.Focuser";
        public static string FocuserDriverDescription = "Sony Camera Focuser";
        public static string FocuserDriverInfo = $"Focuser  that allows connection to a camera-controlled autofocus lens. Help/Support: <retrodotkiwi@gmail.com>. Version: {DriverVersion}";

        public static SonyProfile Settings = new SonyProfile();
        private static TraceLogger Logger = new TraceLogger("", "SonyMirrorless");

        private static SonyCamera camera = null;
        private static bool cameraConnected = false;
        private static bool focuserConnected = false;

        // Common to both
        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";
        internal static string cameraProfileName = "Camera ID";
        internal static string cameraDefault = "";

        // Specific to Camera
        internal static string readoutModeDefaultProfileName = "Readout Mode";
        internal static string readoutModeDefault = "0";
        internal static string useLiveviewProfileName = "Use Camera Liveview";
        internal static string useLiveviewDefault = "true";
        internal static string autoLiveviewProfileName = "Auto Liveview";
        internal static string autoLiveviewDefault = "false";
        internal static string personalityProfileName = "Personality";
        internal static string personalityDefault = "1";
        internal static string bulbModeEnableProfileName = "Bulb Mode Enable";
        internal static string bulbModeEnableDefault = "true";
        internal static string bulbModeTimeProfileName = "Bulb Mode Time";
        internal static string bulbModeTimeDefault = "1";
        internal static string allowISOAdjustProfileName = "Allow ISO Adjust";
        internal static string allowISOAdjustDefault = "false";

        // Specific to Focuser
        // ...

        static public SonyCamera Camera
        {
            get
            {
                return camera;
            }
        }

        static public bool CameraConnected
        {
            get
            {
                return cameraConnected;
            }

            set
            {
                bool oldValue = cameraConnected;

                cameraConnected = value;

                try
                {
                    EnsureCameraConnection();
                }
                catch
                {
                    cameraConnected = oldValue;
                }
            }
        }

        static public bool FocuserConnected
        {
            get
            {
                return focuserConnected;
            }

            set
            {
                bool oldValue = focuserConnected;

                focuserConnected = value;

                try
                {
                    EnsureCameraConnection();
                }
                catch
                {
                    focuserConnected = oldValue;
                }
            }
        }

        public static void LogCameraMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            Logger.LogMessage($"[camera] {identifier}", msg);
        }

        public static void LogFocuserMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            Logger.LogMessage($"[focuser] {identifier}", msg);
        }

        private static void Log(String message, String source = "DriverCommon")
        {
            Logger.LogMessage(source, message);
        }

        public static bool ReadProfile()
        {
            // First read for camera, then read for focuser
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Camera";

                Settings.EnableLogging = Convert.ToBoolean(driverProfile.GetValue(CameraDriverId, traceStateProfileName, string.Empty, traceStateDefault));
                Settings.DeviceId = driverProfile.GetValue(CameraDriverId, cameraProfileName, string.Empty, cameraDefault);
                Settings.DefaultReadoutMode = Convert.ToInt16(driverProfile.GetValue(CameraDriverId, readoutModeDefaultProfileName, string.Empty, readoutModeDefault));
                Settings.UseLiveview = Convert.ToBoolean(driverProfile.GetValue(CameraDriverId, useLiveviewProfileName, string.Empty, useLiveviewDefault));
                Settings.Personality = Convert.ToInt16(driverProfile.GetValue(CameraDriverId, personalityProfileName, string.Empty, personalityDefault));
                Settings.AutoLiveview = Convert.ToBoolean(driverProfile.GetValue(CameraDriverId, autoLiveviewProfileName, string.Empty, autoLiveviewDefault));
                Settings.BulbModeEnable = Convert.ToBoolean(driverProfile.GetValue(CameraDriverId, bulbModeEnableProfileName, string.Empty, bulbModeEnableDefault));
                Settings.BulbModeTime = Convert.ToInt16(driverProfile.GetValue(CameraDriverId, bulbModeTimeProfileName, string.Empty, bulbModeTimeDefault));
                Settings.AllowISOAdjust = Convert.ToBoolean(driverProfile.GetValue(CameraDriverId, allowISOAdjustProfileName, string.Empty, allowISOAdjustDefault));

                if (Settings.DefaultReadoutMode == 0)
                {
                    Settings.DefaultReadoutMode = SonyCommon.OUTPUTFORMAT_RGGB;
                }

                // This needs to actually save to registry
                Settings.ARWAutosave = Convert.ToBoolean(Registry.GetValue("HKEY_CURRENT_USER\\Software\\retro.kiwi\\SonyMTPCamera.dll", "File Auto Save", 0));
                Settings.ARWAutosaveFolder = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\retro.kiwi\\SonyMTPCamera.dll", "File Save Path", "");
                Settings.ARWAutosaveWithDate = Convert.ToBoolean(Registry.GetValue("HKEY_CURRENT_USER\\Software\\retro.kiwi\\SonyMTPCamera.dll", "File Save Path Add Date", 0));
                Settings.ARWAutosaveAlwaysCreateEmptyFolder = Convert.ToBoolean(Registry.GetValue("HKEY_CURRENT_USER\\Software\\retro.kiwi\\SonyMTPCamera.dll", "File Save Path Create Multiple Directories", 0));

                Logger.Enabled = Settings.EnableLogging;

                Log($"DeviceID:                            {Settings.DeviceId}", "ReadProfile");
                Log($"Default Readout Mode:                {Settings.DefaultReadoutMode}", "ReadProfile");
                Log($"Save Raw files:                      {Settings.ARWAutosave}", "ReadProfile");
                Log($"Save Raw files Path:                 {Settings.ARWAutosaveFolder}", "ReadProfile");
                Log($"Save Raw files Path Add Date:        {Settings.ARWAutosaveWithDate}", "ReadProfile");
                Log($"Save Raw files Always Create Folder: {Settings.ARWAutosaveWithDate}", "ReadProfile");
                Log($"Use Liveview:                        {Settings.UseLiveview}", "ReadProfile");
                Log($"AutoLiveview @ 0.0s:                 {Settings.AutoLiveview}", "ReadProfile");
                Log($"Personality:                         {Settings.Personality}", "ReadProfile");
                Log($"Bulb Mode Enable:                    {Settings.BulbModeEnable}", "ReadProfile");
                Log($"Bulb Mode Time:                      {Settings.BulbModeTime}", "ReadProfile");
            }

            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
            }

            return true;
        }

        public static bool WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Camera";
                driverProfile.WriteValue(CameraDriverId, traceStateProfileName, Settings.EnableLogging.ToString());
                driverProfile.WriteValue(CameraDriverId, readoutModeDefaultProfileName, Settings.DefaultReadoutMode.ToString());
                driverProfile.WriteValue(CameraDriverId, useLiveviewProfileName, Settings.UseLiveview.ToString());
                driverProfile.WriteValue(CameraDriverId, autoLiveviewProfileName, Settings.AutoLiveview.ToString());
                driverProfile.WriteValue(CameraDriverId, personalityProfileName, Settings.Personality.ToString());
                driverProfile.WriteValue(CameraDriverId, bulbModeEnableProfileName, Settings.BulbModeEnable.ToString());
                driverProfile.WriteValue(CameraDriverId, bulbModeTimeProfileName, Settings.BulbModeTime.ToString());
                driverProfile.WriteValue(CameraDriverId, allowISOAdjustProfileName, Settings.AllowISOAdjust.ToString());

                Registry.SetValue("HKEY_CURRENT_USER\\Software\\retro.kiwi\\SonyMTPCamera.dll", "File Auto Save", Settings.ARWAutosave ? 1 : 0);
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\retro.kiwi\\SonyMTPCamera.dll", "File Save Path", Settings.ARWAutosaveFolder);
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\retro.kiwi\\SonyMTPCamera.dll", "File Save Path Add Date", Settings.ARWAutosaveWithDate ? 1 : 0);
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\retro.kiwi\\SonyMTPCamera.dll", "File Save Path Create Multiple Directories", Settings.ARWAutosaveAlwaysCreateEmptyFolder ? 1 : 0);

                if (Settings.DeviceId != null && Settings.DeviceId != "")
                {
                    driverProfile.WriteValue(CameraDriverId, cameraProfileName, Settings.DeviceId.ToString());
                }

                Logger.Enabled = Settings.EnableLogging;
            }

            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
//                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
            }


            return true;
        }

        private static void EnsureCameraConnection()
        {
            if (cameraConnected || focuserConnected)
            {
                if (camera == null)
                {
                    Log("Camera does not currently exist, seeing if we can create", "EnsureCameraConnection");

                    // See if we can create a camera using deviceId
                    if (Settings.DeviceId != "")
                    {
                        Log($"Camera selected for connection {Settings.DeviceId}, searching", "EnsureCameraConnection");

                        SonyCameraEnumerator enumerator = new SonyCameraEnumerator();

                        foreach (SonyCamera candidate in enumerator.Cameras)
                        {
                            if (camera == null && candidate.DisplayName == Settings.DeviceId)
                            {
                                Log("Found camera, hooray!", "EnsureCameraConnection");
                                camera = candidate;
                                Settings.ImageWidth = (int)(camera.Info.CropMode == 0 ? camera.Info.ImageWidthPixels : camera.Info.ImageWidthCroppedPixels);
                                Settings.ImageHeight = (int)(camera.Info.CropMode == 0 ? camera.Info.ImageHeightPixels : camera.Info.ImageHeightCroppedPixels);

                                switch (Settings.DefaultReadoutMode)
                                {
                                    case 0:
                                        camera.OutputMode = SonyCamera.ImageMode.RGB;
                                        break;

                                    case 1:
                                        camera.OutputMode = SonyCamera.ImageMode.RGGB;
                                        break;
                                }

                                camera.BulbMode = Settings.BulbModeEnable;
                                camera.BulbModeTime = Settings.BulbModeTime;
                            }
                        }
                    }
                }

                if (camera != null)
                {
                    bool needsConnect = cameraConnected | focuserConnected;

                    Log($"Ensure Camera {needsConnect}", "EnsureCameraConnection");

                    if (needsConnect == camera.Connected)
                    {
                        Log("Camera already connected", "EnsureCameraConnection");
                        return;
                    }

                    Log("Connecting", "EnsureCameraConnection");
                    camera.Connected = needsConnect;
                }
            }
            else
            {
                // Need to ensure disconnected
                if (camera != null)
                {
                    camera.Connected = false;
                    camera = null;
                }
            }
        }
    }
}
