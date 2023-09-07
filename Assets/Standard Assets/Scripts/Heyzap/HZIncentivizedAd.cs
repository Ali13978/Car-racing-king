using System.Collections;
using UnityEngine;

namespace Heyzap
{
	public class HZIncentivizedAd : MonoBehaviour
	{
		public delegate void AdDisplayListener(string state, string tag);

		private static AdDisplayListener adDisplayListener;

		private static HZIncentivizedAd _instance;

		public static void Fetch()
		{
			Fetch(null);
		}

		public static void Fetch(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			HZIncentivizedAdAndroid.Fetch(tag);
		}

		public static void Show()
		{
			ShowWithOptions(null);
		}

		public static void ShowWithOptions(HZIncentivizedShowOptions showOptions)
		{
			if (showOptions == null)
			{
				showOptions = new HZIncentivizedShowOptions();
			}
			HZIncentivizedAdAndroid.ShowWithOptions(showOptions);
		}

		public static bool IsAvailable()
		{
			return IsAvailable(null);
		}

		public static bool IsAvailable(string tag)
		{
			tag = HeyzapAds.TagForString(tag);
			return HZIncentivizedAdAndroid.IsAvailable(tag);
		}

		public static void SetDisplayListener(AdDisplayListener listener)
		{
			adDisplayListener = listener;
		}

		public static void InitReceiver()
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("HZIncentivizedAd");
				Object.DontDestroyOnLoad(gameObject);
				_instance = gameObject.AddComponent<HZIncentivizedAd>();
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
