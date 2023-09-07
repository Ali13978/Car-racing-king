using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Shots", "Shot", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.CameraShot
	})]
	public class CinemaShot : CinemaGlobalAction, IRevertable
	{
		public Camera shotCamera;

		[SerializeField]
		private RevertMode editorRevertMode;

		[SerializeField]
		private RevertMode runtimeRevertMode;

		public RevertMode EditorRevertMode
		{
			get
			{
				return editorRevertMode;
			}
			set
			{
				editorRevertMode = value;
			}
		}

		public RevertMode RuntimeRevertMode
		{
			get
			{
				return runtimeRevertMode;
			}
			set
			{
				runtimeRevertMode = value;
			}
		}

		public override void Initialize()
		{
			if (!(shotCamera != null))
			{
			}
		}

		public override void Trigger()
		{
			if (shotCamera != null)
			{
				shotCamera.gameObject.SetActive(value: true);
			}
		}

		public override void End()
		{
			if (shotCamera != null)
			{
				shotCamera.gameObject.SetActive(value: false);
			}
		}

		public RevertInfo[] CacheState()
		{
			List<RevertInfo> list = new List<RevertInfo>();
			if (shotCamera != null)
			{
				GameObject gameObject = shotCamera.gameObject;
				list.Add(new RevertInfo(this, gameObject.gameObject, "SetActive", shotCamera.gameObject.activeSelf));
			}
			return list.ToArray();
		}
	}
}
