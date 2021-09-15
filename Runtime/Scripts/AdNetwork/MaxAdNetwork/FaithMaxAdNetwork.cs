namespace com.faith.sdk
{
#if FaithSdk_MaxAdNetwork
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
            RewardedAd = new FaithRewardedAdOnMaxAdNetwork(adConfiguretion);
            InterstitialAd = new FaithInterstitialAdOnMaxAdNetwork(adConfiguretion);
            BannerAd = new FaithBannerAdOnMaxAdNetwork(adConfiguretion);
        }

    #endregion
    }
#endif
}


