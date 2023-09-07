using UnityEngine;

namespace Heyzap
{
	public class HZVideoAdAndroid : MonoBehaviour
	{
		public static void ShowWithOptions(HZShowOptions showOptions)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("showVideo", showOptions.Tag);
				}
			}
		}

		public static void Fetch(string tag)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("fetchVideo", tag);
				}
			}
		}

		public static bool IsAvailable(string tag)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return false;
			}
			AndroidJNIHelper.debug = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
			{
				return androidJavaClass.CallStatic<bool>("isVideoAvailable", new object[1]
				{
					tag
				});
			}
		}
	}
}
