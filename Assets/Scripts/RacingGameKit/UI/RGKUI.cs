using RacingGameKit.Interfaces;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RacingGameKit.UI
{
	[AddComponentMenu("Racing Game Kit/UI/Race UI")]
	[RequireComponent(typeof(Race_Manager))]
	public class RGKUI : MonoBehaviour, IRGKUI
	{
		public AdsManager adCallerScript;

		private float m_FPSTimeLeft;

		private float m_FPSAccum;

		private int m_FPSFrames;

		private Text m_txtFPS;

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

		private GameObject m_pnlWrongWay;

		private bool m_IsWrongWayActive;

		private GameObject m_pnlLapPos;

		private Text m_lblHudLapValue;

		private Text m_lblHudLapText;

		private Text m_lblHudPosText;

		private Text m_lblHudPosValue;

		private Text m_lblHudCurrentTimeValue;

		private Text m_lblHudBestTimeValue;

		private Text m_lblHudBestTimeText;

		private GameObject m_pnlGridPlayers;

		private GameObject m_GridPlayersRowTemplate;

		private GameObject m_pnlHud;

		private GameObject m_pnlResults;

		private Race_Manager m_RaceManager;

		private Racer_Detail m_PlayerDetail;

		private bool m_IsUiInitilized;

		private GameObject m_pnlTouchDrive;

		[Space]
		public bool m_ShowPlayerGrid;

		[Tooltip("Caution! Lower values like 0 causes overhead and may effect FPS on mobile devices! On Mobile use minimum 0.5")]
		public float m_GridUpdateInterval = 0.5f;

		private float m_GridUpdateLeft;

		[Space]
		[Tooltip("Show hide time friction of Current Time on hud.")]
		public bool m_EnableTimerFriction;

		[Space]
		public bool m_ShowFPS;

		public float m_FpsUpdateInterval = 1f;

		private float countFadeDelayTimer = 1f;

		private bool countFadeDelay;

		private float countFadeTimer = 1f;

		private bool countStartFade;

		private float messageFadeDelayTimer = 1f;

		private bool messageFadeDelay;

		private float messageFadeTimer = 1f;

		private bool messageStartfade;

		private bool finalLapDisplayed;

		private GameObject gridRowParentForPlayerGrid;

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
				if (m_pnlGridPlayers != null && !m_ShowPlayerGrid)
				{
					m_pnlGridPlayers.SetActive(value: false);
				}
				GameObject gameObject = m_pnlHud.FindInChildren("lblFPS").gameObject;
				if (gameObject != null)
				{
					m_txtFPS = gameObject.GetComponent<Text>();
				}
				m_pnlLapPos = m_UiCanvas.FindInChildren("pnlLapPos").gameObject;
				m_pnlWrongWay = m_UiCanvas.FindInChildren("pnlWrongWay").gameObject;
				m_pnlWrongWay.GetComponent<CanvasGroup>().alpha = 0f;
				m_IsWrongWayActive = false;
				m_pnlMessage = m_UiCanvas.FindInChildren("pnlMessage").gameObject;
				m_pnlMessage.GetComponent<CanvasGroup>().alpha = 0f;
				m_pnlCountDown = m_UiCanvas.FindInChildren("pnlCountDown").gameObject;
				m_imgCountDown3 = m_pnlCountDown.FindInChildren("imgCountDown3").gameObject;
				m_imgCountDown2 = m_pnlCountDown.FindInChildren("imgCountDown2").gameObject;
				m_imgCountDown1 = m_pnlCountDown.FindInChildren("imgCountDown1").gameObject;
				m_pnlCountDown.GetComponent<CanvasGroup>().alpha = 1f;
				m_lblHudLapValue = m_pnlLapPos.FindInChildren("lblHudLapValue").GetComponent<Text>();
				m_lblHudLapText = m_pnlLapPos.FindInChildren("lblHudLapText").GetComponent<Text>();
				m_lblHudPosText = m_pnlLapPos.FindInChildren("lblHudPosText").GetComponent<Text>();
				m_lblHudPosValue = m_pnlLapPos.FindInChildren("lblHudPosValue").GetComponent<Text>();
				m_lblHudCurrentTimeValue = m_pnlLapPos.FindInChildren("lblHudCurrentTimeVal").GetComponent<Text>();
				m_lblHudBestTimeValue = m_pnlLapPos.FindInChildren("lblHudBestTimeVal").GetComponent<Text>();
				m_lblHudBestTimeText = m_pnlLapPos.FindInChildren("lblHudBestTimeText").GetComponent<Text>();
				m_pnlResults = m_UiCanvas.FindInChildren("pnlResults").gameObject;
				m_pnlResults.GetComponent<CanvasGroup>().alpha = 0f;
				m_pnlTimer = m_UiCanvas.FindInChildren("pnlTimer").gameObject;
				m_lblTimerMessage = m_pnlTimer.FindInChildren("lblTimerMessage").GetComponent<Text>();
				m_lblTimerValue = m_pnlTimer.FindInChildren("lblTimerValue").GetComponent<Text>();
				m_IsUiInitilized = true;
			}
		}

		private void RaceManager_OnRaceInitiated()
		{
			switch (m_RaceManager.RaceType)
			{
			case RaceTypeEnum.TimeAttack:
				m_pnlTimer.GetComponent<CanvasGroup>().alpha = 1f;
				m_lblTimerMessage.text = "To Next Checkpoint";
				m_lblHudLapText.enabled = false;
				m_lblHudLapValue.enabled = false;
				m_lblHudPosText.text = "LAP";
				break;
			case RaceTypeEnum.Sprint:
				m_lblHudLapText.enabled = false;
				m_lblHudLapValue.enabled = false;
				m_lblHudBestTimeText.text = "Progress";
				break;
			case RaceTypeEnum.Speedtrap:
				if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestSpeed)
				{
					m_lblHudBestTimeText.text = "Highest Speed";
				}
				else if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestTotalSpeed)
				{
					m_lblHudBestTimeText.text = "Total Speed";
				}
				else if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestSpeedInRace)
				{
					m_lblHudBestTimeText.text = "Top Speed";
				}
				break;
			}
			if (m_ShowPlayerGrid)
			{
				BuildPlayerUIGrid();
			}
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
		}

		private void RaceManager_OnRaceFinished(RaceTypeEnum RaceType)
		{
			UnityEngine.Debug.Log("GameOver/LevelClear ------------------------------------------------------");
			MonoBehaviour.print("------------------------------------------------------------- game finished ---------------------------------- ");
			//adCallerScript.callLevelClearAd();
			m_pnlHud.GetComponent<CanvasGroup>().alpha = 0f;
			m_pnlResults.FindInChildren("btnResultsRestart").GetComponent<Button>().Select();
			if (m_PlayerDetail != null)
			{
				switch (RaceType)
				{
				case RaceTypeEnum.Speedtrap:
					m_pnlResults.FindInChildren("lblStandings").GetComponent<Text>().text = RGKUI_Utils.Ordinal(Convert.ToInt16(m_PlayerDetail.RacerStanding));
					m_pnlResults.FindInChildren("lblTotalTimeValue").GetComponent<Text>().text = RGKUI_Utils.FormatTime(m_PlayerDetail.RacerTotalTime, ShowFraction: true, 2);
					if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestSpeed)
					{
						m_pnlResults.FindInChildren("lblBestLap").GetComponent<Text>().text = "Highest Speed :";
						m_pnlResults.FindInChildren("lblBestLapValue").GetComponent<Text>().text = RGKUI_Utils.FormatSpeed(m_PlayerDetail.RacerHighestSpeed, IsMile: false);
					}
					else if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestTotalSpeed)
					{
						m_pnlResults.FindInChildren("lblBestLap").GetComponent<Text>().text = "Total Speed :";
						m_pnlResults.FindInChildren("lblBestLapValue").GetComponent<Text>().text = RGKUI_Utils.FormatSpeed(m_PlayerDetail.RacerSumOfSpeeds, IsMile: false);
					}
					else if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestSpeedInRace)
					{
						m_pnlResults.FindInChildren("lblBestLap").GetComponent<Text>().text = "Top Speed :";
						m_pnlResults.FindInChildren("lblBestLapValue").GetComponent<Text>().text = RGKUI_Utils.FormatSpeed(m_PlayerDetail.RacerSumOfSpeeds, IsMile: false);
					}
					break;
				case RaceTypeEnum.TimeAttack:
					if (!m_PlayerDetail.RacerDestroyed)
					{
						m_pnlResults.FindInChildren("lblStandings").GetComponent<Text>().text = "WIN!";
						m_PlayerDetail.RacerStanding = 1f;
					}
					else
					{
						m_pnlResults.FindInChildren("lblStandings").GetComponent<Text>().text = "FAIL!";
					}
					m_pnlResults.FindInChildren("lblTotalTimeValue").GetComponent<Text>().text = RGKUI_Utils.FormatTime(m_PlayerDetail.RacerTotalTime, ShowFraction: true, 2);
					m_pnlResults.FindInChildren("lblBestLapValue").GetComponent<Text>().text = RGKUI_Utils.FormatTime(m_PlayerDetail.RacerBestTime, ShowFraction: true, 2);
					break;
				default:
					m_pnlResults.FindInChildren("lblStandings").GetComponent<Text>().text = RGKUI_Utils.Ordinal(Convert.ToInt16(m_PlayerDetail.RacerStanding));
					m_pnlResults.FindInChildren("lblTotalTimeValue").GetComponent<Text>().text = RGKUI_Utils.FormatTime(m_PlayerDetail.RacerTotalTime, ShowFraction: true, 2);
					m_pnlResults.FindInChildren("lblBestLapValue").GetComponent<Text>().text = RGKUI_Utils.FormatTime(m_PlayerDetail.RacerBestTime, ShowFraction: true, 2);
					break;
				}
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
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				bool flag = false;
				foreach (Racer_Detail registeredRacer in m_RaceManager.RegisteredRacers)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2);
					gameObject3.name = "resultRow" + num.ToString();
					gameObject3.transform.position = gameObject2.transform.position;
					gameObject3.transform.rotation = gameObject2.transform.rotation;
					gameObject3.transform.SetParent(gameObject.transform);
					gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
					switch (RaceType)
					{
					case RaceTypeEnum.LapKnockout:
						if (!registeredRacer.RacerDestroyed)
						{
							gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
							gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatTime(registeredRacer.RacerTotalTime, ShowFraction: true, 2);
						}
						else
						{
							gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
							gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = "DNF";
						}
						break;
					case RaceTypeEnum.Speedtrap:
						if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestSpeed)
						{
							if (!flag)
							{
								gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
								gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatSpeed(registeredRacer.RacerHighestSpeed, IsMile: false);
							}
							else
							{
								num3 -= UnityEngine.Random.Range(5f, 30f);
								gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
								gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatSpeed(num3, IsMile: false);
							}
						}
						else if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestTotalSpeed)
						{
							if (!flag)
							{
								gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
								gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatSpeed(registeredRacer.RacerSumOfSpeeds, IsMile: false);
							}
							else
							{
								num4 -= UnityEngine.Random.Range(10f, 150f);
								gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
								gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatSpeed(num4, IsMile: false);
							}
						}
						else if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestSpeedInRace)
						{
							if (!flag)
							{
								gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
								gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatSpeed(registeredRacer.RacerHighestSpeedInRaceAsKm, IsMile: false);
							}
							else
							{
								num3 -= UnityEngine.Random.Range(2f, 15f);
								gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
								gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatSpeed(num3, IsMile: false);
							}
						}
						break;
					default:
						if (!flag)
						{
							gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
							gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatTime(registeredRacer.RacerTotalTime, ShowFraction: true, 2);
						}
						else
						{
							num2 += UnityEngine.Random.Range(5f, 10f);
							gameObject3.FindInChildren("gridRowText").GetComponent<Text>().text = registeredRacer.RacerName;
							gameObject3.FindInChildren("gridRowValue").GetComponent<Text>().text = RGKUI_Utils.FormatTime(num2, ShowFraction: true, 2);
						}
						break;
					}
					if (registeredRacer.IsPlayer)
					{
						num2 = registeredRacer.RacerTotalTime;
						num3 = registeredRacer.RacerHighestSpeed;
						num4 = registeredRacer.RacerSumOfSpeeds;
						flag = true;
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
			if (m_ShowPlayerGrid)
			{
				UpdateGridInterval();
			}
			if (m_ShowFPS)
			{
				CaptureFPS();
				if (!m_txtFPS.gameObject.activeInHierarchy)
				{
					m_txtFPS.gameObject.SetActive(value: true);
				}
			}
			else if (m_txtFPS.gameObject.activeInHierarchy)
			{
				m_txtFPS.gameObject.SetActive(value: false);
			}
		}

		private void UpdateGridInterval()
		{
			if (m_pnlGridPlayers != null)
			{
				m_GridUpdateLeft -= Time.deltaTime;
				if (m_GridUpdateLeft <= 0f)
				{
					UpdatePlayerUIGrid();
					m_GridUpdateLeft = m_GridUpdateInterval;
				}
			}
		}

		private void CaptureFPS()
		{
			if (!(m_txtFPS == null))
			{
				m_FPSTimeLeft -= Time.deltaTime;
				m_FPSAccum += Time.timeScale / Time.deltaTime;
				m_FPSFrames++;
				if (m_FPSTimeLeft <= 0f)
				{
					float num = m_FPSAccum / (float)m_FPSFrames;
					string text = $"{num:F0} FPS";
					m_txtFPS.text = text;
					m_FPSTimeLeft = m_FpsUpdateInterval;
					m_FPSAccum = 0f;
					m_FPSFrames = 0;
				}
			}
		}

		private void DoUiUpdate()
		{
			if (m_IsUiInitilized)
			{
				if (m_PlayerDetail == null && m_RaceManager.Player1 != null)
				{
					m_PlayerDetail = m_RaceManager.Player1.GetComponent<Racer_Register>().RacerDetail;
				}
				if (m_PlayerDetail != null)
				{
					m_lblHudCurrentTimeValue.text = RGKUI_Utils.FormatTime(m_PlayerDetail.RacerTotalTime, m_EnableTimerFriction, 1);
					switch (m_RaceManager.RaceType)
					{
					case RaceTypeEnum.Sprint:
					{
						float num = m_RaceManager.RaceLength - m_PlayerDetail.RacerDistance;
						if (num < 0f)
						{
							num = 0f;
						}
						m_lblHudBestTimeValue.text = $"{num * 100f / m_RaceManager.RaceLength:0} %";
						m_lblHudPosValue.text = m_PlayerDetail.RacerStanding + "/" + m_RaceManager.RegisteredRacers.Count;
						m_lblHudLapValue.text = m_PlayerDetail.RacerLap + "/" + m_RaceManager.RaceLaps.ToString();
						break;
					}
					case RaceTypeEnum.Speedtrap:
						if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestSpeed)
						{
							m_lblHudBestTimeValue.text = $"{m_PlayerDetail.RacerHighestSpeed:0} Km/h";
						}
						else if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestTotalSpeed)
						{
							m_lblHudBestTimeValue.text = $"{m_PlayerDetail.RacerSumOfSpeeds:0} Km/h";
						}
						else if (m_RaceManager.SpeedTrapMode == eSpeedTrapMode.HighestSpeedInRace)
						{
							m_lblHudBestTimeValue.text = $"{m_PlayerDetail.RacerHighestSpeedInRaceAsKm:0} Km/h";
						}
						m_lblHudPosValue.text = m_PlayerDetail.RacerStanding + "/" + m_RaceManager.RegisteredRacers.Count;
						m_lblHudLapValue.text = m_PlayerDetail.RacerLap + "/" + m_RaceManager.RaceLaps.ToString();
						break;
					case RaceTypeEnum.TimeAttack:
						m_lblHudBestTimeValue.text = RGKUI_Utils.FormatTime(m_PlayerDetail.RacerBestTime, ShowFraction: true, 1);
						m_lblHudPosValue.text = m_PlayerDetail.RacerLap + "/" + m_RaceManager.RaceLaps.ToString();
						break;
					default:
						m_lblHudBestTimeValue.text = RGKUI_Utils.FormatTime(m_PlayerDetail.RacerBestTime, ShowFraction: true, 1);
						m_lblHudPosValue.text = m_PlayerDetail.RacerStanding + "/" + m_RaceManager.RegisteredRacers.Count;
						m_lblHudLapValue.text = m_PlayerDetail.RacerLap + "/" + m_RaceManager.RaceLaps.ToString();
						break;
					}
					if (m_PlayerDetail.RacerWrongWay)
					{
						if (!m_IsWrongWayActive)
						{
							m_pnlWrongWay.GetComponent<CanvasGroup>().alpha = 1f;
							m_IsWrongWayActive = true;
						}
					}
					else if (m_IsWrongWayActive)
					{
						m_pnlWrongWay.GetComponent<CanvasGroup>().alpha = 0f;
						m_IsWrongWayActive = false;
					}
					if (m_RaceManager.RaceLaps > 1 && m_PlayerDetail.RacerLap == (double)m_RaceManager.RaceLaps && !finalLapDisplayed)
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
