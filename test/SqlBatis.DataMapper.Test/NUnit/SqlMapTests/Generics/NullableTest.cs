using System;

using NUnit.Framework;

using SqlBatis.DataMapper.Test.Domain;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests.Generics
{
    /// <summary>
    /// Summary description for ResultClassTest.
    /// </summary>
    [TestFixture]
    public class NullableTest : BaseTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "Nullable-init.sql");
        }

        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        {}

        #endregion

        #region NullableClass
        /// <summary>
        ///  Test a NullableClass resultClass
        /// </summary>
        [Test]
        public void TestNullableViaResultClass()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetClassNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestBool, Is.Null);
            Assert.That(clazz.TestByte, Is.Null);
            Assert.That(clazz.TestChar, Is.Null);
            Assert.That(clazz.TestDateTime, Is.Null);
            Assert.That(clazz.TestDecimal, Is.Null);
            Assert.That(clazz.TestDouble, Is.Null);
            Assert.That(clazz.TestGuid, Is.Null);
            Assert.That(clazz.TestInt16, Is.Null);
            Assert.That(clazz.TestInt32, Is.Null);
            Assert.That(clazz.TestInt64, Is.Null);
            Assert.That(clazz.TestSingle, Is.Null);
        }

        /// <summary>
        ///  Test a NullableClass resultClass
        /// </summary>
        [Test]
        public void TestNullableViaResultClass2()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestBool = true;
            clazz.TestByte = 155;
            clazz.TestChar = 'a';
            DateTime? date = new DateTime?(DateTime.Now);
            clazz.TestDateTime = date;
            clazz.TestDecimal = 99.53M;
            clazz.TestDouble = 99.5125;
            Guid? guid = new Guid?(Guid.NewGuid());
            clazz.TestGuid = guid;
            clazz.TestInt16 = 45;
            clazz.TestInt32 = 99;
            clazz.TestInt64 = 1234567890123456789;
            clazz.TestSingle = 4578.46445454112f;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetClassNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestBool.Value, Is.True);
            Assert.That(clazz.TestByte, Is.EqualTo(155));
            Assert.That(clazz.TestChar, Is.EqualTo('a'));
            Assert.That(clazz.TestDateTime.Value.ToString(), Is.EqualTo(date.Value.ToString()));
            Assert.That(clazz.TestDecimal, Is.EqualTo(99.53M));
            Assert.That(clazz.TestDouble, Is.EqualTo(99.5125));
            Assert.That(clazz.TestGuid, Is.EqualTo(guid));
            Assert.That(clazz.TestInt16, Is.EqualTo(45));
            Assert.That(clazz.TestInt32, Is.EqualTo(99));
            Assert.That(clazz.TestInt64, Is.EqualTo(1234567890123456789));
            Assert.That(clazz.TestSingle, Is.EqualTo(4578.46445454112f));
        } 
        #endregion

        #region bool

        /// <summary>
        /// Test nullable bool
        /// </summary>
        [Test]
        public void TestNullableBool()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestBool, Is.Null);
        }

        /// <summary>
        /// Test not nullable bool
        /// </summary>
        [Test]
        public void TestNotNullableBool()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestBool = false;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestBool, Is.EqualTo(false));
        } 
        #endregion

        #region byte
        /// <summary>
        /// Test nullable byte
        /// </summary>
        [Test]
        public void TestNullableByte()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestByte, Is.Null);
        }

        /// <summary>
        /// Test not nullable byte
        /// </summary>
        [Test]
        public void TestNotNullableByte()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestByte = 155;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestByte, Is.EqualTo(155));
        } 
        #endregion

        #region char
        /// <summary>
        /// Test nullable char
        /// </summary>
        [Test]
        public void TestNullableChar()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestChar, Is.Null);
        }

        /// <summary>
        /// Test not nullable char
        /// </summary>
        [Test]
        public void TestNotNullableChar()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestChar = 'a';

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestChar, Is.EqualTo('a'));
        } 
        #endregion

        #region datetime
        /// <summary>
        /// Test nullable datetime
        /// </summary>
        [Test]
        public void TestNullableDateTime()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestDateTime, Is.Null);
        }

        /// <summary>
        /// Test not nullable datetime
        /// </summary>
        [Test]
        public void TestNotNullableDateTime()
        {
            NullableClass clazz = new NullableClass();
            DateTime? date = new DateTime?(DateTime.Now);
            clazz.TestDateTime = date;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestDateTime.Value.ToString(), Is.EqualTo(date.Value.ToString()));
        }
        #endregion

        #region decimal
        /// <summary>
        /// Test nullable decimal
        /// </summary>
        [Test]
        public void TestNullableDecimal()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestDecimal, Is.Null);
        }

        /// <summary>
        /// Test not nullable decimal
        /// </summary>
        [Test]
        public void TestNotNullableDecimal()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestDecimal = 99.53M;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestDecimal, Is.EqualTo(99.53M));
        }
        #endregion

        #region Double
        /// <summary>
        /// Test nullable Double
        /// </summary>
        [Test]
        public void TestNullableDouble()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestDouble, Is.Null);
        }

        /// <summary>
        /// Test not nullable Double
        /// </summary>
        [Test]
        public void TestNotNullableDouble()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestDouble = 99.5125;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestDouble, Is.EqualTo(99.5125));
        }
        #endregion

        #region Guid
        /// <summary>
        /// Test nullable Guid
        /// </summary>
        [Test]
        public void TestNullableGuid()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestGuid, Is.Null);
        }

        /// <summary>
        /// Test not nullable Guid
        /// </summary>
        [Test]
        public void TestNotNullableGuid()
        {
            NullableClass clazz = new NullableClass();
            Guid? guid = new Guid?(Guid.NewGuid());
            clazz.TestGuid = guid;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestGuid, Is.EqualTo(guid));
        }
        #endregion

        #region Int16
        /// <summary>
        /// Test nullable Int16
        /// </summary>
        [Test]
        public void TestNullableInt16()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestInt16, Is.Null);
        }

        /// <summary>
        /// Test not nullable Int16
        /// </summary>
        [Test]
        public void TestNotNullableInt16()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestInt16 = 45;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestInt16, Is.EqualTo(45));
        }
        #endregion

        #region int 32
        /// <summary>
        /// Test nullable int32
        /// </summary>
        [Test]
        public void TestNullableInt32()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestInt32, Is.Null);
        }

        /// <summary>
        /// Test not nullable int32
        /// </summary>
        [Test]
        public void TestNotNullableInt32()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestInt32 = 99;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestInt32, Is.EqualTo(99));
        } 
        #endregion

        #region Int64
        /// <summary>
        /// Test nullable Int64
        /// </summary>
        [Test]
        public void TestNullableInt64()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestInt64, Is.Null);
        }

        /// <summary>
        /// Test not nullable Int64
        /// </summary>
        [Test]
        public void TestNotNullableInt64()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestInt64 = 1234567890123456789;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestInt64, Is.EqualTo(1234567890123456789));
        }
        #endregion

        #region Single
        /// <summary>
        /// Test nullable Single
        /// </summary>
        [Test]
        public void TestNullableSingle()
        {
            NullableClass clazz = new NullableClass();

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestSingle, Is.Null);
        }

        /// <summary>
        /// Test not nullable Single
        /// </summary>
        [Test]
        public void TestNotNullableSingle()
        {
            NullableClass clazz = new NullableClass();
            clazz.TestSingle = 4578.46445454112f;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestSingle, Is.EqualTo(4578.46445454112f));
        }
        #endregion

        #region timespan
        
        [Test]
        public void TestNullableTimeSpan()
        {
            NullableClass clazz = new NullableClass();
            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestTimeSpan, Is.Null);
        }

        /// <summary>
        /// Test not nullable timespan
        /// </summary>
        [Test]
        public void TestNotNullableTimeSpan()
        {
            NullableClass clazz = new NullableClass();
            TimeSpan? span = new TimeSpan?(new TimeSpan(1, 2, 3, 4, 5));
            clazz.TestTimeSpan = span;

            sqlMap.Insert("InsertNullable", clazz);
            clazz = null;
            clazz = sqlMap.QueryForObject<NullableClass>("GetNullable", 1);

            Assert.That(clazz, Is.Not.Null);
            Assert.That(clazz.Id, Is.EqualTo(1));
            Assert.That(clazz.TestTimeSpan.Value.ToString(), Is.EqualTo(span.Value.ToString()));
        }

        #endregion
    }
}
