using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScene1 : MonoBehaviour
{
	private enum PlayMode
	{
		Home,
		Rain,
		Blood,
		SplashIn,
		SplashOut,
		Frozen
	}

	[SerializeField]
	private List<RainCameraController> rainControllers;

	private PlayMode playMode;

	private float frozenValue;

	private float rainAlpha = 1f;

	private void Awake()
	{
		SetResolution(512);
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Application.targetFrameRate = 60;
	}

	private void SetResolution(int resolution)
	{
		float num = Mathf.Min(1f, (float)resolution / (float)Screen.height);
		int width = (int)((float)Screen.width * num);
		int height = (int)((float)Screen.height * num);
		Screen.SetResolution(width, height, fullscreen: true, 15);
	}

	private void StopAll()
	{
		foreach (RainCameraController rainController in rainControllers)
		{
			rainController.StopImmidiate();
		}
	}

	private IEnumerator Start()
	{
		yield return null;
		StopAll();
	}

	private void OnGUI()
	{
		int num = 0;
		foreach (RainCameraController rainController in rainControllers)
		{
			if (GuiButton($"Rain[{num}]"))
			{
				StopAll();
				rainController.Play();
			}
			num++;
		}
	}

	private bool GuiButton(string buttonName)
	{
		return GUILayout.Button(buttonName, GUILayout.Height(40f), GUILayout.Width(150f));
	}
}
