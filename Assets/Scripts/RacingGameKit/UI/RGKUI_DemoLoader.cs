using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RacingGameKit.UI
{
	[AddComponentMenu("")]
	public class RGKUI_DemoLoader : MonoBehaviour
	{
		public Slider m_LoadingSlider;

		public int m_TargetLevelIndex = 1;

		private AsyncOperation m_AsyncLoadingProcess;

		private void Start()
		{
			StartCoroutine(LoadLevel(m_TargetLevelIndex));
		}

		private void Update()
		{
			if (m_LoadingSlider != null)
			{
				m_LoadingSlider.value = m_AsyncLoadingProcess.progress * 100f;
			}
		}

		private IEnumerator LoadLevel(int TrackIndex)
		{
			m_AsyncLoadingProcess = SceneManager.LoadSceneAsync(TrackIndex);
			yield return m_AsyncLoadingProcess;
		}
	}
}
