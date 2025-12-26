using System;

using SqlBatis.DataMapper.Test.Domain;
using SqlBatis.DataMapper.Utilities;
using SqlBatis.DataMapper.Utilities.Objects.Members;
using NUnit.Framework;

namespace SqlBatis.DataMapper.Test.NUnit.CommonTests.Utilities
{
    [TestFixture]
    public class PropertyAccessorTest : BaseMemberTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            intSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Int");
            intGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Int");

            longSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Long");
            longGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Long");

            sbyteSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "SByte");
            sbyteGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "SByte");

            stringSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "String");
            stringGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "String");

            datetimeSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "DateTime");
            datetimeGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "DateTime");

            decimalSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Decimal");
            decimalGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Decimal");

            byteSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Byte");
            byteGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Byte");

            charSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Char");
            charGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Char");

            shortSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Short");
            shortGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Short");

            ushortSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "UShort");
            ushortGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "UShort");

            uintSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "UInt");
            uintGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "UInt");

            ulongSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "ULong");
            ulongGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "ULong");

            boolSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Bool");
            boolGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Bool");

            doubleSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Double");
            doubleGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Double");

            floatSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Float");
            floatGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Float");

            guidSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Guid");
            guidGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Guid");

            timespanSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "TimeSpan");
            timespanGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "TimeSpan");

            accountSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Account");
            accountGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Account");

            enumSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "Day");
            enumGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "Day");

            nullableSetAccessor = factorySet.CreateSetAccessor(typeof(Property), "IntNullable");
            nullableGetAccessor = factoryGet.CreateGetAccessor(typeof(Property), "IntNullable");
        }


        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        {
        }

        #endregion

        /// <summary>
        /// Test Finding properties on interfaces which "inherites" other interfaces
        /// </summary>
        [Test]
        public void TestJIRA210OnGet()
        {
            //----------------------------
            IGetAccessor addressGet = factoryGet.CreateGetAccessor(typeof(User), "Address");

            User user = new User();
            user.Address = new Address();
            Guid newGuid = Guid.NewGuid();
            user.Address.Id = newGuid;

            IAddress address = (IAddress)addressGet.Get(user);
            Assert.That(address, Is.Not.Null);
            Assert.That(address.Id, Is.EqualTo(newGuid));

            IGetAccessor domainGet = factoryGet.CreateGetAccessor(typeof(IAddress), "Id");

            Guid guid = (Guid)domainGet.Get(address);
            Assert.That(guid, Is.EqualTo(newGuid));
        }

        /// <summary>
        /// Test Finding properties on interfaces which "inherites" other interfaces
        /// </summary>
        [Test]
        public void TestJIRA210OnSet()
        {
            Address adr = new Address();

            Guid newGuid = Guid.NewGuid();

            ISetAccessor domainSet = factorySet.CreateSetAccessor(typeof(IAddress), "Id");

            domainSet.Set(adr, newGuid);
            Assert.That(adr.Id, Is.EqualTo(newGuid));
        }

        /// <summary>
        /// Test multiple call to factory
        /// </summary>
        [Test]
        public void TestMemberAccessorFactory()
        {
            IGetAccessor accessor11 = factoryGet.CreateGetAccessor(typeof(Property), "Int");
            IGetAccessor accessor12 = factoryGet.CreateGetAccessor(typeof(Property), "Int");

            Assert.That(HashCodeProvider.GetIdentityHashCode(accessor12), Is.EqualTo(HashCodeProvider.GetIdentityHashCode(accessor11)));

            ISetAccessor accessor21 = factorySet.CreateSetAccessor(typeof(Property), "Int");
            ISetAccessor accessor22 = factorySet.CreateSetAccessor(typeof(Property), "Int");

            Assert.That(HashCodeProvider.GetIdentityHashCode(accessor22), Is.EqualTo(HashCodeProvider.GetIdentityHashCode(accessor21)));

        }

        /// <summary>
        /// Test multiple IGetAccessor
        /// </summary>
        [Test]
        public void TestMultipleMemberAccessorFactory()
        {
            Property prop = new Property();
            IGetAccessor accessor1 = factoryGet.CreateGetAccessor(typeof(Property), "Int");

            IGetAccessorFactory factory2 = new GetAccessorFactory(true);
            IGetAccessor accessor2 = factory2.CreateGetAccessor(typeof(Property), "Int");

            Assert.That(accessor1.Get(prop), Is.EqualTo(int.MinValue));
            Assert.That(accessor2.Get(prop), Is.EqualTo(int.MinValue));
        }


        /// <summary>
        /// Test accessor on virtual property
        /// </summary>
        [Test]
        //[ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Test virtual")]
        public void TestVirtualIMemberAccessor1()
        {
            IGetAccessor accessorGet = factoryGet.CreateGetAccessor(typeof(PropertySon), "Account");
            ISetAccessor accessorSet = factorySet.CreateSetAccessor(typeof(PropertySon), "Account");

            PropertySon son = new PropertySon();
            Account account = (Account)accessorGet.Get(son);

            Assert.That(account.Days, Is.EqualTo(Days.Wed));
            ;

            Assert.That(() => accessorSet.Set(son, new Account()), Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Test virtual"));
        }

        /// <summary>
        /// Test accessor on virtual property
        /// </summary>
        [Test]
        //[ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Test virtual")]
        public void TestVirtualIMemberAccessor2()
        {
            IGetAccessor accessorGet = factoryGet.CreateGetAccessor(typeof(PropertySon), "Int");
            ISetAccessor accessorSet = factorySet.CreateSetAccessor(typeof(PropertySon), "Int");

            PropertySon son = new PropertySon();
            Int32 i = (Int32)accessorGet.Get(son);

            Assert.That(i, Is.EqualTo(-88));

            Assert.That(() => accessorSet.Set(son, 9), Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Test virtual"));
        }

        /// <summary>
        /// Test IMemberAccessor on virtual property
        /// </summary>
        [Test]
        //[ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Test virtual")]
        public void TestVirtualIMemberAccessor3()
        {
            IGetAccessor accessorGet = factoryGet.CreateGetAccessor(typeof(PropertySon), "DateTime");
            ISetAccessor accessorSet = factorySet.CreateSetAccessor(typeof(PropertySon), "DateTime");

            PropertySon son = new PropertySon();
            DateTime date = (DateTime)accessorGet.Get(son);

            Assert.That(date, Is.EqualTo(new DateTime(2000, 1, 1)));

            Assert.That(() => accessorSet.Set(son, DateTime.Now), Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Test virtual"));
        }

        /// <summary>
        /// Test IMemberAccessor on private set property
        /// </summary>
        [Test]
        public void TestPrivateSetAccessor()
        {
            ISetAccessor accessorSet = factorySet.CreateSetAccessor(typeof(PropertySon), "PrivateIndex");

            PropertySon son = new PropertySon();
            accessorSet.Set(son, -99);

            Assert.That(son.Index, Is.EqualTo(-99));
        }

        /// <summary>
        /// Test IMemberAccessor on protected set property
        /// </summary>
        [Test]
        public void TestProtectedSetAccessor()
        {
            ISetAccessor accessorSet = factorySet.CreateSetAccessor(typeof(PropertySon), "Index");

            PropertySon son = new PropertySon();
            accessorSet.Set(son, -99);

            Assert.That(son.Index, Is.EqualTo(-99));
        }

        /// <summary>
        /// Test set IMemberAccessor on a property override by new
        /// </summary>
        [Test]
        public void TestSetPropertyOverrideByNew()
        {
            ISetAccessor accessorSet = factorySet.CreateSetAccessor(typeof(PropertySon), "Float");

            PropertySon son = new PropertySon();
            accessorSet.Set(son, -99);

            Assert.That(son.Float, Is.EqualTo(-99 * 2));
        }

        /// <summary>
        /// Test get IMemberAccessor on a property override by new
        /// </summary>
        [Test]
        public void TestGetPropertyOverrideByNew()
        {
            IGetAccessor accessorGet = factoryGet.CreateGetAccessor(typeof(PropertySon), "Float");

            PropertySon son = new PropertySon();
            son.Float = -99;

            Assert.That(accessorGet.Get(son), Is.EqualTo(-99 * 2));
        }


        /// <summary>
        /// Test getter access to Public Generic Property
        /// </summary>
        [Test]
        public void TestPublicGetterOnGenericProperty2()
        {
            IGetAccessor accessorGet = factoryGet.CreateGetAccessor(typeof(SpecialReference<Account>), "Value");

            SpecialReference<Account> referenceAccount = new SpecialReference<Account>();
            Account account = new Account(5);
            referenceAccount.Value = account;

            Account acc = accessorGet.Get(referenceAccount) as Account;
            Assert.That(acc, Is.EqualTo(referenceAccount.Value));
            Assert.That(acc.Id, Is.EqualTo(referenceAccount.Value.Id));
        }

        /// <summary>
        /// Test getter access to Public Generic Property
        /// </summary>
        [Test]
        public void TestPublicGetterOnGenericProperty()
        {
            IGetAccessor accessorGet = factoryGet.CreateGetAccessor(typeof(PropertySon), "ReferenceAccount");

            PropertySon son = new PropertySon();
            son.ReferenceAccount = new SpecialReference<Account>();
            Account account = new Account(5);
            son.ReferenceAccount.Value = account;

            SpecialReference<Account> acc = accessorGet.Get(son) as SpecialReference<Account>;
            Assert.That(acc.Value, Is.EqualTo(account));
            Assert.That(acc.Value.Id, Is.EqualTo(account.Id));
        }

        /// <summary>
        /// Test setter access to Public Generic Property
        /// </summary>
        [Test]
        public void TestPublicSetterOnGenericVariable()
        {
            ISetAccessor accessorSet = factorySet.CreateSetAccessor(typeof(PropertySon), "ReferenceAccount");

            PropertySon son = new PropertySon();
            SpecialReference<Account> referenceAccount = new SpecialReference<Account>();
            Account account = new Account(5);
            referenceAccount.Value = account;
            accessorSet.Set(son, referenceAccount);

            Assert.That(son.ReferenceAccount, Is.EqualTo(referenceAccount));
            Assert.That(son.ReferenceAccount.Value.Id, Is.EqualTo(referenceAccount.Value.Id));
        }

        /// <summary>
        /// Test setter access to Public Generic Property
        /// </summary>
        [Test]
        public void TestPublicSetterOnGenericVariable2()
        {
            ISetAccessor accessorSet = factorySet.CreateSetAccessor(typeof(SpecialReference<Account>), "Value");

            SpecialReference<Account> referenceAccount = new SpecialReference<Account>();
            Account account = new Account(5);
            accessorSet.Set(referenceAccount, account);

            Assert.That(referenceAccount.Value, Is.EqualTo(account));
            Assert.That(referenceAccount.Value.Id, Is.EqualTo(account.Id));
        }

        /// <summary>
        /// Test getter access to private Generic Property
        /// </summary>
        [Test]
        public void TestPrivateGetterOnGenericProperty()
        {
            IGetAccessor accessorGet = factoryGet.CreateGetAccessor(typeof(PropertySon), "_referenceAccount");

            PropertySon son = new PropertySon();
            son.ReferenceAccount = new SpecialReference<Account>();
            Account account = new Account(5);
            son.ReferenceAccount.Value = account;

            SpecialReference<Account> acc = accessorGet.Get(son) as SpecialReference<Account>;
            Assert.That(acc.Value, Is.EqualTo(account));
            Assert.That(acc.Value.Id, Is.EqualTo(account.Id));
        }

        /// <summary>
        /// Test getter access to private Generic Property
        /// </summary>
        [Test]
        public void TestPrivateGetterOnGenericProperty2()
        {
            IGetAccessor accessorGet = factoryGet.CreateGetAccessor(typeof(SpecialReference<Account>), "_value");

            SpecialReference<Account> referenceAccount = new SpecialReference<Account>();
            Account account = new Account(5);
            referenceAccount.Value = account;

            Account acc = accessorGet.Get(referenceAccount) as Account;
            Assert.That(acc, Is.EqualTo(referenceAccount.Value));
            Assert.That(acc.Id, Is.EqualTo(referenceAccount.Value.Id));
        }

        /// <summary>
        /// Test setter access to Public Generic Property
        /// </summary>
        [Test]
        public void TestPrivateSetterOnGenericVariable()
        {
            ISetAccessor accessorSet = factorySet.CreateSetAccessor(typeof(PropertySon), "_referenceAccount");

            PropertySon son = new PropertySon();
            SpecialReference<Account> referenceAccount = new SpecialReference<Account>();
            Account account = new Account(5);
            referenceAccount.Value = account;
            accessorSet.Set(son, referenceAccount);

            Assert.That(son.ReferenceAccount, Is.EqualTo(referenceAccount));
            Assert.That(son.ReferenceAccount.Value.Id, Is.EqualTo(referenceAccount.Value.Id));
        }

        /// <summary>
        /// Test setter access to Public Generic Property
        /// </summary>
        [Test]
        public void TestPrivateSetterOnGenericVariable2()
        {
            ISetAccessor accessorSet = factorySet.CreateSetAccessor(typeof(SpecialReference<Account>), "_value");

            SpecialReference<Account> referenceAccount = new SpecialReference<Account>();
            Account account = new Account(5);
            accessorSet.Set(referenceAccount, account);

            Assert.That(referenceAccount.Value, Is.EqualTo(account));
            Assert.That(referenceAccount.Value.Id, Is.EqualTo(account.Id));
        }
    }
}