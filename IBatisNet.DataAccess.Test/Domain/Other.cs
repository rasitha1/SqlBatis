using System;

namespace IBatisNet.DataAccess.Test.Domain
{
	/// <summary>
	/// Description résumée de Other.
	/// </summary>
	public class Other
	{
		private int _int;
		private long _long;

		public int Int
		{
			get
			{
				return _int; 
			}
			set
			{ 
				_int = value; 
			}
		}

		public long Long
		{
			get
			{
				return _long; 
			}
			set
			{ 
				_long = value; 
			}
		}
	}
}
