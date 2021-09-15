namespace com.faith.sdk
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;

#if FaithSdk_LionKit
    using LionStudios;
#endif

#if FaithSdk_GameAnalytics
    using GameAnalyticsSDK;
#endif

    public static class FaithInterstitialAd
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

#if FaithSdk_LionKit
            Analytics.LogEvent(eventName, eventParams);
#else

#if FaithSdk_Facebook
                            FaithFacebookWrapper.Instance.AdEvent(
                                    eventName,
                                    eventParams
                                );
#endif

#if FaithSdk_Adjust
                            FaithAdjustWrapper.Instance.AdEvent(
                                    eventName,
                                    eventParams
                                );
#endif

#if FaithSdk_Firebase
                            FaithFirebaseWrapper.Instance.AdEvent(
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
                return _apSdkConfiguretionInfo.SelectedAdConfig.IsInterstitialAdReady();
            }

            return false;
        }

        public static void Show(
            string adPlacement = "interstitial",
            UnityAction OnAdFailed = null,
            UnityAction OnAdClosed = null)
        {
            if (_apSdkConfiguretionInfo.SelectedAdConfig != null) {

                _apSdkConfiguretionInfo.SelectedAdConfig.ShowInterstitialAd(
                        adPlacement,
                        OnAdClosed: () => {

                            OnAdClosed?.Invoke();

                            string paramName = adPlacement;
                            string paramValue = "shown";

                            string eventName = "interstitialAd";
                            Dictionary<string, object> eventParams = new Dictionary<string, object>();
                            eventParams.Add(paramName, paramValue);

                            LogEvent(paramName, paramValue, eventName, eventParams);

#if FaithSdk_GameAnalytics
                            FaithGameAnalyticsWrapper.Instance.AdEvent(
                                GAAdAction.Show,
                                GAAdType.Interstitial,
                                _apSdkConfiguretionInfo.SelectedAdConfig.NameOfConfiguretion,
                                adPlacement
                            );
#endif
                        },
                        OnAdFailed: () => {

                            OnAdFailed?.Invoke();

                            string paramName = adPlacement;
                            string paramValue = "failed";

                            string eventName = "interstitialAd";
                            Dictionary<string, object> eventParams = new Dictionary<string, object>();
                            eventParams.Add(paramName, paramValue);

                            LogEvent(paramName, paramValue, eventName, eventParams);

#if FaithSdk_GameAnalytics
                            FaithGameAnalyticsWrapper.Instance.AdEvent(
                                GAAdAction.FailedShow,
                                GAAdType.Interstitial,
                                _apSdkConfiguretionInfo.SelectedAdConfig.NameOfConfiguretion,
                                adPlacement
                            );
#endif
                        }
                    );
            }
            else
            {

                FaithSdkLogger.LogError("Failed to display 'RewardedAd' as no 'AdNetwork' is selected/enabled");
            }
        }

        #endregion
    }


}


