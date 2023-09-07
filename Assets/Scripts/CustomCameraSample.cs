using RacingGameKit.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class CustomCameraSample : MonoBehaviour, IRGKCamera
{
	public Transform target;

	public float distance = 5f;

	public float height = 2f;

	public float heightDamping = 3f;

	public float rotationDamping = 3f;

	public string ControlBindingCameraBack
	{
		set
		{
		}
	}

	public string ControlBindingCameraChange
	{
		set
		{
		}
	}

	public string ControlBindingCameraLeft
	{
		set
		{
		}
	}

	public string ControlBindingCameraRight
	{
		set
		{
		}
	}

	public int CurrentCount
	{
		set
		{
		}
	}

	public bool IsStartupAnimationEnabled
	{
		set
		{
		}
	}

	public List<Transform> TargetObjects
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Transform TargetVehicle
	{
		set
		{
			target = value;
		}
	}

	private void Update()
	{
		if ((bool)target)
		{
			Vector3 eulerAngles = target.eulerAngles;
			float y = eulerAngles.y;
			Vector3 position = target.position;
			float b = position.y + height;
			Vector3 eulerAngles2 = base.transform.eulerAngles;
			float y2 = eulerAngles2.y;
			Vector3 position2 = base.transform.position;
			float y3 = position2.y;
			y2 = Mathf.LerpAngle(y2, y, rotationDamping * Time.deltaTime);
			y3 = Mathf.Lerp(y3, b, heightDamping * Time.deltaTime);
			Quaternion rotation = Quaternion.Euler(0f, y2, 0f);
			Vector3 vector = target.position;
			vector -= rotation * Vector3.forward * distance;
			vector.y = y3;
			base.transform.position = vector;
			base.transform.LookAt(target);
		}
	}
}
