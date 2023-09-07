using System;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit
{
	[Serializable]
	[AddComponentMenu("")]
	[HideInInspector]
	public class Racer_Detail : IComparable<Racer_Detail>
	{
		public class SectorSpeedAndTime
		{
			public int Lap
			{
				get;
				set;
			}

			public float SectorTime
			{
				get;
				set;
			}

			public float SectorSpeed
			{
				get;
				set;
			}
		}

		private string _ID;

		private float a;

		private float b;

		private string c = string.Empty;

		private float d;

		private float e;

		private double f;

		private bool g;

		private float h;

		private bool i;

		private Vector3 j;

		private Quaternion k;

		private bool l;

		private List<float> n;

		private bool m;

		private List<SectorSpeedAndTime> p;

		private float r;

		private float s;

		private float t;

		private GameObject o;

		private object mo;

		public string ID
		{
			get
			{
				return _ID;
			}
			set
			{
				_ID = value;
			}
		}

		public string RacerName
		{
			get
			{
				return c;
			}
			set
			{
				c = value;
			}
		}

		public float RacerCurrentLapTime
		{
			get
			{
				return r;
			}
			set
			{
				r = value;
			}
		}

		public float RacerCurrentLapTimeFix
		{
			get
			{
				return s;
			}
			set
			{
				s = value;
			}
		}

		public float RacerLastTime
		{
			get
			{
				return a;
			}
			set
			{
				a = value;
			}
		}

		public float RacerBestTime
		{
			get
			{
				return e;
			}
			set
			{
				e = value;
			}
		}

		public float RacerTotalTime
		{
			get
			{
				return h;
			}
			set
			{
				h = value;
			}
		}

		public float RacerStanding
		{
			get
			{
				return b;
			}
			set
			{
				b = value;
			}
		}

		public float RacerDistance
		{
			get
			{
				return d;
			}
			set
			{
				d = value;
			}
		}

		public double RacerLap
		{
			get
			{
				return f;
			}
			set
			{
				f = value;
			}
		}

		public bool RacerFinished
		{
			get
			{
				return g;
			}
			set
			{
				g = value;
			}
		}

		public bool IsPlayer
		{
			get
			{
				return i;
			}
			set
			{
				i = value;
			}
		}

		public Vector3 RacerPostionOnMap
		{
			get
			{
				return j;
			}
			set
			{
				j = value;
			}
		}

		public Quaternion RacerRotationOnMap
		{
			get
			{
				return k;
			}
			set
			{
				k = value;
			}
		}

		public bool RacerDestroyed
		{
			get
			{
				return l;
			}
			set
			{
				l = value;
			}
		}

		public List<float> RacerLapTimes
		{
			get
			{
				return n;
			}
			set
			{
				n = value;
			}
		}

		public bool RacerWrongWay
		{
			get
			{
				return m;
			}
			set
			{
				m = value;
			}
		}

		public List<SectorSpeedAndTime> RacerSectorSpeedAndTime
		{
			get
			{
				return p;
			}
			set
			{
				p = value;
			}
		}

		public float RacerSumOfSpeeds
		{
			get
			{
				if (p != null)
				{
					float num = 0f;
					{
						foreach (SectorSpeedAndTime item in p)
						{
							num += item.SectorSpeed;
						}
						return num;
					}
				}
				return 0f;
			}
		}

		public float RacerHighestSpeed
		{
			get
			{
				if (p != null)
				{
					float num = 0f;
					{
						foreach (SectorSpeedAndTime item in p)
						{
							if (item.SectorSpeed > num)
							{
								num = item.SectorSpeed;
							}
						}
						return num;
					}
				}
				return 0f;
			}
		}

		public float RacerHighestSpeedInRaceAsKm
		{
			get
			{
				return t;
			}
			set
			{
				t = value;
			}
		}

		public float RacerHighestSpeedInRaceAsMile => t * 0.6214f;

		public GameObject RacerObject
		{
			get
			{
				return o;
			}
			set
			{
				o = value;
			}
		}

		public object RacerMiscObject
		{
			get
			{
				return mo;
			}
			set
			{
				mo = value;
			}
		}

		public Racer_Detail()
		{
			n = new List<float>();
		}

		public int CompareTo(Racer_Detail other)
		{
			return RacerDistance.CompareTo(other.RacerDistance);
		}

		public int CompareToHighSpeed(Racer_Detail other)
		{
			return RacerHighestSpeed.CompareTo(other.RacerHighestSpeed);
		}

		public int CompareToSpeedSum(Racer_Detail other)
		{
			return RacerSumOfSpeeds.CompareTo(other.RacerSumOfSpeeds);
		}

		public int ComareToHighSpeedInRace(Racer_Detail other)
		{
			return RacerHighestSpeedInRaceAsKm.CompareTo(other.RacerHighestSpeedInRaceAsKm);
		}
	}
}
