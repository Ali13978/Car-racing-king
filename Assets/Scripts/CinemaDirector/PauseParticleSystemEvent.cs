using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Particle System", "Pause", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class PauseParticleSystemEvent : CinemaActorEvent
	{
		public override void Trigger(GameObject actor)
		{
			if (actor != null)
			{
				ParticleSystem component = actor.GetComponent<ParticleSystem>();
				if (component != null)
				{
					component.Pause();
				}
			}
		}
	}
}
