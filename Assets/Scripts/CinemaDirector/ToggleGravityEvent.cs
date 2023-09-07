using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Physics", "Toggle Gravity", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class ToggleGravityEvent : CinemaActorEvent, IRevertable
	{
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
						list2.Add(new RevertInfo(this, component, "useGravity", component.useGravity));
					}
				}
			}
			return list2.ToArray();
		}

		public override void Trigger(GameObject actor)
		{
			Rigidbody component = actor.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.useGravity = !component.useGravity;
			}
		}

		public override void Reverse(GameObject actor)
		{
			Trigger(actor);
		}
	}
}
