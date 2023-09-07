using UnityEngine;

namespace Heyzap
{
	public class HZIncentivizedAdAndroid : MonoBehaviour
	{
		public static void ShowWithOptions(HZIncentivizedShowOptions showOptions)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("showIncentivized", showOptions.Tag, showOptions.IncentivizedInfo);
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
					androidJavaClass.CallStatic("fetchIncentivized", tag);
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
				return androidJavaClass.CallStatic<bool>("isIncentivizedAvailable", new object[1]
				{
					tag
				});
			}
		}
	}
}
