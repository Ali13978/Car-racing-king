using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.niobiumstudios.dailyrewards
{
	public class DailyRewardsInterface : MonoBehaviour
	{
		public GameObject dailyRewardPrefab;

		public GameObject panelReward;

		public Text txtReward;

		public Button btnClaim;

		public Text txtTimeDue;

		public GridLayoutGroup dailyRewardsGroup;

		private void Start()
		{
			DailyRewards.instance.CheckRewards();
			DailyRewards instance = DailyRewards.instance;
			instance.onClaimPrize = (DailyRewards.OnClaimPrize)Delegate.Combine(instance.onClaimPrize, new DailyRewards.OnClaimPrize(OnClaimPrize));
			DailyRewards instance2 = DailyRewards.instance;
			instance2.onPrizeAlreadyClaimed = (DailyRewards.OnPrizeAlreadyClaimed)Delegate.Combine(instance2.onPrizeAlreadyClaimed, new DailyRewards.OnPrizeAlreadyClaimed(OnPrizeAlreadyClaimed));
			UpdateUI();
		}

		private void OnDestroy()
		{
			DailyRewards instance = DailyRewards.instance;
			instance.onClaimPrize = (DailyRewards.OnClaimPrize)Delegate.Remove(instance.onClaimPrize, new DailyRewards.OnClaimPrize(OnClaimPrize));
			DailyRewards instance2 = DailyRewards.instance;
			instance2.onPrizeAlreadyClaimed = (DailyRewards.OnPrizeAlreadyClaimed)Delegate.Remove(instance2.onPrizeAlreadyClaimed, new DailyRewards.OnPrizeAlreadyClaimed(OnPrizeAlreadyClaimed));
		}

		public void OnClaimClick()
		{
			DailyRewards.instance.ClaimPrize(DailyRewards.instance.availableReward);
			PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + DailyRewards.rewardValue);
			MonoBehaviour.print(DailyRewards.rewardValue);
			MonoBehaviour.print(PlayerPrefs.GetInt("Cash"));
			UpdateUI();
		}

		public void UpdateUI()
		{
			IEnumerator enumerator = dailyRewardsGroup.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			bool flag = false;
			for (int i = 0; i < DailyRewards.instance.rewards.Count; i++)
			{
				int reward = DailyRewards.instance.rewards[i];
				int num = i + 1;
				GameObject gameObject = UnityEngine.Object.Instantiate(dailyRewardPrefab);
				DailyRewardUI component = gameObject.GetComponent<DailyRewardUI>();
				component.transform.SetParent(dailyRewardsGroup.transform);
				gameObject.transform.localScale = Vector2.one;
				component.day = num;
				component.reward = reward;
				component.isAvailable = (num == DailyRewards.instance.availableReward);
				component.isClaimed = (num <= DailyRewards.instance.lastReward);
				if (component.isAvailable)
				{
					flag = true;
				}
				component.Refresh();
			}
			btnClaim.gameObject.SetActive(flag);
			txtTimeDue.gameObject.SetActive(!flag);
		}

		private void Update()
		{
			if (txtTimeDue.IsActive())
			{
				TimeSpan timeSpan = (DailyRewards.instance.lastRewardTime - DailyRewards.instance.timer).Add(new TimeSpan(0, 24, 0, 0));
				if (timeSpan.TotalSeconds <= 0.0)
				{
					DailyRewards.instance.CheckRewards();
					UpdateUI();
				}
				else
				{
					string str = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
					txtTimeDue.text = "Return in " + str + " to claim your reward";
				}
			}
		}

		private void OnClaimPrize(int day)
		{
			panelReward.SetActive(value: true);
			txtReward.text = "You got " + DailyRewards.instance.rewards[day - 1] + " Cash!";
		}

		private void OnPrizeAlreadyClaimed(int day)
		{
		}

		public void OnCloseRewardsClick()
		{
			panelReward.SetActive(value: false);
		}

		public void OnResetClick()
		{
			DailyRewards.instance.Reset();
			DailyRewards.instance.lastRewardTime = DateTime.MinValue;
			DailyRewards.instance.CheckRewards();
			UpdateUI();
		}

		public void OnAdvanceDayClick()
		{
			DailyRewards.instance.timer = DailyRewards.instance.timer.AddDays(1.0);
			DailyRewards.instance.CheckRewards();
			UpdateUI();
		}

		public void OnAdvanceHourClick()
		{
			DailyRewards.instance.timer = DailyRewards.instance.timer.AddHours(1.0);
			DailyRewards.instance.CheckRewards();
			UpdateUI();
		}
	}
}
