using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Transform", "Look At", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem,
		CutsceneItemGenre.TransformItem
	})]
	public class TransformLookAtAction : CinemaActorAction, IRevertable
	{
		[SerializeField]
		[Tooltip("The target that the Actor should look at.")]
		private GameObject LookAtTarget;

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
					Transform component = transform.GetComponent<Transform>();
					if (component != null)
					{
						list2.Add(new RevertInfo(this, component, "localRotation", component.localRotation));
					}
				}
			}
			return list2.ToArray();
		}

		public override void Trigger(GameObject actor)
		{
			if (!(actor == null) && !(LookAtTarget == null))
			{
				actor.transform.LookAt(LookAtTarget.transform);
			}
		}

		public override void UpdateTime(GameObject actor, float runningTime, float deltaTime)
		{
			if (!(actor == null) && !(LookAtTarget == null))
			{
				actor.transform.LookAt(LookAtTarget.transform);
			}
		}

		public override void End(GameObject actor)
		{
		}
	}
}
