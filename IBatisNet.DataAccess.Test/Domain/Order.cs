using System;
using System.Collections;

namespace IBatisNet.DataAccess.Test.Domain
{
	/// <summary>
	/// Description résumée de Order.
	/// </summary>
	public class Order
	{
		private int _id;
		private Account _account;
		private System.DateTime _date;
		private string _cardType;
		private string _cardNumber;
		private string _cardExpiry;
		private string _street;
		private string _city;
		private string _province;
		private string _postalCode;
		private IList _lineItemsIList;
		private IList _lineItems;//LineItemCollection
		private LineItem[] _lineItemsArray;
		private LineItem _favouriteLineItem;

		public LineItem FavouriteLineItem
		{
			get
			{
				return _favouriteLineItem; 
			}
			set
			{ 
				_favouriteLineItem = value; 
			}
		}

		public IList LineItemsIList
		{
			get
			{
				return _lineItemsIList; 
			}
			set
			{ 
				_lineItemsIList = value; 
			}
		}


		public IList LineItems
		{
			get
			{
				return _lineItems; 
			}
			set
			{ 
				_lineItems = value; 
			}
		}

		public LineItem[] LineItemsArray
		{
			get
			{
				return _lineItemsArray; 
			}
			set
			{ 
				_lineItemsArray = value; 
			}
		}

		public string PostalCode
		{
			get
			{
				return _postalCode; 
			}
			set
			{ 
				_postalCode = value; 
			}
		}

		public string Province
		{
			get
			{
				return _province; 
			}
			set
			{ 
				_province = value; 
			}
		}

		public string City
		{
			get
			{
				return _city; 
			}
			set
			{ 
				_city = value; 
			}
		}

		public string Street
		{
			get
			{
				return _street; 
			}
			set
			{ 
				_street = value; 
			}
		}

		public string CardExpiry
		{
			get
			{
				return _cardExpiry; 
			}
			set
			{ 
				_cardExpiry = value; 
			}
		}

		public string CardNumber
		{
			get
			{
				return _cardNumber; 
			}
			set
			{ 
				_cardNumber = value; 
			}
		}

		public string CardType
		{
			get
			{
				return _cardType; 
			}
			set
			{ 
				_cardType = value; 
			}
		}

		public Account Account
		{
			get
			{
				return _account; 
			}
			set
			{ 
				_account = value; 
			}
		}

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

		public System.DateTime Date
		{
			get
			{
				return _date; 
			}
			set
			{ 
				_date = value; 
			}
		}

		public System.DateTime OrderDate {
			get {
				return _date; 
			}
			set { 
				_date = value; 
			}
		}
	}
}
