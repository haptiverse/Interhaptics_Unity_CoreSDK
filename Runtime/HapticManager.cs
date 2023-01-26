﻿/* ​
* Copyright © 2022 Go Touch VR SAS. All rights reserved. ​
* ​
*/

namespace Interhaptics
{

    [UnityEngine.AddComponentMenu("Interhaptics/HapticManager")]
    public sealed class HapticManager : Internal.Singleton<HapticManager>
    {

        public void Init()
        {
            Core.HAR.Init();
            Internal.HapticDeviceManager.DeviceInitLoop();
#if UNITY_ANDROID && !ENABLE_METAQUEST
            Platforms.Mobile.GenericAndroidHapticAbstraction.Initialize();
            Platforms.Mobile.GenericAndroidHapticAbstraction.m_timer = 30;
            Platforms.Mobile.GenericAndroidHapticAbstraction.pulse = 30;
            Platforms.Mobile.GenericAndroidHapticAbstraction.m_last_time = UnityEngine.Time.fixedTime;
#elif UNITY_IPHONE
            UnityCoreHaptics.UnityCoreHapticsProxy.CreateEngine();
#endif
        }

        override protected void OnAwake()
        {
#if !UNITY_EDITOR_OSX
            Init();
#endif
        }

        private void LateUpdate()
        {
#if !UNITY_IOS //TODO make sure it does not run on mobile ? Also we will change mobile to use that so idk
            //Compute all haptics event
            Core.HAR.ComputeAllEvents(UnityEngine.Time.realtimeSinceStartup);

            //Insert device rendering loop here
            Internal.HapticDeviceManager.DeviceRenderLoop();
#endif
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            // TODO pause the haptic playback
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            // TODO pause the haptic playback
        }

        protected override void OnOnApplicationQuit()
        {
#if !UNITY_EDITOR_OSX
            Internal.HapticDeviceManager.DeviceCleanLoop();
            Core.HAR.ClearActiveEvents();
            Core.HAR.ClearInactiveEvents();
            Core.HAR.Quit();
#endif
        }

    }

}