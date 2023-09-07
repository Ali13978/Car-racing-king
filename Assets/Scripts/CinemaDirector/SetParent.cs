using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Transform", "Set Parent", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class SetParent : CinemaActorEvent
	{
		public GameObject parent;

		public override void Trigger(GameObject actor)
		{
			if (actor != null && parent != null)
			{
				actor.transform.parent = parent.transform;
			}
		}

		public override void Reverse(GameObject actor)
		{
		}
	}
}
