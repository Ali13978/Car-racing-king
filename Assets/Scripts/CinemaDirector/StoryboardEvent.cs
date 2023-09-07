using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Utility", "Storyboard", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.GlobalItem
	})]
	public class StoryboardEvent : CinemaGlobalEvent
	{
		public string FolderName = "Storyboard";

		public static int Count;

		public override void Trigger()
		{
			UnityEngine.ScreenCapture.CaptureScreenshot($"Assets\\{base.gameObject.name}{Count++}.png");
		}
	}
}
