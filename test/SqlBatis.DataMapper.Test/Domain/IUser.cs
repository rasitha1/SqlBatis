

namespace SqlBatis.DataMapper.Test.Domain
{
    public interface IUser : IBaseDomain
    {
        IAddress Address { get; set; }
    } 
}
