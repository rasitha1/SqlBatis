using System.Collections.Generic;
using System.Collections;
using IBatisNet.Common.Test.Domain;

namespace IBatisNet.DataMapper.Test.Domain
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
    public class ApplicationUser
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;
        public string UserName
        {
            get { return name; }
            set { name = value; }
        }

        private Address address;
        public Address Address
        {
            get { return address; }
            set { address = value; }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private IList<Role> roles;
        public IList<Role> Roles
        {
            get { return roles; }
            set { roles = value; }
        }
    }
}
