using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Helpers/Racer Tags")]
	public class Racer_Tags : MonoBehaviour
	{
		private GameObject _GameCamera;

		private void Start()
		{
			_GameCamera = GameObject.Find("_RaceCamera");
			if (!(_GameCamera == null))
			{
			}
		}

		private void LateUpdate()
		{
			if (_GameCamera != null)
			{
				base.transform.LookAt(base.transform.position + _GameCamera.transform.rotation * Vector3.forward);
			}
		}
	}
}
