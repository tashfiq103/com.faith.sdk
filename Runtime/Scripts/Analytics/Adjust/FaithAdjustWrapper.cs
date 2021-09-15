#if APSdk_Adjust

namespace com.faith.sdk
{
    using System.Collections.Generic;
    using UnityEngine;
    using com.adjust.sdk;

    public class FaithAdjustWrapper : MonoBehaviour
    {
        #region Public Variables

        public static FaithAdjustWrapper Instance;

        #endregion

        #region Private Variables

        private FaithSdkConfiguretionInfo _apSdkConfiguretionInfo;
        private FaithAdjustConfiguretion _adjustConfiguretion;


        #endregion

        #region Mono Behaviour

        private void OnApplicationPause(bool pause)
        {

#if UNITY_EDITOR
    return;
#elif UNITY_IOS
            // No action, iOS SDK is subscribed to iOS lifecycle notifications.
#elif UNITY_ANDROID
            if (pause)
                {
                    AdjustAndroid.OnPause();
                }
                else
                {
                    AdjustAndroid.OnResume();
                }
#endif
        }

        #endregion

        #region Public Callback

        public void Initialize(FaithSdkConfiguretionInfo apSdkConfiguretionInfo, FaithAdjustConfiguretion adjustConfiguretion) {

            _apSdkConfiguretionInfo = apSdkConfiguretionInfo;
            _adjustConfiguretion = adjustConfiguretion;

            AdjustConfig adjustConfig = new AdjustConfig(
                adjustConfiguretion.appToken,
                adjustConfiguretion.Environment,
                adjustConfiguretion.LogLevel == AdjustLogLevel.Suppress);

            adjustConfig.setLogLevel(adjustConfiguretion.LogLevel);
            adjustConfig.setSendInBackground(adjustConfiguretion.SendInBackground);
            adjustConfig.setEventBufferingEnabled(adjustConfiguretion.EventBuffering);
            adjustConfig.setLaunchDeferredDeeplink(adjustConfiguretion.LaunchDeferredDeeplink);

            adjustConfig.setDelayStart(adjustConfiguretion.StartDelay);

            Adjust.start(adjustConfig);

            FaithSdkLogger.Log("Adjust Initialized");
        }


        public void ProgressionEvent(string eventName, Dictionary<string, object> eventParams)
        {

            if (_adjustConfiguretion.IsTrackingProgressionEvent)
            {
                LogEvent(eventName, eventParams);
            }
            else
            {
                FaithSdkLogger.LogWarning("'ProgressionEvent' is disabled for 'AdjustSDK'");
            }
        }

        public void AdEvent(string eventName, Dictionary<string, object> eventParams)
        {

            if (_adjustConfiguretion.IsTrackingAdEvent)
            {
                LogEvent(eventName, eventParams);
            }
            else
            {
                FaithSdkLogger.LogWarning("'AdEvent' is disabled for 'AdjustSDK'");
            }
        }

        public void LogEvent(string eventName, Dictionary<string, object> eventParams)
        {
            if (_apSdkConfiguretionInfo.IsAnalyticsEventEnabled)
            {

                if (_adjustConfiguretion.IsAnalyticsEventEnabled)
                {
                    AdjustEvent newEvent = new AdjustEvent(eventName);
                    Adjust.trackEvent(newEvent);
                }
                else
                {

                    FaithSdkLogger.LogWarning("'logAdjustEvent' is currently turned off from APSDkIntegrationManager, please set it to 'true'");
                }
            }
            else {

                FaithSdkLogger.LogWarning("Analytics events are currently disabled under the 'Analytics'->'EnableAnalyticsEvents' on 'APSdk IntegrationManager'");
            }
        }

        #endregion
    }
}

#endif



