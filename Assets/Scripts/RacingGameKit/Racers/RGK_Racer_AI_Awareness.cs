//using Photon.Pun;
using RacingGameKit.Interfaces;
using UnityEngine;

namespace RacingGameKit.Racers
{
	[AddComponentMenu("Racing Game Kit/Racers/RGK Racer - AI Awareness for Human Racer")]
	public class RGK_Racer_AI_Awareness : MonoBehaviour, IRGKRacer
	{
		private float CarSpeedKm;

		private Transform myTransform;

		private float VehicleSpeed;

		public bool DrawCollisonGizmo = true;

		public float DedectionRadius = 2f;

		public float Speed
		{
			get
			{
				return CarSpeedKm;
			}
			set
			{
			}
		}

		public Transform CachedTransform => base.transform;

		public GameObject CachedGameObject => base.gameObject;

		public eAIRoadPosition CurrentRoadPosition => eAIRoadPosition.UnKnown;

		public Vector3 Velocity => myTransform.forward * VehicleSpeed;

		public Vector3 Position => myTransform.position;

		public float EscapeDistance => 0f;

		private void Awake()
        {
            if (myTransform == null)
			{
				myTransform = base.transform.GetComponent<Transform>();
				if (myTransform == null)
				{
					myTransform = base.transform.parent.GetComponent<Transform>();
				}
			}
		}

		private void Update()
        {
            CarSpeedKm = Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude * 3.6f);
		}

		private void OnDrawGizmos()
		{
			if (DrawCollisonGizmo)
			{
				if (myTransform == null)
				{
					myTransform = base.transform;
				}
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(Position, DedectionRadius);
			}
		}
	}
}
