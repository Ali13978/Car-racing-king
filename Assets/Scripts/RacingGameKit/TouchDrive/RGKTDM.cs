using System.Collections.Generic;
using UnityEngine;

namespace RacingGameKit.TouchDrive
{
	public abstract class RGKTDM : MonoBehaviour
	{
		public abstract List<iRGKTouchItem> TouchItems
		{
			get;
			set;
		}
	}
}
