using System;
using System.Collections.Generic;

namespace RacingGameKit.Helpers
{
	internal class RacerComparerByHighSpeed : IComparer<Racer_Detail>
	{
		public int Compare(Racer_Detail Racer1, Racer_Detail Racer2)
		{
			return -((IComparable)Racer1.RacerHighestSpeed).CompareTo((object)Racer2.RacerHighestSpeed);
		}
	}
}
