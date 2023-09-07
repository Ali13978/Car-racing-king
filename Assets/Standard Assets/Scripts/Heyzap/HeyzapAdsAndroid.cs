using UnityEngine;

namespace Heyzap
{
	public class HeyzapAdsAndroid : MonoBehaviour
	{
		public static void Start(string publisher_id, int options = 0)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("start", publisher_id, options);
				}
			}
		}

		public static bool IsNetworkInitialized(string network)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				return androidJavaClass.CallStatic<bool>("isNetworkInitialized", new object[1]
				{
					network
				});
			}
		}

		public static bool OnBackPressed()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				return androidJavaClass.CallStatic<bool>("onBackPressed", new object[0]);
			}
		}

		public static void ShowMediationTestSuite()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("showNetworkActivity");
				}
			}
		}

		public static string GetRemoteData()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return "{}";
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				return androidJavaClass.CallStatic<string>("getCustomPublisherData", new object[0]);
			}
		}

		public static void ShowDebugLogs()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("showDebugLogs");
				}
			}
		}

		public static void HideDebugLogs()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("hideDebugLogs");
				}
			}
		}

		public static void ResumeExpensiveWork()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("resumeExpensiveWork");
				}
			}
		}

		public static void PauseExpensiveWork()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("pauseExpensiveWork");
				}
			}
		}

		public static void SetBundleIdentifier(string bundleID)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.ads.HeyzapAds"))
				{
					androidJavaClass.CallStatic("setBundleId", bundleID);
				}
			}
		}
	}
}
