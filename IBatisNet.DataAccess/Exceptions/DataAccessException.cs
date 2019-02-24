
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 383115 $
 * $Date: 2006-03-04 15:21:51 +0100 (sam., 04 mars 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

using System;
using System.Runtime.Serialization;

using IBatisNet.Common.Exceptions;

namespace IBatisNet.DataAccess.Exceptions
{
	/// <summary>
	/// The DataAccessException is thrown when an error in the Dao occurs.
	/// </summary>
	[Serializable]
	public class DataAccessException : IBatisNetException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IBatisNet.DataAccess.Exceptions.DataAccessException"/> class.
		/// </summary>
		/// <remarks>
		/// This constructor initializes the <para>Message</para> property of the new instance 
		/// to a system-supplied message that describes the error.
		/// </remarks>
		public DataAccessException() : base("iBatis.NET DataAccess component caused an exception.") { }

		/// <summary>
		/// Initializes a new instance of the <see cref="IBatisNet.DataAccess.Exceptions.DataAccessException"/> 
		/// class with a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <remarks>
		/// This constructor initializes the Message property of the new instance 
		/// using the message parameter.
		/// </remarks>
		/// <param name="ex">
		/// The exception that is the cause of the current exception. 
		/// If the innerException parameter is not a null reference (Nothing in Visual Basic), 
		/// the current exception is raised in a catch block that handles the inner exception.
		/// </param>
		public DataAccessException(Exception ex) : base("iBatis.NET DataAccess framework caused an exception.", ex) {  }

		/// <summary>
		/// Initializes a new instance of the <see cref="IBatisNet.DataAccess.Exceptions.DataAccessException"/> 
		/// class with a specified error message.
		/// </summary>
		/// <remarks>
		/// This constructor initializes the Message property of the new instance to 
		/// the Message property of the passed in exception. 
		/// </remarks>
		/// <param name="message">The message that describes the error.</param>
		public DataAccessException( string message ) : base ( message ) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="IBatisNet.DataAccess.Exceptions.DataAccessException"/> 
		/// class with a specified error message and a reference to the inner exception 
		/// that is the cause of this exception.
		/// </summary>
		/// <remarks>
		/// An exception that is thrown as a direct result of a previous exception should include a reference to the previous 
		/// exception in the InnerException property. 
		/// The InnerException property returns the same value that is passed into the constructor, or a null reference 
		/// (Nothing in Visual Basic) if the InnerException property does not supply the inner exception value to the constructor.
		/// </remarks>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="inner">The exception that caused the error</param>
		public DataAccessException( string message, Exception inner ) : base ( message, inner ) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="IBatisNet.DataAccess.Exceptions.DataAccessException"/> 
		/// class with serialized data.
		/// </summary>
		/// <remarks>
		/// This constructor is called during deserialization to reconstitute the 
		/// exception object transmitted over a stream.
		/// </remarks>
		/// <param name="info">
		/// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized 
		/// object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual 
		/// information about the source or destination. 
		/// </param>
		protected DataAccessException(SerializationInfo info, StreamingContext context) : base (info, context) {}

	}
}
