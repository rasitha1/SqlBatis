
namespace IBatisNet.Common.Test.Domain
{
    public class User : BaseDomain, IUser
    {
        private IAddress address;

        public IAddress Address
        {
            get { return address; } 
            set { address = value; } 
        }

    } 
}
