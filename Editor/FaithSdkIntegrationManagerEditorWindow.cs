
namespace com.faith.sdk
{
#if UNITY_EDITOR
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public class FaithSdkIntegrationManagerEditorWindow : EditorWindow
    {
    #region Public Variables

        public const float LabelWidth = 200;

    #endregion

    #region Private Variables   :   General

        private static EditorWindow _reference;
        

        private bool _IsInformationFetched = false;
        private Vector2 _scrollPosition;

        private GUIStyle _settingsTitleStyle;
        private GUIStyle _hyperlinkStyle;

        private List<FaithBaseClassForAnalyticsConfiguretion> _listOfAnalyticsConfiguretion;
        private List<FaithBaseClassForAdConfiguretion> _listOfAdConfiguretion;

        private const string _linkForDownload       = "https://github.com/ap-tashfiq/com.alphapotato.sdk/releases/";
        private const string _linkForDocumetation   = "https://github.com/ap-tashfiq/com.alphapotato.sdk/blob/main/README.md";

    #endregion


    #region Private Variables   :   APSdkConfiguretionInfo

        private FaithSdkConfiguretionInfo  _apSDKConfiguretionInfo;
        private SerializedObject        _serializedSDKConfiguretionInfo;

        private GUIContent              _generalSettingContent;
        private GUIContent              _lionKitSettingContent;
        private GUIContent              _analyticsSettingContent;
        private GUIContent              _adNetworkSettingContent;
        private GUIContent              _abTestSettingContent;
        private GUIContent              _debuggingSettingContent;
        

        private SerializedProperty      _showGeneralSettings;
        private SerializedProperty      _showLionKitSettings;
        private SerializedProperty      _showAnalytics;
        private SerializedProperty      _showAdNetworks;
        private SerializedProperty      _showABTestSetting;
        private SerializedProperty      _showDebuggingSettings;

        private SerializedProperty      _enableAnalyticsEvents;

        private SerializedProperty      _selectedAdConfiguretion;

        private SerializedProperty      _showMaxMediationDebugger;

        private SerializedProperty      _showAPSdkLogInConsole;

        private SerializedProperty      _infoLogColor;
        private SerializedProperty      _warningLogColor;
        private SerializedProperty      _errorLogColor;



    #endregion

    #region Editor

        [MenuItem("Faith/FaithSdk Integration Manager")]
        public static void Create()
        {
            if (_reference == null)
                _reference = GetWindow<FaithSdkIntegrationManagerEditorWindow>("FaithSdk Integration Manager", typeof(FaithSdkIntegrationManagerEditorWindow));
            else
                _reference.Show();

            _reference.Focus();
        }

        private void OnEnable()
        {
            FetchAllTheReference();
            
        }

        private void OnDisable()
        {
            _IsInformationFetched = false;
        }

        private void OnFocus()
        {
            FetchAllTheReference();
        }

        private void OnLostFocus()
        {
            _IsInformationFetched = false;
        }

        private void OnGUI()
        {
            if (!_IsInformationFetched) {

                FetchAllTheReference();
                _IsInformationFetched = true;
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, false);
            {
                EditorGUILayout.Space();

                EditorGUI.indentLevel += 1;
                {
                    GeneralSettingGUI();

#if FaithSdk_LionKit
                    EditorGUILayout.Space();
                    LionKitSettingsGUI();
#endif

                    EditorGUILayout.Space();
                    AnalyticsSettingsGUI();

                    EditorGUILayout.Space();
                    AdNetworksSettingsGUI();

                    EditorGUILayout.Space();
                    ABTestSettingsGUI();

                    EditorGUILayout.Space();
                    DebuggingSettingsGUI();
                }
                EditorGUI.indentLevel -= 1;

                
            }
            EditorGUILayout.EndScrollView();

        }

    #endregion

    #region CustomGUI

        private void DrawHeaderGUI(string title, ref GUIContent gUIContent, ref GUIStyle gUIStyle, ref SerializedProperty serializedProperty) {

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                if (GUILayout.Button(gUIContent, gUIStyle, GUILayout.Width(EditorGUIUtility.currentViewWidth)))
                {
                    serializedProperty.boolValue = !serializedProperty.boolValue;
                    serializedProperty.serializedObject.ApplyModifiedProperties();

                    gUIContent = new GUIContent(
                        "[" + (!serializedProperty.boolValue ? "+" : "-") + "] " + title
                    );
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawAnalyticsGUI(FaithBaseClassForAnalyticsConfiguretion analyticsConfiguretion) {

            //Referencing Variables
            SerializedObject serailizedAnalyticsConfiguretion = new SerializedObject(analyticsConfiguretion);

            SerializedProperty _nameOfConfiguretion = serailizedAnalyticsConfiguretion.FindProperty("_nameOfConfiguretion");
            SerializedProperty _isSDKIntegrated     = serailizedAnalyticsConfiguretion.FindProperty("_isSDKIntegrated");

            SerializedProperty _showSettings = serailizedAnalyticsConfiguretion.FindProperty("_showSettings");

            SerializedProperty _enableAnalyticsEvent = serailizedAnalyticsConfiguretion.FindProperty("_enableAnalyticsEvent");

            SerializedProperty _trackProgressionEvent = serailizedAnalyticsConfiguretion.FindProperty("_trackProgressionEvent");
            SerializedProperty _trackAdEvent = serailizedAnalyticsConfiguretion.FindProperty("_trackAdEvent");

            SerializedProperty _subscribeToLionEvent = serailizedAnalyticsConfiguretion.FindProperty("_subscribeToLionEvent");
            SerializedProperty _subscribeToLionEventUA = serailizedAnalyticsConfiguretion.FindProperty("_subscribeToLionEventUA");

            //Setting Titles
            GUIContent titleContent = new GUIContent(
                    "[" + (!_showSettings.boolValue ? "+" : "-") + "] " + (_nameOfConfiguretion.stringValue + (_isSDKIntegrated.boolValue ? "" : " - (SDK Not Found)"))
                );
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
            titleStyle.alignment = TextAnchor.MiddleLeft;
            titleStyle.padding.left = 18;

            EditorGUI.BeginDisabledGroup(!_isSDKIntegrated.boolValue);
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    if (GUILayout.Button(titleContent, titleStyle, GUILayout.Width(EditorGUIUtility.currentViewWidth - 100f)))
                    {
                        _showSettings.boolValue = !_showSettings.boolValue;
                        _showSettings.serializedObject.ApplyModifiedProperties();

                        titleContent = new GUIContent(
                            "[" + (!_showSettings.boolValue ? "+" : "-") + "] " + (_nameOfConfiguretion.stringValue + (_isSDKIntegrated.boolValue ? "" : " - (SDK Not Found)"))
                        );
                    }

                    if (GUILayout.Button(_enableAnalyticsEvent.boolValue ? "Disable" : "Enable", GUILayout.Width(80)))
                    {
                        _enableAnalyticsEvent.boolValue = !_enableAnalyticsEvent.boolValue;
                        _enableAnalyticsEvent.serializedObject.ApplyModifiedProperties();
                    }

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();


                //Showing Settings
                if (_showSettings.boolValue)
                {

                    EditorGUI.BeginDisabledGroup(!_enableAnalyticsEvent.boolValue);
                    {
                        EditorGUI.indentLevel += 1;
                        {
                            analyticsConfiguretion.PreCustomEditorGUI();

                            if (analyticsConfiguretion.CanBeSubscribedToLionLogEvent())
                            {

#if FaithSdk_LionKit
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField(_subscribeToLionEvent.displayName, GUILayout.Width(LabelWidth));
                                EditorGUI.BeginChangeCheck();
                                _subscribeToLionEvent.boolValue = EditorGUILayout.Toggle(_subscribeToLionEvent.boolValue);
                                if (EditorGUI.EndChangeCheck())
                                    _subscribeToLionEvent.serializedObject.ApplyModifiedProperties();
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField(_subscribeToLionEventUA.displayName, GUILayout.Width(LabelWidth));
                                EditorGUI.BeginChangeCheck();
                                _subscribeToLionEventUA.boolValue = EditorGUILayout.Toggle(_subscribeToLionEventUA.boolValue);
                                if (EditorGUI.EndChangeCheck())
                                    _subscribeToLionEventUA.serializedObject.ApplyModifiedProperties();
                            }
                            EditorGUILayout.EndHorizontal();

#else
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField(_trackProgressionEvent.displayName, GUILayout.Width(LabelWidth));
                                    EditorGUI.BeginChangeCheck();
                                    _trackProgressionEvent.boolValue = EditorGUILayout.Toggle(_trackProgressionEvent.boolValue);
                                    if (EditorGUI.EndChangeCheck())
                                        _trackProgressionEvent.serializedObject.ApplyModifiedProperties();
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField(_trackAdEvent.displayName, GUILayout.Width(LabelWidth));
                                    EditorGUI.BeginChangeCheck();
                                    _trackAdEvent.boolValue = EditorGUILayout.Toggle(_trackAdEvent.boolValue);
                                    if (EditorGUI.EndChangeCheck())
                                        _trackAdEvent.serializedObject.ApplyModifiedProperties();
                                }
                                EditorGUILayout.EndHorizontal();
#endif
                            }
                            else
                            {

                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField(_trackProgressionEvent.displayName, GUILayout.Width(LabelWidth));
                                    EditorGUI.BeginChangeCheck();
                                    _trackProgressionEvent.boolValue = EditorGUILayout.Toggle(_trackProgressionEvent.boolValue);
                                    if (EditorGUI.EndChangeCheck())
                                        _trackProgressionEvent.serializedObject.ApplyModifiedProperties();
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField(_trackAdEvent.displayName, GUILayout.Width(LabelWidth));
                                    EditorGUI.BeginChangeCheck();
                                    _trackAdEvent.boolValue = EditorGUILayout.Toggle(_trackAdEvent.boolValue);
                                    if (EditorGUI.EndChangeCheck())
                                        _trackAdEvent.serializedObject.ApplyModifiedProperties();
                                }
                                EditorGUILayout.EndHorizontal();
                            }

                            analyticsConfiguretion.PostCustomEditorGUI();
                        }
                        EditorGUI.indentLevel -= 1;

                    }
                    EditorGUI.EndDisabledGroup();

                }

            }
            EditorGUI.EndDisabledGroup();

        }

        private void DrawAdNetworkGUI(FaithBaseClassForAdConfiguretion adConfiguretion) {

            //Referencing Variables
            SerializedObject serailizedAdConfiguretion  = new SerializedObject(adConfiguretion);

            SerializedProperty _nameOfConfiguretion = serailizedAdConfiguretion.FindProperty("_nameOfConfiguretion");
            SerializedProperty _isSDKIntegrated = serailizedAdConfiguretion.FindProperty("_isSDKIntegrated");

            SerializedProperty _showSettings            = serailizedAdConfiguretion.FindProperty("_showSettings");
            SerializedProperty _showRewardedAdSettings  = serailizedAdConfiguretion.FindProperty("_showRewardedAdSettings");
            SerializedProperty _showInterstitialAdSettings  = serailizedAdConfiguretion.FindProperty("_showInterstitialAdSettings");
            SerializedProperty _showBannerAdSettings    = serailizedAdConfiguretion.FindProperty("_showBannerAdSettings");
            SerializedProperty _showCrossPromoAdSettings = serailizedAdConfiguretion.FindProperty("_showCrossPromoAdSettings");

            SerializedProperty _enableRewardedAd   = serailizedAdConfiguretion.FindProperty("_enableRewardedAd");
            SerializedProperty _adUnitIdForRewardedAd_Android = serailizedAdConfiguretion.FindProperty("_adUnitIdForRewardedAd_Android");
            SerializedProperty _adUnitIdForRewardedAd_iOS = serailizedAdConfiguretion.FindProperty("_adUnitIdForRewardedAd_iOS");

            SerializedProperty _enableInterstitialAd    = serailizedAdConfiguretion.FindProperty("_enableInterstitialAd");
            SerializedProperty _adUnitIdForInterstitialAd_Android = serailizedAdConfiguretion.FindProperty("_adUnitIdForInterstitialAd_Android");
            SerializedProperty _adUnitIdForInterstitialAd_iOS = serailizedAdConfiguretion.FindProperty("_adUnitIdForInterstitialAd_iOS");

            SerializedProperty _enableBannerAd          = serailizedAdConfiguretion.FindProperty("_enableBannerAd");
            SerializedProperty _adUnitIdForBannerAd_Android = serailizedAdConfiguretion.FindProperty("_adUnitIdForBannerAd_Android");
            SerializedProperty _adUnitIdForBannerAd_iOS = serailizedAdConfiguretion.FindProperty("_adUnitIdForBannerAd_iOS");
            SerializedProperty _showBannerAdManually    = serailizedAdConfiguretion.FindProperty("_showBannerAdManually");

            SerializedProperty _enableCrossPromoAd      = serailizedAdConfiguretion.FindProperty("_enableCrossPromoAd");

            string displayName_AdUnitId_Android = "AdUnitID Android";
            string displayName_AdUnitId_iOS = "AdUnitID iOS";

            //Setting Titles
            GUIContent titleContent = new GUIContent(
                    "[" + (!_showSettings.boolValue ? "+" : "-") + "] " + (_nameOfConfiguretion.stringValue + (_isSDKIntegrated.boolValue ? "" : " - (SDK Not Found)"))
                );
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
            titleStyle.alignment = TextAnchor.MiddleLeft;
            titleStyle.padding.left = 18;

            EditorGUI.BeginDisabledGroup(!_isSDKIntegrated.boolValue);
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    if (GUILayout.Button(titleContent, titleStyle, GUILayout.Width(EditorGUIUtility.currentViewWidth - 100f)))
                    {
                        _showSettings.boolValue = !_showSettings.boolValue;
                        _showSettings.serializedObject.ApplyModifiedProperties();

                        titleContent = new GUIContent(
                            "[" + (!_showSettings.boolValue ? "+" : "-") + "] " + (_nameOfConfiguretion.stringValue + (_isSDKIntegrated.boolValue ? "" : " - (SDK Not Found)"))
                        );
                    }

                    if (_apSDKConfiguretionInfo.SelectedAdConfig == adConfiguretion)
                    {
                        if (GUILayout.Button("Disable", GUILayout.Width(80)))
                        {
                            _selectedAdConfiguretion.objectReferenceValue = null;
                            _selectedAdConfiguretion.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Enable", GUILayout.Width(80)))
                        {
                            _selectedAdConfiguretion.objectReferenceValue = adConfiguretion;
                            _selectedAdConfiguretion.serializedObject.ApplyModifiedProperties();
                        }
                    }

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();

                if (_showSettings.boolValue)
                {

                    EditorGUI.BeginDisabledGroup((_apSDKConfiguretionInfo.SelectedAdConfig == adConfiguretion) ? false : true);
                    {
                        EditorGUI.indentLevel += 1;
                        adConfiguretion.PreCustomEditorGUI();
                        EditorGUI.indentLevel -= 1;

                        //AdType Configuretion
                        GUIStyle adTypeStyle = new GUIStyle(EditorStyles.boldLabel);
                        adTypeStyle.alignment = TextAnchor.MiddleLeft;
                        adTypeStyle.padding.left = 36;

                        //------------------------------
    #region RewardedAd

                        EditorGUI.indentLevel += 1;
                        {
                            EditorGUILayout.BeginHorizontal(GUI.skin.box);
                            {
                                string rewardedAdLabel = "[" + (!_showRewardedAdSettings.boolValue ? "+" : "-") + "] [RewardedAd]";
                                GUIContent rewardedAdLabelContent = new GUIContent(
                                        rewardedAdLabel

                                    );

                                if (GUILayout.Button(rewardedAdLabelContent, adTypeStyle, GUILayout.Width(EditorGUIUtility.currentViewWidth)))
                                {
                                    _showRewardedAdSettings.boolValue = !_showRewardedAdSettings.boolValue;
                                    _showRewardedAdSettings.serializedObject.ApplyModifiedProperties();
                                }
                            }
                            EditorGUILayout.EndHorizontal();

                            if (_showRewardedAdSettings.boolValue)
                            {

                                EditorGUI.indentLevel += 2;
                                {
                                    if (adConfiguretion.AskForAdIds()) {

                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField(displayName_AdUnitId_Android, GUILayout.Width(LabelWidth));
                                            EditorGUI.BeginChangeCheck();
                                            _adUnitIdForRewardedAd_Android.stringValue = EditorGUILayout.TextField(_adUnitIdForRewardedAd_Android.stringValue);
                                            if (EditorGUI.EndChangeCheck())
                                                _adUnitIdForRewardedAd_Android.serializedObject.ApplyModifiedProperties();
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField(displayName_AdUnitId_iOS, GUILayout.Width(LabelWidth));
                                            EditorGUI.BeginChangeCheck();
                                            _adUnitIdForRewardedAd_iOS.stringValue = EditorGUILayout.TextField(_adUnitIdForRewardedAd_iOS.stringValue);
                                            if (EditorGUI.EndChangeCheck())
                                                _adUnitIdForRewardedAd_iOS.serializedObject.ApplyModifiedProperties();
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        FaithSdkEditorModule.DrawHorizontalLine();
                                    }

                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField(_enableRewardedAd.displayName, GUILayout.Width(LabelWidth));
                                        EditorGUI.BeginChangeCheck();
                                        _enableRewardedAd.boolValue = EditorGUILayout.Toggle(_enableRewardedAd.boolValue);
                                        if (EditorGUI.EndChangeCheck())
                                            _enableRewardedAd.serializedObject.ApplyModifiedProperties();
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUI.indentLevel -= 2;

                            }
                        }
                        EditorGUI.indentLevel -= 1;


    #endregion

                        //------------------------------
    #region InterstitialAd

                        EditorGUI.indentLevel += 1;
                        {
                            EditorGUILayout.BeginHorizontal(GUI.skin.box);
                            {
                                string interstitialAdLabel = "[" + (!_showInterstitialAdSettings.boolValue ? "+" : "-") + "] [InterstitialAd]";
                                GUIContent interstialAdLabelContent = new GUIContent(
                                        interstitialAdLabel

                                    );

                                if (GUILayout.Button(interstialAdLabelContent, adTypeStyle, GUILayout.Width(EditorGUIUtility.currentViewWidth)))
                                {
                                    _showInterstitialAdSettings.boolValue = !_showInterstitialAdSettings.boolValue;
                                    _showInterstitialAdSettings.serializedObject.ApplyModifiedProperties();
                                }
                            }
                            EditorGUILayout.EndHorizontal();

                            if (_showInterstitialAdSettings.boolValue)
                            {
                                EditorGUI.indentLevel += 2;
                                {

                                    if (adConfiguretion.AskForAdIds())
                                    {

                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField(displayName_AdUnitId_Android, GUILayout.Width(LabelWidth));
                                            EditorGUI.BeginChangeCheck();
                                            _adUnitIdForInterstitialAd_Android.stringValue = EditorGUILayout.TextField(_adUnitIdForInterstitialAd_Android.stringValue);
                                            if (EditorGUI.EndChangeCheck())
                                                _adUnitIdForInterstitialAd_Android.serializedObject.ApplyModifiedProperties();
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField(displayName_AdUnitId_iOS, GUILayout.Width(LabelWidth));
                                            EditorGUI.BeginChangeCheck();
                                            _adUnitIdForInterstitialAd_iOS.stringValue = EditorGUILayout.TextField(_adUnitIdForInterstitialAd_iOS.stringValue);
                                            if (EditorGUI.EndChangeCheck())
                                                _adUnitIdForInterstitialAd_iOS.serializedObject.ApplyModifiedProperties();
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        FaithSdkEditorModule.DrawHorizontalLine();
                                    }

                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField(_enableInterstitialAd.displayName, GUILayout.Width(LabelWidth));
                                        EditorGUI.BeginChangeCheck();
                                        _enableInterstitialAd.boolValue = EditorGUILayout.Toggle(_enableInterstitialAd.boolValue);
                                        if (EditorGUI.EndChangeCheck())
                                            _enableInterstitialAd.serializedObject.ApplyModifiedProperties();
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUI.indentLevel -= 2;

                            }
                        }
                        EditorGUI.indentLevel -= 1;


    #endregion

                        //------------------------------
    #region BannerAd

                        EditorGUI.indentLevel += 1;
                        {
                            EditorGUILayout.BeginHorizontal(GUI.skin.box);
                            {
                                string bannerAdLabel = "[" + (!_showBannerAdSettings.boolValue ? "+" : "-") + "] [BannerAd]";
                                GUIContent bannerAdLabelContent = new GUIContent(
                                        bannerAdLabel

                                    );

                                if (GUILayout.Button(bannerAdLabelContent, adTypeStyle, GUILayout.Width(EditorGUIUtility.currentViewWidth)))
                                {
                                    _showBannerAdSettings.boolValue = !_showBannerAdSettings.boolValue;
                                    _showBannerAdSettings.serializedObject.ApplyModifiedProperties();
                                }
                            }
                            EditorGUILayout.EndHorizontal();

                            if (_showBannerAdSettings.boolValue)
                            {
                                EditorGUI.indentLevel += 2;
                                {

                                    if (adConfiguretion.AskForAdIds())
                                    {

                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField(displayName_AdUnitId_Android, GUILayout.Width(LabelWidth));
                                            EditorGUI.BeginChangeCheck();
                                            _adUnitIdForBannerAd_Android.stringValue = EditorGUILayout.TextField(_adUnitIdForBannerAd_Android.stringValue);
                                            if (EditorGUI.EndChangeCheck())
                                                _adUnitIdForBannerAd_Android.serializedObject.ApplyModifiedProperties();
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField(displayName_AdUnitId_iOS, GUILayout.Width(LabelWidth));
                                            EditorGUI.BeginChangeCheck();
                                            _adUnitIdForBannerAd_iOS.stringValue = EditorGUILayout.TextField(_adUnitIdForBannerAd_iOS.stringValue);
                                            if (EditorGUI.EndChangeCheck())
                                                _adUnitIdForBannerAd_iOS.serializedObject.ApplyModifiedProperties();
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        FaithSdkEditorModule.DrawHorizontalLine();
                                    }

                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField(_enableBannerAd.displayName, GUILayout.Width(LabelWidth));
                                        EditorGUI.BeginChangeCheck();
                                        _enableBannerAd.boolValue = EditorGUILayout.Toggle(_enableBannerAd.boolValue);
                                        if (EditorGUI.EndChangeCheck())
                                            _enableBannerAd.serializedObject.ApplyModifiedProperties();
                                    }
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField(_showBannerAdManually.displayName, GUILayout.Width(LabelWidth));
                                        EditorGUI.BeginChangeCheck();
                                        _showBannerAdManually.boolValue = EditorGUILayout.Toggle(_showBannerAdManually.boolValue);
                                        if (EditorGUI.EndChangeCheck())
                                            _showBannerAdManually.serializedObject.ApplyModifiedProperties();
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUI.indentLevel -= 2;

                            }
                        }
                        EditorGUI.indentLevel -= 1;


    #endregion

                        //------------------------------
    #region CrossPromoAd

                        EditorGUI.indentLevel += 1;
                        {
                            EditorGUILayout.BeginHorizontal(GUI.skin.box);
                            {
                                string crossPromoAdLabel = "[" + (!_showCrossPromoAdSettings.boolValue ? "+" : "-") + "] [CrossPromoAd]";
                                GUIContent crossPromoAdLabelContent = new GUIContent(
                                        crossPromoAdLabel

                                    );

                                if (GUILayout.Button(crossPromoAdLabelContent, adTypeStyle, GUILayout.Width(EditorGUIUtility.currentViewWidth)))
                                {
                                    _showCrossPromoAdSettings.boolValue = !_showCrossPromoAdSettings.boolValue;
                                    _showCrossPromoAdSettings.serializedObject.ApplyModifiedProperties();
                                }
                            }
                            EditorGUILayout.EndHorizontal();

                            if (_showCrossPromoAdSettings.boolValue)
                            {

                                EditorGUI.indentLevel += 2;
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField(_enableCrossPromoAd.displayName, GUILayout.Width(LabelWidth));
                                        EditorGUI.BeginChangeCheck();
                                        _enableCrossPromoAd.boolValue = EditorGUILayout.Toggle(_enableCrossPromoAd.boolValue);
                                        if (EditorGUI.EndChangeCheck())
                                            _enableCrossPromoAd.serializedObject.ApplyModifiedProperties();
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUI.indentLevel -= 2;

                            }
                        }
                        EditorGUI.indentLevel -= 1;


    #endregion

                        EditorGUI.indentLevel += 1;
                        adConfiguretion.PostCustomEditorGUI();
                        EditorGUI.indentLevel -= 1;
                    }
                    EditorGUI.EndDisabledGroup();

                }

            }
            EditorGUI.EndDisabledGroup();
        }

        private void GeneralSettingGUI()
        {
            DrawHeaderGUI("General", ref _generalSettingContent, ref _settingsTitleStyle, ref _showGeneralSettings);

            if (_showGeneralSettings.boolValue) {

                EditorGUI.indentLevel += 1;
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Reference/Link", GUILayout.Width(LabelWidth + 30));
                        if (GUILayout.Button("Download", _hyperlinkStyle, GUILayout.Width(100))) {
                            Application.OpenURL(_linkForDownload);
                        }
                        if (GUILayout.Button("Documentation", _hyperlinkStyle, GUILayout.Width(100)))
                        {
                            Application.OpenURL(_linkForDocumetation);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                }
                EditorGUI.indentLevel -= 1;
            }
        }

        private void LionKitSettingsGUI() {

            DrawHeaderGUI("LionKit", ref _lionKitSettingContent, ref _settingsTitleStyle, ref _showLionKitSettings);

            if (_showLionKitSettings.boolValue) {

                EditorGUI.indentLevel += 1;
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(_showMaxMediationDebugger.displayName, GUILayout.Width(LabelWidth));
                        EditorGUI.BeginChangeCheck();
                        _showMaxMediationDebugger.boolValue = EditorGUILayout.Toggle(_showMaxMediationDebugger.boolValue);
                        if (EditorGUI.EndChangeCheck())
                        {
                            _showMaxMediationDebugger.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel -= 1;
            }
        }

        private void AnalyticsSettingsGUI() {

            DrawHeaderGUI("Analytics", ref _analyticsSettingContent, ref _settingsTitleStyle, ref _showAnalytics);

            if (_showAnalytics.boolValue)
            {
                EditorGUI.indentLevel += 1;
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(new GUIContent(
                            _enableAnalyticsEvents.displayName,
                            "Toggle all analytics event (ProgressionEvent/AdEvent etc) for all the sdk (Firebase, GA etc)"),
                            GUILayout.Width(LabelWidth));

                        EditorGUI.BeginChangeCheck();
                        _enableAnalyticsEvents.boolValue = EditorGUILayout.Toggle(_enableAnalyticsEvents.boolValue);
                        if (EditorGUI.EndChangeCheck())
                            _enableAnalyticsEvents.serializedObject.ApplyModifiedProperties();

                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel -= 1;

                FaithSdkEditorModule.DrawHorizontalLine();

                foreach (FaithBaseClassForAnalyticsConfiguretion analyticsConfiguretion in _listOfAnalyticsConfiguretion)
                {
                    if (analyticsConfiguretion != null)
                        DrawAnalyticsGUI(analyticsConfiguretion);
                }
            }
        }

        private void AdNetworksSettingsGUI() {

            DrawHeaderGUI("AdNetworks", ref _adNetworkSettingContent, ref _settingsTitleStyle, ref _showAdNetworks);

            if (_showAdNetworks.boolValue) {

                foreach (FaithBaseClassForAdConfiguretion analyticsConfiguretion in _listOfAdConfiguretion)
                {
                    if (analyticsConfiguretion != null)
                        DrawAdNetworkGUI(analyticsConfiguretion);
                }
            }
        }

        private void ABTestSettingsGUI() {

            DrawHeaderGUI("A/B Test", ref _abTestSettingContent, ref _settingsTitleStyle, ref _showABTestSetting);

            if (_showABTestSetting.boolValue) {

                EditorGUI.indentLevel += 1;
                {
                    EditorGUILayout.HelpBox("The following section is under development!", MessageType.Info);
                }
                EditorGUI.indentLevel -= 1;
            }

            
        }

        private void DebuggingSettingsGUI() {

            DrawHeaderGUI("Debugging", ref _debuggingSettingContent, ref _settingsTitleStyle, ref _showDebuggingSettings);

            if (_showDebuggingSettings.boolValue)
            {
                EditorGUI.indentLevel += 1;

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(_showAPSdkLogInConsole.displayName, GUILayout.Width(LabelWidth));
                        EditorGUI.BeginChangeCheck();
                        _showAPSdkLogInConsole.boolValue = EditorGUILayout.Toggle(_showAPSdkLogInConsole.boolValue);
                        if (EditorGUI.EndChangeCheck())
                        {
                            _showAPSdkLogInConsole.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(_infoLogColor);
                        if (EditorGUI.EndChangeCheck())
                        {

                            _infoLogColor.serializedObject.ApplyModifiedProperties();
                        }

                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(_warningLogColor);
                        if (EditorGUI.EndChangeCheck())
                        {
                            _warningLogColor.serializedObject.ApplyModifiedProperties();
                        }

                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(_errorLogColor);
                        if (EditorGUI.EndChangeCheck())
                        {
                            _errorLogColor.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUI.indentLevel -= 1;
            }
        }

    #endregion


    #region Configuretion

        private void FetchAllTheReference() {

    #region APSdkConfiguretionInfo

            _apSDKConfiguretionInfo = Resources.Load<FaithSdkConfiguretionInfo>("APSdkConfiguretionInfo");
            _serializedSDKConfiguretionInfo = new SerializedObject(_apSDKConfiguretionInfo);

            
            _showGeneralSettings = _serializedSDKConfiguretionInfo.FindProperty("_showGeneralSetting");
            _showLionKitSettings = _serializedSDKConfiguretionInfo.FindProperty("_showLionKitSettings");
            _showAnalytics = _serializedSDKConfiguretionInfo.FindProperty("_showAnalytics");
            _showAdNetworks = _serializedSDKConfiguretionInfo.FindProperty("_showAdNetworks");
            _showABTestSetting = _serializedSDKConfiguretionInfo.FindProperty("_showABTestSetting");
            _showDebuggingSettings = _serializedSDKConfiguretionInfo.FindProperty("_showDebuggingSetting");

            _enableAnalyticsEvents = _serializedSDKConfiguretionInfo.FindProperty("_enableAnalyticsEvents");

            _selectedAdConfiguretion = _serializedSDKConfiguretionInfo.FindProperty("_selectedAdConfiguretion");

            _showMaxMediationDebugger = _serializedSDKConfiguretionInfo.FindProperty("_showMaxMediationDebugger");

            _showAPSdkLogInConsole = _serializedSDKConfiguretionInfo.FindProperty("_showAPSdkLogInConsole");

            _infoLogColor = _serializedSDKConfiguretionInfo.FindProperty("_infoLogColor");
            _warningLogColor = _serializedSDKConfiguretionInfo.FindProperty("_warningLogColor");
            _errorLogColor = _serializedSDKConfiguretionInfo.FindProperty("_errorLogColor");

            _generalSettingContent = new GUIContent(
                        "[" + (!_showGeneralSettings.boolValue ? "+" : "-") + "] General"
                    );

            _lionKitSettingContent = new GUIContent(
                        "[" + (!_showLionKitSettings.boolValue ? "+" : "-") + "] LionKit"
                    );

            _analyticsSettingContent = new GUIContent(
                        "[" + (!_showAnalytics.boolValue ? "+" : "-") + "] " + "Analytics"
                    );

            _adNetworkSettingContent = new GUIContent(
                        "[" + (!_showAdNetworks.boolValue ? "+" : "-") + "] " + "AdNetwork"
                    );

            _abTestSettingContent = new GUIContent(
                        "[" + (!_showABTestSetting.boolValue ? "+" : "-") + "] A/B Test"
                    );

            _debuggingSettingContent = new GUIContent(
                        "[" + (!_showDebuggingSettings.boolValue ? "+" : "-") + "] Debugging"
                    );

            _settingsTitleStyle = new GUIStyle(EditorStyles.boldLabel);
            _settingsTitleStyle.alignment = TextAnchor.MiddleLeft;

            _hyperlinkStyle = new GUIStyle(EditorStyles.boldLabel);
            _hyperlinkStyle.normal.textColor = new Color(50 / 255.0f, 139 / 255.0f, 217 / 255.0f);
            _hyperlinkStyle.wordWrap = true;
            _hyperlinkStyle.richText = true;

    #endregion

            //-------------

            _listOfAnalyticsConfiguretion = new List<FaithBaseClassForAnalyticsConfiguretion>();

            Object[] analyticsConfiguretionObjects = Resources.LoadAll("", typeof(FaithBaseClassForAnalyticsConfiguretion));
            foreach (Object analyticsConfiguretionObject in analyticsConfiguretionObjects)
            {
                FaithBaseClassForAnalyticsConfiguretion analyticsConfiguretion = (FaithBaseClassForAnalyticsConfiguretion)analyticsConfiguretionObject;
                if (analyticsConfiguretion != null)
                    _listOfAnalyticsConfiguretion.Add(analyticsConfiguretion);
            }

            _listOfAdConfiguretion = new List<FaithBaseClassForAdConfiguretion>();

            Object[] adNetworkConfiguretionObjects = Resources.LoadAll("", typeof(FaithBaseClassForAdConfiguretion));
            foreach (Object adNetoworkConfiguretionObject in adNetworkConfiguretionObjects)
            {

                FaithBaseClassForAdConfiguretion adNetworkConfiguretion = (FaithBaseClassForAdConfiguretion)adNetoworkConfiguretionObject;
                if (adNetworkConfiguretion != null)
                    _listOfAdConfiguretion.Add(adNetworkConfiguretion);
            }
            //-------------

            FaithSdkAssetPostProcessor.LookForSDK();

        }

    #endregion

    }


#endif


}

