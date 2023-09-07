using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Transform", "Set Position", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class SetPositionEvent : CinemaActorEvent, IRevertable
	{
		public Vector3 Position;

		public Vector3 InitialPosition;

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
				InitialPosition = actor.transform.position;
				actor.transform.position = Position;
			}
		}

		public override void Reverse(GameObject actor)
		{
			if (actor != null)
			{
				actor.transform.position = InitialPosition;
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
					list2.Add(new RevertInfo(this, transform.gameObject.transform, "position", transform.gameObject.transform.position));
				}
			}
			return list2.ToArray();
		}
	}
}
