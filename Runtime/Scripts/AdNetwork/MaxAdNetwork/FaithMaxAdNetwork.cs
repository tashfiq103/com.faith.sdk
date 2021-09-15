namespace com.faith.sdk
{
#if APSdk_MaxAdNetwork
    public static class APMaxAdNetwork
    {
    #region Public Variables

        public static FaithRewardedAdOnMaxAdNetwork RewardedAd { get; private set; }
        public static FaithInterstitialAdOnMaxAdNetwork InterstitialAd { get; private set; }
        public static FaithBannerAdOnMaxAdNetwork BannerAd { get; private set; }

    #endregion

    #region Public Variables

        public static void Initialize(FaithBaseClassForAdConfiguretion adConfiguretion)
        {
            RewardedAd = new APRewardedAdOnMaxAdNetwork(adConfiguretion);
            InterstitialAd = new APInterstitialAdOnMaxAdNetwork(adConfiguretion);
            BannerAd = new APBannerAdOnMaxAdNetwork(adConfiguretion);
        }

    #endregion
    }
#endif
}


