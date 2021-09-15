namespace com.faith.sdk
{
#if APSdk_MaxAdNetwork

    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Events;

    public class FaithRewardedAdOnMaxAdNetwork : FaithBaseClassForRewardedAdForAdNetwork
    {
    #region Private Variables

        private int _retryAttempt;

    #endregion

    #region Public Callback

        public FaithRewardedAdOnMaxAdNetwork(FaithBaseClassForAdConfiguretion adConfiguretion) {

            _adConfiguretion = adConfiguretion;

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            LoadAd();
        }

    #endregion

    #region Configuretion

        private async void LoadAd(float delayInSeconds = 0) {

            await Task.Delay((int) delayInSeconds * 1000);

            MaxSdk.LoadRewardedAd(_adConfiguretion.AdUnitId_RewardedAd);
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

            // Reset retry attempt
            _retryAttempt = 0;
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load 
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

            _retryAttempt++;
            float retryDelay = Mathf.Pow(2, Mathf.Min(6, _retryAttempt));

            LoadAd(retryDelay);
            _OnAdFailed?.Invoke();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {

            APSdkLogger.Log("Displayed Rewarded Ad");

            _isEligibleForReward = false;
            IsAdRunning = true;

            
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {

            APSdkLogger.LogError("Failed To Display Rewarded Ad");

            // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
            IsAdRunning = false;

            LoadAd();
            _OnAdFailed?.Invoke();
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {

            APSdkLogger.Log("Clicked Rewarded Ad");
        }

        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {

            APSdkLogger.Log("Closed Rewarded Ad");

            // Rewarded ad is hidden. Pre-load the next ad
            IsAdRunning = false;
            

            LoadAd();
            _OnAdClosed?.Invoke(_isEligibleForReward);
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {

            APSdkLogger.Log("Received Reward");

            // The rewarded ad displayed and the user should receive the reward.
            _isEligibleForReward = true;
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Ad revenue paid. Use this callback to track user revenue.
        }

    #endregion

    #region Override Method

        public override bool IsRewardedAdReady()
        {
            return MaxSdk.IsRewardedAdReady(_adConfiguretion.AdUnitId_RewardedAd);
        }

        public override void ShowRewardedAd(string adPlacement, UnityAction<bool> OnAdClosed, UnityAction OnAdFailed = null)
        {
            if (_adConfiguretion.IsRewardedAdEnabled)
            {
                _adPlacement = string.IsNullOrEmpty(adPlacement) ? "rewarded_video" : adPlacement;
                _OnAdClosed = OnAdClosed;
                _OnAdFailed = OnAdFailed;

                MaxSdk.ShowRewardedAd(_adConfiguretion.AdUnitId_RewardedAd);
            }
            else
            {
                APSdkLogger.LogError(string.Format("RewardedAd is set to disabled in APSDKIntegrationManager. Please set the flag to 'true' to see RewardedAd"));
            }
        }

    #endregion
    }
#endif
}




