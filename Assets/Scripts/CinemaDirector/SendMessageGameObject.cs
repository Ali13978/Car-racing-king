using UnityEngine;

namespace CinemaDirector
{
	[CutsceneItem("Game Object", "Send Message", new CutsceneItemGenre[]
	{
		CutsceneItemGenre.ActorItem
	})]
	public class SendMessageGameObject : CinemaActorEvent
	{
		public enum SendMessageValueType
		{
			None,
			Int,
			Float,
			Double,
			Bool,
			String
		}

		public string MethodName = string.Empty;

		public object Parameter;

		public SendMessageValueType ParameterType = SendMessageValueType.Int;

		public SendMessageOptions SendMessageOptions = SendMessageOptions.DontRequireReceiver;

		public int intValue;

		public float floatValue;

		public double doubleValue;

		public bool boolValue;

		public string stringValue = string.Empty;

		public override void Trigger(GameObject actor)
		{
			if (actor != null)
			{
				switch (ParameterType)
				{
				case SendMessageValueType.Int:
					Parameter = intValue;
					break;
				case SendMessageValueType.Float:
					Parameter = floatValue;
					break;
				case SendMessageValueType.Double:
					Parameter = doubleValue;
					break;
				case SendMessageValueType.Bool:
					Parameter = boolValue;
					break;
				case SendMessageValueType.String:
					Parameter = stringValue;
					break;
				default:
					Parameter = null;
					break;
				}
				actor.SendMessage(MethodName, Parameter, SendMessageOptions);
			}
		}
	}
}
