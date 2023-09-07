using RacingGameKit.Interfaces;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RacingGameKit.UI
{
	[AddComponentMenu("Racing Game Kit/UI/Race UI for Split Screen")]
	public class RGKUI_SplitScreen : MonoBehaviour, IRGKUI
	{
		public GameObject m_UiCanvas;

		public Sprite m_SpriteStartRed;

		public Sprite m_SpriteStartYellow;

		public Sprite m_SpriteStartGreen;

		private GameObject m_pnlCountDown;

		private GameObject m_imgCountDown3;

		private GameObject m_imgCountDown2;

		private GameObject m_imgCountDown1;

		private GameObject m_pnlMessage;

		private GameObject m_pnlTimer;

		private Text m_lblTimerMessage;

		private Text m_lblTimerValue;

		private GameObject m_pnlWrongWayP1;

		private GameObject m_pnlWrongWayP2;

		private bool m_IsWrongWayP1Active;

		private bool m_IsWrongWayP2Active;

		private GameObject m_pnlLapPosP1;

		private Text m_lblHudLapValueP1;

		private Text m_lblHudLapTextP1;

		private Text m_lblHudPosValueP1;

		private Text m_lblHudCurrentTimeValueP1;

		private Text m_lblHudBestTimeValueP1;

		private Text m_lblHudBestTimeTextP1;

		private GameObject m_pnlLapPosP2;

		private Text m_lblHudLapValueP2;

		private Text m_lblHudPosValueP2;

		private Text m_lblHudCurrentTimeValueP2;

		private Text m_lblHudBestTimeValueP2;

		private GameObject m_pnlGridPlayers;

		private GameObject m_GridPlayersRowTemplate;

		private GameObject m_pnlHud;

		private GameObject m_pnlResults;

		private Race_Manager m_RaceManager;

		private Racer_Detail m_PlayerDetailP1;

		private Racer_Detail m_PlayerDetailP2;

		private bool m_IsUiInitilized;

		private GameObject m_pnlTouchDrive;

		public float m_PlayerGridUpdateInterval;

		private float m_TimeStart;

		private float m_GridUpdateNext;

		public bool m_EnablePlayerGrid;

		public bool m_EnableTimerFriction;

		private GameObject gridRowParentForPlayerGrid;

		private float countFadeDelayTimer = 1f;

		private bool countFadeDelay;

		private float countFadeTimer = 1f;

		private bool countStartFade;

		private float messageFadeDelayTimer = 1f;

		private bool messageFadeDelay;

		private float messageFadeTimer = 1f;

		private bool messageStartfade;

		private bool finalLapDisplayed;

		public float CurrentCount
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public bool ShowCountdownWindow
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool ShowWrongWayWindow
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		private void Awake()
		{
			m_RaceManager = GetComponent<Race_Manager>();
			if (m_RaceManager != null)
			{
				m_RaceManager.OnRaceInitiated += RaceManager_OnRaceInitiated;
				m_RaceManager.OnRaceFinished += RaceManager_OnRaceFinished;
				m_RaceManager.OnCountDownFinished += RaceManager_OnCountDownFinished;
				m_RaceManager.OnCountDownStarted += RaceManager_OnCountDownStarted;
				m_RaceManager.OnRacerKicked += RaceManager_OnRacerKicked;
				InitializeUIStuff();
			}
			if (m_UiCanvas != null)
			{
				m_pnlTouchDrive = m_UiCanvas.GetComponent<RGKUI_PauseMenu>().m_TouchDriveManager;
			}
		}

		private void InitializeUIStuff()
		{
			if (m_RaceManager != null)
			{
				m_pnlHud = m_UiCanvas.FindInChildren("pnlHud").gameObject;
				m_pnlGridPlayers = m_pnlHud.FindInChildren("pnlGridRacers").gameObject;
				m_pnlLapPosP1 = m_UiCanvas.FindInChildren("pnlLapPosP1").gameObject;
				m_pnlLapPosP2 = m_UiCanvas.FindInChildren("pnlLapPosP2").gameObject;
				m_pnlWrongWayP1 = m_UiCanvas.FindInChildren("pnlWrongWayP1").gameObject;
				m_pnlWrongWayP1.GetComponent<CanvasGroup>().alpha = 0f;
				m_pnlWrongWayP2 = m_UiCanvas.FindInChildren("pnlWrongWayP2").gameObject;
				m_pnlWrongWayP2.GetComponent<CanvasGroup>().alpha = 0f;
				m_IsWrongWayP1Active = false;
				m_IsWrongWayP2Active = false;
				m_pnlMessage = m_UiCanvas.FindInChildren("pnlMessage").gameObject;
				m_pnlMessage.GetComponent<CanvasGroup>().alpha = 0f;
				m_pnlCountDown = m_UiCanvas.FindInChildren("pnlCountDown").gameObject;
				m_imgCountDown3 = m_pnlCountDown.FindInChildren("imgCountDown3").gameObject;
				m_imgCountDown2 = m_pnlCountDown.FindInChildren("imgCountDown2").gameObject;
				m_imgCountDown1 = m_pnlCountDown.FindInChildren("imgCountDown1").gameObject;
				m_pnlCountDown.GetComponent<CanvasGroup>().alpha = 1f;
				m_lblHudLapValueP1 = m_pnlLapPosP1.FindInChildren("lblHudLapValue").GetComponent<Text>();
				m_lblHudLapTextP1 = m_pnlLapPosP1.FindInChildren("lblHudLapText").GetComponent<Text>();
				m_lblHudPosValueP1 = m_pnlLapPosP1.FindInChildren("lblHudPosValue").GetComponent<Text>();
				m_lblHudCurrentTimeValueP1 = m_pnlLapPosP1.FindInChildren("lblHudCurrentTimeVal").GetComponent<Text>();
				m_lblHudBestTimeValueP1 = m_pnlLapPosP1.FindInChildren("lblHudBestTimeVal").GetComponent<Text>();
				m_lblHudBestTimeTextP1 = m_pnlLapPosP1.FindInChildren("lblHudBestTimeText").GetComponent<Text>();
				m_lblHudLapValueP2 = m_pnlLapPosP2.FindInChildren("lblHudLapValue").GetComponent<Text>();
				m_lblHudPosValueP2 = m_pnlLapPosP2.FindInChildren("lblHudPosValue").GetComponent<Text>();
				m_lblHudCurrentTimeValueP2 = m_pnlLapPosP2.FindInChildren("lblHudCurrentTimeVal").GetComponent<Text>();
				m_lblHudBestTimeValueP2 = m_pnlLapPosP2.FindInChildren("lblHudBestTimeVal").GetComponent<Text>();
				m_pnlResults = m_UiCanvas.FindInChildren("pnlResults").gameObject;
				m_pnlResults.GetComponent<CanvasGroup>().alpha = 0f;
				m_pnlTimer = m_UiCanvas.FindInChildren("pnlTimer").gameObject;
				m_lblTimerMessage = m_pnlTimer.FindInChildren("lblTimerMessage").GetComponent<Text>();
				m_lblTimerValue = m_pnlTimer.FindInChildren("lblTimerValue").GetComponent<Text>();
				switch (m_RaceManager.RaceType)
				{
				case RaceTypeEnum.TimeAttack:
					m_pnlTimer.GetComponent<CanvasGroup>().alpha = 1f;
					m_lblTimerMessage.text = "To Next Checkpoint";
					break;
				case RaceTypeEnum.Sprint:
					m_lblHudLapTextP1.enabled = false;
					m_lblHudLapValueP1.enabled = false;
					m_lblHudBestTimeTextP1.text = "Progress";
					break;
				}
				m_IsUiInitilized = true;
			}
		}

		private void RaceManager_OnRaceInitiated()
		{
			BuildPlayerUIGrid();
		}

		private void RaceManager_OnRacerKicked(Racer_Detail RacerData)
		{
			ShowMessage(RacerData.RacerName + " is Kicked!");
		}

		private void RaceManager_OnCountDownStarted()
		{
			m_pnlCountDown.GetComponent<CanvasGroup>().alpha = 1f;
		}

		private void RaceManager_OnCountDownFinished()
		{
			countFadeDelay = true;
			m_TimeStart = Time.time;
		}

		private void RaceManager_OnRaceFinished(RaceTypeEnum RaceType)
		{
			m_pnlHud.GetComponent<CanvasGroup>().alpha = 0f;
			m_pnlResults.FindInChildren("btnResultsRestart").GetComponent<Button>().Select();
			if (m_PlayerDetailP1 != null)
			{
				m_pnlResults.FindInChildren("lblPlayerNameP1").GetComponent<Text>().text = m_PlayerDetailP1.RacerName;
				m_pnlResults.FindInChildren("lblBestLapValueP1").GetComponent<Text>().text = RGKUI_Utils.FormatTime(m_PlayerDetailP1.RacerBestTime, ShowFraction: true, 2);
				m_pnlResults.FindInChildren("lblTotalTimeValueP1").GetComponent<Text>().text = RGKUI_Utils.FormatTime(m_PlayerDetailP1.RacerTotalTime, ShowFraction: true, 2);
			}
			if (m_PlayerDetailP2 != null)
			{
				m_pnlResults.FindInChildren("lblPlayerNameP2").GetComponent<Text>().text = m_PlayerDetailP2.RacerName;
				m_pnlResults.FindInChildren("lblBestLapValueP2").GetComponent<Text>().text = RGKUI_Utils.FormatTime(m_PlayerDetailP2.RacerBestTime, ShowFraction: true, 2);
				m_pnlResults.FindInChildren("lblTotalTimeValueP2").GetComponent<Text>().text = RGKUI_Utils.FormatTime(m_PlayerDetailP2.RacerTotalTime, ShowFraction: true, 2);
			}
			m_pnlResults.GetComponent<CanvasGroup>().alpha = 1f;
			m_pnlResults.GetComponent<CanvasGroup>().blocksRaycasts = true;
			m_pnlResults.GetComponent<CanvasGroup>().interactable = true;
			if (m_pnlTouchDrive != null)
			{
				m_pnlTouchDrive.GetComponent<CanvasGroup>().alpha = 0f;
				m_pnlTouchDrive.GetComponent<CanvasGroup>().blocksRaycasts = false;
				m_pnlTouchDrive.GetComponent<CanvasGroup>().interactable = false;
			}
			GameObject gameObject = m_pnlResults.FindInChildren("ResultsGrid");
			if (gameObject != null)
			{
				GameObject gameObject2 = gameObject.FindInChildren("gridRowTemplate");
				int num = 1;
				foreach (Racer_Detail registeredRacer in m_RaceManager.RegisteredRacers)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2);
					gameObject3.name = "resultRow" + num.ToString();
					gameObject3.transform.position = gameObject2.transform.position;
					gameObject3.transform.rotation = gameObject2.transform.rotation;
					gameObject3.transform.SetParent(gameObject.transform);
					gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
					if (RaceType != RaceTypeEnum.LapKnockout)
					{
						gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
						gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatTime(registeredRacer.RacerTotalTime, ShowFraction: true, 2);
					}
					else if (!registeredRacer.RacerDestroyed)
					{
						gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
						gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatTime(registeredRacer.RacerTotalTime, ShowFraction: true, 2);
					}
					else
					{
						gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
						gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = "DNF";
					}
					num++;
				}
				UnityEngine.Object.DestroyImmediate(gameObject2);
			}
			else
			{
				UnityEngine.Debug.Log("Results Grid Not Found!?");
			}
		}

		private void BuildPlayerUIGrid()
		{
			if (m_pnlGridPlayers != null)
			{
				gridRowParentForPlayerGrid = m_pnlGridPlayers.gameObject.FindInChildren("gridRows");
				if (m_GridPlayersRowTemplate == null)
				{
					m_GridPlayersRowTemplate = m_pnlGridPlayers.gameObject.FindInChildren("gridRowTemplate");
					m_GridPlayersRowTemplate.transform.SetParent(null);
					m_GridPlayersRowTemplate.transform.position = new Vector3(-5000f, -5000f);
				}
				FlushGoChildren(gridRowParentForPlayerGrid);
				foreach (Racer_Detail registeredRacer in m_RaceManager.RegisteredRacers)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(m_GridPlayersRowTemplate);
					gameObject.name = registeredRacer.ID.ToString();
					gameObject.transform.position = gridRowParentForPlayerGrid.transform.position;
					gameObject.transform.rotation = gridRowParentForPlayerGrid.transform.rotation;
					gameObject.transform.SetParent(gridRowParentForPlayerGrid.transform);
					gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
					Text component = gameObject.gameObject.FindInChildren("txtGridPlayerName").GetComponent<Text>();
					component.text = registeredRacer.RacerName;
					Image component2 = gameObject.gameObject.FindInChildren("imgGridPlayerIcon").GetComponent<Image>();
					if (!registeredRacer.IsPlayer)
					{
						component2.enabled = false;
					}
				}
			}
		}

		private void UpdatePlayerUIGrid()
		{
			if (m_pnlGridPlayers != null && m_RaceManager != null)
			{
				if (gridRowParentForPlayerGrid == null)
				{
					gridRowParentForPlayerGrid = m_pnlGridPlayers.gameObject.FindInChildren("gridRows");
				}
				foreach (Racer_Detail registeredRacer in m_RaceManager.RegisteredRacers)
				{
					Transform transform = gridRowParentForPlayerGrid.transform.Find(registeredRacer.ID);
					if (transform != null)
					{
						transform.SetSiblingIndex(Convert.ToInt16(registeredRacer.RacerStanding));
						transform.gameObject.FindInChildren("txtGridPlayerPos").GetComponent<Text>().text = Convert.ToInt16(registeredRacer.RacerStanding).ToString();
						Text component = transform.gameObject.FindInChildren("txtGridPlayerDistance").GetComponent<Text>();
						if (registeredRacer.RacerDistance > 0f && !registeredRacer.RacerDestroyed)
						{
							component.text = $"{registeredRacer.RacerDistance:#}m";
						}
						else if (registeredRacer.RacerDestroyed)
						{
							component.text = "DNF";
						}
						else
						{
							component.text = "Finished";
						}
					}
				}
			}
		}

		private void LateUpdate()
		{
			DoUiUpdate();
			if (m_pnlGridPlayers != null && m_EnablePlayerGrid)
			{
				float num = Time.time - m_TimeStart;
				if (num >= m_GridUpdateNext)
				{
					UpdatePlayerUIGrid();
					m_GridUpdateNext = num + m_PlayerGridUpdateInterval;
				}
			}
		}

		private void DoUiUpdate()
		{
			if (m_IsUiInitilized)
			{
				if (m_PlayerDetailP1 == null && m_RaceManager.Player1 != null)
				{
					m_PlayerDetailP1 = m_RaceManager.Player1.GetComponent<Racer_Register>().RacerDetail;
				}
				if (m_PlayerDetailP2 == null && m_RaceManager.Player2 != null)
				{
					m_PlayerDetailP2 = m_RaceManager.Player2.GetComponent<Racer_Register>().RacerDetail;
				}
				if (m_PlayerDetailP1 != null)
				{
					m_lblHudCurrentTimeValueP1.text = RGKUI_Utils.FormatTime(m_PlayerDetailP1.RacerTotalTime, m_EnableTimerFriction, 1);
					m_lblHudLapValueP1.text = m_PlayerDetailP1.RacerLap + "/" + m_RaceManager.RaceLaps.ToString();
					m_lblHudPosValueP1.text = m_PlayerDetailP1.RacerStanding + "/" + m_RaceManager.RegisteredRacers.Count;
					if (m_RaceManager.RaceType == RaceTypeEnum.Sprint)
					{
						float num = m_RaceManager.RaceLength - m_PlayerDetailP1.RacerDistance;
						if (num < 0f)
						{
							num = 0f;
						}
						m_lblHudBestTimeValueP1.text = $"{num * 100f / m_RaceManager.RaceLength:0} %";
					}
					else
					{
						m_lblHudBestTimeValueP1.text = RGKUI_Utils.FormatTime(m_PlayerDetailP1.RacerBestTime, ShowFraction: true, 1);
					}
					if (m_PlayerDetailP1.RacerWrongWay)
					{
						if (!m_IsWrongWayP1Active)
						{
							m_pnlWrongWayP1.GetComponent<CanvasGroup>().alpha = 1f;
							m_IsWrongWayP1Active = true;
						}
					}
					else if (m_IsWrongWayP1Active)
					{
						m_pnlWrongWayP1.GetComponent<CanvasGroup>().alpha = 0f;
						m_IsWrongWayP1Active = false;
					}
					if (m_RaceManager.RaceLaps > 1 && m_PlayerDetailP1.RacerLap == (double)m_RaceManager.RaceLaps && !finalLapDisplayed)
					{
						ShowMessage("Final Lap!");
						finalLapDisplayed = true;
					}
				}
				if (m_PlayerDetailP2 != null)
				{
					m_lblHudCurrentTimeValueP2.text = RGKUI_Utils.FormatTime(m_PlayerDetailP2.RacerTotalTime, m_EnableTimerFriction, 1);
					m_lblHudLapValueP2.text = m_PlayerDetailP2.RacerLap + "/" + m_RaceManager.RaceLaps.ToString();
					m_lblHudPosValueP2.text = m_PlayerDetailP2.RacerStanding + "/" + m_RaceManager.RegisteredRacers.Count;
					if (m_RaceManager.RaceType == RaceTypeEnum.Sprint)
					{
						float num2 = m_RaceManager.RaceLength - m_PlayerDetailP2.RacerDistance;
						if (num2 < 0f)
						{
							num2 = 0f;
						}
						m_lblHudBestTimeValueP2.text = $"{num2 * 100f / m_RaceManager.RaceLength:0} %";
					}
					else
					{
						m_lblHudBestTimeValueP2.text = RGKUI_Utils.FormatTime(m_PlayerDetailP2.RacerBestTime, ShowFraction: true, 1);
					}
					if (m_PlayerDetailP2.RacerWrongWay)
					{
						if (!m_IsWrongWayP2Active)
						{
							m_pnlWrongWayP2.GetComponent<CanvasGroup>().alpha = 1f;
							m_IsWrongWayP2Active = true;
						}
					}
					else if (m_IsWrongWayP2Active)
					{
						m_pnlWrongWayP2.GetComponent<CanvasGroup>().alpha = 0f;
						m_IsWrongWayP2Active = false;
					}
					if (m_RaceManager.RaceLaps > 1 && m_PlayerDetailP2.RacerLap == (double)m_RaceManager.RaceLaps && !finalLapDisplayed)
					{
						ShowMessage("Final Lap!");
						finalLapDisplayed = true;
					}
				}
				if (!m_RaceManager.IsRaceStarted)
				{
					switch (m_RaceManager.CurrentCount)
					{
					case 2:
						m_imgCountDown1.GetComponent<Image>().sprite = m_SpriteStartYellow;
						break;
					case 1:
						m_imgCountDown2.GetComponent<Image>().sprite = m_SpriteStartYellow;
						break;
					case 0:
						m_imgCountDown1.GetComponent<Image>().sprite = m_SpriteStartGreen;
						m_imgCountDown2.GetComponent<Image>().sprite = m_SpriteStartGreen;
						m_imgCountDown3.GetComponent<Image>().sprite = m_SpriteStartGreen;
						break;
					}
				}
				RaceTypeEnum raceType = m_RaceManager.RaceType;
				if (raceType == RaceTypeEnum.TimeAttack)
				{
					m_lblTimerValue.text = RGKUI_Utils.FormatTime(m_RaceManager.TimeNextCheckPoint, ShowFraction: true, 1);
				}
			}
			ProcessTimers();
		}

		private void ProcessTimers()
		{
			if (countFadeDelay)
			{
				countFadeDelayTimer -= Time.deltaTime;
				if (countFadeDelayTimer < 0f)
				{
					countStartFade = true;
					countFadeDelay = false;
				}
			}
			if (countStartFade)
			{
				countFadeTimer -= Time.deltaTime;
				if (countFadeTimer >= 0f)
				{
					m_pnlCountDown.GetComponent<CanvasGroup>().alpha = countFadeTimer;
				}
				else
				{
					m_pnlCountDown.GetComponent<CanvasGroup>().alpha = 0f;
					countStartFade = false;
				}
			}
			if (messageFadeDelay)
			{
				messageFadeDelayTimer -= Time.deltaTime;
				if (messageFadeDelayTimer < 0f)
				{
					messageStartfade = true;
					messageFadeDelay = false;
				}
			}
			if (messageStartfade)
			{
				messageFadeTimer -= Time.deltaTime;
				if (messageFadeTimer >= 0f)
				{
					m_pnlMessage.GetComponent<CanvasGroup>().alpha = messageFadeTimer;
				}
				else
				{
					messageStartfade = false;
				}
			}
		}

		private void ShowMessage(string Message)
		{
			messageFadeDelayTimer = 1f;
			messageFadeTimer = 1f;
			messageFadeDelay = true;
			m_pnlMessage.GetComponent<CanvasGroup>().alpha = 1f;
			m_pnlMessage.FindInChildren("lblMessage").GetComponent<Text>().text = Message;
		}

		private void FlushGoChildren(GameObject Target)
		{
			IEnumerator enumerator = Target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		public void PlayerCheckPointPassed(CheckPointItem PassedCheckpoint)
		{
		}

		public void RaceFinished(string RaceType)
		{
		}

		public void ShowResultsWindow()
		{
		}
	}
}
