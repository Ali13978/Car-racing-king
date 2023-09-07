namespace Heyzap
{
	public class HZIncentivizedShowOptions : HZShowOptions
	{
		private const string DEFAULT_INCENTIVIZED_INFO = "";

		private string incentivizedInfo = string.Empty;

		public string IncentivizedInfo
		{
			get
			{
				return incentivizedInfo;
			}
			set
			{
				if (value != null)
				{
					incentivizedInfo = value;
				}
				else
				{
					incentivizedInfo = string.Empty;
				}
			}
		}
	}
}
