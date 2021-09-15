namespace com.faith.sdk
{
    using UnityEngine;
    using UnityEngine.Events;


    //[CreateAssetMenu(fileName = "APMaxAdNetworkConfiguretion", menuName = "APMaxAdNetworkConfiguretion")]
    public class FaithMaxAdNetworkConfiguretion : FaithBaseClassForAdConfiguretion
    {
        #region Override Method

        public override bool AskForAdIds()
        {
            return true;
        }

        public override void HideBannerAd()
        {
#if FaithSdk_MaxAdNetwork
            APMaxAdNetwork.BannerAd.HideBannerAd();
#endif
        }

        public override void HideCrossPromoAd()
        {
            throw new System.NotImplementedException();
        }

        public override void Initialize(FaithSdkConfiguretionInfo apSdkConfiguretionInfo, bool isATTEnable = false)
        {
#if FaithSdk_MaxAdNetwork
            APMaxAdNetwork.Initialize(this);
#endif
        }

        public override bool IsBannerAdReady()
        {
#if FaithSdk_MaxAdNetwork
            return APMaxAdNetwork.BannerAd.IsBannerAdReady();
#else
            return false;
#endif
        }

        public override bool IsCrossPromoAdReady()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsInterstitialAdReady()
        {
#if FaithSdk_MaxAdNetwork
            return APMaxAdNetwork.InterstitialAd.IsInterstitialAdReady();
#else
            return false;
#endif
        }

        public override bool IsRewardedAdReady()
        {
#if FaithSdk_MaxAdNetwork
            return APMaxAdNetwork.RewardedAd.IsRewardedAdReady();
#else
            return false;
#endif
        }

        public override void PostCustomEditorGUI()
        {
            
        }

        public override void PreCustomEditorGUI()
        {

        }

        public override void SetNameAndIntegrationStatus()
        {
            string sdkName = FaithSdkConstant.NameOfSDK + "_MaxAdNetwork";
            //(!APSdkScriptDefiniedSymbol.CheckLionKitIntegration(APSdkConstant.NameOfSDK + "LionKit") && APSdkScriptDefiniedSymbol.CheckMaxAdNetworkIntegrated(sdkName)) ? true : false
            SetNameOfConfiguretion(sdkName);
#if UNITY_EDITOR
            _isSDKIntegrated = FaithSdkScriptDefiniedSymbol.CheckMaxAdNetworkIntegrated(sdkName);
#endif
        }

        public override void ShowBannerAd(string adPlacement = "banner", int playerLevel = 0)
        {
#if FaithSdk_MaxAdNetwork
            APMaxAdNetwork.BannerAd.ShowBannerAd(adPlacement, playerLevel);
#endif
        }

        public override void ShowCrossPromoAd(string adPlacement = "crossPromo")
        {
            throw new System.NotImplementedException();
        }

        public override void ShowInterstitialAd(string adPlacement = "interstitial", UnityAction OnAdFailed = null, UnityAction OnAdClosed = null)
        {
#if FaithSdk_MaxAdNetwork
            APMaxAdNetwork.InterstitialAd.ShowInterstitialAd(adPlacement, OnAdFailed, OnAdClosed);
#endif
        }

        public override void ShowRewardedAd(string adPlacement, UnityAction<bool> OnAdClosed, UnityAction OnAdFailed = null)
        {
#if FaithSdk_MaxAdNetwork
            APMaxAdNetwork.RewardedAd.ShowRewardedAd(adPlacement, OnAdClosed, OnAdFailed);
#endif
        }

        #endregion
    }
}

