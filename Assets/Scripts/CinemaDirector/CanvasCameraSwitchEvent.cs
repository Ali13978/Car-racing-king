using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("uGUI", "Switch Canvas Camera", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class CanvasCameraSwitchEvent : CinemaActorEvent
	{
		public Camera Camera;

		private Camera initialCamera;

		public override void Trigger(GameObject actor)
		{
			Canvas component = actor.GetComponent<Canvas>();
			if (actor != null && Camera != null && component != null)
			{
				if (component.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					UnityEngine.Debug.LogWarning("Current canvas render mode does not target a camera");
					initialCamera = Camera.main;
				}
				else
				{
					initialCamera = component.worldCamera;
					component.worldCamera = Camera;
				}
			}
		}

		public override void Reverse(GameObject actor)
		{
			Canvas component = actor.GetComponent<Canvas>();
			if (actor != null && Camera != null && component != null)
			{
				if (component.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					UnityEngine.Debug.LogWarning("Current canvas render mode does not target a camera");
				}
				else
				{
					component.worldCamera = initialCamera;
				}
			}
		}

		public override void Stop(GameObject actor)
		{
			Canvas component = actor.GetComponent<Canvas>();
			if (actor != null && component != null)
			{
				component.worldCamera = initialCamera;
			}
		}
	}
}
