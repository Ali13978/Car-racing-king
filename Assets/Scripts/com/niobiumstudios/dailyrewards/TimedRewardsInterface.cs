using System;
using UnityEngine;
using UnityEngine.UI;

namespace com.niobiumstudios.dailyrewards
{
	public class TimedRewardsInterface : MonoBehaviour
	{
		public Button btnClaim;

		public Text txtTimer;

		public GameObject panelReward;

		private const string TIMED_REWARDS_TIME = "TimedRewardsTime";

		private const string FMT = "O";

		private void Start()
		{
			TimedRewards instance = TimedRewards.instance;
			instance.onCanClaim = (TimedRewards.OnCanClaim)Delegate.Combine(instance.onCanClaim, new TimedRewards.OnCanClaim(OnCanClaim));
			Reset();
		}

		private void Update()
		{
			TimeSpan timer = TimedRewards.instance.timer;
			if (timer.TotalSeconds > 0.0)
			{
				txtTimer.text = $"{timer.Hours:D2}:{timer.Minutes:D2}:{timer.Seconds:D2}";
			}
		}

		public void OnClaimClick()
		{
			TimedRewards.instance.Claim();
			Reset();
			panelReward.SetActive(value: true);
		}

		public void OnResetClick()
		{
			Reset();
			TimedRewards.instance.Reset();
		}

		public void OnCloseRewardClick()
		{
			panelReward.SetActive(value: false);
		}

		private void Reset()
		{
			txtTimer.text = string.Empty;
			btnClaim.interactable = false;
		}

		private void OnCanClaim()
		{
			btnClaim.interactable = true;
			txtTimer.text = "You prize is ready to be claimed!";
		}
	}
}
