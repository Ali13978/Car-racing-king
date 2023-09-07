using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector
{
	[CutsceneItem("Transitions", "Color Transition", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.GlobalItem
	})]
	public class ColorTransition : CinemaGlobalAction
	{
		public Color From = Color.black;

		public Color To = Color.clear;

		private void Awake()
		{
			if (base.gameObject.GetComponent<RawImage>() == null)
			{
				base.gameObject.transform.position = Vector3.zero;
				base.gameObject.transform.localScale = new Vector3(100f, 100f, 100f);
				RawImage RawImage = base.gameObject.AddComponent<RawImage>();
				RawImage.enabled = false;
				RawImage.texture = new Texture2D(1, 1);
                RawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                RawImage.color = Color.clear;
			}
		}

		public override void Trigger()
		{
			RawImage component = base.gameObject.GetComponent<RawImage>();
			if (component != null)
			{
				component.enabled = true;
                component.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                component.color = From;
			}
		}

		public override void ReverseTrigger()
		{
			End();
		}

		public override void UpdateTime(float time, float deltaTime)
		{
			float transition = time / base.Duration;
			FadeToColor(From, To, transition);
		}

		public override void SetTime(float time, float deltaTime)
		{
			RawImage component = base.gameObject.GetComponent<RawImage>();
			if (component != null)
			{
				if (time >= 0f && time <= base.Duration)
				{
					component.enabled = true;
					UpdateTime(time, deltaTime);
				}
				else if (component.enabled)
				{
					component.enabled = false;
				}
			}
		}

		public override void End()
		{
			RawImage component = base.gameObject.GetComponent<RawImage>();
			if (component != null)
			{
				component.enabled = false;
			}
		}

		public override void ReverseEnd()
		{
			RawImage component = base.gameObject.GetComponent<RawImage>();
			if (component != null)
			{
				component.enabled = true;
                component.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                component.color = To;
			}
		}

		public override void Stop()
		{
			RawImage component = base.gameObject.GetComponent<RawImage>();
			if (component != null)
			{
				component.enabled = false;
			}
		}

		private void FadeToColor(Color from, Color to, float transition)
		{
			RawImage component = base.gameObject.GetComponent<RawImage>();
			if (component != null)
			{
				component.color = Color.Lerp(from, to, transition);
			}
		}
	}
}
