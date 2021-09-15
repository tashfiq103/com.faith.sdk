namespace com.faith.sdk
{

    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    //[CreateAssetMenu(fileName = "APFacebookConfiguretion", menuName = "APFacebookConfiguretion")]
    public class FaithFacebookConfiguretion : FaithBaseClassForAnalyticsConfiguretion
    {
        #region Private Variables

#if APSdk_Facebook

        [HideInInspector, SerializeField] private string _facebookAppName;
        [HideInInspector, SerializeField] private string _facebookAppId;
        private Facebook.Unity.Settings.FacebookSettings _facebookSettings;
#endif

        #endregion

        #region Override Method

        public override void SetNameAndIntegrationStatus()
        {
            string sdkName = FaithSdkConstant.NameOfSDK + "_Facebook";
            SetNameOfConfiguretion(sdkName);
#if UNITY_EDITOR
            _isSDKIntegrated = FaithSdkScriptDefiniedSymbol.CheckFacebookIntegration(sdkName);
#endif
        }

        public override bool CanBeSubscribedToLionLogEvent()
        {
            return true;
        }

        public override void PostCustomEditorGUI()
        {
            
        }

        public override void PreCustomEditorGUI()
        {
#if UNITY_EDITOR && APSdk_Facebook

            if(_facebookSettings == null)
                _facebookSettings = Resources.Load<Facebook.Unity.Settings.FacebookSettings>("FacebookSettings");

            if (_facebookSettings == null)
            {
                EditorGUILayout.HelpBox(string.Format("You need to create 'FacebookSettings' by going to 'Facebook/Edit Settings' from menu in order to facebook sdk for working properly"), MessageType.Error);
            }
            else {

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("appName", GUILayout.Width(FaithSdkConstant.EDITOR_LABEL_WIDTH));
                    EditorGUI.BeginChangeCheck();
                    _facebookAppName = EditorGUILayout.TextField(_facebookAppName);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Facebook.Unity.Settings.FacebookSettings.AppLabels = new List<string>() { _facebookAppName };
                    }

                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("appId", GUILayout.Width(FaithSdkConstant.EDITOR_LABEL_WIDTH));
                    EditorGUI.BeginChangeCheck();
                    _facebookAppId = EditorGUILayout.TextField(_facebookAppId);
                    if (EditorGUI.EndChangeCheck())
                    {

                        Facebook.Unity.Settings.FacebookSettings.AppIds = new List<string>() { _facebookAppId };
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            

            FaithSdkEditorModule.DrawHorizontalLine();
#endif
        }

        public override void Initialize(FaithSdkConfiguretionInfo apSdkConfiguretionInfo, bool isATTEnable = false)
        {
#if APSdk_Facebook

            if (FaithFacebookWrapper.Instance == null && IsAnalyticsEventEnabled)
            {

                GameObject newAPFacebookWrapper = new GameObject("FaithFacebookWrapper");
                FaithFacebookWrapper.Instance = newAPFacebookWrapper.AddComponent<FaithFacebookWrapper>();

                DontDestroyOnLoad(newAPFacebookWrapper);

#if APSdk_LionKit
                FaithFacebookWrapper.Instance.Initialize(
                        apSdkConfiguretionInfo,
                        this,
                        isATTEnable,
                        () => {
                            if (_subscribeToLionEvent)
                            {

                                LionStudios.Analytics.OnLogEvent += (gameEvent) =>
                                {
                                    FaithLionKitWrapper.LogLionGameEvent("Facebook", gameEvent);
                                    FaithFacebookWrapper.Instance.LogEvent(
                                            gameEvent.eventName,
                                            gameEvent.eventParams
                                        );
                                };
                            }

                            if (_subscribeToLionEventUA)
                            {

                                LionStudios.Analytics.OnLogEventUA += (gameEvent) =>
                                {
                                    FaithLionKitWrapper.LogLionGameEvent("FacebookUA", gameEvent);
                                    FaithFacebookWrapper.Instance.LogEvent(
                                            gameEvent.eventName,
                                            gameEvent.eventParams
                                        );
                                };
                            }
                        });
#else
        FaithFacebookWrapper.Instance.Initialize(apSdkConfiguretionInfo, this, isATTEnable);
#endif

            }

#endif
        }

        #endregion


    }

}

