namespace RacingGameKit.Helpers
{
	public static class RGKMessages
	{
		public const string RaceManagerMissing = "RGK WARNING\r\nRaceManager object is missing!";

		public const string DistanceCheckerMissing = "RGK WARNING\r\nDistance checker object not found! Be sure _DistancePoint named object placed under child!";

		public const string RacerTagObjectsMissing = "RGK WARNING\r\nRacerTag objects is missing! Racer name and standing will not shown above vehicles ";

		public const string SpawnPointsObjectMissing = "RGK WARNING\r\nSpawnpints object is missing! ";

		public const string GameCameraMissing = "RGK WARNING\r\nGameCamera is missing! RGK requires a camera script that implement iRGKCamera";

		public const string FinishPointMissing = "RGK WARNING\r\nFinishPoint is missing";

		public const string CheckpointSystemDisabled = "RGK NOTIFICATION\r\nCheckpoint container not found or empty. Checkpoint System disabled!";

		public const string GameCameraNotAttached = "RGK WARNING\r\nGameCamera not attached to Game Audio script. Please be sure _GameCamera object created or attached. Game Audio Disabled.";
	}
}
