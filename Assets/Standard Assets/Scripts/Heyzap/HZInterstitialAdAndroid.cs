using UnityEngine;

namespace Heyzap
{
	public class HZInterstitialAdAndroid
	{
		public static void ShowWithOptions(HZShowOptions showOptions)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("showInterstitial", showOptions.Tag);
				}
			}
		}

		public static void Fetch(string tag = "default")
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("fetchInterstitial", tag);
				}
			}
		}

		public static bool IsAvailable(string tag = "default")
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				return androidJavaClass.CallStatic<bool>("isInterstitialAvailable", new object[1]
				{
					tag
				});
			}
		}

		public static void chartboostShowForLocation(string location)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("chartboostLocationShow", location);
				}
			}
		}

		public static bool chartboostIsAvailableForLocation(string location)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				return androidJavaClass.CallStatic<bool>("chartboostLocationIsAvailable", new object[1]
				{
					location
				});
			}
		}

		public static void chartboostFetchForLocation(string location)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("chartboostLocationFetch", location);
				}
			}
		}
	}
}
