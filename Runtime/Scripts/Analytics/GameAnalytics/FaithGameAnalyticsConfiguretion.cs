namespace com.faith.sdk
{

    using UnityEngine;
    

#if UNITY_EDITOR
    using UnityEditor;
#endif

    //[CreateAssetMenu(fileName = "APGameAnalyticsConfiguretion", menuName = "APGameAnalyticsConfiguretion")]
    public class FaithGameAnalyticsConfiguretion : FaithBaseClassForAnalyticsConfiguretion
    {
        

        #region Public Variables

        public int DefaultWorldIndex { get { return _defaultWorldIndex; } }

        #endregion

        #region Private Variables

        [HideInInspector, SerializeField] private int _defaultWorldIndex = 1;

#if UNITY_EDITOR && FaithSdk_GameAnalytics
        private GameAnalyticsSDK.Setup.Settings _gaSettings;
        private Editor _gaSettingsEditor;
        private bool _isShowingGASettings;
        
#endif

        #endregion

        #region Override Method

        public override void SetNameAndIntegrationStatus()
        {
            string sdkName = FaithSdkConstant.NameOfSDK + "_GameAnalytics";
            SetNameOfConfiguretion(sdkName);
#if UNITY_EDITOR
            _isSDKIntegrated = FaithSdkScriptDefiniedSymbol.CheckGameAnalyticsIntegration(sdkName);
#endif
        }

        public override bool CanBeSubscribedToLionLogEvent()
        {
            return false;
        }

        public override void PreCustomEditorGUI()
        {
#if UNITY_EDITOR && FaithSdk_GameAnalytics

            if (IsAnalyticsEventEnabled) {

                if (_gaSettings == null)
                    _gaSettings = Resources.Load<GameAnalyticsSDK.Setup.Settings>("GameAnalytics/Settings");

                if (_gaSettings == null)
                    EditorGUILayout.HelpBox("You need to create GA_'Settings' by going to 'Window/Game Analytics/Select Settings' from menu in order to ga_sdk for working properly", MessageType.Error);
                else
                {
                    EditorGUILayout.HelpBox("If you haven't setup your game on GA, please do by loging, adding platform and selecting your games from down below. Make sure to put the right 'sdk key' and 'secret key' for your specefic platform", MessageType.Warning);
                    FaithSdkEditorModule.DrawSettingsEditor(_gaSettings, null, ref _isShowingGASettings, ref _gaSettingsEditor);
                }

                
                FaithSdkEditorModule.DrawHorizontalLine();
            }
            

#endif
        }

        public override void PostCustomEditorGUI()
        {
#if UNITY_EDITOR && FaithSdk_GameAnalytics
                FaithSdkEditorModule.DrawHorizontalLine();

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("DefaultWorldIndexOnGameAnalytics", GUILayout.Width(FaithSdkConstant.EDITOR_LABEL_WIDTH));
                        _defaultWorldIndex = EditorGUILayout.IntField(_defaultWorldIndex);
                    }
                    EditorGUILayout.EndHorizontal();


                }
                EditorGUILayout.EndVertical();

#endif
        }

        public override void Initialize(FaithSdkConfiguretionInfo apSdkConfiguretionInfo, bool isATTEnable = false)
        {
#if FaithSdk_GameAnalytics
            if (FaithGameAnalyticsWrapper.Instance == null && IsAnalyticsEventEnabled)
            {
                Instantiate(Resources.Load("GameAnalytics/AP_GameAnalytics"));

                GameObject newAPGameAnalyticsWrapper = new GameObject("APGameAnalyticsWrapper");
                FaithGameAnalyticsWrapper.Instance = newAPGameAnalyticsWrapper.AddComponent<FaithGameAnalyticsWrapper>();

                DontDestroyOnLoad(newAPGameAnalyticsWrapper);

                FaithGameAnalyticsWrapper.Instance.Initialize(apSdkConfiguretionInfo, this);
            }
#endif
        }

#endregion



    }
}

