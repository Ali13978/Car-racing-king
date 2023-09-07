using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
	private float interval = 0.2f;

	private float startTime;

	private float dt;

	private int flameCnt;

	private int fps;

	private void LateUpdate()
	{
		dt = Time.time - startTime;
		flameCnt++;
		if (dt >= interval)
		{
			fps = (int)((float)flameCnt / dt);
			flameCnt = 0;
			startTime = Time.time;
		}
	}

	private void OnGUI()
	{
		int width = Screen.width;
		int height = Screen.height;
		GUIStyle gUIStyle = new GUIStyle();
		Rect position = new Rect(0f, height - height / 10, width, height / 10);
		gUIStyle.alignment = TextAnchor.UpperLeft;
		gUIStyle.fontSize = height / 10;
		gUIStyle.normal.textColor = Color.white;
		string text = $"FPS:{fps}";
		GUI.Label(position, text, gUIStyle);
	}
}
