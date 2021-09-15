# APSdk Integration Manager
![](https://github.com/ap-tashfiq/com.alphapotato.sdk/blob/main/Assets/_GitHubResources/ss_allSettings.png)

[Introductory Video] : <https://www.youtube.com/watch?v=CoxcUw8iCsM>




## Downloading "ApSdk" package.

Please go to the release section on this repository and download the latest version of "APSdk". Once you download the ".unityPackage", import it to your unity project. Once the project recompilation is complete, you will be able to see the following windows as the given screenshot below (AP -> APSdk Integration Manager)

![](https://github.com/ap-tashfiq/com.alphapotato.sdk/blob/main/Assets/_GitHubResources/ss_apSdkIntegrationManager.png)






## Understanding the basic interface.

APSdk Integration manager comes with the following section

- General
- LionKit (If you have integrated LionKit in your project)
- Analytics
- AdNetworks
- A/B Testing (Under development)
- Debugging

![](https://github.com/ap-tashfiq/com.alphapotato.sdk/blob/main/Assets/_GitHubResources/ss_allSettings.png)




## General
- Download : Will redirect you the following repository
- Documentation : Will redirect you to the README.md file.
![](https://github.com/ap-tashfiq/com.alphapotato.sdk/blob/main/Assets/_GitHubResources/ss_general.png)




## LionKit
- ShowMax Mediation Deugger : Will turn on the max mediation debugger in order to check the LionKit, Apploving & Other AdNetworks integration status.

![](https://github.com/ap-tashfiq/com.alphapotato.sdk/blob/main/Assets/_GitHubResources/ss_lionkit.png)



## Analytics

- Enable Analytics Event : Toggling the logs for all analytics (Facebook, Adjust etc....) events for integrated analyitcs in the following project.

> Each Analytics will have their own section to confiure it settings.
> The tabs would be grayed out if you haven't imported the following SDK with the status message of 'SDK - Not Found'.
> Once the SDK has been imported, you will be able to interact with the section.
> In order to iniatize the SDK and work properly, make sure to "Enable" the imported SDK.
> Analytics SDK can be used both with & without "LionKit".
> With LionKit integrated, and your analytics has generic way to feed the logs, then you will see the options "SubscribeLionEvent" & "SubscribeToLionEventUA" (UA means the events for Ads, IAP etc). 
> If LionKit is not integrated or there are no generic ways to feed the data for analytics (GameAnalytics etc...), then you will see the option "TrackProgressionEvent" (For LevelStarted, LevelComplete & LevelFailed) and "TrackAdEvent" (For RewardedAd, InterstitialAd & BannerAd)

- APIs
```sh
using APSdk;
public static class AnalyticsCall
{
    public static void LogLevelStarted(int levelIndex = 0) {

        APAnalytics.LevelStarted(levelIndex);
    }

    public static void LogLevelComplete(int levelIndex = 0)
    {
        APAnalytics.LevelComplete(levelIndex);
    }

    public static void LogLevelFailed(int levelIndex = 0)
    {
        APAnalytics.LevelFailed(levelIndex);
    }
}
```

![](https://github.com/ap-tashfiq/com.alphapotato.sdk/blob/main/Assets/_GitHubResources//ss_analytics.png)






## AdNetworks

> Even if you added multiple "Ad Mediation Network" like AppLovin, MaxSDK or GoogleAdSense to your project, you have to choose your desired "AdNetwork" by clicking on the "Enable" button on the right side of your desired ad network.
> Remember, if you haven't choosed any, no ads will be displayed.

- APIs
```sh
using APSdk;
using UnityEngine.Events;
public static class AdNetworkCall
{
    //Ad    :   RewardedAd
    public static bool IsRewardedAdReady()
    {
        return APRewardedAd.IsAdReady();
    }

    /// <param name="adPlacement">In 'adPlacement', you need to pass the info, for which the Ad has been shown. If it was for revive, then maybe you can pass "rewadedAd_revive"</param>
    /// <param name="OnAdClosed">It will pass "true" if the user is eligible for reward, else it will pass "false" </param>
    /// <param name="OnAdFailed">[Optional] if somehow, it shown was failed (Network error, ads not ready etc....)</param>
    public static void ShowRewardedAd(string adPlacement, UnityAction<bool> OnAdClosed, UnityAction OnAdFailed = null)
    {
        APRewardedAd.Show(adPlacement, OnAdClosed, OnAdFailed);
    }


    //----------
    //Ad    :   InterstitialAd
    public static bool IsInterstitialAdReady()
    {
        return APInterstitialAd.IsAdReady();
    }

    /// <param name="adPlacement">[Optional] For 'adPlacement', you can pass the information where the Ad's been shown. If it was shown after the level failed, you can pass 'interstitialAd_levelFailed'</param>
    /// <param name="OnAdClosed">[Optional] When user press the close button</param>
    /// <param name="OnAdFailed">[Optional] if somehow, it shown was failed (Network error, ads not ready etc....)</param>
    public static void ShowInterstitialAd(string adPlacement = "InterstitialAd", UnityAction OnAdClosed = null, UnityAction OnAdFailed = null)
    {
        APInterstitialAd.Show(adPlacement, OnAdClosed, OnAdFailed);
    }


    //----------
    //Ad    :   BannerAd
    public static bool IsBannerAdReady()
    {
        return APBannerAd.IsAdReady();
    }

    
    /// <param name="adPlacement"></param>
    /// <param name="playerLevel"></param>
    public static void ShowBannerAd(string adPlacement = "bannerAd", int playerLevel = 0)
    {
        APBannerAd.Show();
    }

    public static void HideBannerAd()
    {
        APBannerAd.Hide();
    }
}
```




## A/B Testing (Under Development)





## Debugging

- You will be able to "Toggle" the APSdk log by taping "Show AP Sdk Log In Console".
- You will be able to change the colors of the log on the following section as well.


