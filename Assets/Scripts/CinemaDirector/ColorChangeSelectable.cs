using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{
	[CutsceneItem("uGUI", "Change Color Selectable", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class ColorChangeSelectable : CinemaActorAction, IRevertable
	{
		private enum ColorBlockChoices
		{
			normalColor,
			highlightedColor,
			pressedColor,
			disabledColor
		}

		[SerializeField]
		private ColorBlockChoices colorField;

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
					Selectable component = transform.GetComponent<Selectable>();
					if (component != null)
					{
						list2.Add(new RevertInfo(this, component, "colors", component.colors));
					}
				}
			}
			return list2.ToArray();
		}

		public override void Trigger(GameObject actor)
		{
			if (!(actor != null))
			{
				return;
			}
			Selectable component = actor.GetComponent<Selectable>();
			if (component != null)
			{
				switch (colorField)
				{
				case ColorBlockChoices.normalColor:
					initialColor = component.colors.normalColor;
					break;
				case ColorBlockChoices.highlightedColor:
					initialColor = component.colors.highlightedColor;
					break;
				case ColorBlockChoices.pressedColor:
					initialColor = component.colors.pressedColor;
					break;
				case ColorBlockChoices.disabledColor:
					initialColor = component.colors.disabledColor;
					break;
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
			if (!(actor != null))
			{
				return;
			}
			float t = runningTime / base.Duration;
			Selectable component = actor.GetComponent<Selectable>();
			if (component != null)
			{
				ColorBlock colors = component.colors;
				Color color = Color.Lerp(initialColor, colorValue, t);
				switch (colorField)
				{
				case ColorBlockChoices.normalColor:
					colors.normalColor = color;
					break;
				case ColorBlockChoices.highlightedColor:
					colors.highlightedColor = color;
					break;
				case ColorBlockChoices.pressedColor:
					colors.pressedColor = color;
					break;
				case ColorBlockChoices.disabledColor:
					colors.disabledColor = color;
					break;
				}
				component.colors = colors;
			}
		}

		public override void End(GameObject actor)
		{
			if (!(actor != null))
			{
				return;
			}
			Selectable component = actor.GetComponent<Selectable>();
			if (component != null)
			{
				ColorBlock colors = component.colors;
				switch (colorField)
				{
				case ColorBlockChoices.normalColor:
					colors.normalColor = colorValue;
					break;
				case ColorBlockChoices.highlightedColor:
					colors.highlightedColor = colorValue;
					break;
				case ColorBlockChoices.pressedColor:
					colors.pressedColor = colorValue;
					break;
				case ColorBlockChoices.disabledColor:
					colors.disabledColor = colorValue;
					break;
				}
				if (component != null)
				{
					component.colors = colors;
				}
			}
		}
	}
}
