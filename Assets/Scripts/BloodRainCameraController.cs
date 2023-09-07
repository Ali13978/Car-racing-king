using UnityEngine;

public class BloodRainCameraController : MonoBehaviour
{
	public RainCameraController FrameBloodCamera;

	public RainCameraController SplatterBloodCamera;

	public int HP = 100;

	public float FrameEffectInterval = 1f;

	public float Smooth = 2f;

	private float timeElapsed;

	private float currentAlpha;

	private float oldAlpha;

	private float lerpStart;

	private float lerpTime;

	[SerializeField]
	private AnimationCurve hpHigh;

	[SerializeField]
	private AnimationCurve hpMid;

	[SerializeField]
	private AnimationCurve hpLow;

	public void Attack(int damage)
	{
		HP = Mathf.Max(0, HP - damage);
		SplatterBloodCamera.Refresh();
		SplatterBloodCamera.Play();
	}

	public void Reset()
	{
		HP = 100;
		ResetLerpTime();
		FrameBloodCamera.Refresh();
		SplatterBloodCamera.Refresh();
	}

	private void Update()
	{
		currentAlpha = (float)(100 - HP) / 100f;
		if (currentAlpha != oldAlpha)
		{
			lerpTime = 0f;
			lerpStart = oldAlpha;
			oldAlpha = currentAlpha;
		}
		FrameBloodCamera.Play();
		timeElapsed += Time.deltaTime;
		if (timeElapsed > FrameEffectInterval)
		{
			timeElapsed -= FrameEffectInterval;
		}
		lerpTime += Smooth * Time.deltaTime;
		if (HP == 100)
		{
			FrameBloodCamera.Alpha = 0f;
		}
		else if (HP >= 70)
		{
			FrameBloodCamera.Alpha = currentAlpha * LerpTime(lerpTime) * hpHigh.Evaluate(timeElapsed);
		}
		else if (HP >= 20)
		{
			FrameBloodCamera.Alpha = currentAlpha * LerpTime(lerpTime) * hpMid.Evaluate(timeElapsed);
		}
		else
		{
			FrameBloodCamera.Alpha = currentAlpha * LerpTime(lerpTime) * hpLow.Evaluate(timeElapsed);
		}
	}

	private float LerpTime(float lerpTime)
	{
		return Mathf.Lerp(lerpStart, currentAlpha, lerpTime);
	}

	private void ResetLerpTime()
	{
		lerpTime = 0f;
	}
}
