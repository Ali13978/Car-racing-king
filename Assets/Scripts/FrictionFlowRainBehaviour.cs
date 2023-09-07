using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class FrictionFlowRainBehaviour : RainBehaviourBase
{
	[SerializeField]
	public FrictionFlowRainVariables Variables;

	[HideInInspector]
	private FrictionFlowRainController rainController
	{
		get;
		set;
	}

	public override int CurrentDrawCall
	{
		get
		{
			if (rainController == null)
			{
				return 0;
			}
			return rainController.drawers.FindAll((FrictionFlowRainController.FrictionFlowRainDrawerContainer x) => x.Drawer.enabled).Count();
		}
	}

	public override int MaxDrawCall => Variables.MaxRainSpawnCount;

	public override bool IsPlaying
	{
		get
		{
			if (rainController == null)
			{
				return false;
			}
			return rainController.IsPlaying;
		}
	}

	public override bool IsEnabled => Alpha != 0f && CurrentDrawCall != 0;

	public override void Refresh()
	{
		if (rainController != null)
		{
			UnityEngine.Object.DestroyImmediate(rainController.gameObject);
			rainController = null;
		}
		rainController = CreateController();
		rainController.Refresh();
		rainController.NoMoreRain = true;
	}

	public override void StartRain()
	{
		if (rainController == null)
		{
			rainController = CreateController();
			rainController.Refresh();
		}
		rainController.NoMoreRain = false;
		rainController.Play();
	}

	public override void StopRain()
	{
		if (!(rainController == null))
		{
			rainController.NoMoreRain = true;
		}
	}

	public override void StopRainImmidiate()
	{
		if (!(rainController == null))
		{
			UnityEngine.Object.DestroyImmediate(rainController.gameObject);
			rainController = null;
		}
	}

	public override void ApplyFinalDepth(int depth)
	{
		if (!(rainController == null))
		{
			rainController.RenderQueue = depth;
		}
	}

	private void Start()
	{
		if (Application.isPlaying && Variables.AutoStart)
		{
			StartRain();
		}
	}

	public override void Update()
	{
		InitParams();
		if (!(rainController == null))
		{
			rainController.ShaderType = ShaderType;
			rainController.Alpha = Alpha;
			rainController.UpdateController();
		}
	}

	private FrictionFlowRainController CreateController()
	{
		Transform transform = RainDropTools.CreateHiddenObject("Controller", base.transform);
		FrictionFlowRainController frictionFlowRainController = transform.gameObject.AddComponent<FrictionFlowRainController>();
		frictionFlowRainController.Variables = Variables;
		frictionFlowRainController.Alpha = 0f;
		frictionFlowRainController.NoMoreRain = false;
		frictionFlowRainController.camera = GetComponentInParent<Camera>();
		return frictionFlowRainController;
	}

	public void InitParams()
	{
		if (Variables != null)
		{
			if (Variables.SizeMinX > Variables.SizeMaxX)
			{
				swap(ref Variables.SizeMinX, ref Variables.SizeMaxX);
			}
			if (Variables.LifetimeMin > Variables.LifetimeMax)
			{
				swap(ref Variables.LifetimeMin, ref Variables.LifetimeMax);
			}
			if (Variables.AccelerationMin > Variables.AccelerationMax)
			{
				swap(ref Variables.AccelerationMin, ref Variables.AccelerationMax);
			}
			if (Variables.EmissionRateMin > Variables.EmissionRateMax)
			{
				swap(ref Variables.EmissionRateMin, ref Variables.EmissionRateMax);
			}
		}
	}

	private void swap(ref float a, ref float b)
	{
		float num = a;
		a = b;
		b = num;
	}

	private void swap(ref int a, ref int b)
	{
		int num = a;
		a = b;
		b = num;
	}
}
