using RacingGameKit.EventHandlers;
using RacingGameKit.Helpers;
using RacingGameKit.Interfaces;
using RacingGameKit.RGKCar.CarControllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Game Mechanics/Race Manager")]
	[RequireComponent(typeof(SplineInterpolator))]
	public class Race_Manager : MonoBehaviour
	{
		public enum eOrientationMode
		{
			NODE,
			TANGENT
		}

		public Racer_Detail positionCheck;

		[HideInInspector]
		public GameObject Waypoints;

		[HideInInspector]
		public GameObject SpawnPoints;

		[HideInInspector]
		public GameObject CheckPoints;

		[HideInInspector]
		public GameObject StartPoint;

		[HideInInspector]
		public GameObject FinishPoint;

		[HideInInspector]
		public RaceTypeEnum RaceType;

		[HideInInspector]
		public int RaceLaps = 1;

		public int RacePlayers = 2;

		[HideInInspector]
		public float RaceLength;

		[HideInInspector]
		public GameObject HumanRacerPrefab;

		[HideInInspector]
		public int PlayerSpawnPosition = 1;

		[HideInInspector]
		public GameObject[] AIRacerPrefab;

		private List<int> m_SpawnedAIs = new List<int>();

		private int LastSpawnedAIIndex;

		public eAISpawnOrder AiSpawnOrder = eAISpawnOrder.Random;

		public eAISpawnMode AiSpawnMode = eAISpawnMode.Random;

		public eAINamingMode AiNamingMode = eAINamingMode.Random;

		[Obsolete("This method is obsolate. Please use AINamingMode property instead!")]
		[HideInInspector]
		public bool RandomAiNames;

		public bool AiContinuesAfterFinish;

		public bool PlayerContinuesAfterFinish;

		public bool StopRaceOnPlayerFinish = true;

		public bool KickLastRacerAfter2ndLast = true;

		public bool MoveKickedRacerToIgnoreLayer = true;

		public bool MoveRespawnToIgnoreLayer = true;

		public bool RemoveKickedRacersFromScene = true;

		public bool IsRaceReady;

		public bool RaceInitsOnStartup = true;

		public bool RaceStartsOnStartup = true;

		[NonSerialized]
		public int RaceActiveLap = 1;

		public float TimeTotal;

		public float TimeCurrent;

		[HideInInspector]
		public float CurrentTimeFixForPlayer;

		public float TimePlayerLast;

		public float TimePlayerBest;

		public float TimeNextCheckPoint;

		public float TimeTotalCheckPoint;

		public int TimerCountdownFrom = 5;

		public bool IsRaceStarted;

		public bool IsRaceFinished;

		public bool EnableStartupCamera;

		private bool tmpFinished;

		private bool isCounting;

		private float TimeTotalFix;

		[HideInInspector]
		public int CurrentCount;

		[HideInInspector]
		public IRGKUI GameUIComponent;

		[HideInInspector]
		public IRGKRaceAudio GameAudioComponent;

		public bool StartMusicAfterCountdown = true;

		public float DistancePointDensity = 5f;

		[HideInInspector]
		public GameObject DistanceTransformContainer;

		private Transform[] DistanceMeasurementObjects;

		public bool ShowDistanceGizmos;

		private List<Racer_Detail> ListRacers = new List<Racer_Detail>();

		private string[] AIRacerNames;

		private List<string> ListRacerNames;

		[HideInInspector]
		public string LastRacerID;

		[HideInInspector]
		public string LastRacerName;

		private bool HumanDeployed;

		private IRGKCamera m_RGKCam_P1;

		private IRGKCamera m_RGKCam_P2;

		private bool m_IsRaceFinishPushed;

		[HideInInspector]
		public eSpeedTrapMode SpeedTrapMode = eSpeedTrapMode.HighestTotalSpeed;

		[HideInInspector]
		public bool EnableCheckpointArrow;

		[HideInInspector]
		public float WorkingFPS;

		public GameObject oCamera1;

		public GameObject oCamera2;

		public GameObject Player1;

		//public GameObject Player2;

		public GameObject oAudioListener;

		private bool blnIsAudioMoved = true;

		private int checkCount = 5;

		//public bool m_SplitScreen;

		private bool m_IsP1Deployed;

		private RGKCar_C2_SplitScreenManager m_SplitScreenManager;

		private WayPointManager m_WPManager;

		private List<WayPointItem> m_WaypointItems;

		[HideInInspector]
		private List<int> Indexes;

		private int m_LastRacerIndex = -1;

		private float countdown_internal_timer = 1f;

		[HideInInspector]
		public List<Racer_Detail> RegisteredRacers
		{
			get
			{
				return ListRacers;
			}
			set
			{
				ListRacers = value;
			}
		}

		public bool IsCounting => isCounting;

		//public bool SplitScreen
		//{
		//	get
		//	{
		//		return m_SplitScreen;
		//	}
		//	set
		//	{
		//		m_SplitScreen = value;
		//	}
		//}

		public List<WayPointItem> WaypointItems
		{
			get
			{
				return m_WaypointItems;
			}
			set
			{
				m_WaypointItems = value;
			}
		}

		public List<int> SpawnedAIs => m_SpawnedAIs;

		public event RaceCountDownStartedEventHandler OnCountDownStarted;

		public event RaceCountDownFinishedEventHandler OnCountDownFinished;

		public event RaceInititatedEventHandler OnRaceInitiated;

		public event RaceStartedEventHandler OnRaceStarted;

		public event RaceFinishedEventHandler OnRaceFinished;

		public event RacerKickedEventHandler OnRacerKicked;

		public int RegisterToGame(Racer_Detail GamerDetail)
		{
			int result = -1;
			if (GamerDetail != null)
			{
				ListRacers.Add(GamerDetail);
				result = ListRacers.FindIndex((Racer_Detail _racerNew) => _racerNew.ID == GamerDetail.ID);
			}
			return result;
		}

		public string GetNameForRacer(string NameOnRegisterComponent)
		{
			float num = 0f;
			string result = string.Empty;
			switch (AiNamingMode)
			{
			case eAINamingMode.Random:
				num = UnityEngine.Random.Range(0, ListRacerNames.Count);
				if (num > (float)ListRacerNames.Count)
				{
					num = ListRacerNames.Count - 1;
				}
				result = ListRacerNames[Convert.ToInt16(num)].ToString();
				ListRacerNames.RemoveAt(Convert.ToInt16(num));
				break;
			case eAINamingMode.Order:
				if (num > (float)ListRacerNames.Count)
				{
					num = ListRacerNames.Count - 1;
				}
				result = ListRacerNames[Convert.ToInt16(num)].ToString();
				ListRacerNames.RemoveAt(Convert.ToInt16(num));
				break;
			case eAINamingMode.FromRacerRegister:
				result = NameOnRegisterComponent;
				break;
			}
			return result;
		}

		private void OnDrawGizmos()
		{
			if (ShowDistanceGizmos && DistanceMeasurementObjects != null)
			{
				Transform[] distanceMeasurementObjects = DistanceMeasurementObjects;
				foreach (Transform transform in distanceMeasurementObjects)
				{
					Gizmos.color = Color.white;
					Gizmos.DrawSphere(transform.position, 0.25f);
				}
			}
		}

		private void Awake()
		{
			if (MoveRespawnToIgnoreLayer)
			{
				if (LayerMask.NameToLayer("IGNORE") > 0 && LayerMask.NameToLayer("AI") > 0)
				{
					Physics.IgnoreLayerCollision(LayerMask.NameToLayer("AI"), LayerMask.NameToLayer("IGNORE"), ignore: true);
					Physics.IgnoreLayerCollision(LayerMask.NameToLayer("IGNORE"), LayerMask.NameToLayer("IGNORE"), ignore: true);
				}
				else
				{
					UnityEngine.Debug.LogWarning("RACING GAME KIT WARNING\r\nOne of more required layers (AI,IGNORE or OBSTACLE) not created. Please check documentation for required layers!");
				}
			}
			AIRacerNames = JsonUtils.GetRacerNames();
			if (Waypoints != null)
			{
				m_WPManager = Waypoints.GetComponent<WayPointManager>();
				WaypointItems = m_WPManager.GetWaypointItems();
			}
		}

		private void Start()
		{
			UnityEngine.Debug.Log("==== Starting RGK v1.6 ====");
			//if (m_SplitScreen)
			//{
			//	GameObject gameObject = GameObject.Find("_SplitScreenManager");
			//	if (!(gameObject != null))
			//	{
			//		UnityEngine.Debug.LogWarning("RACING GAME KIT WARNING\r\nSplit Screen requires _SplitScreenManager object added to scene! Please check documentation for details!");
			//		return;
			//	}
			//	m_SplitScreenManager = gameObject.GetComponent<RGKCar_C2_SplitScreenManager>();
			//	if (oAudioListener == null)
			//	{
			//		UnityEngine.Debug.LogWarning("RACING GAME KIT WARNING\r\nSplit Screen requires AudioListener UnityEngine.Object added to scene! Please check documentation for details!");
			//		return;
			//	}
			//}
			if (RaceInitsOnStartup)
			{
				InitRaceInternal();
			}
			positionCheck = Player1.GetComponent<Racer_Register>().RacerDetail;
		}

		public void InitRace()
		{
			InitRaceInternal();
		}

		private void InitRaceInternal()
		{
			if (RaceLaps < 1)
			{
				RaceLaps = 1;
			}
			if (RacePlayers < 1)
			{
				RacePlayers = 1;
			}
			GameUIComponent = (IRGKUI)base.transform.GetComponent(typeof(IRGKUI));
			if (oCamera1 == null)
			{
				GameObject gameObject = GameObject.Find("_RaceCamera");
				if (!(gameObject != null))
				{
					UnityEngine.Debug.LogError("RGK WARNING\r\nGameCamera is missing! RGK requires a camera script that implement iRGKCamera");
					return;
				}
				oCamera1 = gameObject;
				m_RGKCam_P1 = (gameObject.GetComponent(typeof(IRGKCamera)) as IRGKCamera);
			}
			else if (oCamera1 != null)
			{
				m_RGKCam_P1 = (oCamera1.GetComponent(typeof(IRGKCamera)) as IRGKCamera);
				//if (m_SplitScreen || oCamera2 != null)
				//{
				//	m_RGKCam_P2 = (oCamera2.GetComponent(typeof(IRGKCamera)) as IRGKCamera);
				//}
			}
			//if (m_SplitScreen || (oCamera1 != null && oCamera2 != null))
			//{
			//	IRGKCamera rGKCam_P = m_RGKCam_P1;
			//	bool enableStartupCamera = EnableStartupCamera;
			//	m_RGKCam_P2.IsStartupAnimationEnabled = enableStartupCamera;
			//	rGKCam_P.IsStartupAnimationEnabled = enableStartupCamera;
			//	Transform transform = oCamera1.transform.Find("CheckpointArrow");
			//	if (transform != null)
			//	{
			//		transform.gameObject.SetActive(EnableCheckpointArrow);
			//	}
			//	Transform transform2 = oCamera2.transform.Find("CheckpointArrow");
			//	if (transform2 != null)
			//	{
			//		transform2.gameObject.SetActive(EnableCheckpointArrow);
			//	}
			//}
			//else
			//{
				m_RGKCam_P1.IsStartupAnimationEnabled = EnableStartupCamera;
				Transform transform3 = oCamera1.transform.Find("CheckpointArrow");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(EnableCheckpointArrow);
				}
			//}
			ListRacerNames = new List<string>(AIRacerNames);
			if (CheckPoints == null || CheckPoints.transform.childCount == 0)
			{
				UnityEngine.Debug.Log("RGK NOTIFICATION\r\nCheckpoint container not found or empty. Checkpoint System disabled!");
			}
			else if (CheckPoints.activeInHierarchy)
			{
				GetFirstCheckPoint();
			}
			if (SpawnPoints != null)
			{
				Transform[] childTransforms = GetChildTransforms(SpawnPoints.transform);
				if (m_RGKCam_P1.TargetObjects == null)
				{
					m_RGKCam_P1.TargetObjects = new List<Transform>();
				}
				if (PlayerSpawnPosition > RacePlayers)
				{
					PlayerSpawnPosition = RacePlayers;
				}
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < childTransforms.GetLength(0); i++)
				{
					GameObject gameObject2 = null;
					if (HumanRacerPrefab != null && !HumanDeployed && i + 1 == PlayerSpawnPosition)
					{
						flag = true;
					}
					else if (AIRacerPrefab.Length > 0 && AIRacerPrefab[0] != null)
					{
						int num = 0;
						if (AiSpawnOrder == eAISpawnOrder.Random)
						{
							if (AIRacerPrefab.Length > 1)
							{
								if (AiSpawnMode == eAISpawnMode.Random)
								{
									num = UnityEngine.Random.Range(0, AIRacerPrefab.Length);
								}
								else if (AiSpawnMode == eAISpawnMode.OneTimeEach)
								{
									num = FindNextNotSpawnedAiIndex();
									if (num != -1)
									{
										m_SpawnedAIs.Add(num);
										UnityEngine.Debug.Log("Spawn No " + num);
									}
									else
									{
										if (!HumanDeployed)
										{
											flag = true;
										}
										flag2 = true;
									}
								}
							}
						}
						else if (AiSpawnOrder == eAISpawnOrder.Order)
						{
							num = LastSpawnedAIIndex;
							LastSpawnedAIIndex++;
							if (LastSpawnedAIIndex > AIRacerPrefab.Length)
							{
								num = -1;
								flag2 = true;
							}
						}
						if (num != -1)
						{
							gameObject2 = UnityEngine.Object.Instantiate(AIRacerPrefab[num], childTransforms[i].transform.position, childTransforms[i].transform.rotation);
							gameObject2.GetType();
							Transform transform4 = gameObject2.transform.Find("_CameraTarget");
							Racer_Register component = gameObject2.GetComponent<Racer_Register>();
							if (transform4 == null)
							{
								transform4 = gameObject2.transform;
							}
							if (m_RGKCam_P1.TargetObjects != null)
							{
								m_RGKCam_P1.TargetObjects.Add(transform4);
							}
							if (component != null)
							{
								//if (m_SplitScreen)
								//{
								//	if (component.IsPlayer)
								//	{
								//		if (!m_IsP1Deployed)
								//		{
								//			m_RGKCam_P1.TargetVehicle = transform4;
								//			ChangeSplitScreenPlayerControlrs(gameObject2, IsPlayer1: true);
								//			m_IsP1Deployed = true;
								//			Player1 = gameObject2;
								//		}
								//		else
								//		{
								//			m_RGKCam_P2.TargetVehicle = transform4;
								//			ChangeSplitScreenPlayerControlrs(gameObject2, IsPlayer1: false);
								//			Player2 = gameObject2;
								//		}
								//	}
								//}
								/*else */if (component.IsPlayer)
								{
									Player1 = gameObject2;
									m_RGKCam_P1.TargetVehicle = transform4;
									m_IsP1Deployed = true;
									HumanDeployed = true;
								}
							}
							if (!m_IsP1Deployed)
							{
								m_RGKCam_P1.TargetVehicle = transform4;
							}
						}
					}
					if (flag)
					{
						gameObject2 = (Player1 = UnityEngine.Object.Instantiate(HumanRacerPrefab, childTransforms[i].transform.position, childTransforms[i].transform.rotation));
						Transform transform5 = gameObject2.transform.Find("_CameraTarget");
						if (transform5 == null)
						{
							transform5 = gameObject2.transform;
						}
						if (m_RGKCam_P1.TargetObjects != null)
						{
							m_RGKCam_P1.TargetObjects.Add(transform5);
						}
						m_RGKCam_P1.TargetVehicle = transform5;
						HumanDeployed = true;
						flag = false;
						m_IsP1Deployed = true;
					}
					if (i + 1 == RacePlayers || flag2)
					{
						break;
					}
				}
				CreateDistanceMeasurementTransforms();
				CurrentCount = TimerCountdownFrom;
				GameAudioComponent = (IRGKRaceAudio)base.transform.GetComponent(typeof(IRGKRaceAudio));
				if (GameAudioComponent != null)
				{
					GameAudioComponent.InitAudio();
				}
				else
				{
					UnityEngine.Debug.LogWarning("RACING GAME KIT WARNING\r\nRace Audio component not found or disabled! Race Audio will not managed by RaceManager");
				}
				IsRaceReady = true;
				if (this.OnRaceInitiated != null)
				{
					this.OnRaceInitiated();
				}
				if (RaceStartsOnStartup)
				{
					StartRace();
				}
				if (!StartMusicAfterCountdown && GameAudioComponent != null)
				{
					GameAudioComponent.PlayBackgroundMusic = true;
				}
				blnIsAudioMoved = false;
			}
			else
			{
				UnityEngine.Debug.LogWarning("RGK WARNING\r\nSpawnpints object is missing! ");
				UnityEngine.Debug.DebugBreak();
			}
		}

		[Obsolete("This function is obsolete.Use StartRace instead.", false)]
		public void StartGame()
		{
			StartRace();
		}

		public void StartRace()
		{
			if (!IsRaceReady)
			{
				return;
			}
			if (GameUIComponent != null)
			{
				GameUIComponent.ShowCountdownWindow = true;
				GameUIComponent.CurrentCount = CurrentCount;
				if (m_RGKCam_P1 != null)
				{
					m_RGKCam_P1.CurrentCount = CurrentCount;
				}
				if (m_RGKCam_P2 != null)
				{
					m_RGKCam_P2.CurrentCount = CurrentCount;
				}
				if (this.OnCountDownStarted != null)
				{
					this.OnCountDownStarted();
				}
			}
			isCounting = true;
		}

		private void StartRaceInternal()
		{
			if (this.OnRaceStarted != null)
			{
				this.OnRaceStarted();
			}
			if (GameUIComponent != null)
			{
				GameUIComponent.ShowCountdownWindow = false;
			}
			IsRaceStarted = true;
			if (StartMusicAfterCountdown && GameAudioComponent != null)
			{
				GameAudioComponent.PlayBackgroundMusic = true;
			}
		}

		private void LateUpdate()
		{
			//if (!blnIsAudioMoved && m_SplitScreen)
			//{
			//	MoveAudioSourcesToAudioListener(Player1);
			//	MoveAudioSourcesToAudioListener(Player2);
			//	checkCount--;
			//	if (checkCount == 0)
			//	{
			//		blnIsAudioMoved = true;
			//	}
			//}
		}

		private void MoveAudioSourcesToAudioListener(GameObject Player)
		{
			if (!(oAudioListener != null))
			{
				return;
			}
			for (int i = 0; i < Player.transform.childCount; i++)
			{
				Transform child = Player.transform.GetChild(i);
				if ((bool)child.GetComponent<AudioSource>())
				{
					child.parent = oAudioListener.transform;
					child.localPosition = Vector3.zero;
				}
			}
		}

		private void ChangeSplitScreenPlayerControlrs(GameObject Player, bool IsPlayer1)
		{
			RGKCar_C2_Human component = Player.GetComponent<RGKCar_C2_Human>();
			RGKCar_C2_GamePadRace component2 = Player.GetComponent<RGKCar_C2_GamePadRace>();
			if (IsPlayer1)
			{
				bool flag = false;
				if (m_SplitScreenManager.P1_Controller == eSplitScreenController.Keyboard)
				{
					if (component != null)
					{
						component.enabled = true;
					}
					else
					{
						UnityEngine.Debug.LogWarning("RACING GAME KIT WARNING\r\n Keyboard Input for Player1 selected but Keyboard (Human) controller component not found on Player1 car!");
					}
					if (component2 != null)
					{
						component2.enabled = false;
					}
				}
				else
				{
					if (component2 != null)
					{
						component2.enabled = true;
						flag = true;
					}
					else
					{
						UnityEngine.Debug.LogWarning("RACING GAME KIT WARNING\r\n GamePad Input for Player1 selected but GamePad  controller component not found on Player1 car!");
					}
					if (component != null)
					{
						component.enabled = false;
					}
				}
				if (flag)
				{
					component2.ControlsThrottleBinding = m_SplitScreenManager.P1_Throttle;
					component2.ControlsBrakeBinding = m_SplitScreenManager.P1_Brake;
					component2.ControlsSteeringBinding = m_SplitScreenManager.P1_Steer;
					component2.ControlsHandbrakeBinding = m_SplitScreenManager.P1_Handbrake;
					component2.ControlsResetBinding = m_SplitScreenManager.P1_Reset;
					component2.ControlsHeadlightsBinding = m_SplitScreenManager.P1_Lights;
					component2.ControlsShiftUpBinding = m_SplitScreenManager.P1_ShiftUp;
					component2.ControlsShiftDownBinding = m_SplitScreenManager.P1_ShiftDown;
					component2.ControlsNitroBinding = m_SplitScreenManager.P1_Nitro;
					component2.ControlsCameraLookBackBinding = m_SplitScreenManager.P1_CameraBack;
					component2.ControlsCameraChangeBinding = m_SplitScreenManager.P1_CameraChange;
					component2.ControlsCameraLookLeftBinding = m_SplitScreenManager.P1_CameraLeft;
					component2.ControlsCameraLookRightBinding = m_SplitScreenManager.P1_CameraRight;
				}
				return;
			}
			bool flag2 = false;
			if (m_SplitScreenManager.P2_Controller == eSplitScreenController.Keyboard)
			{
				if (component != null)
				{
					component.enabled = true;
				}
				else
				{
					UnityEngine.Debug.LogWarning("RACING GAME KIT WARNING\r\n Keyboard Input for Player2 selected but Keyboard (Human) controller component not found on Player2 car!");
				}
				if (component2 != null)
				{
					component2.enabled = false;
				}
			}
			else
			{
				if (component2 != null)
				{
					component2.enabled = true;
					flag2 = true;
				}
				else
				{
					UnityEngine.Debug.LogWarning("RACING GAME KIT WARNING\r\n GamePad Input for Player2 selected but GamePad  controller component not found on Player2 car!");
				}
				if (component != null)
				{
					component.enabled = false;
				}
			}
			if (flag2)
			{
				component2.ControlsThrottleBinding = m_SplitScreenManager.P2_Throttle;
				component2.ControlsBrakeBinding = m_SplitScreenManager.P2_Brake;
				component2.ControlsSteeringBinding = m_SplitScreenManager.P2_Steer;
				component2.ControlsHandbrakeBinding = m_SplitScreenManager.P2_Handbrake;
				component2.ControlsResetBinding = m_SplitScreenManager.P2_Reset;
				component2.ControlsHeadlightsBinding = m_SplitScreenManager.P2_Lights;
				component2.ControlsShiftUpBinding = m_SplitScreenManager.P2_ShiftUp;
				component2.ControlsShiftDownBinding = m_SplitScreenManager.P2_ShiftDown;
				component2.ControlsNitroBinding = m_SplitScreenManager.P2_Nitro;
				component2.ControlsCameraLookBackBinding = m_SplitScreenManager.P2_CameraBack;
				component2.ControlsCameraChangeBinding = m_SplitScreenManager.P2_CameraChange;
				component2.ControlsCameraLookLeftBinding = m_SplitScreenManager.P2_CameraLeft;
				component2.ControlsCameraLookRightBinding = m_SplitScreenManager.P2_CameraRight;
			}
		}

		private int FindNextNotSpawnedAiIndex()
		{
			int result = -1;
			Indexes = new List<int>();
			for (int i = 0; i < AIRacerPrefab.Length; i++)
			{
				Indexes.Add(i);
			}
			ShuffleIndexes(Indexes);
			foreach (int index in Indexes)
			{
				if (!m_SpawnedAIs.Contains(index))
				{
					return index;
				}
			}
			return result;
		}

		private void ShuffleIndexes<T>(IList<T> list)
		{
			System.Random random = new System.Random();
			int num = list.Count;
			while (num > 1)
			{
				num--;
				int index = random.Next(num + 1);
				T value = list[index];
				list[index] = list[num];
				list[num] = value;
			}
		}

		private void OnDestroy()
		{
			UnityEngine.Object.DestroyObject(DistanceTransformContainer);
		}

		private void Update()
		{
			DoUpdate();
		}

		private void DoUpdate()
		{
			float smoothDeltaTime = Time.smoothDeltaTime;
			if ((double)smoothDeltaTime != 0.0)
			{
				WorkingFPS = 1f / smoothDeltaTime;
			}
			if (isCounting)
			{
				CountDownForGameStart();
			}
			switch (RaceType)
			{
			case RaceTypeEnum.Sprint:
				CalculateStandingsByDistance();
				StartTimer();
				break;
			case RaceTypeEnum.Circuit:
				CalculateStandingsByDistance();
				StartTimer();
				break;
			case RaceTypeEnum.LapKnockout:
				CalculateStandingsByDistance();
				StartTimer();
				GetLastRacer();
				break;
			case RaceTypeEnum.TimeAttack:
				StartTimer();
				CalculateCheckPointTime();
				break;
			case RaceTypeEnum.Speedtrap:
				if (SpeedTrapMode == eSpeedTrapMode.HighestSpeed)
				{
					CalculateStandingsByHighSpeed();
				}
				else if (SpeedTrapMode == eSpeedTrapMode.HighestTotalSpeed)
				{
					CalculateStandingsByTotalHighSpeed();
				}
				else if (SpeedTrapMode == eSpeedTrapMode.HighestSpeedInRace)
				{
					CalculateStandingsByHighSpeedInRace();
				}
				StartTimer();
				break;
			}
			CheckIsRaceFinished();
		}

		public void ShowWrongWayForPlayer(bool isWrongWay)
		{
			if (isWrongWay)
			{
				if (GameUIComponent != null)
				{
					GameUIComponent.ShowWrongWayWindow = true;
				}
			}
			else if (GameUIComponent != null)
			{
				GameUIComponent.ShowWrongWayWindow = false;
			}
		}

		private void CheckIsRaceFinished()
		{
			if (IsRaceStarted && !IsRaceFinished)
			{
				for (int i = 0; i < ListRacers.Count; i++)
				{
					if (StopRaceOnPlayerFinish && ListRacers[i].IsPlayer && ListRacers[i].RacerFinished)
					{
						UnityEngine.Debug.Log("Player Finished");
						tmpFinished = true;
						break;
					}
				}
				if (!tmpFinished)
				{
					for (int j = 0; j < ListRacers.Count; j++)
					{
						if (ListRacers[j].RacerFinished)
						{
							tmpFinished = true;
							continue;
						}
						tmpFinished = false;
						break;
					}
				}
				if (IsRaceStarted && tmpFinished)
				{
					IsRaceFinished = true;
				}
			}
			if (!IsRaceFinished || m_IsRaceFinishPushed)
			{
				return;
			}
			if (GameUIComponent != null)
			{
				GameUIComponent.RaceFinished(RaceType.ToString());
			}
			if (this.OnRaceFinished != null)
			{
				this.OnRaceFinished(RaceType);
				MonoBehaviour.print(positionCheck.RacerStanding);
				MonoBehaviour.print("------------------------------ we think this is fro level clear -------------------------------");
				UnityEngine.Debug.Log("GameOver/LevelClear ------------------------------------------------------");
				if (positionCheck.RacerStanding < 4f && positionCheck.RacerStanding > 0f)
				{
					PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 20000);
					if (!PlayerPrefs.HasKey("Level" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex))
					{
						PlayerPrefs.SetInt("Level" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, 1);
						switch (PlayerPrefs.GetInt("environmentNumber"))
						{
						case 0:
							PlayerPrefs.SetInt("environment0levelCleared", PlayerPrefs.GetInt("environment0levelCleared") + 1);
							break;
						case 1:
							PlayerPrefs.SetInt("environment1levelCleared", PlayerPrefs.GetInt("environment1levelCleared") + 1);
							break;
						case 2:
							PlayerPrefs.SetInt("environment2levelCleared", PlayerPrefs.GetInt("environment2levelCleared") + 1);
							break;
						case 3:
							PlayerPrefs.SetInt("environment3levelCleared", PlayerPrefs.GetInt("environment3levelCleared") + 1);
							break;
						case 4:
							PlayerPrefs.SetInt("environment4levelCleared", PlayerPrefs.GetInt("environment4levelCleared") + 1);
							break;
						}
					}
				}
			}
			UnityEngine.Debug.Log("Race Finished!");
			m_IsRaceFinishPushed = true;
		}

		private void GetLastRacer()
		{
			if (KickLastRacerAfter2ndLast && m_LastRacerIndex > 0 && !ListRacers[m_LastRacerIndex - 1].RacerDestroyed && (ListRacers[m_LastRacerIndex - 1].RacerLap > ListRacers[m_LastRacerIndex].RacerLap || ListRacers[m_LastRacerIndex - 1].RacerFinished))
			{
				LastRacerID = ListRacers[m_LastRacerIndex].ID.ToString();
				LastRacerName = ListRacers[m_LastRacerIndex].RacerName.ToString();
				ListRacers[m_LastRacerIndex].RacerFinished = true;
				ListRacers[m_LastRacerIndex].RacerDestroyed = true;
				ListRacers[m_LastRacerIndex].RacerTotalTime = TimeTotal;
				if (this.OnRacerKicked != null)
				{
					this.OnRacerKicked(ListRacers[m_LastRacerIndex]);
				}
				m_LastRacerIndex = -1;
				return;
			}
			int num = ListRacers.Count - 1;
			while (true)
			{
				if (num < 1)
				{
					return;
				}
				if (KickLastRacerAfter2ndLast)
				{
					if (!ListRacers[num].RacerDestroyed && m_LastRacerIndex == -1)
					{
						m_LastRacerIndex = num;
					}
				}
				else if (!ListRacers[num].RacerDestroyed && m_LastRacerIndex == -1)
				{
					break;
				}
				num--;
			}
			LastRacerID = ListRacers[num].ID.ToString();
			LastRacerName = ListRacers[num].RacerName.ToString();
		}

		private void CalculateStandingsByDistance()
		{
			int num = 0;
			foreach (Racer_Detail listRacer in ListRacers)
			{
				if (listRacer.RacerFinished && !listRacer.RacerDestroyed)
				{
					num++;
				}
			}
			if (num >= ListRacers.Count)
			{
				return;
			}
			RacerComparer comparer = new RacerComparer();
			ListRacers.Sort(num, ListRacers.Count - num, comparer);
			for (int i = num; i < ListRacers.Count; i++)
			{
				if (!ListRacers[i].RacerFinished && !ListRacers[i].RacerDestroyed)
				{
					ListRacers[i].RacerStanding = i + 1;
				}
			}
			ListRacers.Sort((Racer_Detail r1, Racer_Detail r2) => r1.RacerStanding.CompareTo(r2.RacerStanding));
		}

		private void CalculateStandingsByHighSpeed()
		{
			int num = 0;
			bool flag = false;
			foreach (Racer_Detail listRacer in ListRacers)
			{
				if (listRacer.RacerFinished && !listRacer.RacerDestroyed)
				{
					num++;
				}
				if (listRacer.RacerHighestSpeed > 0f)
				{
					flag = true;
				}
			}
			if (num >= ListRacers.Count || !flag)
			{
				return;
			}
			RacerComparerByHighSpeed comparer = new RacerComparerByHighSpeed();
			ListRacers.Sort(num, ListRacers.Count - num, comparer);
			for (int i = num; i < ListRacers.Count; i++)
			{
				if (!ListRacers[i].RacerFinished && !ListRacers[i].RacerDestroyed)
				{
					ListRacers[i].RacerStanding = i + 1;
				}
			}
			ListRacers.Sort((Racer_Detail r1, Racer_Detail r2) => r1.RacerStanding.CompareTo(r2.RacerStanding));
		}

		private void CalculateStandingsByTotalHighSpeed()
		{
			int num = 0;
			bool flag = false;
			foreach (Racer_Detail listRacer in ListRacers)
			{
				if (listRacer.RacerFinished && !listRacer.RacerDestroyed)
				{
					num++;
				}
				if (listRacer.RacerHighestSpeed > 0f)
				{
					flag = true;
				}
			}
			if (num >= ListRacers.Count || !flag)
			{
				return;
			}
			RacerComparerByTotalHighSpeed comparer = new RacerComparerByTotalHighSpeed();
			ListRacers.Sort(num, ListRacers.Count - num, comparer);
			for (int i = num; i < ListRacers.Count; i++)
			{
				if (!ListRacers[i].RacerFinished && !ListRacers[i].RacerDestroyed)
				{
					ListRacers[i].RacerStanding = i + 1;
				}
			}
			ListRacers.Sort((Racer_Detail r1, Racer_Detail r2) => r1.RacerStanding.CompareTo(r2.RacerStanding));
		}

		private void CalculateStandingsByHighSpeedInRace()
		{
			int num = 0;
			bool flag = false;
			foreach (Racer_Detail listRacer in ListRacers)
			{
				if (listRacer.RacerFinished && !listRacer.RacerDestroyed)
				{
					num++;
				}
				if (listRacer.RacerHighestSpeedInRaceAsKm > 0f)
				{
					flag = true;
				}
			}
			if (num >= ListRacers.Count || !flag)
			{
				return;
			}
			RacerComparerByHighSpeedInRace comparer = new RacerComparerByHighSpeedInRace();
			ListRacers.Sort(num, ListRacers.Count - num, comparer);
			for (int i = num; i < ListRacers.Count; i++)
			{
				if (!ListRacers[i].RacerFinished && !ListRacers[i].RacerDestroyed)
				{
					ListRacers[i].RacerStanding = i + 1;
				}
			}
			ListRacers.Sort((Racer_Detail r1, Racer_Detail r2) => r1.RacerStanding.CompareTo(r2.RacerStanding));
		}

		private void CountDownForGameStart()
		{
			if (!isCounting)
			{
				return;
			}
			countdown_internal_timer -= Time.deltaTime;
			if (CurrentCount >= 1)
			{
				if (countdown_internal_timer < 0.1f)
				{
					PlayAudio(eRaceAudioFXName.RaceCountdown);
					CurrentCount--;
					if (GameUIComponent != null)
					{
						GameUIComponent.CurrentCount = CurrentCount;
					}
					m_RGKCam_P1.CurrentCount = CurrentCount;
					if (m_RGKCam_P2 != null)
					{
						m_RGKCam_P2.CurrentCount = CurrentCount;
					}
					UnityEngine.Debug.Log("Race Starting in " + CurrentCount);
					countdown_internal_timer = 1f;
				}
			}
			else
			{
				if (this.OnCountDownFinished != null)
				{
					this.OnCountDownFinished();
				}
				PlayAudio(eRaceAudioFXName.RaceStart);
				StartRaceInternal();
				m_RGKCam_P1.IsStartupAnimationEnabled = false;
				isCounting = false;
			}
		}

		private void StartTimer()
		{
			if (IsRaceStarted && !IsRaceFinished)
			{
				TimeTotal = Time.time - TimeTotalFix;
				//if (!m_SplitScreen)
				//{
					TimeCurrent = TimeTotal - CurrentTimeFixForPlayer;
				//}
				//else
				//{
				//	TimeCurrent = TimeTotal;
				//}
			}
			else
			{
				TimeTotalFix = Time.time;
			}
		}

		private void GetFirstCheckPoint()
		{
			Transform[] childTransforms = GetChildTransforms(CheckPoints.transform);
			CheckPointItem component = childTransforms[0].GetComponent<CheckPointItem>();
			TimeTotalCheckPoint = component.CheckpointTime;
		}

		private void CalculateCheckPointTime()
		{
			if (IsRaceStarted && !IsRaceFinished)
			{
				TimeNextCheckPoint = TimeTotalCheckPoint - Time.timeSinceLevelLoad + (float)TimerCountdownFrom;
				if (TimeNextCheckPoint <= 0f)
				{
					ListRacers[0].RacerFinished = true;
					ListRacers[0].RacerDestroyed = true;
					IsRaceFinished = true;
				}
			}
		}

		public void PlayerPassedCheckPoint(CheckPointItem PassedCheckPoint)
		{
			TimeTotalCheckPoint += PassedCheckPoint.CheckpointBonus;
		}

		public void PlayAudio(eRaceAudioFXName Audio)
		{
			if (GameAudioComponent != null)
			{
				GameAudioComponent.PlayAudio(Audio);
			}
		}

		private void CreateDistanceMeasurementTransforms()
		{
			if (Waypoints == null)
			{
				return;
			}
			DistanceTransformContainer = new GameObject();
			DistanceTransformContainer.name = "_DistanceContainer";
			Transform[] childTransforms = GetChildTransforms(Waypoints.transform);
			if (childTransforms.Length < 2)
			{
				return;
			}
			if (DistancePointDensity < (float)childTransforms.Length)
			{
				DistancePointDensity = childTransforms.Length;
			}
			SplineInterpolator component = GetComponent<SplineInterpolator>();
			SetupSplineInterpolator(component, childTransforms);
			component.StartInterpolation(null, bRotations: false, eWrapMode.ONCE);
			for (int i = 0; (float)i <= DistancePointDensity - 1f; i++)
			{
				float timeParam = (float)(i * 5) / DistancePointDensity;
				Vector3 hermiteAtTime = component.GetHermiteAtTime(timeParam);
				GameObject gameObject = new GameObject();
				if (i < 10)
				{
					gameObject.name = "0" + i.ToString();
				}
				else
				{
					gameObject.name = i.ToString();
				}
				gameObject.transform.parent = DistanceTransformContainer.transform;
				gameObject.transform.localPosition = hermiteAtTime;
			}
			if (FinishPoint != null)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.name = DistancePointDensity.ToString();
				gameObject2.transform.parent = DistanceTransformContainer.transform;
				gameObject2.transform.localPosition = FinishPoint.transform.position;
				gameObject2.transform.localRotation = FinishPoint.transform.rotation;
				DistanceMeasurementObjects = GetChildTransforms(DistanceTransformContainer.transform);
				for (int j = 0; j < DistanceMeasurementObjects.GetUpperBound(0); j++)
				{
					if (j < DistanceMeasurementObjects.GetUpperBound(0) && j != DistanceMeasurementObjects.GetUpperBound(0))
					{
						RaceLength += Vector3.Distance(DistanceMeasurementObjects[j].transform.position, DistanceMeasurementObjects[j + 1].transform.position);
					}
				}
				FixDistancePointRotations(DistanceTransformContainer);
			}
			else
			{
				UnityEngine.Debug.LogWarning("RGK WARNING\r\nFinishPoint is missing");
			}
		}

		public static void FixDistancePointRotations(GameObject DistancePointContainer)
		{
			List<Component> list = new List<Component>(DistancePointContainer.gameObject.GetComponentsInChildren(typeof(Transform)));
			List<Transform> list2 = new List<Transform>();
			foreach (Component item in list)
			{
				list2.Add((Transform)item);
			}
			list2.Remove(DistancePointContainer.gameObject.transform);
			list2.Sort((Transform a, Transform b) => Convert.ToInt16(a.name).CompareTo(Convert.ToInt16(b.name)));
			Transform[] array = list2.ToArray();
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (array[i + 1] != null)
				{
					array[i].transform.LookAt(array[i + 1].transform);
				}
				if (i == array.Length - 1)
				{
					array[i].transform.LookAt(array[1].transform);
				}
			}
		}

		private void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
		{
			bool flag = false;
			float num = 5f;
			eOrientationMode eOrientationMode = eOrientationMode.NODE;
			interp.Reset();
			float num2 = (!flag) ? (num / (float)(trans.Length - 1)) : (num / (float)trans.Length);
			int i;
			for (i = 0; i < trans.Length; i++)
			{
				switch (eOrientationMode)
				{
				case eOrientationMode.NODE:
					interp.AddPoint(trans[i].position, trans[i].rotation, num2 * (float)i, new Vector2(0f, 1f));
					break;
				case eOrientationMode.TANGENT:
					interp.AddPoint(quat: (i != trans.Length - 1) ? Quaternion.LookRotation(trans[i + 1].position - trans[i].position, trans[i].up) : ((!flag) ? trans[i].rotation : Quaternion.LookRotation(trans[0].position - trans[i].position, trans[i].up)), pos: trans[i].position, timeInSeconds: num2 * (float)i, easeInOut: new Vector2(0f, 1f));
					break;
				}
			}
			if (flag)
			{
				interp.SetAutoCloseMode(num2 * (float)i);
			}
		}

		private Transform[] GetChildTransforms(Transform RootTransform)
		{
			if (RootTransform == null)
			{
				return new Transform[0];
			}
			List<Component> list = new List<Component>(RootTransform.GetComponentsInChildren(typeof(Transform)));
			List<Transform> list2 = new List<Transform>();
			foreach (Component item in list)
			{
				list2.Add((Transform)item);
			}
			list2.Remove(RootTransform.transform);
			list2.Sort((Transform a, Transform b) => Convert.ToInt32(a.name).CompareTo(Convert.ToInt32(b.name)));
			return list2.ToArray();
		}
	}
}
