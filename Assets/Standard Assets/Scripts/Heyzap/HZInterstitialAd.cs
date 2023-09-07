using System.Collections;
using UnityEngine;

namespace Heyzap
{
	public class HZInterstitialAd : MonoBehaviour
	{
		public delegate void AdDisplayListener(string state, string tag);

		private static AdDisplayListener adDisplayListener;

		private static HZInterstitialAd _instance;

		public static void Show()
		{
			ShowWithOptions(null);
		}

		public static void ShowWithOptions(HZShowOptions showOptions)
		{
			if (showOptions == null)
			{
				showOptions = new HZShowOptions();
			}
			HZInterstitialAdAndroid.ShowWithOptions(showOptions);
		}

		public static void Fetch()
		{
			Fetch(null);
		}

		public static void Fetch(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			HZInterstitialAdAndroid.Fetch(tag);
		}

		public static bool IsAvailable()
		{
			return IsAvailable(null);
		}

		public static bool IsAvailable(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			return HZInterstitialAdAndroid.IsAvailable(tag);
		}

		public static void SetDisplayListener(AdDisplayListener listener)
		{
			adDisplayListener = listener;
		}

		public static void ChartboostFetchForLocation(string location)
		{
			HZInterstitialAdAndroid.chartboostFetchForLocation(location);
		}

		public static bool ChartboostIsAvailableForLocation(string location)
		{
			return HZInterstitialAdAndroid.chartboostIsAvailableForLocation(location);
		}

		public static void ChartboostShowForLocation(string location)
		{
			HZInterstitialAdAndroid.chartboostShowForLocation(location);
		}

		public static void InitReceiver()
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("HZInterstitialAd");
				Object.DontDestroyOnLoad(gameObject);
				_instance = gameObject.AddComponent<HZInterstitialAd>();
			}
		}

		public void SetCallback(string message)
		{
			string[] array = message.Split(',');
			SetCallbackStateAndTag(array[0], array[1]);
		}

		protected static void SetCallbackStateAndTag(string state, string tag)
		{
			if (adDisplayListener != null)
			{
				adDisplayListener(state, tag);
			}
		}

		protected static IEnumerator InvokeCallbackNextFrame(string state, string tag)
		{
			yield return null;
			SetCallbackStateAndTag(state, tag);
		}
	}
}
