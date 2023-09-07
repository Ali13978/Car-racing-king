using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Particle System", "Play", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class PlayParticleSystemEvent : CinemaActorEvent
	{
		public override void Trigger(GameObject actor)
		{
			if (actor != null)
			{
				ParticleSystem component = actor.GetComponent<ParticleSystem>();
				if (component != null)
				{
					component.Play();
				}
			}
		}

		public override void Reverse(GameObject actor)
		{
			if (actor != null)
			{
				ParticleSystem component = actor.GetComponent<ParticleSystem>();
				if (component != null)
				{
					component.Stop();
				}
			}
		}
	}
}
