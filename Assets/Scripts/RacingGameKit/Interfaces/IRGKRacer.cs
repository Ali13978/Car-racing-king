using UnityEngine;

namespace RacingGameKit.Interfaces
{
	public interface IRGKRacer
	{
		float Speed
		{
			get;
			set;
		}

		Vector3 Position
		{
			get;
		}

		Transform CachedTransform
		{
			get;
		}

		GameObject CachedGameObject
		{
			get;
		}

		eAIRoadPosition CurrentRoadPosition
		{
			get;
		}

		Vector3 Velocity
		{
			get;
		}

		float EscapeDistance
		{
			get;
		}
	}
}
