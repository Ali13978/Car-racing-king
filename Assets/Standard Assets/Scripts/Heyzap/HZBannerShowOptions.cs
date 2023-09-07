namespace Heyzap
{
	public class HZBannerShowOptions : HZShowOptions
	{
		public const string POSITION_TOP = "top";

		public const string POSITION_BOTTOM = "bottom";

		private const string DEFAULT_POSITION = "bottom";

		private string position = "bottom";

		public string Position
		{
			get
			{
				return position;
			}
			set
			{
				if (value == "top" || value == "bottom")
				{
					position = value;
				}
			}
		}
	}
}
