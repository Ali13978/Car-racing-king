using UnityEngine;

namespace RacingGameKit.UI
{
	public abstract class RGKPauseMenuBase : MonoBehaviour
	{
		public abstract bool IsPaused
		{
			get;
			set;
		}

		public abstract void PauseGame();

		public abstract void ResumeGame();
	}
}
