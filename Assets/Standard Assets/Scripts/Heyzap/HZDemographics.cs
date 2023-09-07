using System;
using UnityEngine;

namespace Heyzap
{
	public class HZDemographics : MonoBehaviour
	{
		public enum Gender
		{
			UNKNOWN,
			MALE,
			FEMALE,
			OTHER
		}

		public enum MaritalStatus
		{
			UNKNOWN,
			SINGLE,
			MARRIED
		}

		public enum EducationLevel
		{
			UNKNOWN,
			GRADE_SCHOOL,
			HIGH_SCHOOL_UNFINISHED,
			HIGH_SCHOOL_FINISEHD,
			COLLEGE_UNFINISHED,
			ASSOCIATE_DEGREE,
			BACHELORS_DEGREE,
			GRADUATE_DEGREE,
			POSTGRADUATE_DEGREE
		}

		private static HZDemographics _instance;

		public static void SetUserGender(Gender gender)
		{
			if (Enum.IsDefined(typeof(Gender), gender))
			{
				HeyzapDemographicsAndroid.SetUserGender(gender.ToString());
			}
		}

		public static void SetUserLocation(float latitude, float longitude, float horizontalAccuracy, float verticalAccuracy, float altitude, double timestamp)
		{
			HeyzapDemographicsAndroid.SetUserLocation(latitude, longitude, horizontalAccuracy, verticalAccuracy, altitude, timestamp);
		}

		public static void SetUserPostalCode(string postalCode)
		{
			HeyzapDemographicsAndroid.SetUserPostalCode(postalCode);
		}

		public static void SetUserHouseholdIncome(int householdIncome)
		{
			HeyzapDemographicsAndroid.SetUserHouseholdIncome(householdIncome);
		}

		public static void SetUserMaritalStatus(MaritalStatus maritalStatus)
		{
			if (Enum.IsDefined(typeof(MaritalStatus), maritalStatus))
			{
				HeyzapDemographicsAndroid.SetUserMaritalStatus(maritalStatus.ToString());
			}
		}

		public static void SetUserEducationLevel(EducationLevel educationLevel)
		{
			if (Enum.IsDefined(typeof(EducationLevel), educationLevel))
			{
				HeyzapDemographicsAndroid.SetUserEducationLevel(educationLevel.ToString());
			}
		}

		public static void SetUserBirthDate(string yyyyMMdd_date)
		{
			HeyzapDemographicsAndroid.SetUserBirthDate(yyyyMMdd_date);
		}

		public static void InitReceiver()
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("HZDemographics");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				_instance = gameObject.AddComponent<HZDemographics>();
			}
		}
	}
}
