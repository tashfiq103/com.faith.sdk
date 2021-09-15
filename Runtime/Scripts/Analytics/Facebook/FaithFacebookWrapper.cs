
#if FaithSdk_Facebook

namespace com.faith.sdk
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Facebook.Unity;

    public class FaithFacebookWrapper : MonoBehaviour
    {
    #region Public Variables

        public static FaithFacebookWrapper Instance;

        public static bool IsFacebookInitialized { get { return FB.IsInitialized; } }

        #endregion

        #region private Variables

        
        private FaithSdkConfiguretionInfo _apSdkConfiguretionInfo;
        private FaithFacebookConfiguretion _facebookConfiguretion;
        private bool _isATTEnabled = false;
        private UnityAction _OnInitialized;
        

        #endregion

        #region Configuretion

        

        private void OnInitializeCallback() {

            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                FaithSdkLogger.Log("FacebookSDK initialized");


#if UNITY_IOS

                FaithSdkLogger.Log(string.Format("Facebook ATT Status (iOS) = {0}", _isATTEnabled));
                FB.Mobile.SetAdvertiserTrackingEnabled(_isATTEnabled);
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    FaithSdkLogger.Log(string.Format("AudienceNetwork = {0}", _isATTEnabled));
                    AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(_isATTEnabled);
                }
                else
                    FaithSdkLogger.LogWarning(string.Format("AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled() -> is not set as non iOS platform"));

#endif
                _OnInitialized?.Invoke();
            }
            else
                FaithSdkLogger.LogError("Failed to Initialize the Facebook SDK");
        }

        private void OnHideUnityCallback(bool isGameShown) {


        }

#endregion

#region Public Callback

        public void Initialize(FaithSdkConfiguretionInfo apSdkConfiguretionInfo, FaithFacebookConfiguretion facebookConfiguretion, bool isATTEnabled, UnityAction OnInitialized = null) {

            _apSdkConfiguretionInfo = apSdkConfiguretionInfo;
            _facebookConfiguretion = facebookConfiguretion;
            _isATTEnabled = isATTEnabled;
            _OnInitialized = OnInitialized;

            if (!FB.IsInitialized)
            {
                FB.Init(OnInitializeCallback, OnHideUnityCallback);
            }
            else {

                FaithSdkLogger.Log("FacebookSDK already initialized");
            }
        }

        public void ProgressionEvent(string eventName, Dictionary<string, object> eventParams)
        {

            if (_facebookConfiguretion.IsTrackingProgressionEvent)
            {
                LogEvent(eventName, eventParams);
            }
            else
            {
                FaithSdkLogger.LogWarning("'ProgressionEvent' is disabled for 'FacebookSDK'");
            }
        }

        public void AdEvent(string eventName, Dictionary<string, object> eventParams) {

            if (_facebookConfiguretion.IsTrackingAdEvent)
            {
                LogEvent(eventName, eventParams);
            }
            else {
                FaithSdkLogger.LogWarning("'AdEvent' is disabled for 'FacebookSDK'");
            }
        }

        public void LogEvent(string eventName, Dictionary<string, object> eventParams) {

            if (_apSdkConfiguretionInfo.IsAnalyticsEventEnabled)
            {

                if (_facebookConfiguretion.IsAnalyticsEventEnabled)
                {

                    if (FB.IsInitialized)
                    {
                        FB.LogAppEvent(
                                eventName,
                                parameters: eventParams
                            );
                    }
                    else
                    {
                        FaithSdkLogger.LogError(string.Format("{0}\n{1}", "Failed to log event for facebook analytics! as it's not initialized", eventName, eventParams));
                    }
                }
                else
                {
                    FaithSdkLogger.LogWarning("'logFacebookEvent' is currently turned off from APSDkIntegrationManager, please set it to 'true'");
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

