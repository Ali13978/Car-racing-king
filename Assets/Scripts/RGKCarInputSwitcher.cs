using RacingGameKit;
using RacingGameKit.RGKCar.CarControllers;
using UnityEngine;

public class RGKCarInputSwitcher : MonoBehaviour
{
	public bool m_UseGamePad;

	private bool m_SetByExternalCall;

	private Race_Manager m_RaceManager;

	private RGKCar_C2_Human m_KeyboarcController;

	private RGKCar_C2_MobileRace m_MobileController;

	private RGKCar_C2_GamePadRace m_GamePadController;

	private void Awake()
	{
		GameObject gameObject = GameObject.Find("_RaceManager");
		if (gameObject != null)
		{
			m_RaceManager = gameObject.GetComponent<Race_Manager>();
		}
		m_KeyboarcController = GetComponent<RGKCar_C2_Human>();
		m_MobileController = GetComponent<RGKCar_C2_MobileRace>();
		m_GamePadController = GetComponent<RGKCar_C2_GamePadRace>();
		if (m_KeyboarcController != null)
		{
			m_KeyboarcController.enabled = false;
		}
		if (m_GamePadController != null)
		{
			m_GamePadController.enabled = false;
		}
		if (m_MobileController != null)
		{
			m_MobileController.enabled = false;
		}
	}

	private void Start()
	{
		if (m_SetByExternalCall || m_RaceManager.SplitScreen)
		{
			return;
		}
		if (Application.platform == RuntimePlatform.tvOS)
		{
			m_KeyboarcController.enabled = false;
			m_GamePadController.enabled = false;
			m_MobileController.enabled = true;
			m_MobileController.UseXAxis = false;
		}
		else if (Application.isMobilePlatform)
		{
			m_KeyboarcController.enabled = false;
			m_GamePadController.enabled = false;
			m_MobileController.enabled = true;
			if (Application.platform == RuntimePlatform.Android)
			{
				m_MobileController.UseXAxis = true;
				UnityEngine.Debug.Log("Switching Player Controller To Android!");
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				m_MobileController.UseXAxis = false;
				UnityEngine.Debug.Log("Switching Player Controller To iOS!");
			}
		}
		else if (!m_UseGamePad)
		{
			m_KeyboarcController.enabled = true;
			m_MobileController.enabled = false;
			m_GamePadController.enabled = false;
			UnityEngine.Debug.Log("Switching Player Controller To Keyboard!");
		}
		else
		{
			m_GamePadController.enabled = true;
			m_KeyboarcController.enabled = false;
			m_MobileController.enabled = false;
			UnityEngine.Debug.Log("Switching Player Controller To GamePad!");
		}
	}

	public void SwitchController(bool ToGamePad)
	{
		m_UseGamePad = ToGamePad;
		if (!m_UseGamePad)
		{
			m_KeyboarcController.enabled = true;
			m_MobileController.enabled = false;
			m_GamePadController.enabled = false;
			UnityEngine.Debug.Log("Switching Player Controller To Keyboard!");
		}
		else
		{
			m_GamePadController.enabled = true;
			m_KeyboarcController.enabled = false;
			m_MobileController.enabled = false;
			UnityEngine.Debug.Log("Switching Player Controller To GamePad!");
		}
		m_SetByExternalCall = true;
	}
}
