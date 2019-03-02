using System;

namespace IBatisNet.Common.Test.NUnit.CommonTests.DynamicProxy
{
	/// <summary>
	/// Description résumée de MyInterfaceImpl.
	/// </summary>
	public class MyInterfaceImpl : IMyInterface
	{
		private string _name;
		private bool _started;

		public MyInterfaceImpl()
		{
		}

		#region IMyInterface Members

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
