using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Light", "Set Intensity", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class SetIntensityLight : CinemaActorEvent, IRevertable
	{
		public float Intensity;

		[SerializeField]
		private RevertMode editorRevertMode;

		[SerializeField]
		private RevertMode runtimeRevertMode;

		private float previousIntensity;

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
					Light component = transform.GetComponent<Light>();
					if (component != null)
					{
						list2.Add(new RevertInfo(this, component, "intensity", component.intensity));
					}
				}
			}
			return list2.ToArray();
		}

		public override void Initialize(GameObject actor)
		{
			Light component = actor.GetComponent<Light>();
			if (component != null)
			{
				previousIntensity = component.intensity;
			}
		}

		public override void Trigger(GameObject actor)
		{
			Light component = actor.GetComponent<Light>();
			if (component != null)
			{
				previousIntensity = component.intensity;
				component.intensity = Intensity;
			}
		}

		public override void Reverse(GameObject actor)
		{
			Light component = actor.GetComponent<Light>();
			if (component != null)
			{
				component.intensity = previousIntensity;
			}
		}
	}
}
