namespace Heyzap
{
	public class HZShowOptions
	{
		private string tag = "default";

		public string Tag
		{
			get
			{
				return tag;
			}
			set
			{
				if (value != null)
				{
					tag = value;
				}
				else
				{
					tag = "default";
				}
			}
		}
	}
}
