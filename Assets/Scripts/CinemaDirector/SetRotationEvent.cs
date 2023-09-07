using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Transform", "Set Rotation", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class SetRotationEvent : CinemaActorEvent, IRevertable
	{
		public Vector3 Rotation;

		private Quaternion InitialRotation;

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

		public override void Trigger(GameObject actor)
		{
			if (actor != null)
			{
				InitialRotation = actor.transform.rotation;
				actor.transform.rotation = Quaternion.Euler(Rotation);
			}
		}

		public override void Reverse(GameObject actor)
		{
			if (actor != null)
			{
				actor.transform.rotation = InitialRotation;
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
					list2.Add(new RevertInfo(this, transform.gameObject.transform, "rotation", transform.gameObject.transform.rotation));
				}
			}
			return list2.ToArray();
		}
	}
}
