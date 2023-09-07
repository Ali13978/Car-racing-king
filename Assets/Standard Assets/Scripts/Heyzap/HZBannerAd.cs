using System;
using System.Collections;
using UnityEngine;

namespace Heyzap
{
	public class HZBannerAd : MonoBehaviour
	{
		public delegate void AdDisplayListener(string state, string tag);

		private static AdDisplayListener adDisplayListener;

		private static HZBannerAd _instance;

		[Obsolete("This constant has been relocated to HZBannerShowOptions")]
		public const string POSITION_TOP = "top";

		[Obsolete("This constant has been relocated to HZBannerShowOptions")]
		public const string POSITION_BOTTOM = "bottom";

		public static void ShowWithOptions(HZBannerShowOptions showOptions)
		{
			if (showOptions == null)
			{
				showOptions = new HZBannerShowOptions();
			}
			HZBannerAdAndroid.ShowWithOptions(showOptions);
		}

		public static bool GetCurrentBannerDimensions(out Rect banner)
		{
			return HZBannerAdAndroid.GetCurrentBannerDimensions(out banner);
		}

		public static void Hide()
		{
			HZBannerAdAndroid.Hide();
		}

		public static void Destroy()
		{
			HZBannerAdAndroid.Destroy();
		}

		public static void SetDisplayListener(AdDisplayListener listener)
		{
			adDisplayListener = listener;
		}

		public static void InitReceiver()
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("HZBannerAd");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				_instance = gameObject.AddComponent<HZBannerAd>();
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
