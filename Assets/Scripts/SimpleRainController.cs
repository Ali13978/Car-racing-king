using RainDropEffect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRainController : MonoBehaviour
{
	public enum DrawState
	{
		Playing,
		Disabled
	}

	[Serializable]
	public class SimpleRainDrawerContainer : RainDrawerContainer<RainDrawer>
	{
		public DrawState currentState = DrawState.Disabled;

		public Vector3 startSize;

		public Vector3 startPos;

		public float TimeElapsed;

		public float lifetime;

		public SimpleRainDrawerContainer(string name, Transform parent)
			: base(name, parent)
		{
		}
	}

	private int oldSpawnLimit;

	private bool isOneShot;

	private float oneShotTimeleft;

	private float timeElapsed;

	private float interval;

	private bool isWaitingDelay;

	public List<SimpleRainDrawerContainer> drawers = new List<SimpleRainDrawerContainer>();

	public SimpleRainVariables Variables
	{
		get;
		set;
	}

	[HideInInspector]
	public int RenderQueue
	{
		get;
		set;
	}

	public Camera camera
	{
		get;
		set;
	}

	public float Alpha
	{
		get;
		set;
	}

	public bool NoMoreRain
	{
		get;
		set;
	}

	public RainDropTools.RainDropShaderType ShaderType
	{
		get;
		set;
	}

	public bool IsPlaying => drawers.FindAll((SimpleRainDrawerContainer t) => t.currentState == DrawState.Disabled).Count != drawers.Count;

	public void Refresh()
	{
		foreach (SimpleRainDrawerContainer drawer in drawers)
		{
			drawer.Drawer.Hide();
			UnityEngine.Object.DestroyImmediate(drawer.Drawer.gameObject);
		}
		drawers.Clear();
		for (int i = 0; i < Variables.MaxRainSpawnCount; i++)
		{
			SimpleRainDrawerContainer simpleRainDrawerContainer = new SimpleRainDrawerContainer("Simple RainDrawer " + i, base.transform);
			simpleRainDrawerContainer.currentState = DrawState.Disabled;
			drawers.Add(simpleRainDrawerContainer);
		}
	}

	public void Play()
	{
		StartCoroutine(PlayDelay(Variables.Delay));
	}

	private IEnumerator PlayDelay(float delay)
	{
		float t = 0f;
		while (t <= delay)
		{
			isWaitingDelay = true;
			t += Time.deltaTime;
			yield return null;
		}
		isWaitingDelay = false;
		if (drawers.Find((SimpleRainDrawerContainer x) => x.currentState == DrawState.Playing) == null)
		{
			for (int i = 0; i < drawers.Count; i++)
			{
				InitializeDrawer(drawers[i]);
				drawers[i].currentState = DrawState.Disabled;
			}
			isOneShot = Variables.PlayOnce;
			if (isOneShot)
			{
				oneShotTimeleft = Variables.Duration;
			}
		}
	}

	public void UpdateController()
	{
		if (Variables == null)
		{
			return;
		}
		CheckSpawnNum();
		if (NoMoreRain)
		{
			timeElapsed = 0f;
		}
		else if (isOneShot)
		{
			oneShotTimeleft -= Time.deltaTime;
			if (oneShotTimeleft > 0f)
			{
				CheckSpawnTime();
			}
		}
		else if (!isWaitingDelay)
		{
			CheckSpawnTime();
		}
		for (int i = 0; i < drawers.Count(); i++)
		{
			UpdateInstance(drawers[i], i);
		}
	}

	private void CheckSpawnNum()
	{
		int num = Variables.MaxRainSpawnCount - drawers.Count();
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				SimpleRainDrawerContainer simpleRainDrawerContainer = new SimpleRainDrawerContainer("Simple RainDrawer " + (drawers.Count() + i), base.transform);
				simpleRainDrawerContainer.currentState = DrawState.Disabled;
				drawers.Add(simpleRainDrawerContainer);
			}
		}
		if (num < 0)
		{
			int num2 = -num;
			List<SimpleRainDrawerContainer> list = drawers.FindAll((SimpleRainDrawerContainer x) => x.currentState != DrawState.Playing).Take(num2).ToList();
			if (list.Count() < num2)
			{
				list.AddRange(drawers.FindAll((SimpleRainDrawerContainer x) => x.currentState == DrawState.Playing).Take(num2 - list.Count()));
			}
			foreach (SimpleRainDrawerContainer item in list)
			{
				item.Drawer.Hide();
				UnityEngine.Object.DestroyImmediate(item.Drawer.gameObject);
			}
			drawers.RemoveAll((SimpleRainDrawerContainer x) => x.Drawer == null);
		}
	}

	private void CheckSpawnTime()
	{
		if (interval == 0f)
		{
			interval = Variables.Duration / (float)RainDropTools.Random(Variables.EmissionRateMin, Variables.EmissionRateMax);
		}
		timeElapsed += Time.deltaTime;
		if (timeElapsed >= interval)
		{
			int num = (int)Mathf.Min(timeElapsed / interval, Variables.MaxRainSpawnCount - drawers.FindAll((SimpleRainDrawerContainer x) => x.currentState == DrawState.Playing).Count());
			for (int i = 0; i < num; i++)
			{
				Spawn();
			}
			interval = Variables.Duration / (float)RainDropTools.Random(Variables.EmissionRateMin, Variables.EmissionRateMax);
			timeElapsed = 0f;
		}
	}

	private void Spawn()
	{
		SimpleRainDrawerContainer simpleRainDrawerContainer = drawers.Find((SimpleRainDrawerContainer x) => x.currentState == DrawState.Disabled);
		if (simpleRainDrawerContainer != null)
		{
			InitializeDrawer(simpleRainDrawerContainer);
			simpleRainDrawerContainer.currentState = DrawState.Playing;
		}
	}

	private float GetProgress(SimpleRainDrawerContainer dc)
	{
		return dc.TimeElapsed / dc.lifetime;
	}

	private void InitializeDrawer(SimpleRainDrawerContainer dc)
	{
		dc.TimeElapsed = 0f;
		dc.lifetime = RainDropTools.Random(Variables.LifetimeMin, Variables.LifetimeMax);
		dc.transform.localPosition = RainDropTools.GetSpawnLocalPos(base.transform, camera, 0f, Variables.SpawnOffsetY);
		dc.startPos = dc.transform.localPosition;
		dc.startSize = new Vector3(RainDropTools.Random(Variables.SizeMinX, Variables.SizeMaxX), RainDropTools.Random(Variables.SizeMinY, Variables.SizeMaxY), 1f);
		dc.transform.localRotation = Quaternion.Euler(Vector3.zero + Vector3.forward * ((!Variables.AutoRotate) ? 0f : UnityEngine.Random.Range(0f, 179.9f)));
		dc.Drawer.NormalMap = Variables.NormalMap;
		dc.Drawer.ReliefTexture = Variables.OverlayTexture;
		dc.Drawer.Darkness = Variables.Darkness;
		dc.Drawer.Hide();
	}

	private void UpdateShader(SimpleRainDrawerContainer dc, int index)
	{
		float progress = GetProgress(dc);
		dc.Drawer.RenderQueue = RenderQueue + index;
		dc.Drawer.NormalMap = Variables.NormalMap;
		dc.Drawer.ReliefTexture = Variables.OverlayTexture;
		dc.Drawer.OverlayColor = new Color(Variables.OverlayColor.r, Variables.OverlayColor.g, Variables.OverlayColor.b, Variables.OverlayColor.a * Variables.AlphaOverLifetime.Evaluate(progress) * Alpha);
		dc.Drawer.DistortionStrength = Variables.DistortionValue * Variables.DistortionOverLifetime.Evaluate(progress) * Alpha;
		dc.Drawer.ReliefValue = Variables.ReliefValue * Variables.ReliefOverLifetime.Evaluate(progress) * Alpha;
		dc.Drawer.Blur = Variables.Blur * Variables.BlurOverLifetime.Evaluate(progress) * Alpha;
		dc.Drawer.Darkness = Variables.Darkness * Alpha;
		dc.transform.localScale = dc.startSize * Variables.SizeOverLifetime.Evaluate(progress);
		dc.transform.localPosition = dc.startPos + Vector3.up * Variables.PosYOverLifetime.Evaluate(progress);
		dc.Drawer.ShaderType = ShaderType;
		dc.Drawer.Show();
	}

	private void UpdateInstance(SimpleRainDrawerContainer dc, int index)
	{
		if (dc.currentState == DrawState.Playing)
		{
			if (GetProgress(dc) >= 1f)
			{
				dc.Drawer.Hide();
				dc.currentState = DrawState.Disabled;
			}
			else
			{
				dc.TimeElapsed += Time.deltaTime;
				UpdateShader(dc, index);
			}
		}
	}
}
