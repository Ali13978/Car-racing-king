using UnityEngine;

namespace Heyzap
{
	public class HZBannerAdAndroid : MonoBehaviour
	{
		public static bool GetCurrentBannerDimensions(out Rect banner)
		{
			banner = new Rect(0f, 0f, 0f, 0f);
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				string text = androidJavaClass.CallStatic<string>("getBannerDimensions", new object[0]);
				if (text == null || text.Length == 0)
				{
					return false;
				}
				string[] array = text.Split(' ');
				if (array.Length != 4)
				{
					return false;
				}
				banner = new Rect(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]), int.Parse(array[3]));
				return true;
			}
		}

		public static void ShowWithOptions(HZBannerShowOptions showOptions)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("showBanner", showOptions.Tag, showOptions.Position);
				}
			}
		}

		public static void Hide()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("hideBanner");
				}
			}
		}

		public static void Destroy()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("destroyBanner");
				}
			}
		}
	}
}
