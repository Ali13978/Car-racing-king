using CinemaDirector.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[Serializable]
	[ExecuteInEditMode]
	public class Cutscene : MonoBehaviour, IOptimizable
	{
		public enum CutsceneState
		{
			Inactive,
			Playing,
			PreviewPlaying,
			Scrubbing,
			Paused
		}

		[SerializeField]
		private float duration = 30f;

		[SerializeField]
		private float playbackSpeed = 1f;

		[SerializeField]
		private bool isSkippable = true;

		[SerializeField]
		private bool isLooping;

		[SerializeField]
		private bool canOptimize = true;

		[NonSerialized]
		private float runningTime;

		[NonSerialized]
		private CutsceneState state;

		private bool hasBeenOptimized;

		private bool hasBeenInitialized;

		private TrackGroup[] trackGroupCache;

		private List<RevertInfo> revertCache = new List<RevertInfo>();

		public float Duration
		{
			get
			{
				return duration;
			}
			set
			{
				duration = value;
				if (duration <= 0f)
				{
					duration = 0.1f;
				}
			}
		}

		public CutsceneState State => state;

		public float RunningTime
		{
			get
			{
				return runningTime;
			}
			set
			{
				runningTime = Mathf.Clamp(value, 0f, duration);
			}
		}

		public TrackGroup[] TrackGroups => GetComponentsInChildren<TrackGroup>();

		public DirectorGroup DirectorGroup => GetComponentInChildren<DirectorGroup>();

		public bool CanOptimize
		{
			get
			{
				return canOptimize;
			}
			set
			{
				canOptimize = value;
			}
		}

		public bool IsSkippable
		{
			get
			{
				return isSkippable;
			}
			set
			{
				isSkippable = value;
			}
		}

		public bool IsLooping
		{
			get
			{
				return isLooping;
			}
			set
			{
				isLooping = value;
			}
		}

		public event CutsceneHandler CutsceneStarted;

		public event CutsceneHandler CutsceneFinished;

		public event CutsceneHandler CutscenePaused;

		public void Optimize()
		{
			if (canOptimize)
			{
				trackGroupCache = GetTrackGroups();
				hasBeenOptimized = true;
			}
			TrackGroup[] trackGroups = GetTrackGroups();
			for (int i = 0; i < trackGroups.Length; i++)
			{
				trackGroups[i].Optimize();
			}
		}

		public void Play()
		{
			if (state == CutsceneState.Inactive)
			{
				StartCoroutine(freshPlay());
			}
			else if (state == CutsceneState.Paused)
			{
				state = CutsceneState.Playing;
				StartCoroutine(updateCoroutine());
			}
			if (this.CutsceneStarted != null)
			{
				this.CutsceneStarted(this, new CutsceneEventArgs());
			}
		}

		private IEnumerator freshPlay()
		{
			yield return StartCoroutine(PreparePlay());
			yield return null;
			state = CutsceneState.Playing;
			StartCoroutine(updateCoroutine());
		}

		public void Pause()
		{
			if (state == CutsceneState.Playing)
			{
				StopCoroutine("updateCoroutine");
			}
			if (state == CutsceneState.PreviewPlaying || state == CutsceneState.Playing || state == CutsceneState.Scrubbing)
			{
				TrackGroup[] trackGroups = GetTrackGroups();
				for (int i = 0; i < trackGroups.Length; i++)
				{
					trackGroups[i].Pause();
				}
			}
			state = CutsceneState.Paused;
			if (this.CutscenePaused != null)
			{
				this.CutscenePaused(this, new CutsceneEventArgs());
			}
		}

		public void Skip()
		{
			if (isSkippable)
			{
				SetRunningTime(Duration);
				state = CutsceneState.Inactive;
				Stop();
			}
		}

		public void Stop()
		{
			runningTime = 0f;
			TrackGroup[] trackGroups = GetTrackGroups();
			for (int i = 0; i < trackGroups.Length; i++)
			{
				trackGroups[i].Stop();
			}
			if (state != 0)
			{
				revert();
			}
			if (state == CutsceneState.Playing)
			{
				StopCoroutine("updateCoroutine");
				if (state == CutsceneState.Playing && isLooping)
				{
					state = CutsceneState.Inactive;
					Play();
				}
				else
				{
					state = CutsceneState.Inactive;
				}
			}
			else
			{
				state = CutsceneState.Inactive;
			}
			if (state == CutsceneState.Inactive && this.CutsceneFinished != null)
			{
				this.CutsceneFinished(this, new CutsceneEventArgs());
			}
		}

		public void UpdateCutscene(float deltaTime)
		{
			RunningTime += deltaTime * playbackSpeed;
			TrackGroup[] trackGroups = GetTrackGroups();
			for (int i = 0; i < trackGroups.Length; i++)
			{
				trackGroups[i].UpdateTrackGroup(RunningTime, deltaTime * playbackSpeed);
			}
			if (state != CutsceneState.Scrubbing && (runningTime >= duration || runningTime < 0f))
			{
				Stop();
			}
		}

		public void PreviewPlay()
		{
			if (state == CutsceneState.Inactive)
			{
				EnterPreviewMode();
			}
			else if (state == CutsceneState.Paused)
			{
				resume();
			}
			if (Application.isPlaying)
			{
				state = CutsceneState.Playing;
			}
			else
			{
				state = CutsceneState.PreviewPlaying;
			}
		}

		public void ScrubToTime(float newTime)
		{
			float num = Mathf.Clamp(newTime, 0f, Duration) - RunningTime;
			state = CutsceneState.Scrubbing;
			if (num != 0f)
			{
				if (num > 71f / (678f * (float)Math.PI))
				{
					float num2 = RunningTime;
					List<float> milestones = getMilestones(RunningTime + num);
					for (int i = 0; i < milestones.Count; i++)
					{
						float deltaTime = milestones[i] - num2;
						UpdateCutscene(deltaTime);
						num2 = milestones[i];
					}
				}
				else
				{
					UpdateCutscene(num);
				}
			}
			else
			{
				Pause();
			}
		}

		public void SetRunningTime(float time)
		{
			List<float> milestones = getMilestones(time);
			for (int i = 0; i < milestones.Count; i++)
			{
				for (int j = 0; j < TrackGroups.Length; j++)
				{
					TrackGroups[j].SetRunningTime(milestones[i]);
				}
			}
			RunningTime = time;
		}

		public void EnterPreviewMode()
		{
			if (state == CutsceneState.Inactive)
			{
				initialize();
				bake();
				SetRunningTime(RunningTime);
				state = CutsceneState.Paused;
			}
		}

		public void ExitPreviewMode()
		{
			Stop();
		}

		protected void OnDestroy()
		{
			if (!Application.isPlaying)
			{
				Stop();
			}
		}

		public void Refresh()
		{
			if (state != 0)
			{
				float num = runningTime;
				Stop();
				EnterPreviewMode();
				SetRunningTime(num);
			}
		}

		private void bake()
		{
			if (!Application.isEditor)
			{
				return;
			}
			for (int i = 0; i < TrackGroups.Length; i++)
			{
				if (TrackGroups[i] is IBakeable)
				{
					(TrackGroups[i] as IBakeable).Bake();
				}
			}
		}

		public TrackGroup[] GetTrackGroups()
		{
			if (hasBeenOptimized)
			{
				return trackGroupCache;
			}
			return TrackGroups;
		}

		private void initialize()
		{
			saveRevertData();
			for (int i = 0; i < TrackGroups.Length; i++)
			{
				TrackGroups[i].Initialize();
			}
			hasBeenInitialized = true;
		}

		private void saveRevertData()
		{
			revertCache.Clear();
			MonoBehaviour[] componentsInChildren = GetComponentsInChildren<MonoBehaviour>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				IRevertable revertable = componentsInChildren[i] as IRevertable;
				if (revertable != null)
				{
					RevertInfo[] array = revertable.CacheState();
					if (array == null || array.Length < 1)
					{
						UnityEngine.Debug.Log($"Cinema Director tried to cache the state of {componentsInChildren[i].name}, but failed.");
					}
					else
					{
						revertCache.AddRange(array);
					}
				}
			}
		}

		private void revert()
		{
			for (int i = 0; i < revertCache.Count; i++)
			{
				RevertInfo revertInfo = revertCache[i];
				if (revertInfo != null && ((revertInfo.EditorRevert == RevertMode.Revert && !Application.isPlaying) || (revertInfo.RuntimeRevert == RevertMode.Revert && Application.isPlaying)))
				{
					revertInfo.Revert();
				}
			}
		}

		private List<float> getMilestones(float time)
		{
			List<float> list = new List<float>();
			list.Add(time);
			for (int i = 0; i < TrackGroups.Length; i++)
			{
				List<float> milestones = TrackGroups[i].GetMilestones(RunningTime, time);
				for (int j = 0; j < milestones.Count; j++)
				{
					if (!list.Contains(milestones[j]))
					{
						list.Add(milestones[j]);
					}
				}
			}
			list.Sort();
			if (time < RunningTime)
			{
				list.Reverse();
			}
			return list;
		}

		private IEnumerator PreparePlay()
		{
			if (!hasBeenOptimized)
			{
				Optimize();
			}
			if (!hasBeenInitialized)
			{
				initialize();
			}
			yield return null;
		}

		private IEnumerator updateCoroutine()
		{
			while (state == CutsceneState.Playing)
			{
				UpdateCutscene(Time.deltaTime);
				yield return null;
			}
		}

		private void resume()
		{
			for (int i = 0; i < TrackGroups.Length; i++)
			{
				TrackGroups[i].Resume();
			}
		}

		public void recache()
		{
			if (state != 0)
			{
				float newTime = RunningTime;
				ExitPreviewMode();
				EnterPreviewMode();
				ScrubToTime(newTime);
			}
		}
	}
}
