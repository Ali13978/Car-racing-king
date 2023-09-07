using System;
using UnityEngine;

namespace RacingGameKit.UI
{
	[Serializable]
	public class RGKUI_RaceData
	{
		public string RaceTitle;

		public string TrackName;

		public int TrackIndex;

		public Sprite TrackSprite;

		[TextArea(2, 5)]
		public string RaceInfo;

		public string RaceType;

		public int RaceLaps;

		public int Opponents;

		public RaceTypeEnum TrackRaceTypeEnum;

		public eSpeedTrapMode SpeedTrapEnum;

		public string TrackAiIndexes;
	}
}
