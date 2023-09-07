using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Transform", "Rotate to Follow", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem,
		CutsceneItemGenre.TransformItem
	})]
	public class RotateFollow : CinemaActorAction, IRevertable
	{
		[SerializeField]
		[Tooltip("The target that the Actor should look at.")]
		private GameObject LookAtTarget;

		[SerializeField]
		private RevertMode editorRevertMode;

		[SerializeField]
		private RevertMode runtimeRevertMode;

		private Quaternion initialRotation;

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
				initialRotation = actor.transform.rotation;
				updateRotation(actor, 0f);
			}
		}

		public override void UpdateTime(GameObject actor, float runningTime, float deltaTime)
		{
			if (!(actor == null) && !(LookAtTarget == null))
			{
				updateRotation(actor, runningTime);
			}
		}

		public override void End(GameObject actor)
		{
			if (!(actor == null) && !(LookAtTarget == null))
			{
				updateRotation(actor, base.Duration);
			}
		}

		private void updateRotation(GameObject actor, float runningTime)
		{
			Quaternion b = Quaternion.LookRotation(LookAtTarget.transform.position - actor.transform.position);
			float t = runningTime / base.Duration;
			actor.transform.rotation = Quaternion.Slerp(initialRotation, b, t);
		}
	}
}
