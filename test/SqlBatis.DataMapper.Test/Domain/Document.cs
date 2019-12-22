using System;

namespace IBatisNet.DataMapper.Test.Domain
{
	/// <summary>
	/// Summary for Document.
	/// </summary>
    [Serializable]
	public class Document
	{
		private int _id = -1;
		private string _title = string.Empty;

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}
		
		public string Test
		{
			set { _title = value; }
		}
		
	}

    /// <summary>
    /// Summary description for Document.
    /// </summary>
    public abstract class AbstractDocument
    {
        private DateTime _date = DateTime.MinValue;
        private int _nb = int.MinValue;

        public DateTime Creation
        {
            get { return _date; }
            set { _date = value; }
        }

        public int PageNumber
        {
            get { return _nb; }
        }
    }
}
