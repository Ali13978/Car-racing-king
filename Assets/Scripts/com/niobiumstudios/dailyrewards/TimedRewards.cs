using System;
using System.Globalization;
using UnityEngine;

namespace com.niobiumstudios.dailyrewards
{
	public class TimedRewards : MonoBehaviour
	{
		public delegate void OnCanClaim();

		public DateTime lastRewardTime;

		public TimeSpan timer;

		public float maxTime = 3600f;

		public OnCanClaim onCanClaim;

		private bool canClaim;

		private const string TIMED_REWARDS_TIME = "TimedRewardsTime";

		private const string FMT = "O";

		private static TimedRewards _instance;

		public static TimedRewards instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = UnityEngine.Object.FindObjectOfType<TimedRewards>();
					if (_instance == null)
					{
						GameObject gameObject = new GameObject();
						gameObject.hideFlags = HideFlags.HideAndDontSave;
						_instance = gameObject.AddComponent<TimedRewards>();
					}
				}
				return _instance;
			}
		}

		private void Awake()
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
			InitializeTimer();
		}

		private void InitializeTimer()
		{
			string @string = PlayerPrefs.GetString("TimedRewardsTime");
			if (!string.IsNullOrEmpty(@string))
			{
				lastRewardTime = DateTime.ParseExact(@string, "O", CultureInfo.InvariantCulture);
				timer = (lastRewardTime - DateTime.Now).Add(TimeSpan.FromSeconds(maxTime));
			}
			else
			{
				timer = TimeSpan.FromSeconds(maxTime);
			}
		}

		private void Update()
		{
			if (canClaim)
			{
				return;
			}
			timer = timer.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
			if (timer.TotalSeconds <= 0.0)
			{
				canClaim = true;
				if (onCanClaim != null)
				{
					onCanClaim();
				}
			}
			else
			{
				PlayerPrefs.SetString("TimedRewardsTime", DateTime.Now.Add(timer - TimeSpan.FromSeconds(maxTime)).ToString("O"));
			}
		}

		public void Claim()
		{
			PlayerPrefs.SetString("TimedRewardsTime", DateTime.Now.ToString("O"));
			timer = TimeSpan.FromSeconds(maxTime);
			canClaim = false;
		}

		public void Reset()
		{
			PlayerPrefs.DeleteKey("TimedRewardsTime");
			canClaim = false;
			timer = TimeSpan.FromSeconds(maxTime);
		}
	}
}
