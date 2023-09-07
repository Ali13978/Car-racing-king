using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RacingGameKit.Helpers
{
	internal static class JsonUtils
	{
		public static string ReadJsonFile(string FileName)
		{
			string text = string.Empty;
			StringReader stringReader = null;
			TextAsset textAsset = (TextAsset)Resources.Load(FileName, typeof(TextAsset));
			stringReader = new StringReader(textAsset.text);
			if (stringReader == null)
			{
				UnityEngine.Debug.LogWarning(FileName + ".txt not found in DATA folder or its not readable!");
			}
			else
			{
				string text2;
				while ((text2 = stringReader.ReadLine()) != null)
				{
					if (text2.Length > 2)
					{
						if (!text2.Substring(0, 2).Equals("//"))
						{
							text += text2;
						}
					}
					else
					{
						text += text2;
					}
				}
			}
			return text;
		}

		public static string[] GetRacerNames()
		{
			string[] result = new string[8]
			{
				"AI Player 1",
				"AI Player 2",
				"AI Player 3",
				"AI Player 4",
				"AI Player 5",
				"AI Player 6",
				"AI Player 7",
				"AI Player 8"
			};
			try
			{
				List<string> list = new List<string>();
				string text = ReadJsonFile("ai_names").Replace("\r\n", string.Empty);
				if (text != string.Empty)
				{
					JSONObject jSONObject = new JSONObject(text);
					if (jSONObject.HasField("ainames"))
					{
						for (int i = 0; i < jSONObject.GetField("ainames").list.Count; i++)
						{
							JSONObject jSONObject2 = new JSONObject(jSONObject.GetField("ainames").list[i].ToString());
							list.Add(jSONObject2.GetField("ainame").ToString().Replace("\"", string.Empty));
						}
					}
					return list.ToArray();
				}
				return result;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("ai_names.txt document have invalid JSON data. Using stock AI Names! Please consult documentation.\r\n" + ex.Message + "\r\n" + ex.InnerException + "\r\n" + ex.StackTrace);
				return result;
			}
		}
	}
}
