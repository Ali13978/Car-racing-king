using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Physics", "Set Mass", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class SetMassEvent : CinemaActorEvent, IRevertable
	{
		public float Mass = 1f;

		[SerializeField]
		private RevertMode editorRevertMode;

		[SerializeField]
		private RevertMode runtimeRevertMode;

		public RevertMode EditorRevertMode
		{
			get
			{
				return editorRevertMode;
			}
			set
			{
				editorRevertMode = value;
			}
		}

		public RevertMode RuntimeRevertMode
		{
			get
			{
				return runtimeRevertMode;
			}
			set
			{
				runtimeRevertMode = value;
			}
		}

		public RevertInfo[] CacheState()
		{
			List<Transform> list = new List<Transform>(GetActors());
			List<RevertInfo> list2 = new List<RevertInfo>();
			for (int i = 0; i < list.Count; i++)
			{
				Transform transform = list[i];
				if (transform != null)
				{
					Rigidbody component = transform.GetComponent<Rigidbody>();
					if (component != null)
					{
						list2.Add(new RevertInfo(this, component, "mass", component.mass));
					}
				}
			}
			return list2.ToArray();
		}

		public override void Trigger(GameObject actor)
		{
			if (!(actor == null))
			{
				Rigidbody component = actor.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.mass = Mass;
				}
			}
		}
	}
}
