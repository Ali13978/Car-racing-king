using UnityEngine;

public abstract class RainBehaviourBase : MonoBehaviour
{
	public int Depth;

	[HideInInspector]
	public float Alpha;

	[HideInInspector]
	public RainDropTools.RainDropShaderType ShaderType;

	public virtual bool IsPlaying => false;

	public virtual bool IsEnabled => false;

	public virtual int CurrentDrawCall => 0;

	public virtual int MaxDrawCall => 0;

	public virtual void Refresh()
	{
	}

	public virtual void StartRain()
	{
	}

	public virtual void StopRain()
	{
	}

	public virtual void StopRainImmidiate()
	{
	}

	public virtual void ApplyFinalDepth(int depth)
	{
	}

	public virtual void Awake()
	{
	}

	public virtual void Update()
	{
	}
}
