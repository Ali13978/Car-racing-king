using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlowRainController : MonoBehaviour
{
	public enum DrawState
	{
		Playing,
		Disabled
	}

	[Serializable]
	public class FlowRainDrawerContainer : RainDrawerContainer<TrailRenderer>
	{
		public DrawState currentState = DrawState.Disabled;

		public float initRnd;

		public float posXDt;

		public float rnd1;

		public float fluctuationRate = 5f;

		public float acceleration = 0.1f;

		public Vector3 startPos;

		public float TimeElapsed;

		public float lifetime;

		public bool IsEnable => Drawer.material != null && Drawer.enabled;

		public FlowRainDrawerContainer(string name, Transform parent)
			: base(name, parent)
		{
		}
	}

	private int oldSpawnLimit;

	private bool isOneShot;

	private bool isWaitingDelay;

	private float oneShotTimeleft;

	private float timeElapsed;

	private float interval;

	public List<FlowRainDrawerContainer> drawers = new List<FlowRainDrawerContainer>();

	public FlowRainVariables Variables
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

	public bool IsPlaying => drawers.FindAll((FlowRainDrawerContainer t) => t.currentState == DrawState.Disabled).Count != drawers.Count;

	public void Refresh()
	{
		foreach (FlowRainDrawerContainer drawer in drawers)
		{
			UnityEngine.Object.DestroyImmediate(drawer.Drawer.gameObject);
		}
		drawers.Clear();
		for (int i = 0; i < Variables.MaxRainSpawnCount; i++)
		{
			FlowRainDrawerContainer flowRainDrawerContainer = new FlowRainDrawerContainer("Flow RainDrawer " + i, base.transform);
			flowRainDrawerContainer.currentState = DrawState.Disabled;
			drawers.Add(flowRainDrawerContainer);
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
		if (drawers.Find((FlowRainDrawerContainer x) => x.currentState == DrawState.Playing) == null)
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
		for (int i = 0; i < drawers.Count; i++)
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
				FlowRainDrawerContainer flowRainDrawerContainer = new FlowRainDrawerContainer("Flow RainDrawer " + (drawers.Count() + i), base.transform);
				flowRainDrawerContainer.currentState = DrawState.Disabled;
				drawers.Add(flowRainDrawerContainer);
			}
		}
		if (num < 0)
		{
			int num2 = -num;
			List<FlowRainDrawerContainer> list = drawers.FindAll((FlowRainDrawerContainer x) => x.currentState != DrawState.Playing).Take(num2).ToList();
			if (list.Count() < num2)
			{
				list.AddRange(drawers.FindAll((FlowRainDrawerContainer x) => x.currentState == DrawState.Playing).Take(num2 - list.Count()));
			}
			foreach (FlowRainDrawerContainer item in list)
			{
				item.Drawer.Clear();
				UnityEngine.Object.DestroyImmediate(item.Drawer.gameObject);
			}
			drawers.RemoveAll((FlowRainDrawerContainer x) => x.Drawer == null);
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
			int num = (int)Mathf.Min(timeElapsed / interval, Variables.MaxRainSpawnCount - drawers.FindAll((FlowRainDrawerContainer x) => x.currentState == DrawState.Playing).Count());
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
		FlowRainDrawerContainer flowRainDrawerContainer = drawers.Find((FlowRainDrawerContainer x) => x.currentState == DrawState.Disabled);
		if (flowRainDrawerContainer != null)
		{
			InitializeDrawer(flowRainDrawerContainer);
			flowRainDrawerContainer.currentState = DrawState.Playing;
		}
	}

	private float GetProgress(FlowRainDrawerContainer dc)
	{
		return dc.TimeElapsed / dc.lifetime;
	}

	private void InitializeDrawer(FlowRainDrawerContainer dc)
	{
		dc.TimeElapsed = 0f;
		dc.lifetime = RainDropTools.Random(Variables.LifetimeMin, Variables.LifetimeMax);
		dc.fluctuationRate = RainDropTools.Random(Variables.fluctuationRateMin, Variables.fluctuationRateMax);
		dc.acceleration = RainDropTools.Random(Variables.AccelerationMin, Variables.AccelerationMax);
		dc.transform.localPosition = RainDropTools.GetSpawnLocalPos(base.transform, camera, 0f, Variables.SpawnOffsetY);
		dc.startPos = dc.transform.localPosition;
		dc.acceleration = RainDropTools.Random(Variables.AccelerationMin, Variables.AccelerationMax);
		Material material = RainDropTools.CreateRainMaterial(ShaderType, RenderQueue);
		RainDropTools.ApplyRainMaterialValue(material, ShaderType, Variables.NormalMap, Variables.OverlayTexture, Variables.DistortionValue, Variables.OverlayColor, Variables.ReliefValue, Variables.Blur, Variables.Darkness);
		dc.Drawer.material = material;
		dc.Drawer.widthCurve = Variables.TrailWidth;
		dc.Drawer.widthMultiplier = RainDropTools.Random(Variables.SizeMinX, Variables.SizeMaxX);
		dc.Drawer.textureMode = LineTextureMode.Stretch;
		dc.Drawer.Clear();
		dc.Drawer.enabled = false;
	}

	private void UpdateTransform(FlowRainDrawerContainer dc)
	{
		Action initRnd = delegate
		{
			dc.rnd1 = RainDropTools.Random(-0.1f * Variables.Amplitude, 0.1f * Variables.Amplitude);
			dc.posXDt = 0f;
		};
		if (dc.posXDt == 0f)
		{
			StartCoroutine(Wait(0.01f, 0.01f, (int)(1f / dc.fluctuationRate * 100f), delegate
			{
				initRnd();
			}));
		}
		dc.posXDt += 0.01f * Variables.Smooth * Time.deltaTime;
		if (dc.rnd1 == 0f)
		{
			initRnd();
		}
		float num = dc.TimeElapsed;
		Transform transform = dc.transform;
		Vector3 vector = Vector3.Slerp(dc.transform.localPosition, dc.transform.localPosition + Vector3.right * dc.rnd1, dc.posXDt);
		transform.localPosition = new Vector3(vector.x, dc.startPos.y - 0.5f * num * num * dc.acceleration - Variables.InitialVelocity * num, 0.001f);
	}

	private void UpdateShader(FlowRainDrawerContainer dc, int index)
	{
		float progress = GetProgress(dc);
		dc.Drawer.material.renderQueue = RenderQueue + index;
		if (dc.Drawer.material.shader.name != RainDropTools.GetShaderName(ShaderType))
		{
			dc.Drawer.material = RainDropTools.CreateRainMaterial(ShaderType, RenderQueue + index);
		}
		float num = Variables.DistortionValue * Variables.DistortionOverLifetime.Evaluate(progress) * Alpha;
		float num2 = Variables.ReliefValue * Variables.ReliefOverLifetime.Evaluate(progress) * Alpha;
		float num3 = Variables.Blur * Variables.BlurOverLifetime.Evaluate(progress) * Alpha;
		Color value = new Color(Variables.OverlayColor.r, Variables.OverlayColor.g, Variables.OverlayColor.b, Variables.OverlayColor.a * Variables.AlphaOverLifetime.Evaluate(progress) * Alpha);
		switch (ShaderType)
		{
		case RainDropTools.RainDropShaderType.Expensive:
			if (num == 0f && num2 == 0f && value.a == 0f && num3 == 0f)
			{
				dc.Drawer.enabled = false;
				return;
			}
			break;
		case RainDropTools.RainDropShaderType.Cheap:
			if (num == 0f)
			{
				dc.Drawer.enabled = false;
				return;
			}
			break;
		case RainDropTools.RainDropShaderType.NoDistortion:
			if (num2 == 0f && value.a == 0f)
			{
				dc.Drawer.enabled = false;
				return;
			}
			break;
		}
		RainDropTools.ApplyRainMaterialValue(dc.Drawer.material, ShaderType, Variables.NormalMap, Variables.OverlayTexture, num, value, num2, num3, Variables.Darkness * Alpha);
		dc.Drawer.enabled = true;
	}

	private void UpdateInstance(FlowRainDrawerContainer dc, int index)
	{
		if (dc.currentState == DrawState.Playing)
		{
			if (GetProgress(dc) >= 1f)
			{
				dc.Drawer.Clear();
				dc.currentState = DrawState.Disabled;
			}
			else
			{
				dc.TimeElapsed += Time.deltaTime;
				UpdateTransform(dc);
				UpdateShader(dc, index);
			}
		}
	}

	private IEnumerator Wait(float atLeast = 0.5f, float step = 0.1f, int rndMax = 20, Action callBack = null)
	{
		float elapsed2 = 0f;
		while (elapsed2 < atLeast)
		{
			elapsed2 += Time.deltaTime;
			yield return null;
		}
		while (RainDropTools.Random(0, rndMax) != 0)
		{
			elapsed2 = 0f;
			while (elapsed2 < step)
			{
				elapsed2 += Time.deltaTime;
				yield return null;
			}
		}
		callBack?.Invoke();
	}
}
