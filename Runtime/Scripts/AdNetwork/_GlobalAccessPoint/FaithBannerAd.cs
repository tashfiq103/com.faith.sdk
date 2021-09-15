namespace com.faith.sdk
{
    using UnityEngine;
    using System.Collections.Generic;

#if APSdk_LionKit
    using LionStudios;
#endif

#if APSdk_GameAnalytics
    using GameAnalyticsSDK;
#endif

    public static class FaithBannerAd
    {
        #region Private Variables

        private static FaithSdkConfiguretionInfo _apSdkConfiguretionInfo;

        #endregion

        #region Configuretion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnGameStart()
        {
            _apSdkConfiguretionInfo = Resources.Load<FaithSdkConfiguretionInfo>("APSdkConfiguretionInfo");
        }

        private static void LogEvent(string paramName, string paramValue, string eventName, Dictionary<string, object> eventParams)
        {

#if APSdk_LionKit
            Analytics.LogEvent(eventName, eventParams);
#else

#if APSdk_Facebook
                            APFacebookWrapper.Instance.AdEvent(
                                    eventName,
                                    eventParams
                                );
#endif

#if APSdk_Adjust
                            APAdjustWrapper.Instance.AdEvent(
                                    eventName,
                                    eventParams
                                );
#endif

#if APSdk_Firebase
                            APFirebaseWrapper.Instance.AdEvent(
                                    eventName,
                                    paramName,
                                    paramValue
                                );
#endif

#endif
        }

        #endregion

        #region Public Callback

        public static bool IsAdReady()
        {

            if (_apSdkConfiguretionInfo.SelectedAdConfig != null)
            {
                return _apSdkConfiguretionInfo.SelectedAdConfig.IsBannerAdReady();
            }

            return false;
        }

        public static void Show(string adPlacement = "banner", int playerLevel = 0) {

            if (_apSdkConfiguretionInfo.SelectedAdConfig != null) {

                _apSdkConfiguretionInfo.SelectedAdConfig.ShowBannerAd(
                        adPlacement,
                        playerLevel
                    );

                if (IsAdReady())
                {

                    string paramName = adPlacement;
                    string paramValue = "shown";

                    string eventName = "bannerAd";
                    Dictionary<string, object> eventParams = new Dictionary<string, object>();
                    eventParams.Add(paramName, paramValue);

                    LogEvent(paramName, paramValue, eventName, eventParams);

#if APSdk_GameAnalytics
                    FaithGameAnalyticsWrapper.Instance.AdEvent(
                        GAAdAction.Show,
                        GAAdType.Banner,
                        _apSdkConfiguretionInfo.SelectedAdConfig.NameOfConfiguretion,
                        adPlacement
                    );
#endif
                }
                else {

                    string paramName = adPlacement;
                    string paramValue = "failed";

                    string eventName = "bannerAd";
                    Dictionary<string, object> eventParams = new Dictionary<string, object>();
                    eventParams.Add(paramName, paramValue);

                    LogEvent(paramName, paramValue, eventName, eventParams);

#if APSdk_GameAnalytics
                    FaithGameAnalyticsWrapper.Instance.AdEvent(
                        GAAdAction.FailedShow,
                        GAAdType.Banner,
                        _apSdkConfiguretionInfo.SelectedAdConfig.NameOfConfiguretion,
                        adPlacement
                    );
#endif
                }
            }
        }

        public static void Hide() {

            if (_apSdkConfiguretionInfo.SelectedAdConfig != null) {

                _apSdkConfiguretionInfo.SelectedAdConfig.HideBannerAd();

                string paramName = "banner";
                string paramValue = "hide";

                string eventName = "bannerAd";
                Dictionary<string, object> eventParams = new Dictionary<string, object>();
                eventParams.Add(paramName, paramValue);

                LogEvent(paramName, paramValue, eventName, eventParams);

#if APSdk_GameAnalytics
                FaithGameAnalyticsWrapper.Instance.AdEvent(
                    GAAdAction.Undefined,
                    GAAdType.Banner,
                    _apSdkConfiguretionInfo.SelectedAdConfig.NameOfConfiguretion,
                    paramName
                );
#endif
            }
        }

        #endregion
    }
}

