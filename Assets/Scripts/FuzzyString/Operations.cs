using System.Collections.Generic;
using System.Linq;

namespace FuzzyString
{
	public static class Operations
	{
		public static string Capitalize(this string source)
		{
			return source.ToUpper();
		}

		public static string[] SplitIntoIndividualElements(string source)
		{
			string[] array = new string[source.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = source[i].ToString();
			}
			return array;
		}

		public static string MergeIndividualElementsIntoString(IEnumerable<string> source)
		{
			string text = string.Empty;
			for (int i = 0; i < source.Count(); i++)
			{
				text += source.ElementAt(i);
			}
			return text;
		}

		public static List<string> ListPrefixes(this string source)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < source.Length; i++)
			{
				list.Add(source.Substring(0, i));
			}
			return list;
		}

		public static List<string> ListBiGrams(this string source)
		{
			return source.ListNGrams(2);
		}

		public static List<string> ListTriGrams(this string source)
		{
			return source.ListNGrams(3);
		}

		public static List<string> ListNGrams(this string source, int n)
		{
			List<string> list = new List<string>();
			if (n > source.Length)
			{
				return null;
			}
			if (n == source.Length)
			{
				list.Add(source);
				return list;
			}
			for (int i = 0; i < source.Length - n; i++)
			{
				list.Add(source.Substring(i, n));
			}
			return list;
		}
	}
}
