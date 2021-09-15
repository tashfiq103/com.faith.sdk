namespace com.faith.sdk
{

#if UNITY_EDITOR

    using UnityEngine;
    using UnityEditor;
    public class FaithSdkAssetPostProcessor : AssetPostprocessor
    {
        public static void LookForSDK() {

            FaithSdkConfiguretionInfo _apSDKConfiguretionInfo = Resources.Load<FaithSdkConfiguretionInfo>("APSdkConfiguretionInfo");
            SerializedObject _serializedSDKConfiguretionInfo = new SerializedObject(_apSDKConfiguretionInfo);

            SerializedProperty _isLionKitIntegrated = _serializedSDKConfiguretionInfo.FindProperty("_isLionKitIntegrated");

            _isLionKitIntegrated.boolValue = FaithSdkScriptDefiniedSymbol.CheckLionKitIntegration(FaithSdkConstant.APSdk_LionKit);
            _isLionKitIntegrated.serializedObject.ApplyModifiedProperties();

            Object[] analyticsConfiguretionObjects = Resources.LoadAll("", typeof(FaithBaseClassForAnalyticsConfiguretion));
            foreach (Object analyticsConfiguretionObject in analyticsConfiguretionObjects) {

                FaithBaseClassForAnalyticsConfiguretion analyticsConfiguretion = (FaithBaseClassForAnalyticsConfiguretion)analyticsConfiguretionObject;
                if (analyticsConfiguretion != null)
                    analyticsConfiguretion.SetNameAndIntegrationStatus();
            }

            Object[] adNetworkConfiguretionObjects = Resources.LoadAll("", typeof(FaithBaseClassForAdConfiguretion));
            foreach (Object adNetoworkConfiguretionObject in adNetworkConfiguretionObjects)
            {

                FaithBaseClassForAdConfiguretion adNetworkConfiguretion = (FaithBaseClassForAdConfiguretion)adNetoworkConfiguretionObject;
                if (adNetworkConfiguretion != null)
                    adNetworkConfiguretion.SetNameAndIntegrationStatus();
            }

            _serializedSDKConfiguretionInfo.ApplyModifiedProperties();
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            LookForSDK();
        }
    }
#endif


}
