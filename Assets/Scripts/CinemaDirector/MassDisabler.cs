using CinemaDirector.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Utility", "Mass Disabler", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.GlobalItem
	})]
	public class MassDisabler : CinemaGlobalAction, IRevertable
	{
		public List<GameObject> GameObjects = new List<GameObject>();

		public List<string> Tags = new List<string>();

		private List<GameObject> tagsCache = new List<GameObject>();

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

		public RevertInfo[] CacheState()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < Tags.Count; i++)
			{
				string tag = Tags[i];
				GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
				foreach (GameObject gameObject in array)
				{
					if (gameObject != null)
					{
						list.Add(gameObject);
					}
				}
			}
			list.AddRange(GameObjects);
			List<RevertInfo> list2 = new List<RevertInfo>();
			for (int k = 0; k < list.Count; k++)
			{
				GameObject gameObject2 = list[k];
				if (gameObject2 != null)
				{
					list2.Add(new RevertInfo(this, gameObject2, "SetActive", gameObject2.activeInHierarchy));
				}
			}
			return list2.ToArray();
		}

		public override void Trigger()
		{
			tagsCache.Clear();
			for (int i = 0; i < Tags.Count; i++)
			{
				string tag = Tags[i];
				GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
				for (int j = 0; j < array.Length; j++)
				{
					tagsCache.Add(array[j]);
				}
			}
			setActive(enabled: false);
		}

		public override void End()
		{
			setActive(enabled: true);
		}

		public override void ReverseTrigger()
		{
			End();
		}

		public override void ReverseEnd()
		{
			Trigger();
		}

		private void setActive(bool enabled)
		{
			for (int i = 0; i < GameObjects.Count; i++)
			{
				GameObjects[i].SetActive(enabled);
			}
			for (int j = 0; j < tagsCache.Count; j++)
			{
				tagsCache[j].SetActive(enabled);
			}
		}
	}
}
