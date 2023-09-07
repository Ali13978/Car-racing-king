using ChartboostSDK;
using System.Collections.Generic;
using UnityEngine;

public class ChartboostExample : MonoBehaviour
{
	public GameObject inPlayIcon;

	public GameObject inPlayText;

	public Texture2D logo;

	private CBInPlay inPlayAd;

	public Vector2 scrollPosition = Vector2.zero;

	private List<string> delegateHistory;

	private bool hasInterstitial;

	private bool hasMoreApps;

	private bool hasRewardedVideo;

	private bool hasInPlay;

	private int frameCount;

	private bool ageGate;

	private bool autocache = true;

	private bool activeAgeGate;

	private bool showInterstitial = true;

	private bool showMoreApps = true;

	private bool showRewardedVideo = true;

	private int BANNER_HEIGHT = 110;

	private int REQUIRED_HEIGHT = 650;

	private int ELEMENT_WIDTH = 190;

	private Rect scrollRect;

	private Rect scrollArea;

	private Vector3 guiScale;

	private float scale;

	private Vector2 beginFinger;

	private float deltaFingerY;

	private Vector2 beginPanel;

	private Vector2 latestPanel;

	private void OnEnable()
	{
		SetupDelegates();
	}

	private void Start()
	{
		delegateHistory = new List<string>();
		Chartboost.setAutoCacheAds(autocache);
		Chartboost.setMediation(CBMediation.AdMob, "1.0");
		AddLog("Is Initialized: " + Chartboost.isInitialized());
	}

	private void SetupDelegates()
	{
		Chartboost.didInitialize += didInitialize;
		Chartboost.didFailToLoadInterstitial += didFailToLoadInterstitial;
		Chartboost.didDismissInterstitial += didDismissInterstitial;
		Chartboost.didCloseInterstitial += didCloseInterstitial;
		Chartboost.didClickInterstitial += didClickInterstitial;
		Chartboost.didCacheInterstitial += didCacheInterstitial;
		Chartboost.shouldDisplayInterstitial += shouldDisplayInterstitial;
		Chartboost.didDisplayInterstitial += didDisplayInterstitial;
		Chartboost.didFailToLoadMoreApps += didFailToLoadMoreApps;
		Chartboost.didDismissMoreApps += didDismissMoreApps;
		Chartboost.didCloseMoreApps += didCloseMoreApps;
		Chartboost.didClickMoreApps += didClickMoreApps;
		Chartboost.didCacheMoreApps += didCacheMoreApps;
		Chartboost.shouldDisplayMoreApps += shouldDisplayMoreApps;
		Chartboost.didDisplayMoreApps += didDisplayMoreApps;
		Chartboost.didFailToRecordClick += didFailToRecordClick;
		Chartboost.didFailToLoadRewardedVideo += didFailToLoadRewardedVideo;
		Chartboost.didDismissRewardedVideo += didDismissRewardedVideo;
		Chartboost.didCloseRewardedVideo += didCloseRewardedVideo;
		Chartboost.didClickRewardedVideo += didClickRewardedVideo;
		Chartboost.didCacheRewardedVideo += didCacheRewardedVideo;
		Chartboost.shouldDisplayRewardedVideo += shouldDisplayRewardedVideo;
		Chartboost.didCompleteRewardedVideo += didCompleteRewardedVideo;
		Chartboost.didDisplayRewardedVideo += didDisplayRewardedVideo;
		Chartboost.didCacheInPlay += didCacheInPlay;
		Chartboost.didFailToLoadInPlay += didFailToLoadInPlay;
		Chartboost.didPauseClickForConfirmation += didPauseClickForConfirmation;
		Chartboost.willDisplayVideo += willDisplayVideo;
	}

	private void Update()
	{
		UpdateScrolling();
		frameCount++;
		if (frameCount > 30)
		{
			hasInterstitial = Chartboost.hasInterstitial(CBLocation.Default);
			hasMoreApps = Chartboost.hasMoreApps(CBLocation.Default);
			hasRewardedVideo = Chartboost.hasRewardedVideo(CBLocation.Default);
			hasInPlay = Chartboost.hasInPlay(CBLocation.Default);
			frameCount = 0;
		}
	}

	private void UpdateScrolling()
	{
		if (UnityEngine.Input.touchCount == 1)
		{
			Touch touch = Input.touches[0];
			if (touch.phase == TouchPhase.Began)
			{
				beginFinger = touch.position;
				beginPanel = scrollPosition;
			}
			if (touch.phase == TouchPhase.Moved)
			{
				Vector2 position = touch.position;
				deltaFingerY = position.y - beginFinger.y;
				float y = beginPanel.y + deltaFingerY / scale;
				latestPanel = beginPanel;
				latestPanel.y = y;
				scrollPosition = latestPanel;
			}
		}
	}

	private void AddLog(string text)
	{
		UnityEngine.Debug.Log(text);
		delegateHistory.Insert(0, text + "\n");
		int count = delegateHistory.Count;
		if (count > 20)
		{
			delegateHistory.RemoveRange(20, count - 20);
		}
	}

	private void OnGUI()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		float a = num / 240f;
		float b = num2 / 210f;
		float num3 = Mathf.Min(6f, Mathf.Min(a, b));
		if (scale != num3)
		{
			scale = num3;
			guiScale = new Vector3(scale, scale, 1f);
		}
		GUI.matrix = Matrix4x4.Scale(guiScale);
		ELEMENT_WIDTH = (int)(num / scale) - 30;
		float num4 = REQUIRED_HEIGHT;
		if (inPlayAd != null)
		{
			num4 += 60f;
		}
		scrollRect = new Rect(0f, BANNER_HEIGHT, ELEMENT_WIDTH + 30, num2 / scale - (float)BANNER_HEIGHT);
		scrollArea = new Rect(-10f, BANNER_HEIGHT, ELEMENT_WIDTH, num4);
		LayoutHeader();
		if (activeAgeGate)
		{
			GUI.ModalWindow(1, new Rect(0f, 0f, Screen.width, Screen.height), LayoutAgeGate, "Age Gate");
			return;
		}
		scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, scrollArea);
		LayoutButtons();
		LayoutToggles();
		GUI.EndScrollView();
	}

	private void LayoutHeader()
	{
		GUILayout.Label(logo, GUILayout.Height(30f), GUILayout.Width(ELEMENT_WIDTH + 20));
		string text = string.Empty;
		foreach (string item in delegateHistory)
		{
			text += item;
		}
		GUILayout.TextArea(text, GUILayout.Height(70f), GUILayout.Width(ELEMENT_WIDTH + 20));
	}

	private void LayoutToggles()
	{
		GUILayout.Space(5f);
		GUILayout.Label("Options:");
		showInterstitial = GUILayout.Toggle(showInterstitial, "Should Display Interstitial");
		showMoreApps = GUILayout.Toggle(showMoreApps, "Should Display More Apps");
		showRewardedVideo = GUILayout.Toggle(showRewardedVideo, "Should Display Rewarded Video");
		if (GUILayout.Toggle(ageGate, "Should Pause for AgeGate") != ageGate)
		{
			ageGate = !ageGate;
			Chartboost.setShouldPauseClickForConfirmation(ageGate);
		}
		if (GUILayout.Toggle(autocache, "Auto cache ads") != autocache)
		{
			autocache = !autocache;
			Chartboost.setAutoCacheAds(autocache);
		}
	}

	private void LayoutButtons()
	{
		GUILayout.Space(5f);
		GUILayout.Label("Has Interstitial: " + hasInterstitial);
		if (GUILayout.Button("Cache Interstitial", GUILayout.Width(ELEMENT_WIDTH)))
		{
			Chartboost.cacheInterstitial(CBLocation.Default);
		}
		if (GUILayout.Button("Show Interstitial", GUILayout.Width(ELEMENT_WIDTH)))
		{
			Chartboost.showInterstitial(CBLocation.Default);
		}
		GUILayout.Space(5f);
		GUILayout.Label("Has MoreApps: " + hasMoreApps);
		if (GUILayout.Button("Cache More Apps", GUILayout.Width(ELEMENT_WIDTH)))
		{
			Chartboost.cacheMoreApps(CBLocation.Default);
		}
		if (GUILayout.Button("Show More Apps", GUILayout.Width(ELEMENT_WIDTH)))
		{
			Chartboost.showMoreApps(CBLocation.Default);
		}
		GUILayout.Space(5f);
		GUILayout.Label("Has Rewarded Video: " + hasRewardedVideo);
		if (GUILayout.Button("Cache Rewarded Video", GUILayout.Width(ELEMENT_WIDTH)))
		{
			Chartboost.cacheRewardedVideo(CBLocation.Default);
		}
		if (GUILayout.Button("Show Rewarded Video", GUILayout.Width(ELEMENT_WIDTH)))
		{
			Chartboost.showRewardedVideo(CBLocation.Default);
		}
		GUILayout.Space(5f);
		GUILayout.Label("Has InPlay: " + hasInPlay);
		if (GUILayout.Button("Cache InPlay Ad", GUILayout.Width(ELEMENT_WIDTH)))
		{
			Chartboost.cacheInPlay(CBLocation.Default);
		}
		if (GUILayout.Button("Show InPlay Ad", GUILayout.Width(ELEMENT_WIDTH)))
		{
			inPlayAd = Chartboost.getInPlay(CBLocation.Default);
			if (inPlayAd != null)
			{
				inPlayAd.show();
			}
		}
		if (inPlayAd != null)
		{
			GUILayout.Label("app: " + inPlayAd.appName);
			if (GUILayout.Button(inPlayAd.appIcon, GUILayout.Width(ELEMENT_WIDTH)))
			{
				inPlayAd.click();
			}
		}
		GUILayout.Space(5f);
		GUILayout.Label("Post install events:");
		if (GUILayout.Button("Send PIA Main Level Event", GUILayout.Width(ELEMENT_WIDTH)))
		{
			Chartboost.trackLevelInfo("Test Data", CBLevelType.HIGHEST_LEVEL_REACHED, 1, "Test Send mail level Information");
		}
		if (GUILayout.Button("Send PIA Sub Level Event", GUILayout.Width(ELEMENT_WIDTH)))
		{
			Chartboost.trackLevelInfo("Test Data", CBLevelType.HIGHEST_LEVEL_REACHED, 1, 2, "Test Send sub level Information");
		}
		if (GUILayout.Button("Track IAP", GUILayout.Width(ELEMENT_WIDTH)))
		{
			TrackIAP();
		}
	}

	private void LayoutAgeGate(int windowID)
	{
		GUILayout.Space(BANNER_HEIGHT);
		GUILayout.Label("Want to pass the age gate?");
		GUILayout.BeginHorizontal(GUILayout.Width(ELEMENT_WIDTH));
		if (GUILayout.Button("YES"))
		{
			Chartboost.didPassAgeGate(pass: true);
			activeAgeGate = false;
		}
		if (GUILayout.Button("NO"))
		{
			Chartboost.didPassAgeGate(pass: false);
			activeAgeGate = false;
		}
		GUILayout.EndHorizontal();
	}

	private void OnDisable()
	{
		Chartboost.didInitialize -= didInitialize;
		Chartboost.didFailToLoadInterstitial -= didFailToLoadInterstitial;
		Chartboost.didDismissInterstitial -= didDismissInterstitial;
		Chartboost.didCloseInterstitial -= didCloseInterstitial;
		Chartboost.didClickInterstitial -= didClickInterstitial;
		Chartboost.didCacheInterstitial -= didCacheInterstitial;
		Chartboost.shouldDisplayInterstitial -= shouldDisplayInterstitial;
		Chartboost.didDisplayInterstitial -= didDisplayInterstitial;
		Chartboost.didFailToLoadMoreApps -= didFailToLoadMoreApps;
		Chartboost.didDismissMoreApps -= didDismissMoreApps;
		Chartboost.didCloseMoreApps -= didCloseMoreApps;
		Chartboost.didClickMoreApps -= didClickMoreApps;
		Chartboost.didCacheMoreApps -= didCacheMoreApps;
		Chartboost.shouldDisplayMoreApps -= shouldDisplayMoreApps;
		Chartboost.didDisplayMoreApps -= didDisplayMoreApps;
		Chartboost.didFailToRecordClick -= didFailToRecordClick;
		Chartboost.didFailToLoadRewardedVideo -= didFailToLoadRewardedVideo;
		Chartboost.didDismissRewardedVideo -= didDismissRewardedVideo;
		Chartboost.didCloseRewardedVideo -= didCloseRewardedVideo;
		Chartboost.didClickRewardedVideo -= didClickRewardedVideo;
		Chartboost.didCacheRewardedVideo -= didCacheRewardedVideo;
		Chartboost.shouldDisplayRewardedVideo -= shouldDisplayRewardedVideo;
		Chartboost.didCompleteRewardedVideo -= didCompleteRewardedVideo;
		Chartboost.didDisplayRewardedVideo -= didDisplayRewardedVideo;
		Chartboost.didCacheInPlay -= didCacheInPlay;
		Chartboost.didFailToLoadInPlay -= didFailToLoadInPlay;
		Chartboost.didPauseClickForConfirmation -= didPauseClickForConfirmation;
		Chartboost.willDisplayVideo -= willDisplayVideo;
	}

	private void didInitialize(bool status)
	{
		AddLog($"didInitialize: {status}");
	}

	private void didFailToLoadInterstitial(CBLocation location, CBImpressionError error)
	{
		AddLog($"didFailToLoadInterstitial: {error} at location {location}");
	}

	private void didDismissInterstitial(CBLocation location)
	{
		AddLog("didDismissInterstitial: " + location);
	}

	private void didCloseInterstitial(CBLocation location)
	{
		AddLog("didCloseInterstitial: " + location);
	}

	private void didClickInterstitial(CBLocation location)
	{
		AddLog("didClickInterstitial: " + location);
	}

	private void didCacheInterstitial(CBLocation location)
	{
		AddLog("didCacheInterstitial: " + location);
	}

	private bool shouldDisplayInterstitial(CBLocation location)
	{
		AddLog("shouldDisplayInterstitial @" + location + " : " + showInterstitial);
		return showInterstitial;
	}

	private void didDisplayInterstitial(CBLocation location)
	{
		AddLog("didDisplayInterstitial: " + location);
	}

	private void didFailToLoadMoreApps(CBLocation location, CBImpressionError error)
	{
		AddLog($"didFailToLoadMoreApps: {error} at location: {location}");
	}

	private void didDismissMoreApps(CBLocation location)
	{
		AddLog($"didDismissMoreApps at location: {location}");
	}

	private void didCloseMoreApps(CBLocation location)
	{
		AddLog($"didCloseMoreApps at location: {location}");
	}

	private void didClickMoreApps(CBLocation location)
	{
		AddLog($"didClickMoreApps at location: {location}");
	}

	private void didCacheMoreApps(CBLocation location)
	{
		AddLog($"didCacheMoreApps at location: {location}");
	}

	private bool shouldDisplayMoreApps(CBLocation location)
	{
		AddLog($"shouldDisplayMoreApps at location: {location}: {showMoreApps}");
		return showMoreApps;
	}

	private void didDisplayMoreApps(CBLocation location)
	{
		AddLog("didDisplayMoreApps: " + location);
	}

	private void didFailToRecordClick(CBLocation location, CBClickError error)
	{
		AddLog($"didFailToRecordClick: {error} at location: {location}");
	}

	private void didFailToLoadRewardedVideo(CBLocation location, CBImpressionError error)
	{
		AddLog($"didFailToLoadRewardedVideo: {error} at location {location}");
	}

	private void didDismissRewardedVideo(CBLocation location)
	{
		AddLog("didDismissRewardedVideo: " + location);
	}

	private void didCloseRewardedVideo(CBLocation location)
	{
		AddLog("didCloseRewardedVideo: " + location);
	}

	private void didClickRewardedVideo(CBLocation location)
	{
		AddLog("didClickRewardedVideo: " + location);
	}

	private void didCacheRewardedVideo(CBLocation location)
	{
		AddLog("didCacheRewardedVideo: " + location);
	}

	private bool shouldDisplayRewardedVideo(CBLocation location)
	{
		AddLog("shouldDisplayRewardedVideo @" + location + " : " + showRewardedVideo);
		return showRewardedVideo;
	}

	private void didCompleteRewardedVideo(CBLocation location, int reward)
	{
		AddLog($"didCompleteRewardedVideo: reward {reward} at location {location}");
	}

	private void didDisplayRewardedVideo(CBLocation location)
	{
		AddLog("didDisplayRewardedVideo: " + location);
	}

	private void didCacheInPlay(CBLocation location)
	{
		AddLog("didCacheInPlay called: " + location);
	}

	private void didFailToLoadInPlay(CBLocation location, CBImpressionError error)
	{
		AddLog($"didFailToLoadInPlay: {error} at location: {location}");
	}

	private void didPauseClickForConfirmation()
	{
	}

	private void willDisplayVideo(CBLocation location)
	{
		AddLog("willDisplayVideo: " + location);
	}

	private void TrackIAP()
	{
		UnityEngine.Debug.Log("TrackIAP");
		Chartboost.trackInAppGooglePlayPurchaseEvent("SampleItem", "TestPurchase", "0.99", "USD", "ProductID", "PurchaseData", "PurchaseSignature");
	}
}
