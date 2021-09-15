namespace com.faith.sdk
{
    using UnityEngine;
    using UnityEngine.Events;

    //[CreateAssetMenu(fileName = "APLionKitAdNetworkConfiguretion", menuName = "APLionKitAdNetworkConfiguretion")]
    public class FaithLionKitAdNetworkConfiguretion : FaithBaseClassForAdConfiguretion
    {
        
        public override void Initialize(FaithSdkConfiguretionInfo apSdkConfiguretionInfo, bool isATTEnable = false)
        {
#if FaithSdk_LionKit
            FaithLionKitAdNetwork.Initialize(this);
#endif
        }

        public override bool IsBannerAdReady()
        {
#if FaithSdk_LionKit
            return FaithLionKitAdNetwork.BannerAd.IsBannerAdReady();
#else
            return false;
#endif
        }

        public override void ShowBannerAd(string adPlacement = "banner", int playerLevel = 0)
        {
#if FaithSdk_LionKit
            FaithLionKitAdNetwork.BannerAd.ShowBannerAd(adPlacement, playerLevel);
#endif
        }

        public override void HideBannerAd()
        {
#if FaithSdk_LionKit
            FaithLionKitAdNetwork.BannerAd.HideBannerAd();
#endif
        }

        public override bool IsInterstitialAdReady()
        {
#if FaithSdk_LionKit
            return FaithLionKitAdNetwork.InterstitialAd.IsInterstitialAdReady();
#else
            return false;
#endif
        }

        public override void ShowInterstitialAd(string adPlacement = "interstitial", UnityAction OnAdFailed = null, UnityAction OnAdClosed = null)
        {
#if FaithSdk_LionKit
            FaithLionKitAdNetwork.InterstitialAd.ShowInterstitialAd(adPlacement, OnAdFailed, OnAdClosed);
#endif
        }

        public override bool IsRewardedAdReady()
        {
#if FaithSdk_LionKit
            return FaithLionKitAdNetwork.RewardedAd.IsRewardedAdReady();
#else
            return false;
#endif
        }

        public override void ShowRewardedAd(string adPlacement, UnityAction<bool> OnAdClosed, UnityAction OnAdFailed = null)
        {
#if FaithSdk_LionKit
            FaithLionKitAdNetwork.RewardedAd.ShowRewardedAd(adPlacement, OnAdClosed, OnAdFailed);
#endif
        }

        public override void SetNameAndIntegrationStatus()
        {
            string sdkName = FaithSdkConstant.FaithSdk_LionKit;
            SetNameOfConfiguretion(sdkName, "AdNetwork");
#if UNITY_EDITOR
            _isSDKIntegrated = FaithSdkScriptDefiniedSymbol.CheckLionKitIntegration(sdkName);
#endif
        }

        public override bool IsCrossPromoAdReady()
        {
            throw new System.NotImplementedException();
        }

        public override void ShowCrossPromoAd(string adPlacement = "crossPromo")
        {
            throw new System.NotImplementedException();
        }

        public override void HideCrossPromoAd()
        {
            throw new System.NotImplementedException();
        }

        public override bool AskForAdIds()
        {
            return false;
        }

        public override void PreCustomEditorGUI()
        {
            
        }

        public override void PostCustomEditorGUI()
        {
            
        }
    }
}

