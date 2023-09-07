using CinemaDirector.Helpers;
using CinemaSuite.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CinemaDirector
{
	[Serializable]
	[CutsceneItem("Curve Clip", "Actor Curve Clip", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.CurveClipItem
	})]
	public class CinemaActorClipCurve : CinemaClipCurve, IRevertable
	{
		[SerializeField]
		private RevertMode editorRevertMode;

		[SerializeField]
		private RevertMode runtimeRevertMode;

		public GameObject Actor
		{
			get
			{
				GameObject result = null;
				if (base.transform.parent != null)
				{
					CurveTrack component = base.transform.parent.GetComponent<CurveTrack>();
					if (component != null && component.Actor != null)
					{
						result = component.Actor.gameObject;
					}
				}
				return result;
			}
		}

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

		protected override bool initializeClipCurves(MemberClipCurveData data, Component component)
		{
			object currentValue = GetCurrentValue(component, data.PropertyName, data.IsProperty);
			PropertyTypeInfo propertyType = data.PropertyType;
			float firetime = base.Firetime;
			float timeEnd = base.Firetime + base.Duration;
			switch (propertyType)
			{
			case PropertyTypeInfo.Double:
			case PropertyTypeInfo.Float:
			case PropertyTypeInfo.Int:
			case PropertyTypeInfo.Long:
				float.TryParse(currentValue.ToString(), out float result);
				if (float.IsInfinity(result) || float.IsNaN(result))
				{
					return false;
				}
				data.Curve1 = AnimationCurve.Linear(firetime, result, timeEnd, result);
				break;
			case PropertyTypeInfo.Vector2:
			{
				Vector2 vector3 = (Vector2)currentValue;
				if (float.IsInfinity(vector3.x) || float.IsNaN(vector3.x) || float.IsInfinity(vector3.y) || float.IsNaN(vector3.y))
				{
					return false;
				}
				data.Curve1 = AnimationCurve.Linear(firetime, vector3.x, timeEnd, vector3.x);
				data.Curve2 = AnimationCurve.Linear(firetime, vector3.y, timeEnd, vector3.y);
				break;
			}
			case PropertyTypeInfo.Vector3:
			{
				Vector3 vector2 = (Vector3)currentValue;
				if (float.IsInfinity(vector2.x) || float.IsNaN(vector2.x) || float.IsInfinity(vector2.y) || float.IsNaN(vector2.y) || float.IsInfinity(vector2.z) || float.IsNaN(vector2.z))
				{
					return false;
				}
				data.Curve1 = AnimationCurve.Linear(firetime, vector2.x, timeEnd, vector2.x);
				data.Curve2 = AnimationCurve.Linear(firetime, vector2.y, timeEnd, vector2.y);
				data.Curve3 = AnimationCurve.Linear(firetime, vector2.z, timeEnd, vector2.z);
				break;
			}
			case PropertyTypeInfo.Vector4:
			{
				Vector4 vector = (Vector4)currentValue;
				if (float.IsInfinity(vector.x) || float.IsNaN(vector.x) || float.IsInfinity(vector.y) || float.IsNaN(vector.y) || float.IsInfinity(vector.z) || float.IsNaN(vector.z) || float.IsInfinity(vector.w) || float.IsNaN(vector.w))
				{
					return false;
				}
				data.Curve1 = AnimationCurve.Linear(firetime, vector.x, timeEnd, vector.x);
				data.Curve2 = AnimationCurve.Linear(firetime, vector.y, timeEnd, vector.y);
				data.Curve3 = AnimationCurve.Linear(firetime, vector.z, timeEnd, vector.z);
				data.Curve4 = AnimationCurve.Linear(firetime, vector.w, timeEnd, vector.w);
				break;
			}
			case PropertyTypeInfo.Quaternion:
			{
				Quaternion quaternion = (Quaternion)currentValue;
				if (float.IsInfinity(quaternion.x) || float.IsNaN(quaternion.x) || float.IsInfinity(quaternion.y) || float.IsNaN(quaternion.y) || float.IsInfinity(quaternion.z) || float.IsNaN(quaternion.z) || float.IsInfinity(quaternion.w) || float.IsNaN(quaternion.w))
				{
					return false;
				}
				data.Curve1 = AnimationCurve.Linear(firetime, quaternion.x, timeEnd, quaternion.x);
				data.Curve2 = AnimationCurve.Linear(firetime, quaternion.y, timeEnd, quaternion.y);
				data.Curve3 = AnimationCurve.Linear(firetime, quaternion.z, timeEnd, quaternion.z);
				data.Curve4 = AnimationCurve.Linear(firetime, quaternion.w, timeEnd, quaternion.w);
				break;
			}
			case PropertyTypeInfo.Color:
			{
				Color color = (Color)currentValue;
				if (float.IsInfinity(color.r) || float.IsNaN(color.r) || float.IsInfinity(color.g) || float.IsNaN(color.g) || float.IsInfinity(color.b) || float.IsNaN(color.b) || float.IsInfinity(color.a) || float.IsNaN(color.a))
				{
					return false;
				}
				data.Curve1 = AnimationCurve.Linear(firetime, color.r, timeEnd, color.r);
				data.Curve2 = AnimationCurve.Linear(firetime, color.g, timeEnd, color.g);
				data.Curve3 = AnimationCurve.Linear(firetime, color.b, timeEnd, color.b);
				data.Curve4 = AnimationCurve.Linear(firetime, color.a, timeEnd, color.a);
				break;
			}
			}
			return true;
		}

		public object GetCurrentValue(Component component, string propertyName, bool isProperty)
		{
			if (component == null || propertyName == string.Empty)
			{
				return null;
			}
			Type type = component.GetType();
			object obj = null;
			if (isProperty)
			{
				PropertyInfo property = ReflectionHelper.GetProperty(type, propertyName);
				return property.GetValue(component, null);
			}
			FieldInfo field = ReflectionHelper.GetField(type, propertyName);
			return field.GetValue(component);
		}

		public override void Initialize()
		{
			for (int i = 0; i < base.CurveData.Count; i++)
			{
				base.CurveData[i].Initialize(Actor);
			}
		}

		public RevertInfo[] CacheState()
		{
			List<RevertInfo> list = new List<RevertInfo>();
			if (Actor != null)
			{
				for (int i = 0; i < base.CurveData.Count; i++)
				{
					Component component = Actor.GetComponent(base.CurveData[i].Type);
					if (component != null)
					{
						RevertInfo item = new RevertInfo(this, component, base.CurveData[i].PropertyName, base.CurveData[i].getCurrentValue(component));
						list.Add(item);
					}
				}
			}
			return list.ToArray();
		}

		public void SampleTime(float time)
		{
			if (Actor == null || !(base.Firetime <= time) || !(time <= base.Firetime + base.Duration))
			{
				return;
			}
			for (int i = 0; i < base.CurveData.Count; i++)
			{
				if (!(base.CurveData[i].Type == string.Empty) && !(base.CurveData[i].PropertyName == string.Empty))
				{
					Component component = Actor.GetComponent(base.CurveData[i].Type);
					if (component == null)
					{
						break;
					}
					Type type = component.GetType();
					object obj = evaluate(base.CurveData[i], time);
					if (base.CurveData[i].IsProperty)
					{
						PropertyInfo property = ReflectionHelper.GetProperty(type, base.CurveData[i].PropertyName);
						property.SetValue(component, obj, null);
					}
					else
					{
						FieldInfo field = ReflectionHelper.GetField(type, base.CurveData[i].PropertyName);
						try
						{
							field.SetValue(component, obj);
						}
						catch (ArgumentException)
						{
							field.SetValue(component, Mathf.RoundToInt((float)obj));
						}
					}
				}
			}
		}

		internal void Reset()
		{
		}
	}
}
