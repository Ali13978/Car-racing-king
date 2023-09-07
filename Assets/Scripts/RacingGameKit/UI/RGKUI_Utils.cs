using System;
using UnityEngine;

namespace RacingGameKit.UI
{
	public static class RGKUI_Utils
	{
		public static GameObject FindInChildren(this GameObject gameObject, string name)
		{
			if (gameObject == null)
			{
				return null;
			}
			Transform transform = gameObject.transform;
			Transform transform2 = FindChildTransform(transform, name);
			return (!(transform2 != null)) ? null : transform2.gameObject;
		}

		public static Transform FindChildTransform(Transform transform, string name)
		{
			if (transform == null)
			{
				return null;
			}
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (child.name == name)
				{
					return child;
				}
				Transform transform2 = FindChildTransform(child, name);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return null;
		}

		public static string FormatSpeed(float SpeedValue, bool IsMile)
		{
			string text = "-- Km/h";
			return $"{SpeedValue:0} Km/h";
		}

		public static string FormatTime(float TimeValue, bool ShowFraction, int FractionDecimals)
		{
			string result = "--:--.--";
			if (!ShowFraction)
			{
				result = "--:--";
			}
			if (TimeValue > 0f)
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds(TimeValue);
				float num = timeSpan.Minutes;
				float num2 = timeSpan.Seconds;
				if (ShowFraction)
				{
					if (FractionDecimals == 1)
					{
						float num3 = TimeValue * 10f % 10f;
						if (num3 > 9f)
						{
							num3 = 0f;
						}
						result = $"{num:00}:{num2:00}.{num3:0}";
					}
					else
					{
						float num4 = TimeValue * 100f % 100f;
						if (num4 > 99f)
						{
							num4 = 0f;
						}
						result = $"{num:00}:{num2:00}.{num4:00}";
					}
				}
				else
				{
					result = $"{num:00}.{num2:00}";
				}
			}
			return result;
		}

		public static string Ordinal(int number)
		{
			string empty = string.Empty;
			int num = number % 10;
			int num2 = (int)Math.Floor((decimal)number / 10m) % 10;
			if (num2 == 1)
			{
				empty = "th";
			}
			else
			{
				switch (num)
				{
				case 1:
					empty = "st";
					break;
				case 2:
					empty = "nd";
					break;
				case 3:
					empty = "rd";
					break;
				default:
					empty = "th";
					break;
				}
			}
			return $"{number}{empty}";
		}

		public static AudioSource CreateAudioSource(Transform Parent, string AudioSourceName, bool Loop)
		{
			GameObject gameObject = new GameObject(AudioSourceName);
			gameObject.transform.parent = Parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
			audioSource.loop = Loop;
			return audioSource;
		}
	}
}
