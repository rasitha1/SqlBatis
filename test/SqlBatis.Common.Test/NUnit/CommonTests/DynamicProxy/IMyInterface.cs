using System;

namespace IBatisNet.Common.Test.NUnit.CommonTests.DynamicProxy
{
	/// <summary>
	/// Summary description for IMyInterface.
	/// </summary>
	public interface IMyInterface
	{
		string Name
		{
			get;
			set;
		}

		bool Started
		{
			get;
			set;
		}

		// void Calc(int x, int y, out int result);

		int Calc(int x, int y);

		int Calc(int x, int y, int z, Single k);
	}
}
