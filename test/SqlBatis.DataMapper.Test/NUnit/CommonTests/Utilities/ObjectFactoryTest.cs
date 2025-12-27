using SqlBatis.DataMapper.Exceptions;
using SqlBatis.DataMapper.Test.Domain;
using SqlBatis.DataMapper.Utilities.Objects;
using NUnit.Framework;
using System;
using Microsoft.Extensions.Logging.Abstractions;


namespace SqlBatis.DataMapper.Test.NUnit.CommonTests.Utilities
{

    [TestFixture] 
	public class ObjectFactoryTest
    {
        private IObjectFactory _objectFactory;
		[SetUp]
        public void Setup()
        {
            _objectFactory = new ObjectFactory(NullLoggerFactory.Instance);
		}
		[Test]
        //[ExpectedException(typeof(ProbeException))]
		public void AbstractConstructor()
		{
			IFactory factory = _objectFactory.CreateFactory(typeof (AbstractDocument), Type.EmptyTypes );

		    Assert.That(() => factory.CreateInstance(null), Throws.TypeOf<ProbeException>());
        }
    	
		[Test]
		public void DevivedClassConstructor()
		{
			IFactory factory = _objectFactory.CreateFactory(typeof (Book), Type.EmptyTypes );

			Assert.That(factory, Is.Not.Null);
		}
    	
		[Test]
		//[ExpectedException(typeof(ProbeException))]
		public void PrivateConstructor()
		{
			Assert.That(() => _objectFactory.CreateFactory(typeof(PrivateOrder), Type.EmptyTypes), Throws.TypeOf<ProbeException>());
		}

        [Test]
        //[ExpectedException(typeof(ProbeException))]
        public void NoMatchConstructor()
        {
            Assert.That(() => _objectFactory.CreateFactory(typeof(ItemBis), Type.EmptyTypes), Throws.TypeOf<ProbeException>());
        }

		[Test]
		//[ExpectedException(typeof(ProbeException))]
		public void ProtectedConstructor()
		{
		    Assert.That(() => _objectFactory.CreateFactory(typeof(ProtectedItem), Type.EmptyTypes), Throws.TypeOf<ProbeException>());

		}

		[Test]
		public void ClassWithMultipleConstructor()
		{
			Type[] types = {typeof(string)};
			IFactory factory0 = _objectFactory.CreateFactory(typeof (Account), types );

			object[] parameters = {"gilles"};
			object obj0 = factory0.CreateInstance(parameters);

			Assert.That(obj0, Is.InstanceOf<Account>());
			Account account = (Account)obj0;
			Assert.That(account.Test, Is.EqualTo("gilles"));

			IFactory factory1 = _objectFactory.CreateFactory(typeof (Account), Type.EmptyTypes );

			object obj1 = factory1.CreateInstance(parameters);

			Assert.That(obj1, Is.InstanceOf<Account>());
		}

		[Test]
		public void StringConstructor()
		{
			Type[] types = {typeof(string)};
			IFactory factory = _objectFactory.CreateFactory(typeof (Account), types );

			object[] parameters = {"gilles"};
			object obj = factory.CreateInstance(parameters);

			Assert.That(obj, Is.InstanceOf<Account>());
			Account account = (Account)obj;
			Assert.That(account.Test, Is.EqualTo("gilles"));
		}

		[Test]
		public void MultipleParamConstructor1()
		{
			Type[] types = {typeof(string)};
			IFactory factory = _objectFactory.CreateFactory(typeof (Account), types );

			object[] parameters = new object[1];
			parameters[0] = null;
			object obj = factory.CreateInstance(parameters);

			Assert.That(obj, Is.InstanceOf<Account>());
			Account account = (Account)obj;
			Assert.That(account.Test, Is.Null);
		}

		[Test]
		public void IntConstructor()
		{
			Type[] types = {typeof(int)};
			IFactory factory = _objectFactory.CreateFactory(typeof (Account), types );

			object[] parameters = new object[1];
			parameters[0] = -55;
			object obj = factory.CreateInstance(parameters);

			Assert.That(obj, Is.InstanceOf<Account>());
			Account account = (Account)obj;
			Assert.That(account.Id, Is.EqualTo(-55));
		}

		[Test]
		public void EnumConstructorEnum()
		{
			Type[] types = {typeof(Days)};
			IFactory factory = _objectFactory.CreateFactory(typeof (Account), types );

			object[] parameters = new object[1];
			parameters[0] = Days.Sun;
			object obj = factory.CreateInstance(parameters);

			Assert.That(obj, Is.InstanceOf<Account>());
			Account account = (Account)obj;
			Assert.That(account.Days, Is.EqualTo(Days.Sun));
		}

		[Test]
		public void ClassConstructor()
		{
			Type[] types = {typeof(Property)};
			IFactory factory = _objectFactory.CreateFactory(typeof (Account), types );

			object[] parameters = new object[1];
			Property prop = new Property();
			prop.String = "Gilles";
			parameters[0] = prop;
			object obj = factory.CreateInstance(parameters);

			Assert.That(obj, Is.InstanceOf<Account>());
			Account account = (Account)obj;
			Assert.That(account.Property, Is.Not.Null);
			Assert.That(account.Property.String, Is.EqualTo("Gilles"));
		}

		[Test]
		public void DateTimeConstructor()
		{
			Type[] types = {typeof(DateTime)};
			IFactory factory = _objectFactory.CreateFactory(typeof (Account), types );

			object[] parameters = new object[1];
			DateTime date = DateTime.Now;
			parameters[0] = date;
			object obj = factory.CreateInstance(parameters);

			Assert.That(obj, Is.InstanceOf<Account>());
			Account account = (Account)obj;
			Assert.That(account.Date, Is.EqualTo(date));
		}

        [Test]
        public void ArrayParamConstructor()
        {
            Type[] types = { typeof(int[]) };
            IFactory factory = _objectFactory.CreateFactory(typeof(Account), types);

            object[] parameters = new object[1];

            int[] ids = new int[2];
            ids[0] = 1;
            ids[1] = 2;

            parameters[0] = ids;
            object obj = factory.CreateInstance(parameters);

            Assert.That(obj, Is.InstanceOf<Account>());
            Account account = (Account)obj;

            Assert.That(account.Ids.Length, Is.EqualTo(2));
            Assert.That(account.Ids[0], Is.EqualTo(1));
            Assert.That(account.Ids[1], Is.EqualTo(2));
        }

		[Test]
		public void MultipleParamConstructor0()
		{
			Type[] types = {typeof(string), typeof(Property)};
			IFactory factory = _objectFactory.CreateFactory(typeof (Account), types );

			object[] parameters = new object[2];
			Property prop = new Property();
			prop.String = "Gilles";
			parameters[0] = "Héloïse";
			parameters[1] = prop;
			object obj = factory.CreateInstance(parameters);

			Assert.That(obj, Is.InstanceOf<Account>());
			Account account = (Account)obj;
			Assert.That(account.FirstName, Is.EqualTo("Héloïse"));
			Assert.That(account.Property, Is.Not.Null);
			Assert.That(account.Property.String, Is.EqualTo("Gilles"));
		}



		[Test]
		public void DynamicFactoryCreatesTypes()
		{
			IFactory factory = _objectFactory.CreateFactory(typeof (Account), Type.EmptyTypes);
			object obj = factory.CreateInstance(null);
			Assert.That(obj, Is.InstanceOf<Account>());

			factory = _objectFactory.CreateFactory(typeof (Account), Type.EmptyTypes);
			obj = factory.CreateInstance(Type.EmptyTypes);
			Assert.That(obj, Is.InstanceOf<Account>());

			factory = _objectFactory.CreateFactory(typeof (Simple), Type.EmptyTypes);
			obj = factory.CreateInstance(Type.EmptyTypes);
			Assert.That(obj, Is.InstanceOf<Simple>());
		}

		[Test]
		public void CreateInstanceWithDifferentFactories()
		{
			const int TEST_ITERATIONS = 1000000;
			IFactory factory = null;

			#region new
			factory = new NewAccountFactory();

			// create an instance so that Activators can
			// cache the type/constructor/whatever
			factory.CreateInstance(Type.EmptyTypes);

			GC.Collect();
			GC.WaitForPendingFinalizers();

			Timer timer = new Timer();
			timer.Start();
			for (int i = 0; i < TEST_ITERATIONS; i++)
			{
				factory.CreateInstance(Type.EmptyTypes);
			}
			timer.Stop();

            #endregion

		}

		internal class NewAccountFactory : IFactory
		{
			public object CreateInstance(object[] parameters)
			{
				return new Account();
			}

		}

	}
}
