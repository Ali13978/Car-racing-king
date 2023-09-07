using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class RainCameraController : MonoBehaviour
{
	private Camera _cam;

	private List<RainBehaviourBase> _rainBehaviours;

	public int RenderQueue = 3000;

	[Range(0f, 1f)]
	public float Alpha = 1f;

	public RainDropTools.RainDropShaderType ShaderType;

	private Camera cam
	{
		get
		{
			if (_cam == null)
			{
				_cam = GetComponent<Camera>();
			}
			return _cam;
		}
	}

	public List<RainBehaviourBase> rainBehaviours
	{
		get
		{
			if (_rainBehaviours == null)
			{
				_rainBehaviours = GetComponentsInChildren<RainBehaviourBase>(includeInactive: false).ToList();
			}
			return _rainBehaviours;
		}
	}

	public int CurrentDrawCall => (from x in rainBehaviours
		select x.CurrentDrawCall).Sum();

	public int MaxDrawCall => (from x in rainBehaviours
		select x.MaxDrawCall).Sum();

	public bool IsPlaying => rainBehaviours.FindAll((RainBehaviourBase x) => x.IsPlaying).Count != 0;

	private void Awake()
	{
		foreach (RainBehaviourBase rainBehaviour in rainBehaviours)
		{
			rainBehaviour.StopRainImmidiate();
		}
	}

	private void Start()
	{
		if (cam == null)
		{
			UnityEngine.Debug.LogError("You must add component (Camera)");
		}
	}

	private void Update()
	{
		if (!(cam == null))
		{
			cam.orthographic = true;
			cam.orthographicSize = 6.6f;
			cam.nearClipPlane = -0.1f;
			cam.farClipPlane = 0.1f;
			if (base.transform.childCount != _rainBehaviours.Count())
			{
				_rainBehaviours = null;
			}
			rainBehaviours.Sort((RainBehaviourBase a, RainBehaviourBase b) => a.Depth - b.Depth);
			int num = 0;
			int num2 = 0;
			foreach (RainBehaviourBase rainBehaviour in rainBehaviours)
			{
				rainBehaviour.transform.localRotation = Quaternion.Euler(Vector3.zero);
				rainBehaviour.transform.localScale = Vector3.one;
				if (Application.isPlaying)
				{
					rainBehaviour.transform.localPosition = new Vector3(0f, 0f, 0.0001f * (float)num2);
				}
				else
				{
					rainBehaviour.transform.localPosition = Vector3.zero;
				}
				rainBehaviour.ShaderType = ShaderType;
				rainBehaviour.ApplyFinalDepth(RenderQueue + num);
				rainBehaviour.Alpha = Alpha;
				num += rainBehaviour.MaxDrawCall;
				num2++;
			}
		}
	}

	public void Refresh()
	{
		foreach (RainBehaviourBase rainBehaviour in rainBehaviours)
		{
			rainBehaviour.StopRainImmidiate();
		}
		_rainBehaviours = GetComponentsInChildren<RainBehaviourBase>(includeInactive: false).ToList();
		foreach (RainBehaviourBase rainBehaviour2 in rainBehaviours)
		{
			rainBehaviour2.Refresh();
		}
	}

	public void Play()
	{
		foreach (RainBehaviourBase rainBehaviour in rainBehaviours)
		{
			rainBehaviour.StartRain();
		}
	}

	public void Stop()
	{
		foreach (RainBehaviourBase rainBehaviour in rainBehaviours)
		{
			rainBehaviour.StopRain();
		}
	}

	public void StopImmidiate()
	{
		foreach (RainBehaviourBase rainBehaviour in rainBehaviours)
		{
			rainBehaviour.StopRainImmidiate();
		}
	}

	private void OnDrawGizmos()
	{
		if (!(cam == null))
		{
			float num = cam.orthographicSize * 2f;
			float x = num * cam.aspect;
			Gizmos.color = new Color(0f, 0.1f, 0.7f, 0.1f);
			Gizmos.DrawCube(base.transform.position, new Vector3(x, num, cam.farClipPlane - cam.nearClipPlane));
			Gizmos.color = new Color(0f, 0.1f, 0.7f, 0.8f);
			Gizmos.DrawWireCube(base.transform.position, new Vector3(x, num, cam.farClipPlane - cam.nearClipPlane));
		}
	}
}
