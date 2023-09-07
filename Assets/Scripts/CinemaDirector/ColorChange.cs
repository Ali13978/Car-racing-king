using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{
	[CutsceneItem("uGUI", "Change Color", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class ColorChange : CinemaActorAction, IRevertable
	{
		[SerializeField]
		private Color colorValue = Color.white;

		[SerializeField]
		private RevertMode editorRevertMode;

		[SerializeField]
		private RevertMode runtimeRevertMode;

		private Color initialColor;

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
					Graphic component = transform.GetComponent<Graphic>();
					if (component != null)
					{
						list2.Add(new RevertInfo(this, component, "color", component.color));
					}
				}
			}
			return list2.ToArray();
		}

		public override void Trigger(GameObject actor)
		{
			if (actor != null)
			{
				Graphic component = actor.GetComponent<Graphic>();
				if (component != null)
				{
					initialColor = component.color;
				}
			}
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
			if (actor != null)
			{
				float t = runningTime / base.Duration;
				Graphic component = actor.GetComponent<Graphic>();
				if (component != null)
				{
					Color color2 = component.color = Color.Lerp(initialColor, colorValue, t);
				}
			}
		}

		public override void End(GameObject actor)
		{
			if (actor != null)
			{
				Graphic component = actor.GetComponent<Graphic>();
				if (component != null)
				{
					component.color = colorValue;
				}
			}
		}
	}
}
