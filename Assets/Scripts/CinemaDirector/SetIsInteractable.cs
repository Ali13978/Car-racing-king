using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{
	[CutsceneItem("uGUI", "Interactable", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class SetIsInteractable : CinemaActorEvent, IRevertable
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

		public override void Trigger(GameObject actor)
		{
			Selectable component = actor.GetComponent<Selectable>();
			if (component != null)
			{
				component.interactable = !component.interactable;
			}
		}

		public override void Reverse(GameObject actor)
		{
			Selectable component = actor.GetComponent<Selectable>();
			if (component != null && runtimeRevertMode == RevertMode.Revert && Application.isPlaying)
			{
				component.interactable = !component.interactable;
			}
			if (component != null && editorRevertMode == RevertMode.Revert && Application.isEditor && !Application.isPlaying)
			{
				component.interactable = !component.interactable;
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
					Selectable component = transform.GetComponent<Selectable>();
					if (component != null)
					{
						list2.Add(new RevertInfo(this, component, "interactable", component.interactable));
					}
				}
			}
			return list2.ToArray();
		}
	}
}
