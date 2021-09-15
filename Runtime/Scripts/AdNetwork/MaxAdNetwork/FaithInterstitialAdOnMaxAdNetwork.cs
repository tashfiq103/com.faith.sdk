namespace com.faith.sdk
{
#if APSdk_MaxAdNetwork

    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Events;

    public class FaithInterstitialAdOnMaxAdNetwork : FaithBaseClassForInterstitialAdForAdNetwork
    {

    #region Private Variables

        private int _retryAttempt;

    #endregion

    #region Configuretion

        private async void LoadAd(float delayInSeconds = 0)
        {

            await Task.Delay((int)delayInSeconds * 1000);

            MaxSdk.LoadInterstitial(_adConfiguretion.AdUnitId_InterstitialAd);
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

            // Reset retry attempt
            _retryAttempt = 0;
        }

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load 
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

            _retryAttempt++;
            float retryDelay = Mathf.Pow(2, Mathf.Min(6, _retryAttempt));

            LoadAd(retryDelay);

            _OnAdFailed?.Invoke();
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {

            APSdkLogger.Log("Displayed InterstitialAd");
        }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            APSdkLogger.LogError("Failed To Display InterstitialAd");

            // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
            LoadAd();

            _OnAdFailed?.Invoke();
        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {

            APSdkLogger.Log("Clicked InterstitialAd");
        }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            APSdkLogger.Log("Closed InterstitialAd");

            // Interstitial ad is hidden. Pre-load the next ad.
            LoadAd();

            _OnAdClosed?.Invoke();
        }

    #endregion

    #region Public Callback

        public FaithInterstitialAdOnMaxAdNetwork(FaithBaseClassForAdConfiguretion adConfiguretion)
        {
            _adConfiguretion = adConfiguretion;

            // Attach callback
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

            // Load the first interstitial
            LoadAd();
        }

    #endregion

    #region Override Method

        public override bool IsInterstitialAdReady()
        {
            return MaxSdk.IsInterstitialReady(_adConfiguretion.AdUnitId_InterstitialAd);
        }

        public override void ShowInterstitialAd(string adPlacement = "interstitial", UnityAction OnAdFailed = null, UnityAction OnAdClosed = null)
        {
            if (_adConfiguretion.IsInterstitialAdEnabled)
            {
                _adPlacement = string.IsNullOrEmpty(adPlacement) ? "interstitial" : adPlacement;
                _OnAdClosed = OnAdClosed;
                _OnAdFailed = OnAdFailed;

                MaxSdk.ShowInterstitial(_adConfiguretion.AdUnitId_InterstitialAd);
            }
            else
            {
                APSdkLogger.LogError(string.Format("InterstitialAd is set to disabled in APSDKIntegrationManager. Please set the flag to 'true' to see InterstitialAd"));
            }
        }

    #endregion
    }
#endif
}

