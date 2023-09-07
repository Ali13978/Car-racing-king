using ChartboostSDK;
using GoogleMobileAds.Api;
using Heyzap;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
	//[HideInInspector]
	//public enum AdCompany
	//{
	//	AdmobInter,
	//	HeyzapInter,
	//	HeyzapVideo,
	//	UnityVideo,
	//	ChartboostInter,
	//	UnityRewardVideo
	//}

	//public AdCompany pauseAd;

	//public AdCompany gameOverAd;

	//public AdCompany levelClearAd;

	//public AdCompany rewardVideoAd;

	//private BannerView bannerView;

	//private InterstitialAd interstitial;

	//private RewardBasedVideoAd admobreward;

	//private void showAds(AdCompany gamePlayAd)
	//{
	//	switch (gamePlayAd)
	//	{
	//	case AdCompany.AdmobInter:
	//		if (!loadAdmobInterstitial() && !ShowHeyzapInterstitial() && !ShowUnityADS() && !showHeyzapVideo() && ShowChartboostInterstitial())
	//		{
	//		}
	//		break;
	//	case AdCompany.ChartboostInter:
	//		if (!ShowChartboostInterstitial() && !ShowHeyzapInterstitial() && !ShowUnityADS() && !loadAdmobInterstitial() && showHeyzapVideo())
	//		{
	//		}
	//		break;
	//	case AdCompany.HeyzapInter:
	//		if (!ShowHeyzapInterstitial() && !loadAdmobInterstitial() && !ShowUnityADS() && !showHeyzapVideo() && ShowChartboostInterstitial())
	//		{
	//		}
	//		break;
	//	case AdCompany.HeyzapVideo:
	//		if (!showHeyzapVideo() && !ShowUnityADS() && !ShowHeyzapInterstitial() && !loadAdmobInterstitial() && ShowChartboostInterstitial())
	//		{
	//		}
	//		break;
	//	case AdCompany.UnityVideo:
	//		if (!ShowUnityADS() && !ShowHeyzapInterstitial() && !loadAdmobInterstitial() && !showHeyzapVideo() && ShowChartboostInterstitial())
	//		{
	//		}
	//		break;
	//	case AdCompany.UnityRewardVideo:
	//		if (!ShowUnityRewardADS() && ShowAdmobReward())
	//		{
	//		}
	//		break;
	//	default:
	//		UnityEngine.Debug.Log("Error");
	//		break;
	//	}
	//}

	//private void RequestInterstitial()
	//{
	//	interstitial = new InterstitialAd(PlayerPrefs.GetString("Inter"));
	//	AdRequest request = new AdRequest.Builder().Build();
	//	interstitial.LoadAd(request);
	//}

	//private bool loadAdmobInterstitial()
	//{
	//	return true;
	////{
	////	if (interstitial.IsLoaded())
	////	{
	////		interstitial.Show();
	////		return true;
	////	}
	////	return false;
	//}

	//private bool ShowAdmobReward()
	//{
	//	//if (admobreward.IsLoaded())
	//	//{
	//	//	admobreward.Show();
	//	//	return true;
	//	//}
	//	//return false;
	//	return true;
	//}

	//private void RequestRewardBasedVideo()
	//{
	//	string @string = PlayerPrefs.GetString("AdmobReward");
	//	admobreward = RewardBasedVideoAd.Instance;
	//	AdRequest request = new AdRequest.Builder().Build();
	//	admobreward.LoadAd(request, @string);
	//}

	//public void callGameOverAd()
	//{
	//	showAds(gameOverAd);
	//}

	//public void callPauseAd()
	//{
	//	showAds(pauseAd);
	//}

	//public void callLevelClearAd()
	//{
	//	showAds(levelClearAd);
	//}

	//public void callRewardVideoAd()
	//{
	//	showAds(rewardVideoAd);
	//}

	//private void Start()
	//{
	//	if (Advertisement.isSupported)
	//	{
	//		Advertisement.Initialize(PlayerPrefs.GetString("UnityAdsId"));
	//	}
	//	RequestInterstitial();
	//	HZVideoAd.Fetch();
	//	HZInterstitialAd.Fetch();
	//	Chartboost.cacheInterstitial(CBLocation.Default);
	//	RequestRewardBasedVideo();
	//}

	//private void Update()
	//{
	//}

	//public static bool ShowChartboostInterstitial()
	//{
	//	if (Chartboost.hasInterstitial(CBLocation.Default))
	//	{
	//		Chartboost.showInterstitial(CBLocation.Default);
	//		Chartboost.cacheInterstitial(CBLocation.Default);
	//		return true;
	//	}
	//	return false;
	//}

	//public static bool ShowHeyzapInterstitial()
	//{
	//	if (HZInterstitialAd.IsAvailable())
	//	{
	//		HZInterstitialAd.Show();
	//		HZInterstitialAd.Fetch();
	//		return true;
	//	}
	//	return false;
	//}

	//public static bool showHeyzapVideo()
	//{
	//	if (HZVideoAd.IsAvailable())
	//	{
	//		HZVideoAd.Show();
	//		HZVideoAd.Fetch();
	//		return true;
	//	}
	//	return false;
	//}

	//public bool ShowUnityADS()
	//{
	//	if (Advertisement.IsReady())
	//	{
	//		Advertisement.Show("video", new ShowOptions
	//		{
	//			resultCallback = delegate(ShowResult result)
	//			{
	//				switch (result)
	//				{
	//				}
	//			}
	//		});
	//		return true;
	//	}
	//	return false;
	//}

	//public bool ShowUnityRewardADS()
	//{
	//	if (Advertisement.IsReady())
	//	{
	//		Advertisement.Show("rewardedVideo", new ShowOptions
	//		{
	//			resultCallback = delegate(ShowResult result)
	//			{
	//				switch (result)
	//				{
	//				}
	//			}
	//		});
	//		return true;
	//	}
	//	return false;
	//}
}
