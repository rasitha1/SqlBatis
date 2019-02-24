
#if dotnet2
using System;
using System.Collections;
using System.Collections.Generic;
#endif
using IBatisNet.Common.Test.Domain;
using IBatisNet.Common.Utilities;
using NUnit.Framework;

namespace IBatisNet.Common.Test.NUnit.CommonTests.Utilities
{
    [TestFixture]
    public class TypeUtilsTest
    {
        
#if dotnet2        


        /// <summary>
        /// Test IsImplementGenericIListInterface.
        /// </summary>
        [Test]
        public void IsImplementGenericIListInterfaceOnIList()
        {
            Type memberType = typeof(IList<Document>);

            bool isGenericIList = TypeUtils.IsImplementGenericIListInterface(memberType);
            Assert.IsTrue(isGenericIList);

            //Console.Write(typeof(IList).IsAssignableFrom(typeof(Array)));
        }

        /// <summary>
        /// Test IsImplementGenericIListInterface.
        /// </summary>
        [Test]
        public void IsImplementGenericIListInterfaceOnList()
        {
            //GenericDocumentCollection documents = new GenericDocumentCollection();
            Type memberType = typeof(List<Document>);

            bool isGenericIList = TypeUtils.IsImplementGenericIListInterface(memberType);

            Assert.IsTrue(isGenericIList);
        }

        /// <summary>
        /// Test IsImplementGenericIListInterface.
        /// </summary>
        [Test]
        public void IsImplementGenericIListInterfaceOnCustomList()
        {
            Type memberType = typeof(GenericDocumentCollection);

            bool isGenericIList = TypeUtils.IsImplementGenericIListInterface(memberType);

            Assert.IsTrue(isGenericIList);
        }

        /// <summary>
        /// Test IsImplementGenericIListInterface.
        /// </summary>
        [Test]
        public void IsImplementGenericIListInterfaceOnArray()
        {
            Type memberType = typeof(Document[]);

            bool isGenericIList = TypeUtils.IsImplementGenericIListInterface(memberType);

            Assert.IsTrue(isGenericIList);
        }

        /// <summary>
        /// Test IsImplementGenericIListInterface.
        /// </summary>
        [Test]
        public void IsImplementGenericIListInterfaceOnInt()
        {
            Type memberType = typeof(int);

            bool isGenericIList = TypeUtils.IsImplementGenericIListInterface(memberType);

            Assert.IsFalse(isGenericIList);
        }
#endif
    }
}
