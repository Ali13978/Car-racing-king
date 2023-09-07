using UnityEngine;

public class LoadTrack : MonoBehaviour
{
	public string TrackName;

	private void Awake()
	{
		Application.LoadLevelAdditive(TrackName);
	}
}
