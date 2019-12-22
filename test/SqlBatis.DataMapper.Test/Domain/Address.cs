
using System;
using IBatisNet.Common.Test.Domain;

namespace IBatisNet.DataMapper.Test.Domain
{
    public class Address : IAddress
    {

        public Guid Id { get; set; }

        public string Street { get; set; }
        public string Streetname { get; set; }
    }
}
