using System;
using System.Reflection;

namespace CinemaSuite.Common
{
	public static class ReflectionHelper
	{
		public static Assembly[] GetAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		public static Type[] GetTypes(Assembly assembly)
		{
			return assembly.GetTypes();
		}

		public static bool IsSubclassOf(Type type, Type c)
		{
			return type.IsSubclassOf(c);
		}

		public static MemberInfo[] GetMemberInfo(Type type, string name)
		{
			return type.GetMember(name);
		}

		public static FieldInfo GetField(Type type, string name)
		{
			return type.GetField(name);
		}

		public static PropertyInfo GetProperty(Type type, string name)
		{
			return type.GetProperty(name);
		}

		public static T[] GetCustomAttributes<T>(Type type, bool inherited) where T : Attribute
		{
			return (T[])type.GetCustomAttributes(typeof(T), inherited);
		}
	}
}
