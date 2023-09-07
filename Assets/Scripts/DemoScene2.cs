using UnityEngine;

public class DemoScene2 : MonoBehaviour
{
	private enum PlayMode
	{
		None,
		Blood,
		SplashIn,
		SplashOut,
		Frozen
	}

	[SerializeField]
	private BloodRainCameraController bloodRainController;

	[SerializeField]
	private RainCameraController splashInRain;

	[SerializeField]
	private RainCameraController splashOutRain;

	[SerializeField]
	private RainCameraController frozenRain;

	[SerializeField]
	private AudioSource splashInAudio;

	[SerializeField]
	private AudioSource splashOutAudio;

	[SerializeField]
	private AudioSource damageAudio;

	[SerializeField]
	private AudioSource windAudio;

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
		bloodRainController.Reset();
		splashInRain.StopImmidiate();
		splashOutRain.StopImmidiate();
		frozenRain.StopImmidiate();
		splashInAudio.Stop();
		splashOutAudio.Stop();
		damageAudio.Stop();
		windAudio.Stop();
	}

	private void OnGUI()
	{
		if (playMode != 0)
		{
			if (GuiButton("GoBack"))
			{
				StopAll();
				playMode = PlayMode.None;
			}
		}
		else
		{
			if (GuiButton("Blood"))
			{
				playMode = PlayMode.Blood;
			}
			if (GuiButton("Splash (in)"))
			{
				playMode = PlayMode.SplashIn;
			}
			if (GuiButton("Splash (out)"))
			{
				playMode = PlayMode.SplashOut;
			}
			if (GuiButton("Frozen"))
			{
				frozenValue = 0f;
				frozenRain.Play();
				windAudio.Play();
				playMode = PlayMode.Frozen;
			}
		}
		if (playMode == PlayMode.Blood)
		{
			if (GuiButton("Hit Damage"))
			{
				if (bloodRainController.HP <= 30)
				{
					bloodRainController.Reset();
					bloodRainController.HP = 100;
				}
				else
				{
					damageAudio.Play();
					bloodRainController.Attack(30);
				}
			}
			if (GuiButton("Reset"))
			{
				bloodRainController.Reset();
			}
			GUILayout.Label("Current HP = " + bloodRainController.HP.ToString());
			return;
		}
		if (playMode == PlayMode.SplashIn && GuiButton("Play Effect"))
		{
			splashInAudio.Play();
			splashInRain.Refresh();
			splashInRain.Play();
		}
		if (playMode == PlayMode.SplashOut && GuiButton("Play Effect"))
		{
			splashOutAudio.Play();
			splashOutRain.Refresh();
			splashOutRain.Play();
		}
		if (playMode == PlayMode.Frozen)
		{
			frozenRain.Alpha = frozenValue;
			GUILayout.Label("Frozen Value (Sliding right to freeze)");
			frozenValue = GUILayout.HorizontalSlider(frozenValue, 0f, 1f, GUILayout.Height(40f));
			windAudio.volume = frozenValue;
		}
	}

	private bool GuiButton(string buttonName)
	{
		return GUILayout.Button(buttonName, GUILayout.Height(40f), GUILayout.Width(150f));
	}
}
