using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	public class WaterHoseParticles : MonoBehaviour
	{
		public static float lastSoundTime;

		public float force = 1f;

		private List<ParticleCollisionEvent> m_CollisionEvents = new List<ParticleCollisionEvent>(16);

		private ParticleSystem m_ParticleSystem;

		private void Start()
		{
			m_ParticleSystem = GetComponent<ParticleSystem>();
		}

		private void OnParticleCollision(GameObject other)
		{
			int safeCollisionEventSize = m_ParticleSystem.GetSafeCollisionEventSize();
			if (m_CollisionEvents.Count < safeCollisionEventSize)
			{
				m_CollisionEvents = new List<ParticleCollisionEvent>(safeCollisionEventSize);
			}
			int collisionEvents = m_ParticleSystem.GetCollisionEvents(other, m_CollisionEvents);
			for (int i = 0; i < collisionEvents; i++)
			{
				if (Time.time > lastSoundTime + 0.2f)
				{
					lastSoundTime = Time.time;
				}
				Collider collider = m_CollisionEvents[i].colliderComponent as Collider;
				if (collider.attachedRigidbody != null)
				{
					Vector3 velocity = m_CollisionEvents[i].velocity;
					collider.attachedRigidbody.AddForce(velocity * force, ForceMode.Impulse);
				}
				other.BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
