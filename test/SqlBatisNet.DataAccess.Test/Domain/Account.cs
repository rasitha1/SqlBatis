using System;

namespace IBatisNet.DataAccess.Test.Domain
{
	/// <summary>
	/// Description résumée de Account.
	/// </summary>
	public class Account
	{
		private int _id;
		private string _firstName;
		private string _lastName;
		private string _emailAddress;
		private int[] _ids = null;

		public int Id
		{
			get
			{
				return _id; 
			}
			set
			{ 
				_id = value; 
			}
		}

		public string FirstName
		{
			get
			{
				return _firstName; 
			}
			set
			{ 
				_firstName = value; 
			}
		}

		public string LastName
		{
			get
			{
				return _lastName; 
			}
			set
			{ 
				_lastName = value; 
			}
		}

		public string EmailAddress
		{
			get
			{
				return _emailAddress; 
			}
			set
			{ 
				_emailAddress = value; 
			}
		}

		public int[] Ids
		{
			get
			{
				return _ids; 
			}
			set
			{ 
				_ids = value; 
			}
		}
	}
}
