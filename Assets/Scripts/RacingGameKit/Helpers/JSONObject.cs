using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.Helpers
{
	public class JSONObject
	{
		public enum Type
		{
			NULL,
			STRING,
			NUMBER,
			OBJECT,
			ARRAY,
			BOOL
		}

		private const int MAX_DEPTH = 1000;

		public JSONObject parent;

		public Type type;

		public ArrayList list = new ArrayList();

		public ArrayList keys = new ArrayList();

		public string str;

		public float n;

		public bool b;

		public static JSONObject nullJO => new JSONObject(Type.NULL);

		public static JSONObject obj => new JSONObject(Type.OBJECT);

		public static JSONObject arr => new JSONObject(Type.ARRAY);

		public JSONObject this[int index] => (JSONObject)list[index];

		public JSONObject this[string index] => GetField(index);

		public JSONObject(Type t)
		{
			type = t;
			switch (t)
			{
			case Type.ARRAY:
				list = new ArrayList();
				break;
			case Type.OBJECT:
				list = new ArrayList();
				keys = new ArrayList();
				break;
			}
		}

		public JSONObject(bool b)
		{
			type = Type.BOOL;
			this.b = b;
		}

		public JSONObject(float f)
		{
			type = Type.NUMBER;
			n = f;
		}

		public JSONObject(Dictionary<string, string> dic)
		{
			type = Type.OBJECT;
			foreach (KeyValuePair<string, string> item in dic)
			{
				keys.Add(item.Key);
				list.Add(item.Value);
			}
		}

		public JSONObject()
		{
		}

		public JSONObject(string str)
		{
			if (str != null)
			{
				str = str.Replace("\\n", string.Empty);
				str = str.Replace("\\t", string.Empty);
				str = str.Replace("\\r", string.Empty);
				str = str.Replace("\t", string.Empty);
				str = str.Replace("\n", string.Empty);
				str = str.Replace("\\", string.Empty);
				if (str.Length > 0)
				{
					if (str.ToLower() == "true")
					{
						type = Type.BOOL;
						b = true;
					}
					else if (str.ToLower() == "false")
					{
						type = Type.BOOL;
						b = false;
					}
					else if (str == "null")
					{
						type = Type.NULL;
					}
					else if (str[0] == '"')
					{
						type = Type.STRING;
						this.str = str.Substring(1, str.Length - 2);
					}
					else
					{
						try
						{
							n = float.Parse(str);
							type = Type.NUMBER;
						}
						catch (FormatException)
						{
							int num = 0;
							switch (str[0])
							{
							case '{':
								type = Type.OBJECT;
								keys = new ArrayList();
								list = new ArrayList();
								break;
							case '[':
								type = Type.ARRAY;
								list = new ArrayList();
								break;
							default:
								type = Type.NULL;
								UnityEngine.Debug.LogWarning("improper JSON formatting:" + str);
								return;
							}
							int num2 = 0;
							bool flag = false;
							bool flag2 = false;
							for (int i = 1; i < str.Length; i++)
							{
								if (str[i] == '\\')
								{
									i++;
								}
								else
								{
									if (str[i] == '"')
									{
										flag = !flag;
									}
									if (str[i] == '[' || str[i] == '{')
									{
										num2++;
									}
									if (num2 == 0 && !flag)
									{
										if (str[i] == ':' && !flag2)
										{
											flag2 = true;
											try
											{
												keys.Add(str.Substring(num + 2, i - num - 3));
											}
											catch
											{
												UnityEngine.Debug.Log(i + " - " + str.Length + " - " + str);
											}
											num = i;
										}
										if (str[i] == ',')
										{
											flag2 = false;
											list.Add(new JSONObject(str.Substring(num + 1, i - num - 1)));
											num = i;
										}
										if (str[i] == ']' || str[i] == '}')
										{
											list.Add(new JSONObject(str.Substring(num + 1, i - num - 1)));
										}
									}
									if (str[i] == ']' || str[i] == '}')
									{
										num2--;
									}
								}
							}
						}
					}
				}
			}
			else
			{
				type = Type.NULL;
			}
		}

		public void AddField(bool val)
		{
			Add(new JSONObject(val));
		}

		public void AddField(float val)
		{
			Add(new JSONObject(val));
		}

		public void AddField(int val)
		{
			Add(new JSONObject(val));
		}

		public void Add(JSONObject obj)
		{
			if (obj != null)
			{
				if (type != Type.ARRAY)
				{
					type = Type.ARRAY;
					UnityEngine.Debug.LogWarning("tried to add an object to a non-array JSONObject.  We'll do it for you, but you might be doing something wrong.");
				}
				list.Add(obj);
			}
		}

		public void AddField(string name, bool val)
		{
			AddField(name, new JSONObject(val));
		}

		public void AddField(string name, float val)
		{
			AddField(name, new JSONObject(val));
		}

		public void AddField(string name, int val)
		{
			AddField(name, new JSONObject(val));
		}

		public void AddField(string name, string val)
		{
			AddField(name, new JSONObject
			{
				type = Type.STRING,
				str = val
			});
		}

		public void AddField(string name, JSONObject obj)
		{
			if (obj != null)
			{
				if (type != Type.OBJECT)
				{
					type = Type.OBJECT;
					UnityEngine.Debug.LogWarning("tried to add a field to a non-object JSONObject.  We'll do it for you, but you might be doing something wrong.");
				}
				keys.Add(name);
				list.Add(obj);
			}
		}

		public void SetField(string name, JSONObject obj)
		{
			if (HasField(name))
			{
				list.Remove(this[name]);
				keys.Remove(name);
			}
			AddField(name, obj);
		}

		public JSONObject GetField(string name)
		{
			if (type == Type.OBJECT)
			{
				for (int i = 0; i < keys.Count; i++)
				{
					if ((string)keys[i] == name)
					{
						return (JSONObject)list[i];
					}
				}
			}
			return null;
		}

		public bool HasField(string name)
		{
			if (type == Type.OBJECT)
			{
				for (int i = 0; i < keys.Count; i++)
				{
					if ((string)keys[i] == name)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void Clear()
		{
			type = Type.NULL;
			list.Clear();
			keys.Clear();
			str = string.Empty;
			n = 0f;
			b = false;
		}

		public JSONObject Copy()
		{
			return new JSONObject(print());
		}

		public void Merge(JSONObject obj)
		{
			MergeRecur(this, obj);
		}

		private static void MergeRecur(JSONObject left, JSONObject right)
		{
			if (right.type != Type.OBJECT)
			{
				return;
			}
			for (int i = 0; i < right.list.Count; i++)
			{
				if (right.keys[i] == null)
				{
					continue;
				}
				string text = (string)right.keys[i];
				JSONObject jSONObject = (JSONObject)right.list[i];
				if (jSONObject.type == Type.ARRAY || jSONObject.type == Type.OBJECT)
				{
					if (left.HasField(text))
					{
						MergeRecur(left[text], jSONObject);
					}
					else
					{
						left.AddField(text, jSONObject);
					}
				}
				else if (left.HasField(text))
				{
					left.SetField(text, jSONObject);
				}
				else
				{
					left.AddField(text, jSONObject);
				}
			}
		}

		public string print()
		{
			return print(0);
		}

		public string print(int depth)
		{
			if (depth++ > 1000)
			{
				UnityEngine.Debug.Log("reached max depth!");
				return string.Empty;
			}
			string text = string.Empty;
			switch (type)
			{
			case Type.STRING:
				text = "\"" + this.str + "\"";
				break;
			case Type.NUMBER:
				text += n;
				break;
			case Type.OBJECT:
				if (list.Count > 0)
				{
					text = "{";
					text += "\n";
					depth++;
					for (int i = 0; i < list.Count; i++)
					{
						string str = (string)keys[i];
						JSONObject jSONObject = (JSONObject)list[i];
						if (jSONObject != null)
						{
							for (int j = 0; j < depth; j++)
							{
								text += "\t";
							}
							text = text + "\"" + str + "\":";
							text = text + jSONObject.print(depth) + ",";
							text += "\n";
						}
					}
					text = text.Substring(0, text.Length - 1);
					text = text.Substring(0, text.Length - 1);
					text += "}";
				}
				else
				{
					text += "null";
				}
				break;
			case Type.ARRAY:
				if (list.Count > 0)
				{
					text = "[";
					text += "\n";
					depth++;
					IEnumerator enumerator = list.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							JSONObject jSONObject2 = (JSONObject)enumerator.Current;
							if (jSONObject2 != null)
							{
								for (int k = 0; k < depth; k++)
								{
									text += "\t";
								}
								text = text + jSONObject2.print(depth) + ",";
								text += "\n";
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					text = text.Substring(0, text.Length - 1);
					text = text.Substring(0, text.Length - 1);
					text += "]";
				}
				break;
			case Type.BOOL:
				text = ((!b) ? (text + "false") : (text + "true"));
				break;
			case Type.NULL:
				text = "null";
				break;
			}
			return text;
		}

		public override string ToString()
		{
			return print();
		}

		public Dictionary<string, string> ToDictionary()
		{
			if (type == Type.OBJECT)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				for (int i = 0; i < list.Count; i++)
				{
					JSONObject jSONObject = (JSONObject)list[i];
					switch (jSONObject.type)
					{
					case Type.STRING:
						dictionary.Add((string)keys[i], jSONObject.str);
						break;
					case Type.NUMBER:
						dictionary.Add((string)keys[i], jSONObject.n + string.Empty);
						break;
					case Type.BOOL:
						dictionary.Add((string)keys[i], jSONObject.b + string.Empty);
						break;
					default:
						UnityEngine.Debug.LogWarning("Omitting object: " + (string)keys[i] + " in dictionary conversion");
						break;
					}
				}
				return dictionary;
			}
			UnityEngine.Debug.LogWarning("Tried to turn non-Object JSONObject into a dictionary");
			return null;
		}
	}
}
