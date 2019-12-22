
using System;
using SqlBatis.DataMapper.Test.Domain;

namespace SqlBatis.DataMapper.Test.Domain
{
    public class Address : IAddress
    {

        public Guid Id { get; set; }

        public string Street { get; set; }
        public string Streetname { get; set; }
    }
}
