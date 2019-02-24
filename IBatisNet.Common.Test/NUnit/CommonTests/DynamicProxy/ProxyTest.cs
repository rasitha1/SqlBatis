using System;
using System.Collections;

using IBatisNet.Common.Test.NUnit;

using NUnit.Framework;

namespace IBatisNet.Common.Test.NUnit.CommonTests.DynamicProxy
{
	/// <summary>
	/// Summary description for ProxyTest.
	/// </summary>
		[TestFixture] 
	public class ProxyTest
	{
		#region SetUp & TearDown

			/// <summary>
			/// SetUp
			/// </summary>
			[SetUp] 
			public void SetUp() 
			{
			}


			/// <summary>
			/// TearDown
			/// </summary>
			[TearDown] 
			public void Dispose()
			{ 
			} 

			#endregion

		#region Tests

		[Test]
		public void TestGenerationSimpleInterface()
		{
			IInvocationHandler handler = new StandardInvocationHandler( new MyInterfaceImpl() );
			object proxy = ProxyGenerator.CreateProxy( typeof(IMyInterface), handler );

			Assert.IsNotNull( proxy );
			Assert.IsTrue( typeof(IMyInterface).IsAssignableFrom( proxy.GetType() ) );

			IMyInterface inter = (IMyInterface) proxy;

			Assert.AreEqual( 45, inter.Calc( 20, 25 ) );

			inter.Name = "opa";
			Assert.AreEqual( "opa", inter.Name );

			inter.Started = true;
			Assert.AreEqual( true, inter.Started );
		}

		[Test]
		public void TestGenerationWithInterfaceHeritage()
		{
			IInvocationHandler handler = new StandardInvocationHandler( new MySecondInterfaceImpl() );
			object proxy = ProxyGenerator.CreateProxy( typeof(IMySecondInterface), handler );

			Assert.IsNotNull( proxy );
			Assert.IsTrue( typeof(IMyInterface).IsAssignableFrom( proxy.GetType() ) );
			Assert.IsTrue( typeof(IMySecondInterface).IsAssignableFrom( proxy.GetType() ) );

			IMySecondInterface inter = (IMySecondInterface) proxy;
			inter.Calc(1, 1);

			inter.Name = "hammett";
			Assert.AreEqual( "hammett", inter.Name );

			inter.Address = "pereira leite, 44";
			Assert.AreEqual( "pereira leite, 44", inter.Address );
		
			Assert.AreEqual( 45, inter.Calc( 20, 25 ) );
		}

		#endregion
	}
}
