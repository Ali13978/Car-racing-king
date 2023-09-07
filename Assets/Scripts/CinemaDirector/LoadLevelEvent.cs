using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Utility", "Load Level", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.GlobalItem
	})]
	public class LoadLevelEvent : CinemaGlobalEvent
	{
		public enum LoadLevelArgument
		{
			ByIndex,
			ByName
		}

		public enum LoadLevelType
		{
			Standard,
			Additive,
			Async,
			AdditiveAsync
		}

		public LoadLevelArgument Argument;

		public LoadLevelType Type;

		public int Level;

		public string LevelName;

		public override void Trigger()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (Argument == LoadLevelArgument.ByIndex)
			{
				if (Type == LoadLevelType.Standard)
				{
					UnityEngine.SceneManagement.SceneManager.LoadScene(Level);
				}
				else if (Type == LoadLevelType.Additive)
				{
					Application.LoadLevelAdditive(Level);
				}
				else if (Type == LoadLevelType.Async)
				{
					Application.LoadLevelAsync(Level);
				}
				else if (Type == LoadLevelType.AdditiveAsync)
				{
					Application.LoadLevelAdditiveAsync(Level);
				}
			}
			else if (Argument == LoadLevelArgument.ByName)
			{
				if (Type == LoadLevelType.Standard)
				{
					UnityEngine.SceneManagement.SceneManager.LoadScene(LevelName);
				}
				else if (Type == LoadLevelType.Additive)
				{
					Application.LoadLevelAdditive(LevelName);
				}
				else if (Type == LoadLevelType.Async)
				{
					Application.LoadLevelAsync(LevelName);
				}
				else if (Type == LoadLevelType.AdditiveAsync)
				{
					Application.LoadLevelAdditiveAsync(LevelName);
				}
			}
		}
	}
}
