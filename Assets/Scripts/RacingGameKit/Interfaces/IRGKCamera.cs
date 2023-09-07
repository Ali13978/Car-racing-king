using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.Interfaces
{
	public interface IRGKCamera
	{
		Transform TargetVehicle
		{
			set;
		}

		List<Transform> TargetObjects
		{
			get;
			set;
		}

		bool IsStartupAnimationEnabled
		{
			set;
		}

		int CurrentCount
		{
			set;
		}

		string ControlBindingCameraChange
		{
			set;
		}

		string ControlBindingCameraBack
		{
			set;
		}

		string ControlBindingCameraLeft
		{
			set;
		}

		string ControlBindingCameraRight
		{
			set;
		}
	}
}
