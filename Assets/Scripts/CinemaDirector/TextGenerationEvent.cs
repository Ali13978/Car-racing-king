using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{
	[CutsceneItem("uGUI", "Text Generator", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class TextGenerationEvent : CinemaActorAction, IRevertable
	{
		[TextArea(3, 10)]
		public string textValue;

		private string initialTextValue;

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
					Text componentInChildren = transform.GetComponentInChildren<Text>();
					if (componentInChildren != null)
					{
						list2.Add(new RevertInfo(this, componentInChildren, "text", componentInChildren.text));
					}
				}
			}
			return list2.ToArray();
		}

		public override void Trigger(GameObject actor)
		{
			initialTextValue = actor.GetComponentInChildren<Text>().text;
			actor.GetComponentInChildren<Text>().text = string.Empty;
		}

		public override void ReverseTrigger(GameObject actor)
		{
			actor.GetComponentInChildren<Text>().text = initialTextValue;
		}

		public override void SetTime(GameObject actor, float time, float deltaTime)
		{
			if (actor != null && time > 0f && time <= base.Duration)
			{
				UpdateTime(actor, time, deltaTime);
			}
		}

		public override void UpdateTime(GameObject actor, float runningTime, float deltaTime)
		{
			float t = runningTime / base.Duration;
			if (textValue != null)
			{
				int length = (int)Mathf.Round(Mathf.Lerp(0f, textValue.Length, t));
				actor.GetComponentInChildren<Text>().text = textValue.Substring(0, length);
			}
		}

		public override void End(GameObject actor)
		{
			actor.GetComponentInChildren<Text>().text = textValue;
		}
	}
}
