using RacingGameKit.AI;
using RacingGameKit.Racers;
using RacingGameKit.RGKCar.CarControllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Game Mechanics/Racer Register")]
	public class Racer_Register : MonoBehaviour
	{
		public string RacerID;

		public bool IsPlayer;

		public string RacerName = string.Empty;

		public int RacerStanding;

		public bool IsRacerFinished;

		public bool IsRacerStarted;

		public bool IsRacerDestroyed;

		public Race_Manager RaceManager;

		public Racer_Detail RacerDetail;

		private List<Transform> DistanceList = new List<Transform>();

		private int RaceManagerRacerIndex;

		public int GamerCurrentLap = 1;

		private GameObject RacerTag;

		private TextMesh RacerTagName;

		private TextMesh RacerTagPlace;

		private Transform GamerDistanceChecker;

		private Transform[] DistanceArray;

		private Transform[] CheckpointArray;

		[HideInInspector]
		public GameObject CheckpointArrow;

		[HideInInspector]
		private int CurrentCheckpointIndexInContainer;

		private bool IsCheckPointSystemEnabled;

		private float triggerLockTime = 1f;

		private bool isTriggerLocked;

		[SerializeField]
		private int CurrentDistancePointIndex;

		private int LastDistancePointIndex;

		private bool DistanceCheckingStuffLocked;

		private Vector3 RelativeDPPosition;

		private float AngleOfDP;

		private float AngleOfVH;

		private float AngleDif;

		private bool RotatedBack;

		private bool From4;

		public bool IsWrongWay;

		private int DistanceArrayLenght;

		private bool HitLastDP;

		[HideInInspector]
		public float MyTimeFix;

		[HideInInspector]
		public bool PlayerContinueAfterFinish;

		private Rigidbody m_RB;

		public Transform ClosestDP;

		private void Awake()
		{
			if (base.enabled)
			{
				try
				{
					GameObject gameObject = GameObject.Find("_RaceManager");
					if (gameObject != null)
					{
						RaceManager = gameObject.GetComponent<Race_Manager>();
					}
					if (base.gameObject.tag == "Player")
					{
						IsPlayer = true;
					}
					RegisterRacerToRaceManager();
				}
				catch
				{
				}
			}
		}

		private void Start()
		{
			if (!base.enabled)
			{
				return;
			}
			m_RB = base.transform.GetComponent<Rigidbody>();
			GamerDistanceChecker = base.transform.Find("_DistancePoint");
			if (GamerDistanceChecker == null)
			{
				UnityEngine.Debug.LogError("RGK WARNING\r\nDistance checker object not found! Be sure _DistancePoint named object placed under child!");
				UnityEngine.Debug.DebugBreak();
			}
			if (RaceManager.CheckPoints != null)
			{
				CheckpointArray = GetChildTransforms(RaceManager.CheckPoints.transform);
				if (CheckpointArray.Length > 0)
				{
					IsCheckPointSystemEnabled = true;
					Transform transform = RaceManager.oCamera1.transform.Find("CheckpointArrow");
					if (transform != null)
					{
						CheckpointArrow = transform.gameObject;
					}
					if (CheckpointArrow != null && RaceManager.EnableCheckpointArrow)
					{
						CheckpointArrow.SetActive(value: true);
					}
				}
			}
			GameObject distanceTransformContainer = RaceManager.DistanceTransformContainer;
			DistanceArray = GetChildTransforms(distanceTransformContainer.transform);
			DistanceList.AddRange(DistanceArray);
			SortDPByDistance();
			DistanceArrayLenght = DistanceArray.Length;
			if (!(base.gameObject.transform.Find("_RacerTag") != null))
			{
				return;
			}
			RacerTag = base.gameObject.transform.Find("_RacerTag").gameObject;
			if (RacerTag != null && RacerTag.activeInHierarchy)
			{
				RacerTagName = RacerTag.transform.Find("Name").GetComponent<TextMesh>();
				RacerTagPlace = RacerTag.transform.Find("Place").GetComponent<TextMesh>();
				if (RacerTagPlace != null)
				{
					RacerTagPlace.text = RaceManager.RegisteredRacers[RaceManagerRacerIndex].RacerStanding.ToString();
				}
				if (RacerTagName != null)
				{
					RacerTagName.text = RacerName;
				}
			}
		}

		private void RegisterRacerToRaceManager()
		{
			if (RaceManager != null)
			{
				RacerDetail = new Racer_Detail();
				RacerDetail.RacerObject = base.gameObject;
				RacerDetail.ID = UnityEngine.Random.Range(1000, 50000).ToString();
				RacerID = RacerDetail.ID;
				RaceManagerRacerIndex = RaceManager.RegisterToGame(RacerDetail);
				RacerDetail.RacerLap = GamerCurrentLap;
				if (IsPlayer)
				{
					RacerDetail.RacerName = RacerName;
					RacerDetail.IsPlayer = true;
				}
				else
				{
					RacerDetail.RacerName = RaceManager.GetNameForRacer(RacerName);
					RacerName = RacerDetail.RacerName;
				}
			}
		}

		private void OnTriggerEnter(Collider Trigger)
		{
			CheckTriggeredThings(Trigger);
		}

		private void CheckTriggeredThings(Collider Trigger)
		{
			if (!base.enabled)
			{
				return;
			}
			if (IsRacerStarted && !IsRacerFinished)
			{
				if (isTriggerLocked)
				{
					return;
				}
				if (Trigger.name == RaceManager.FinishPoint.transform.name && HitLastDP)
				{
					if (HitLastDP)
					{
						DistanceCheckingStuffLocked = false;
						CurrentDistancePointIndex = 0;
						HitLastDP = false;
					}
					GamerCurrentLap++;
					RacerDetail.RacerLap = GamerCurrentLap;
					float timeTotal = RaceManager.TimeTotal;
					timeTotal -= MyTimeFix;
					if (!IsRacerFinished)
					{
						RacerDetail.RacerLapTimes.Add(timeTotal);
					}
					if (GamerCurrentLap > RaceManager.RaceLaps)
					{
						IsRacerFinished = true;
						RacerDetail.RacerTotalTime = RaceManager.TimeTotal;
						RacerDetail.RacerFinished = true;
						if (IsPlayer && RaceManager.PlayerContinuesAfterFinish)
						{
							SwitchToAIMode();
						}
						GamerCurrentLap--;
						RacerDetail.RacerLap = GamerCurrentLap;
					}
					MyTimeFix += timeTotal;
					RacerDetail.RacerCurrentLapTimeFix = MyTimeFix;
					if (RacerDetail.ID.ToString().Equals(RaceManager.LastRacerID))
					{
						SetRacerDestroyed();
					}
					if (base.gameObject.tag == "Player")
					{
						RacerDetail.RacerLastTime = timeTotal;
						RaceManager.CurrentTimeFixForPlayer = MyTimeFix;
						RaceManager.TimePlayerLast = RacerDetail.RacerLastTime;
						if (RacerDetail.RacerBestTime == 0f)
						{
							RacerDetail.RacerBestTime = RacerDetail.RacerLastTime;
						}
						else if (RacerDetail.RacerLastTime < RacerDetail.RacerBestTime)
						{
							RacerDetail.RacerBestTime = RacerDetail.RacerLastTime;
						}
						RaceManager.TimePlayerBest = RacerDetail.RacerBestTime;
						if (!IsRacerFinished)
						{
							RaceManager.RaceActiveLap++;
						}
					}
					else
					{
						RacerDetail.RacerLastTime = timeTotal;
						if (RacerDetail.RacerBestTime == 0f)
						{
							RacerDetail.RacerBestTime = RacerDetail.RacerLastTime;
						}
						else if (RacerDetail.RacerLastTime < RacerDetail.RacerBestTime)
						{
							RacerDetail.RacerBestTime = RacerDetail.RacerLastTime;
						}
					}
				}
				if (IsCheckPointSystemEnabled && (Trigger.tag == "CheckPoint" || !RacerDetail.RacerFinished))
				{
					if (RacerDetail.RacerSectorSpeedAndTime == null)
					{
						RacerDetail.RacerSectorSpeedAndTime = new List<Racer_Detail.SectorSpeedAndTime>();
					}
					Racer_Detail.SectorSpeedAndTime sectorSpeedAndTime = new Racer_Detail.SectorSpeedAndTime();
					sectorSpeedAndTime.Lap = GamerCurrentLap;
					sectorSpeedAndTime.SectorTime = 0f;
					sectorSpeedAndTime.SectorSpeed = m_RB.velocity.magnitude * 3.6f;
					RacerDetail.RacerSectorSpeedAndTime.Add(sectorSpeedAndTime);
					if (IsPlayer && Trigger.name == CheckpointArray[CurrentCheckpointIndexInContainer].name)
					{
						SetNextCheckPoint(CurrentCheckpointIndexInContainer);
					}
					else if (IsPlayer && Trigger.name != CheckpointArray[CurrentCheckpointIndexInContainer].name)
					{
						PlayWrongCheckpoint();
					}
				}
			}
			if (Trigger.name == RaceManager.StartPoint.transform.name)
			{
				IsRacerStarted = true;
				isTriggerLocked = true;
			}
		}

		private void SetRacerDestroyed()
		{
			UnityEngine.Debug.Log(RacerDetail.RacerName + " DESTROYED!");
			RacerDetail.RacerTotalTime = RaceManager.TimeTotal;
			RacerDetail.RacerFinished = true;
			RacerDetail.RacerDestroyed = true;
			IsRacerDestroyed = true;
			IsRacerFinished = true;
			if (RaceManager.RemoveKickedRacersFromScene && !IsPlayer)
			{
				base.transform.gameObject.SetActive(value: false);
			}
		}

		private void SwitchToAIMode()
		{
			RGK_Racer_Basic_AI component = GetComponent<RGK_Racer_Basic_AI>();
			RGKCar_C2_AI component2 = GetComponent<RGKCar_C2_AI>();
			RGKCar_C2_Human component3 = GetComponent<RGKCar_C2_Human>();
			RGKCar_C2_MobileRace component4 = GetComponent<RGKCar_C2_MobileRace>();
			RGKCar_C2_GamePadRace component5 = GetComponent<RGKCar_C2_GamePadRace>();
			if (component != null && component2 != null)
			{
				component.enabled = true;
				if (component3 != null)
				{
					component3.enabled = false;
				}
				if (component4 != null)
				{
					component4.enabled = false;
				}
				if (component5 != null)
				{
					component5.enabled = false;
				}
				component2.enabled = true;
			}
		}

		private void ChangeLayersRecursively(Transform trans, string LayerName)
		{
			for (int i = 0; i < trans.childCount; i++)
			{
				trans.GetChild(i).gameObject.layer = LayerMask.NameToLayer(LayerName);
				ChangeLayersRecursively(trans.GetChild(i), LayerName);
			}
		}

		private void Update()
		{
			if (!base.enabled)
			{
				return;
			}
			if (!IsRacerFinished && !IsRacerDestroyed)
			{
				RacerDetail.RacerCurrentLapTime = RaceManager.TimeTotal - MyTimeFix;
				RacerDetail.RacerTotalTime = RaceManager.TimeTotal;
			}
			if (RacerDetail.RacerDestroyed != IsRacerDestroyed)
			{
				SetRacerDestroyed();
				DisableAI();
			}
			if (RacerDetail.RacerFinished != IsRacerFinished)
			{
				IsRacerFinished = RacerDetail.RacerFinished;
			}
			if (IsRacerDestroyed && RaceManager.MoveKickedRacerToIgnoreLayer && base.gameObject.layer != LayerMask.NameToLayer("IGNORE"))
			{
				ChangeLayersRecursively(base.transform, "IGNORE");
			}
			if (!IsRacerFinished && !IsRacerDestroyed)
			{
				float num = CalculateRemainingDistance();
				num += (float)(RaceManager.RaceLaps - GamerCurrentLap) * RaceManager.RaceLength;
				RacerDetail.RacerDistance = num;
			}
			RacerDetail.RacerPostionOnMap = base.transform.position;
			RacerDetail.RacerRotationOnMap = base.transform.rotation;
			RacerDetail.RacerDestroyed = IsRacerDestroyed;
			RacerStanding = Convert.ToInt32(RacerDetail.RacerStanding);
			if (RacerTagPlace != null)
			{
				RacerTagPlace.text = RacerStanding.ToString();
			}
			if (IsPlayer)
			{
				if (RacerTag != null && RacerTag.activeInHierarchy)
				{
					RacerTag.SetActive(value: false);
				}
				if (RaceManager.EnableCheckpointArrow)
				{
					UpdateCheckPointArrow();
				}
				RaceManager.ShowWrongWayForPlayer(IsWrongWay);
			}
			if (Mathf.Abs(AngleDif) <= 230f && Mathf.Abs(AngleDif) >= 120f && m_RB.velocity.magnitude * 3.6f > 10f)
			{
				IsWrongWay = true;
				RacerDetail.RacerWrongWay = true;
			}
			if (!RotatedBack)
			{
				IsWrongWay = false;
				RacerDetail.RacerWrongWay = false;
			}
			if (isTriggerLocked)
			{
				triggerLockTime -= Time.deltaTime;
				if (triggerLockTime <= 0f)
				{
					isTriggerLocked = false;
					triggerLockTime = 1f;
				}
			}
			if (IsPlayer && RaceManager.RaceType.Equals(RaceTypeEnum.TimeAttack) && RaceManager.IsRaceFinished)
			{
				IsRacerFinished = true;
			}
			if (IsRacerStarted)
			{
				float num2 = m_RB.velocity.magnitude * 3.6f;
				if (num2 > RacerDetail.RacerHighestSpeedInRaceAsKm)
				{
					RacerDetail.RacerHighestSpeedInRaceAsKm = num2;
				}
			}
		}

		private float CalculateRemainingDistance()
		{
			if (IsRacerFinished && !IsRacerDestroyed)
			{
				return 0f;
			}
			if (!IsRacerStarted)
			{
				CurrentDistancePointIndex = 0;
				LastDistancePointIndex = 0;
			}
			if (CurrentDistancePointIndex.Equals(DistanceArrayLenght - 1))
			{
				DistanceCheckingStuffLocked = true;
			}
			Transform transform = base.transform;
			Vector3 position = DistanceArray[CurrentDistancePointIndex].transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			float y = position2.y;
			Vector3 position3 = DistanceArray[CurrentDistancePointIndex].transform.position;
			RelativeDPPosition = transform.InverseTransformPoint(new Vector3(x, y, position3.z));
			Vector3 eulerAngles = DistanceArray[CurrentDistancePointIndex].transform.eulerAngles;
			AngleOfDP = eulerAngles.y;
			Vector3 eulerAngles2 = base.gameObject.transform.eulerAngles;
			AngleOfVH = eulerAngles2.y;
			AngleDif = AngleOfDP - AngleOfVH;
			if (Mathf.Abs(AngleDif) <= 270f && Mathf.Abs(AngleDif) >= 80f)
			{
				RotatedBack = true;
			}
			else
			{
				RotatedBack = false;
			}
			if (RelativeDPPosition.z <= 1f && !RotatedBack && !HitLastDP)
			{
				ClosestDP = GetClosestDP(DistanceArray);
				if ((CurrentDistancePointIndex + 1).Equals(DistanceArrayLenght))
				{
					if (DistanceCheckingStuffLocked)
					{
						LastDistancePointIndex = 0;
						CurrentDistancePointIndex = 0;
						HitLastDP = false;
					}
				}
				else if (CurrentDistancePointIndex >= LastDistancePointIndex)
				{
					bool flag = false;
					if (Convert.ToInt16(ClosestDP.name) + 1 == DistanceArrayLenght)
					{
						flag = true;
					}
					else if (DistanceArray[CurrentDistancePointIndex + 1].name == DistanceArray[Array.FindIndex(DistanceArray, (Transform tr) => tr.name == ClosestDP.name) + 1].name)
					{
						flag = true;
					}
					LastDistancePointIndex = CurrentDistancePointIndex;
					if (!From4 && flag && !ClosestDP.name.Equals(DistanceArrayLenght - 1))
					{
						CurrentDistancePointIndex++;
						if (CurrentDistancePointIndex == DistanceArrayLenght - 1)
						{
							HitLastDP = true;
						}
					}
					else
					{
						CurrentDistancePointIndex = Array.FindIndex(DistanceArray, (Transform tr) => tr.name == ClosestDP.name);
						From4 = false;
					}
				}
				else if (CurrentDistancePointIndex != 0)
				{
					CurrentDistancePointIndex = LastDistancePointIndex;
					From4 = true;
				}
				else if (CurrentDistancePointIndex == 0 && LastDistancePointIndex == DistanceArrayLenght - 2)
				{
					LastDistancePointIndex = 0;
					HitLastDP = false;
				}
				else if (CurrentDistancePointIndex == DistanceArrayLenght - 1)
				{
					CurrentDistancePointIndex = LastDistancePointIndex;
				}
			}
			float num = 0f;
			Vector3.Distance(GamerDistanceChecker.position, DistanceArray[CurrentDistancePointIndex].transform.position);
			for (int i = CurrentDistancePointIndex; i < DistanceArray.GetUpperBound(0); i++)
			{
				num += Vector3.Distance(DistanceArray[i].transform.position, DistanceArray[i + 1].transform.position);
			}
			num += Vector3.Distance(GamerDistanceChecker.position, DistanceArray[CurrentDistancePointIndex].transform.position);
			UnityEngine.Debug.DrawLine(GamerDistanceChecker.position, DistanceArray[CurrentDistancePointIndex].transform.position, Color.red);
			return num;
		}

		private void SortDPByDistance()
		{
			DistanceList.Sort((Transform t1, Transform t2) => Vector3.Distance(t1.position, GamerDistanceChecker.position).CompareTo(Vector3.Distance(t2.position, GamerDistanceChecker.position)));
		}

		private Transform GetClosestDP(Transform[] DPs)
		{
			Transform result = null;
			float num = float.PositiveInfinity;
			foreach (Transform transform in DPs)
			{
				float num2 = Vector3.Distance(transform.position, GamerDistanceChecker.position);
				if (num2 < num)
				{
					result = transform;
					num = num2;
				}
			}
			return result;
		}

		private void PlayWrongCheckpoint()
		{
			RaceManager.PlayAudio(eRaceAudioFXName.WrongCheckpoint);
		}

		private void SetNextCheckPoint(int PassedCheckpointIndex)
		{
			RaceManager.PlayerPassedCheckPoint(CheckpointArray[CurrentCheckpointIndexInContainer].GetComponent<CheckPointItem>());
			RaceManager.PlayAudio(eRaceAudioFXName.Checkpoint);
			CurrentCheckpointIndexInContainer++;
			if (CurrentCheckpointIndexInContainer == CheckpointArray.Length)
			{
				CurrentCheckpointIndexInContainer = 0;
			}
		}

		private void UpdateCheckPointArrow()
		{
			if (!IsRacerFinished && RaceManager.EnableCheckpointArrow && IsCheckPointSystemEnabled && CheckpointArray.Length > 0)
			{
				if (!IsRacerStarted)
				{
					CurrentCheckpointIndexInContainer = 0;
				}
				UnityEngine.Debug.DrawLine(GamerDistanceChecker.position, CheckpointArray[CurrentCheckpointIndexInContainer].transform.position, Color.white);
				if (CheckpointArrow != null)
				{
					CheckpointArrow.transform.LookAt(CheckpointArray[CurrentCheckpointIndexInContainer].transform);
					Vector3 position = CheckpointArray[CurrentCheckpointIndexInContainer].transform.position;
					float x = position.x;
					Vector3 position2 = CheckpointArrow.transform.position;
					float y = position2.y;
					Vector3 position3 = CheckpointArray[CurrentCheckpointIndexInContainer].transform.position;
					Vector3 worldPosition = new Vector3(x, y, position3.z);
					CheckpointArrow.transform.LookAt(worldPosition);
				}
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

		private void DisableAI()
		{
			AI_Basic component = base.transform.GetComponent<AI_Basic>();
			if (component != null)
			{
				component.Sleep = true;
			}
			AI_Brain component2 = base.transform.GetComponent<AI_Brain>();
			if (component2 != null)
			{
				component2.m_Sleep = true;
			}
		}
	}
}
