using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    private BannerView bannerAd;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    bool isRewarded = false;

    public static AdsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });
        this.RequestBanner();
        this.RequestRewarded();

        this.rewardedAd.OnUserEarnedReward += this.HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += this.HandleRewardedAdClosed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRewarded)
        {
            isRewarded = false;
            //FindObjectOfType<CharacterSelector>().UnlockRandom();
        }
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-3940256099942544/3419835294";
        this.bannerAd = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        this.bannerAd.LoadAd(this.CreateAdRequest());
    }

    public void RequestInterstitial()
    {
        string adUnitId = "ca-app-pub-3940256099942544/3419835294";
        if (this.interstitialAd != null)
            this.interstitialAd.Destroy();

        this.interstitialAd = new InterstitialAd(adUnitId);

        this.interstitialAd.LoadAd(this.CreateAdRequest());
    }

    public void ShowInterstatial()
    {
        if (this.interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstatial ad is not ready yet.");
        }
    }

    public void RequestRewarded()
    {
        string adUnitId = "ca-app-pub-3940256099942544/3419835294";
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewarded()
    {
        if (this.rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
        }
    }

    #region Rewarded callback handlers

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        isRewarded = true;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.RequestRewarded();
    }

    #endregion
}
