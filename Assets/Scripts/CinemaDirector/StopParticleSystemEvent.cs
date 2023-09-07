using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Particle System", "Stop", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class StopParticleSystemEvent : CinemaActorEvent
	{
		public override void Trigger(GameObject actor)
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

		public override void Reverse(GameObject actor)
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
	}
}
