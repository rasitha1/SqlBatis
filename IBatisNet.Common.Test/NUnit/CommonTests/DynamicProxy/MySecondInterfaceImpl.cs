using System;

namespace IBatisNet.Common.Test.NUnit.CommonTests.DynamicProxy
{
	/// <summary>
	/// Description résumée de MySecondInterfaceImpl.
	/// </summary>
	public class MySecondInterfaceImpl : IMySecondInterface
	{
		private string _name;
		private bool _started;
		private string _address;

		#region IMySecondInterface members

		public string Address
		{
			get
			{
				return _address;
			}
			set
			{
				_address = value;
			}
		}

		#endregion

		#region IMyInterface members

		public String Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public bool Started
		{
			get
			{
				return _started;
			}
			set
			{
				_started = value;
			}
		}

		public int Calc(int x, int y)
		{
			return x + y;
		}

		public int Calc(int x, int y, int z, Single k)
		{
			return x + y + z + (int)k;
		}


		#endregion
	}
}
