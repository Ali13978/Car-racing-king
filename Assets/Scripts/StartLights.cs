using RacingGameKit;
using UnityEngine;

public class StartLights : MonoBehaviour
{
	private Race_Manager m_RaceManager;

	public Light Light3;

	public Light Light2;

	public Light Light1;

	public Light Light0;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("_RaceManager");
		if (gameObject != null)
		{
			m_RaceManager = gameObject.GetComponent<Race_Manager>();
			Light0.enabled = false;
			Light1.enabled = false;
			Light2.enabled = false;
			Light3.enabled = false;
		}
		else
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (base.enabled && m_RaceManager != null && m_RaceManager.CurrentCount >= 0 && m_RaceManager.CurrentCount < 4)
		{
			switch (m_RaceManager.CurrentCount)
			{
			case 3:
				Light3.enabled = true;
				break;
			case 2:
				Light2.enabled = true;
				break;
			case 1:
				Light1.enabled = true;
				break;
			case 0:
				Light1.enabled = false;
				Light2.enabled = false;
				Light3.enabled = false;
				Light0.enabled = true;
				break;
			}
		}
	}
}
