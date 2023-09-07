using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Game Object", "Enable", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class EnableGameObject : CinemaActorEvent, IRevertable
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
					list2.Add(new RevertInfo(this, transform.gameObject, "SetActive", transform.gameObject.activeSelf));
				}
			}
			return list2.ToArray();
		}

		public override void Trigger(GameObject actor)
		{
			if (actor != null)
			{
				actor.SetActive(value: true);
			}
		}

		public override void Reverse(GameObject actor)
		{
			if (actor != null)
			{
				actor.SetActive(value: false);
			}
		}
	}
}
