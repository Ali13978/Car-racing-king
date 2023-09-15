using RacingGameKit.RGKCar;
using UnityEngine;

namespace RacingGameKit.UI
{
	[AddComponentMenu("Racing Game Kit/UI/SpeedoUI Connector")]
	[RequireComponent(typeof(RGKCar_Engine))]
	public class RGKUI_RGKCar_SpeedoConnector : MonoBehaviour
	{
		private Race_Manager m_RaceManager;

		private RGKCar_Setup m_RGKCarSetup;

		private RGKCar_Engine m_RGKCarEngine;

		private bool IsSplitScreen = false;

		private SpeedoUI m_SpeedoUI;

		private void Start()
		{
			m_RGKCarSetup = GetComponent<RGKCar_Setup>();
			m_RGKCarEngine = GetComponent<RGKCar_Engine>();
			GameObject gameObject = GameObject.Find("_RaceManager");
			if (gameObject != null)
			{
				m_RaceManager = gameObject.GetComponent<Race_Manager>();
				//if (m_RaceManager.SplitScreen)
				//{
				//	IsSplitScreen = true;
				//}
			}
			if (!IsSplitScreen)
			{
				GameObject gameObject2 = GameObject.Find("SpeedoUI");
				if (gameObject2 != null)
				{
					m_SpeedoUI = gameObject2.GetComponent<SpeedoUI>();
				}
			}
			else if (m_RaceManager.Player1 == base.gameObject)
			{
				GameObject gameObject3 = GameObject.Find("SpeedoUIP1");
				if (gameObject3 != null)
				{
					m_SpeedoUI = gameObject3.GetComponent<SpeedoUI>();
				}
			}
			else
			{
				GameObject gameObject4 = GameObject.Find("SpeedoUIP2");
				if (gameObject4 != null)
				{
					m_SpeedoUI = gameObject4.GetComponent<SpeedoUI>();
				}
			}
		}

		private void FixedUpdate()
		{
			if (m_SpeedoUI != null)
			{
				m_SpeedoUI.Speed = m_RGKCarEngine.SpeedAsKM;
				string gear = m_RGKCarEngine.Gear.ToString();
				if (m_RGKCarEngine.Gear == 0)
				{
					gear = "N";
				}
				else if (m_RGKCarEngine.Gear == -1)
				{
					gear = "R";
				}
				m_SpeedoUI.Gear = gear;
				m_SpeedoUI.Rpm = m_RGKCarEngine.RPM;
				m_SpeedoUI.Nitro = m_RGKCarSetup.Nitro.NitroLeft / m_RGKCarSetup.Nitro.InitialAmount * 100f;
			}
		}
	}
}
