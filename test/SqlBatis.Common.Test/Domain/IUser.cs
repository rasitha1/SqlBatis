

namespace IBatisNet.Common.Test.Domain
{
    public interface IUser : IBaseDomain
    {
        IAddress Address { get; set; }
    } 
}
