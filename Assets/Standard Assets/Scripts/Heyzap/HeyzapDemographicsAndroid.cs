using UnityEngine;

namespace Heyzap
{
	public class HeyzapDemographicsAndroid : MonoBehaviour
	{
		public static void SetUserGender(string gender)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("setUserGender", gender);
				}
			}
		}

		public static void SetUserLocation(float latitude, float longitude, float horizontalAccuracy, float verticalAccuracy, float altitude, double timestamp)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("setUserLocation", latitude, longitude, horizontalAccuracy, verticalAccuracy, altitude, timestamp);
				}
			}
		}

		public static void SetUserPostalCode(string postalCode)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("setUserPostalCode", postalCode);
				}
			}
		}

		public static void SetUserHouseholdIncome(int householdIncome)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("setUserHouseholdIncome", householdIncome);
				}
			}
		}

		public static void SetUserMaritalStatus(string maritalStatus)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("setUserMaritalStatus", maritalStatus);
				}
			}
		}

		public static void SetUserEducationLevel(string educationLevel)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("setUserEducationLevel", educationLevel);
				}
			}
		}

		public static void SetUserBirthDate(string yyyyMMdd_date)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNIHelper.debug = false;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper"))
				{
					androidJavaClass.CallStatic("setUserBirthDate", yyyyMMdd_date);
				}
			}
		}
	}
}
