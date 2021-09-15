namespace com.faith.sdk
{


    using UnityEngine;
    using System.Collections.Generic;

    //[CreateAssetMenu(fileName = "APFirebaseConfiguretion", menuName = "APFirebaseConfiguretion")]
    public class FaithFirebaseConfiguretion : FaithBaseClassForAnalyticsConfiguretion
    {

        #region Override Method

        public override void SetNameAndIntegrationStatus()
        {
            string sdkName = FaithSdkConstant.NameOfSDK + "_Firebase";
            SetNameOfConfiguretion(sdkName);
#if UNITY_EDITOR
            _isSDKIntegrated = FaithSdkScriptDefiniedSymbol.CheckFirebaseIntegration(sdkName);
#endif
        }

        public override bool CanBeSubscribedToLionLogEvent()
        {
            return true;
        }

        public override void PreCustomEditorGUI()
        {
            
        }

        public override void PostCustomEditorGUI()
        {
            
        }

        public override void Initialize(FaithSdkConfiguretionInfo apSdkConfiguretionInfo, bool isATTEnable = false)
        {
#if APSdk_Firebase

            if (APFirebaseWrapper.Instance == null && IsAnalyticsEventEnabled)
            {

                GameObject newAPFirebaseWrapper = new GameObject("APFirebaseWrapper");
                APFirebaseWrapper.Instance = newAPFirebaseWrapper.AddComponent<APFirebaseWrapper>();

                DontDestroyOnLoad(newAPFirebaseWrapper);

#if APSdk_LionKit

                APFirebaseWrapper.Instance.Initialize(
                            apSdkConfiguretionInfo,
                            this,
                            () =>
                            {

                                if (_subscribeToLionEvent)
                                {

                                    LionStudios.Analytics.OnLogEvent += (gameEvent) =>
                                    {
                                        List<Firebase.Analytics.Parameter> parameters = new List<Firebase.Analytics.Parameter>();

                                        if (gameEvent.eventParams != null)
                                        {

                                            List<string> keyList = new List<string>();
                                            List<string> valueList = new List<string>();

                                            Dictionary<string, object>.KeyCollection keys = gameEvent.eventParams.Keys;
                                            Dictionary<string, object>.ValueCollection values = gameEvent.eventParams.Values;

                                            foreach (object key in keys)
                                            {
                                                keyList.Add(key.ToString());
                                            }

                                            foreach (object value in values)
                                            {
                                                valueList.Add(value.ToString());
                                            }

                                            int numberOfEventParams = keyList.Count;

                                            for (int i = 0; i < numberOfEventParams; i++)
                                            {
                                                parameters.Add(new Firebase.Analytics.Parameter(keyList[i], valueList[i]));
                                            }
                                        }

                                        APFirebaseWrapper.Instance.LogFirebaseEvent(gameEvent.eventName, parameters);
                                    };
                                }

                                //------------------------
                                if (_subscribeToLionEventUA)
                                {

                                    LionStudios.Analytics.OnLogEventUA += (gameEvent) =>
                                    {
                                        List<Firebase.Analytics.Parameter> parameters = new List<Firebase.Analytics.Parameter>();

                                        if (gameEvent.eventParams != null)
                                        {

                                            List<string> keyList = new List<string>();
                                            List<string> valueList = new List<string>();

                                            Dictionary<string, object>.KeyCollection keys = gameEvent.eventParams.Keys;
                                            Dictionary<string, object>.ValueCollection values = gameEvent.eventParams.Values;

                                            foreach (object key in keys)
                                            {
                                                keyList.Add(key.ToString());
                                            }

                                            foreach (object value in values)
                                            {
                                                valueList.Add(value.ToString());
                                            }

                                            int numberOfEventParams = keyList.Count;

                                            for (int i = 0; i < numberOfEventParams; i++)
                                            {
                                                parameters.Add(new Firebase.Analytics.Parameter(keyList[i], valueList[i]));
                                            }
                                        }

                                        APFirebaseWrapper.Instance.LogFirebaseEvent(gameEvent.eventName, parameters);
                                    };
                                }
                            }
                        );

#else
            APFirebaseWrapper.Instance.Initialize(apSdkConfiguretionInfo, this);
#endif

            }
#endif
        }

#endregion



    }

}

