namespace com.faith.sdk
{
    using UnityEngine;
#if APSdk_LionKit
    using LionStudios;
#endif

#if UNITY_IOS
    using UnityEngine.iOS;
#endif

    public static class APSdkManager
    {
        public static bool IsATTEnabled
        {
            get;
            private set;
        } = false;

        public static bool IsInitialized
        {
            get;
            private set;
        } = false;

        private static void InitializeAnalytics(FaithSdkConfiguretionInfo _apSdkConfiguretionInfo, bool IsATTEnabled = false) {

            Object[] analyticsConfiguretionObjects = Resources.LoadAll("", typeof(FaithBaseClassForAnalyticsConfiguretion));
            foreach (Object analyticsConfiguretionObject in analyticsConfiguretionObjects)
            {

                FaithBaseClassForAnalyticsConfiguretion analyticsConfiguretion = (FaithBaseClassForAnalyticsConfiguretion)analyticsConfiguretionObject;
                if (analyticsConfiguretion != null)
                    analyticsConfiguretion.Initialize(_apSdkConfiguretionInfo, IsATTEnabled);
            }
        }

        private static void InitializeAdNetworks(FaithSdkConfiguretionInfo _apSdkConfiguretionInfo, bool IsATTEnabled = false) {

            Object[] adNetworkConfiguretionObjects = Resources.LoadAll("", typeof(FaithBaseClassForAdConfiguretion));
            foreach (Object adNetoworkConfiguretionObject in adNetworkConfiguretionObjects)
            {

                FaithBaseClassForAdConfiguretion adNetworkConfiguretion = (FaithBaseClassForAdConfiguretion)adNetoworkConfiguretionObject;
                if (adNetworkConfiguretion != null && _apSdkConfiguretionInfo.SelectedAdConfig == adNetworkConfiguretion)
                {
                    adNetworkConfiguretion.Initialize(_apSdkConfiguretionInfo, IsATTEnabled);
                    if (!adNetworkConfiguretion.IsShowBannerAdManually) {
                        adNetworkConfiguretion.ShowBannerAd();
                    }
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnGameStart()
        {

            FaithSdkConfiguretionInfo _apSdkConfiguretionInfo = Resources.Load<FaithSdkConfiguretionInfo>("APSdkConfiguretionInfo");

            FaithAnalytics.Initialize(_apSdkConfiguretionInfo);

#if APSdk_LionKit

            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                FaithSdkLogger.Log("MaxSDK Initialized");

#if UNITY_IOS
                if (MaxSdkUtils.CompareVersions(Device.systemVersion, "14.5") != MaxSdkUtils.VersionComparisonResult.Lesser)
                {
                    FaithSdkLogger.Log("iOS 14.5+ detected!! SetAdvertiserTrackingEnabled = true");
                    IsATTEnabled = sdkConfiguration.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Authorized;
                }
                else
                {
                    FaithSdkLogger.Log("iOS <14.5 detected!! Normal Mode");
                }
#endif

                LionKit.OnInitialized += () =>
                {
                    FaithSdkLogger.Log("LionKit Initialized");

                    InitializeAnalytics(_apSdkConfiguretionInfo, IsATTEnabled);
                    InitializeAdNetworks(_apSdkConfiguretionInfo, IsATTEnabled);
                };

            };

            

#else
             InitializeAnalytics(_apSdkConfiguretionInfo, IsATTEnabled);
             InitializeAdNetworks(_apSdkConfiguretionInfo, IsATTEnabled);
#endif

        }
    }
}

