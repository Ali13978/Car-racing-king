using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace com.niobiumstudios.dailyrewards
{
	public class DailyRewards : MonoBehaviour
	{
		public delegate void OnClaimPrize(int day);

		public delegate void OnPrizeAlreadyClaimed(int day);

		public static int rewardValue;

		public List<int> rewards;

		public DateTime timer;

		public DateTime lastRewardTime;

		[HideInInspector]
		public int availableReward;

		[HideInInspector]
		public int lastReward;

		public OnClaimPrize onClaimPrize;

		public OnPrizeAlreadyClaimed onPrizeAlreadyClaimed;

		private float t;

		private bool isInitialized;

		private const string LAST_REWARD_TIME = "LastRewardTime";

		private const string LAST_REWARD = "LastReward";

		private const string FMT = "O";

		private static DailyRewards _instance;

		public static DailyRewards instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = UnityEngine.Object.FindObjectOfType<DailyRewards>();
					if (_instance == null)
					{
						GameObject gameObject = new GameObject();
						gameObject.hideFlags = HideFlags.HideAndDontSave;
						_instance = gameObject.AddComponent<DailyRewards>();
					}
				}
				return _instance;
			}
		}

		protected virtual void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (_instance == null)
			{
				_instance = this;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void Update()
		{
			t += Time.deltaTime;
			if (t >= 1f)
			{
				timer = timer.AddSeconds(1.0);
				t = 0f;
			}
		}

		private void Initialize()
		{
			timer = DateTime.Now;
			isInitialized = true;
		}

		public void CheckRewards()
		{
			if (!isInitialized)
			{
				Initialize();
			}
			string @string = PlayerPrefs.GetString("LastRewardTime");
			lastReward = PlayerPrefs.GetInt("LastReward");
			if (!string.IsNullOrEmpty(@string))
			{
				lastRewardTime = DateTime.ParseExact(@string, "O", CultureInfo.InvariantCulture);
				TimeSpan timeSpan = timer - lastRewardTime;
				UnityEngine.Debug.Log("Last claim was " + (long)timeSpan.TotalHours + " hours ago.");
				int num = (int)(Math.Abs(timeSpan.TotalHours) / 24.0);
				switch (num)
				{
				case 0:
					availableReward = 0;
					break;
				case 1:
					if (lastReward == rewards.Count)
					{
						availableReward = 1;
						lastReward = 0;
					}
					else
					{
						availableReward = lastReward + 1;
						UnityEngine.Debug.Log("Player can claim prize " + availableReward);
					}
					break;
				default:
					if (num >= 2)
					{
						availableReward = 1;
						lastReward = 0;
						UnityEngine.Debug.Log("Prize reset ");
					}
					break;
				}
			}
			else
			{
				availableReward = 1;
			}
		}

		public void ClaimPrize(int day)
		{
			if (availableReward == day)
			{
				if (onClaimPrize != null)
				{
					onClaimPrize(day);
				}
				UnityEngine.Debug.Log("Reward [" + rewards[day - 1] + "] Claimed!");
				rewardValue = rewards[day - 1];
				PlayerPrefs.SetInt("LastReward", availableReward);
				string value = timer.ToString("O");
				PlayerPrefs.SetString("LastRewardTime", value);
			}
			else if (day <= lastReward)
			{
				if (onPrizeAlreadyClaimed != null)
				{
					onPrizeAlreadyClaimed(day);
				}
				UnityEngine.Debug.Log("Reward already claimed. Try again tomorrow");
			}
			else
			{
				UnityEngine.Debug.Log("Cannot Claim this reward! Can only claim reward #" + availableReward);
			}
			CheckRewards();
		}

		public void Reset()
		{
			PlayerPrefs.DeleteKey("LastReward");
			PlayerPrefs.DeleteKey("LastRewardTime");
		}
	}
}
