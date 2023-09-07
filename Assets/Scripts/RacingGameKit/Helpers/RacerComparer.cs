using System;
using System.Collections.Generic;

namespace RacingGameKit.Helpers
{
	internal class RacerComparer : IComparer<Racer_Detail>
	{
		public int Compare(Racer_Detail Racer1, Racer_Detail Racer2)
		{
			return ((IComparable)Racer1.RacerDistance).CompareTo((object)Racer2.RacerDistance);
		}
	}
}
