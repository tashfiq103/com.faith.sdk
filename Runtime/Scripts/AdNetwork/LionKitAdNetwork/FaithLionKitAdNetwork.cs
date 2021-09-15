namespace com.faith.sdk
{
#if FaithSdk_LionKit

    public static class FaithLionKitAdNetwork
    {
    #region Public Variables

        public static FaithRewardedAdOnLionKitAdNetwork RewardedAd { get; private set; }
        public static FaithInterstitialAdOnLionKitAdNetwork InterstitialAd { get; private set; }
        public static FaithBannerAdOnLionKitAdNetwork BannerAd { get; private set; }

    #endregion

    #region Public Variables

        public static void Initialize(FaithBaseClassForAdConfiguretion adConfiguretion) {

            RewardedAd = new FaithRewardedAdOnLionKitAdNetwork(adConfiguretion);
            InterstitialAd = new FaithInterstitialAdOnLionKitAdNetwork(adConfiguretion);
            BannerAd = new FaithBannerAdOnLionKitAdNetwork(adConfiguretion);
        }

    #endregion


    }

#endif
}

