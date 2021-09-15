namespace com.faith.sdk
{
#if APSdk_Firebase

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Firebase;
    using Firebase.Analytics;

    public class APFirebaseWrapper : MonoBehaviour
    {
        #region Public Variables

        public static APFirebaseWrapper Instance;

        #endregion

        #region Private Variables

        private FaithSdkConfiguretionInfo _apSdkConfiguretionInfo;
        private FaithFirebaseConfiguretion _apFirebaseConfiguretion;

        #endregion

        #region Configuretion

        private bool CanLogEvent() {

            if (_apSdkConfiguretionInfo.IsAnalyticsEventEnabled)
            {
                if (_apFirebaseConfiguretion.IsAnalyticsEventEnabled)
                {
                    return true;
                }
                else
                {
                    FaithSdkLogger.LogWarning("'logFirebaseEvent' is currently turned off from APSDkIntegrationManager, please set it to 'true'");
                }
            }
            else
            {
                FaithSdkLogger.LogWarning("Analytics events are currently disabled under the 'Analytics'->'EnableAnalyticsEvents' on 'APSdk IntegrationManager'");
            }

            return false;
        }

        #endregion

        #region Public Callback

        public void Initialize(FaithSdkConfiguretionInfo apSdkConfiguretionInfo, FaithFirebaseConfiguretion apFirebaseConfiguretion, UnityAction OnInitialized = null)
        {
            _apSdkConfiguretionInfo = apSdkConfiguretionInfo;
            _apFirebaseConfiguretion = apFirebaseConfiguretion;

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    // subscribe to firebase events
                    // subscribe here so avoid error if dependency check fails

                    FaithSdkLogger.Log("Firebase Initialized");
                    OnInitialized?.Invoke();
                }
                else
                {
                    FaithSdkLogger.LogError($"Firebase: Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }


        public void LogFirebaseEvent(string eventName)
        {
            if (CanLogEvent())
            {
                FirebaseAnalytics.LogEvent(
                   eventName);
            }

        }

        public void LogFirebaseEvent(string eventName, string parameName, string paramValue)
        {

            if (CanLogEvent())
            {
                FirebaseAnalytics.LogEvent(
                        eventName,
                        parameName,
                        paramValue
                    );
            }
        }

        public void LogFirebaseEvent(string eventName, List<Parameter> parameter)
        {

            if (CanLogEvent())
            {

                FirebaseAnalytics.LogEvent(
                    eventName,
                    parameter.ToArray()
                );
            }
        }

        public void ProgressionEvent(string eventName)
        {

            if (_apFirebaseConfiguretion.IsTrackingAdEvent)
            {
                LogFirebaseEvent(eventName);
            }
            else
            {
                FaithSdkLogger.LogWarning("'AdEvent' is disabled for 'FirebaseSDK'");
            }
        }

        public void ProgressionEvent(string eventName, string parameName, string paramValue)
        {

            if (_apFirebaseConfiguretion.IsTrackingProgressionEvent)
            {
                LogFirebaseEvent(eventName, parameName, paramValue);
            }
            else
            {
                FaithSdkLogger.LogWarning("'ProgressionEvent' is disabled for 'FirebaseSDK'");
            }
        }

        public void ProgressionEvent(string eventName, List<Parameter> parameter)
        {

            if (_apFirebaseConfiguretion.IsTrackingProgressionEvent)
            {
                LogFirebaseEvent(eventName, parameter);
            }
            else
            {
                FaithSdkLogger.LogWarning("'ProgressionEvent' is disabled for 'FirebaseSDK'");
            }
        }

        public void AdEvent(string eventName, string parameName, string paramValue)
        {

            if (_apFirebaseConfiguretion.IsTrackingAdEvent)
            {
                LogFirebaseEvent(eventName, parameName, paramValue);
            }
            else
            {
                FaithSdkLogger.LogWarning("'AdEvent' is disabled for 'FirebaseSDK'");
            }
        }

        #endregion
    }

#endif
}




