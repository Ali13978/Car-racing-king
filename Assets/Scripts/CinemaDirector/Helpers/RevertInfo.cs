using CinemaSuite.Common;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CinemaDirector.Helpers
{
	public class RevertInfo
	{
		private MonoBehaviour MonoBehaviour;

		private Type Type;

		private object Instance;

		private MemberInfo[] MemberInfo;

		private object value;

		public RevertMode RuntimeRevert => (MonoBehaviour as IRevertable).RuntimeRevertMode;

		public RevertMode EditorRevert => (MonoBehaviour as IRevertable).EditorRevertMode;

		public RevertInfo(MonoBehaviour monoBehaviour, Type type, string memberName, object value)
		{
			MonoBehaviour = monoBehaviour;
			Type = type;
			this.value = value;
			MemberInfo = ReflectionHelper.GetMemberInfo(type, memberName);
		}

		public RevertInfo(MonoBehaviour monoBehaviour, object obj, string memberName, object value)
		{
			MonoBehaviour = monoBehaviour;
			Instance = obj;
			Type = obj.GetType();
			this.value = value;
			MemberInfo = ReflectionHelper.GetMemberInfo(Type, memberName);
		}

		public void Revert()
		{
			if (MemberInfo == null || MemberInfo.Length <= 0)
			{
				return;
			}
			if (MemberInfo[0] is FieldInfo)
			{
				FieldInfo fieldInfo = MemberInfo[0] as FieldInfo;
				if (fieldInfo.IsStatic || (!fieldInfo.IsStatic && Instance != null))
				{
					fieldInfo.SetValue(Instance, value);
				}
			}
			else if (MemberInfo[0] is PropertyInfo)
			{
				PropertyInfo propertyInfo = MemberInfo[0] as PropertyInfo;
				propertyInfo.SetValue(Instance, value, null);
			}
			else
			{
				if (!(MemberInfo[0] is MethodInfo))
				{
					return;
				}
				Type[] array = new Type[0];
				if (value.GetType().IsArray)
				{
					object[] array2 = (object[])value;
					array = new Type[array2.Length];
					for (int i = 0; i < array2.Length; i++)
					{
						array[i] = array2[i].GetType();
					}
				}
				else if (value != null)
				{
					array = new Type[1]
					{
						value.GetType()
					};
				}
				int num = -1;
				for (int j = 0; j < MemberInfo.Length; j++)
				{
					ParameterInfo[] parameters = (MemberInfo[j] as MethodInfo).GetParameters();
					Type[] array3 = new Type[parameters.Length];
					for (int k = 0; k < parameters.Length; k++)
					{
						array3[k] = parameters[k].ParameterType;
					}
					if (array.SequenceEqual(array3))
					{
						num = j;
						break;
					}
				}
				if (num != -1)
				{
					MethodInfo methodInfo = MemberInfo[num] as MethodInfo;
					if (methodInfo.IsStatic || (!methodInfo.IsStatic && Instance != null))
					{
						if (value == null)
						{
							methodInfo.Invoke(Instance, null);
						}
						else if (value.GetType().IsArray)
						{
							methodInfo.Invoke(Instance, (object[])value);
						}
						else
						{
							methodInfo.Invoke(Instance, new object[1]
							{
								value
							});
						}
					}
				}
				else
				{
					UnityEngine.Debug.LogError("Error while reverting: Could not find method \"" + MemberInfo[0].Name + "\" that accepts parameters {" + string.Join(", ", (from v in array
						select v.ToString()).ToArray()) + "}.");
				}
			}
		}
	}
}
