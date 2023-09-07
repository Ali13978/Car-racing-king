using CinemaDirector;
using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

[CutsceneItem("Examples", "Example Shrink", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem
})]
internal class ExampleShrinkAction : CinemaActorAction, IRevertable
{
	private Vector3 scaleStart = Vector3.one;

	private Vector3 scaleEnd = Vector3.zero;

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
				Transform component = transform.GetComponent<Transform>();
				if (component != null)
				{
					list2.Add(new RevertInfo(this, component, "localScale", component.localScale));
				}
			}
		}
		return list2.ToArray();
	}

	public override void Trigger(GameObject Actor)
	{
		UnityEngine.Debug.Log("Trigger");
		if (Actor != null)
		{
			scaleStart = Actor.transform.localScale;
		}
	}

	public override void End(GameObject Actor)
	{
		UnityEngine.Debug.Log("End");
		if (Actor != null)
		{
			Actor.transform.localScale = scaleEnd;
		}
	}

	public override void UpdateTime(GameObject Actor, float time, float deltaTime)
	{
		float transition = time / base.Duration;
		LerpScale(Actor, scaleStart, scaleEnd, transition);
	}

	public override void SetTime(GameObject Actor, float time, float deltaTime)
	{
		if (Actor != null && time >= 0f && time <= base.Duration)
		{
			UpdateTime(Actor, time, deltaTime);
		}
	}

	public override void ReverseTrigger(GameObject Actor)
	{
		UnityEngine.Debug.Log("ReverseTrigger");
		if (Actor != null)
		{
			Actor.transform.localScale = scaleStart;
		}
	}

	public override void ReverseEnd(GameObject Actor)
	{
		UnityEngine.Debug.Log("ReverseEnd");
		End(Actor);
	}

	private void LerpScale(GameObject Actor, Vector3 scaleFrom, Vector3 scaleTo, float transition)
	{
		if (Actor != null)
		{
			Actor.transform.localScale = Vector3.Lerp(scaleFrom, scaleTo, transition);
		}
	}
}
