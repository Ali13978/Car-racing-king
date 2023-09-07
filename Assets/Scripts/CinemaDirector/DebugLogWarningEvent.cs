using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Debug", "Log Warning", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.GlobalItem
	})]
	public class DebugLogWarningEvent : CinemaGlobalEvent
	{
		public string message = "Warning Message";

		public override void Trigger()
		{
			UnityEngine.Debug.LogWarning(message);
		}

		public override void Reverse()
		{
			UnityEngine.Debug.LogWarning($"Reverse: {message}");
		}
	}
}
