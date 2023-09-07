using ChartboostSDK;
using GoogleMobileAds.Api;
using Heyzap;
using UnityEngine;
using UnityEngine.Advertisements;

public class MenuAds : MonoBehaviour
{
	public enum AdCompany
	{
		AdmobInter,
		HeyzapInter,
		HeyzapVideo,
		UnityVideo,
		ChartboostInter
	}

	[Header("AD IDS")]
	public string HeyzapPublisherID = string.Empty;

	public string AdmobInterstitial = string.Empty;

	public string AdmobBanner = string.Empty;

	public string AdmobReward = string.Empty;

	public string UnityAdsId = string.Empty;

	public bool showBanner;

	public AdCompany menuAd;

	private BannerView bannerView;

	private InterstitialAd interstitial;

	private static bool displayedOnce;

	[HideInInspector]
	private static AdsManager instance;

	private void Start()
	{
		displayedOnce = false;
		PlayerPrefs.SetString("UnityAdsId", UnityAdsId);
		PlayerPrefs.SetString("Inter", AdmobInterstitial);
		PlayerPrefs.SetString("Banner", AdmobBanner);
		PlayerPrefs.SetString("AdmobReward", AdmobReward);
		Chartboost.cacheInterstitial(CBLocation.Default);
		HeyzapAds.Start(HeyzapPublisherID, 0);
		RequestInterstitial();
		if (instance == null)
		{
			instance = base.gameObject.GetComponent<AdsManager>();
			if (UnityAdsId != null && UnityAdsId.Length >= 5 && Advertisement.isSupported)
			{
				Advertisement.Initialize(UnityAdsId);
			}
		}
		HZVideoAd.Fetch();
		HZInterstitialAd.Fetch();
		if (showBanner)
		{
			RequestBanner();
			bannerView.Show();
			showBanner = false;
		}
	}

	private void Update()
	{
		if (displayedOnce)
		{
			return;
		}
		switch (menuAd)
		{
		case AdCompany.AdmobInter:
			if (!loadAdmobInterstitial() && !ShowHeyzapInterstitial() && !ShowChartboostInterstitial() && !ShowUnityADS() && showHeyzapVideo())
			{
			}
			break;
		case AdCompany.ChartboostInter:
			if (!ShowChartboostInterstitial() && !ShowHeyzapInterstitial() && !loadAdmobInterstitial() && !ShowUnityADS() && showHeyzapVideo())
			{
			}
			break;
		case AdCompany.HeyzapInter:
			if (!ShowHeyzapInterstitial() && !loadAdmobInterstitial() && !ShowChartboostInterstitial() && !ShowUnityADS() && showHeyzapVideo())
			{
			}
			break;
		case AdCompany.HeyzapVideo:
			if (!showHeyzapVideo() && !ShowUnityADS() && !ShowHeyzapInterstitial() && !loadAdmobInterstitial() && ShowChartboostInterstitial())
			{
			}
			break;
		case AdCompany.UnityVideo:
			if (!ShowUnityADS() && !showHeyzapVideo() && !ShowHeyzapInterstitial() && !ShowChartboostInterstitial() && loadAdmobInterstitial())
			{
			}
			break;
		default:
			UnityEngine.Debug.Log("Error");
			break;
		}
	}

	private void RequestBanner()
	{
		bannerView = new BannerView(AdmobBanner, AdSize.SmartBanner, AdPosition.Top);
		AdRequest request = new AdRequest.Builder().Build();
		bannerView.LoadAd(request);
	}

	private void RequestInterstitial()
	{
		interstitial = new InterstitialAd(AdmobInterstitial);
		AdRequest request = new AdRequest.Builder().Build();
		interstitial.LoadAd(request);
	}

	private bool loadAdmobInterstitial()
	{
		if (interstitial.IsLoaded())
		{
			interstitial.Show();
			displayedOnce = true;
			return true;
		}
		return false;
	}

	public static bool ShowChartboostInterstitial()
	{
		if (Chartboost.hasInterstitial(CBLocation.Default))
		{
			Chartboost.showInterstitial(CBLocation.Default);
			Chartboost.cacheInterstitial(CBLocation.Default);
			displayedOnce = true;
			return true;
		}
		return false;
	}

	public static bool ShowHeyzapInterstitial()
	{
		if (HZInterstitialAd.IsAvailable())
		{
			HZInterstitialAd.Show();
			HZInterstitialAd.Fetch();
			displayedOnce = true;
			return true;
		}
		return false;
	}

	public static bool showHeyzapVideo()
	{
		if (HZVideoAd.IsAvailable())
		{
			HZVideoAd.Show();
			HZVideoAd.Fetch();
			displayedOnce = true;
			return true;
		}
		return false;
	}

	public bool ShowUnityADS()
	{
		if (Advertisement.IsReady())
		{
			Advertisement.Show("video", new ShowOptions
			{
				resultCallback = delegate(ShowResult result)
				{
					switch (result)
					{
					}
				}
			});
			displayedOnce = true;
			return true;
		}
		return false;
	}
}
