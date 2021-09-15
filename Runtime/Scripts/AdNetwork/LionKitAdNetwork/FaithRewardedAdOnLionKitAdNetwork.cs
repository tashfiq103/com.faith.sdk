namespace com.faith.sdk
{
#if FaithSdk_LionKit

    using UnityEngine.Events;
    using LionStudios.Ads;

    public class FaithRewardedAdOnLionKitAdNetwork : FaithBaseClassForRewardedAdForAdNetwork
    {

        #region Private Variables

        private ShowAdRequest               _showRewardedAdRequest;

        #endregion

        #region Configuretion

        #endregion

        #region Public Callback

        public FaithRewardedAdOnLionKitAdNetwork(FaithBaseClassForAdConfiguretion adConfiguretion) {

            _adConfiguretion = adConfiguretion;

            _showRewardedAdRequest = new ShowAdRequest();

            // Ad event callbacks
            _showRewardedAdRequest.OnDisplayed += adUnitId =>
            {
                FaithSdkLogger.Log("Displayed Rewarded Ad :: Ad Unit ID = " + adUnitId);

                _isEligibleForReward = false;
                IsAdRunning = true;


            };
            _showRewardedAdRequest.OnClicked += adUnitId =>
            {
                FaithSdkLogger.Log("Clicked Rewarded Ad :: Ad Unit ID = " + adUnitId);
            };
            _showRewardedAdRequest.OnHidden += adUnitId =>
            {
                FaithSdkLogger.Log("Closed Rewarded Ad :: Ad Unit ID = " + adUnitId);

                IsAdRunning = false;
                _OnAdClosed?.Invoke(_isEligibleForReward);
            };
            _showRewardedAdRequest.OnFailedToDisplay += (adUnitId, error) =>
            {
                FaithSdkLogger.LogError("Failed To Display Rewarded Ad :: Error = " + error + " :: Ad Unit ID = " + adUnitId);

                IsAdRunning = false;
                _OnAdFailed?.Invoke();
            };
            _showRewardedAdRequest.OnReceivedReward += (adUnitId, reward) =>
            {
                FaithSdkLogger.Log("Received Reward :: Reward = " + reward + " :: Ad Unit ID = " + adUnitId);
                _isEligibleForReward = true;


            };
        }

        public override bool IsRewardedAdReady()
        {
            return LionStudios.Ads.RewardedAd.IsAdReady;
        }

        public override void ShowRewardedAd(string adPlacement, UnityAction<bool> OnAdClosed, UnityAction OnAdFailed = null)
        {
            if (_adConfiguretion.IsRewardedAdEnabled)
            {
                _adPlacement = string.IsNullOrEmpty(adPlacement) ? "rewarded_video" : adPlacement;
                _OnAdClosed = OnAdClosed;
                _OnAdFailed = OnAdFailed;

                LionStudios.Ads.RewardedAd.Show(_showRewardedAdRequest);
            }
            else
            {
                FaithSdkLogger.LogError(string.Format("RewardedAd is set to disabled in APSDKIntegrationManager. Please set the flag to 'true' to see RewardedAd"));
            }
        }

        


    #endregion


    }

#endif


}


