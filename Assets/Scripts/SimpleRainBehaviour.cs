using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class SimpleRainBehaviour : RainBehaviourBase
{
	[SerializeField]
	public SimpleRainVariables Variables;

	[HideInInspector]
	private SimpleRainController rainController
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
			return rainController.drawers.FindAll((SimpleRainController.SimpleRainDrawerContainer x) => x.Drawer.IsEnabled).Count();
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

	private SimpleRainController CreateController()
	{
		Transform transform = RainDropTools.CreateHiddenObject("Controller", base.transform);
		SimpleRainController simpleRainController = transform.gameObject.AddComponent<SimpleRainController>();
		simpleRainController.Variables = Variables;
		simpleRainController.Alpha = 0f;
		simpleRainController.NoMoreRain = false;
		simpleRainController.camera = GetComponentInParent<Camera>();
		return simpleRainController;
	}

	public void InitParams()
	{
		if (Variables != null)
		{
			if (Variables.MaxRainSpawnCount < 0)
			{
				Variables.MaxRainSpawnCount = 0;
			}
			if (Variables.LifetimeMin > Variables.LifetimeMax)
			{
				swap(ref Variables.LifetimeMin, ref Variables.LifetimeMax);
			}
			if (Variables.EmissionRateMin > Variables.EmissionRateMax)
			{
				swap(ref Variables.EmissionRateMin, ref Variables.EmissionRateMax);
			}
			if (Variables.SizeMinX > Variables.SizeMaxX)
			{
				swap(ref Variables.SizeMinX, ref Variables.SizeMaxX);
			}
			if (Variables.SizeMinY > Variables.SizeMaxY)
			{
				swap(ref Variables.SizeMinY, ref Variables.SizeMaxY);
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
