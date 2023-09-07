using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Debug", "Log Message", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.GlobalItem
	})]
	public class DebugLogMessageEvent : CinemaGlobalEvent
	{
		public string message = "Log Message";

		public override void Trigger()
		{
			UnityEngine.Debug.Log(message);
		}

		public override void Reverse()
		{
			UnityEngine.Debug.Log($"Reverse: {message}");
		}
	}
}
