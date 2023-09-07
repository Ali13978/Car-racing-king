using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Transform", "Set Scale", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class SetScaleEvent : CinemaActorEvent, IRevertable
	{
		public Vector3 Scale = Vector3.one;

		private Vector3 InitialScale;

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
				InitialScale = actor.transform.localScale;
				actor.transform.localScale = Scale;
			}
		}

		public override void Reverse(GameObject actor)
		{
			if (actor != null)
			{
				actor.transform.localScale = InitialScale;
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
					list2.Add(new RevertInfo(this, transform.gameObject.transform, "localScale", transform.gameObject.transform.localScale));
				}
			}
			return list2.ToArray();
		}
	}
}
