using System;
using System.Collections;
using System.Collections.Generic;
using IBatisNet.Common.Test.Domain;

namespace IBatisNet.DataMapper.Test.Domain
{
    public interface IAccount
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string EmailAddress { get; set; }
    }

    public class BaseAccount : IAccount
    {
        private int id;
        private string firstName;
        private string lastName;
        private string emailAddress;

        #region IAccount Members

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        #endregion
    }

	/// <summary>
	/// Description résumée de Account.
	/// </summary>
	[Serializable]
	public class Account
	{

        private string _firstName;
		private string _lastName;
		private string _emailAddress;
		private int[] _ids = null;
		private bool _bannerOption = false;
		private bool _cartOption = false;
	    private Document _document = null;
        private int _id = 0;
        private string _test = string.Empty;
        private Days _days;
        private Property _prop = null;
        private DateTime _date = DateTime.MinValue;


        public IList<Document> Documents { get; protected set; }

        public Account()
		{}

        public Account(int[] ids)
        {
            _ids = ids;
        }

        public Account(DateTime date)
        {
            _date = date;
        }

        public Account(int id)
        {
            _id = id;
        }

        public Account(string test)
        {
            _test = test;
        }

        public Account(Days days)
        {
            _days = days;
        }


        public Account(string firstName, Property prop)
        {
            _firstName = firstName;
            _prop = prop;
        }

        public Account(Property prop)
        {
            _prop = prop;
        }

		public Account(int identifiant, string firstName, string lastName)
		{
            _id = identifiant;
			_firstName = firstName;
			_lastName = lastName;
		}

        public Account(int identifiant, string firstName, string lastName, Document document)
        {
            _id = identifiant;
            _firstName = firstName;
            _lastName = lastName;
            _document = document;
        }

		public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}

        public string Test
        {
            get { return _test; }
        }

        public Property Property
        {
            get { return _prop; }
        }

        public DateTime Date
        {
            get { return _date; }
        }

        public Days Days
        {
            get { return _days; }
        }

        public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		public string EmailAddress
		{
			get { return _emailAddress; }
			set { _emailAddress = value; }
		}

		public int[] Ids
		{
			get { return _ids; }
			set { _ids = value; }
		}

		public bool BannerOption
		{
			get { return _bannerOption; }
			set { _bannerOption = value; }
		}

		public bool CartOption
		{
			get { return _cartOption; }
			set { _cartOption = value; }
		}

        public Document Document
        {
            get { return _document; }
            set { _document = value; }
        }
	}
}
