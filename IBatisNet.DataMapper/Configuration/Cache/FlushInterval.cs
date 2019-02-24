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

#region Imports

using System;
using System.Xml.Serialization;

#endregion

namespace IBatisNet.DataMapper.Configuration.Cache
{
    /// <summary>
    ///     Summary description for FlushInterval.
    /// </summary>
    [Serializable]
    [XmlRoot("flushInterval")]
    public class FlushInterval
    {
        #region Methods

        /// <summary>
        ///     Calcul the flush interval value in ticks
        /// </summary>
        public void Initialize()
        {
            if (_milliseconds != 0) Interval += (_milliseconds * TimeSpan.TicksPerMillisecond);
            if (_seconds != 0) Interval += (_seconds * TimeSpan.TicksPerSecond);
            if (_minutes != 0) Interval += (_minutes * TimeSpan.TicksPerMinute);
            if (_hours != 0) Interval += (_hours * TimeSpan.TicksPerHour);

            if (Interval == 0) Interval = CacheModel.NO_FLUSH_INTERVAL;
        }

        #endregion

        #region Fields 

        private int _hours;
        private int _minutes;
        private int _seconds;
        private int _milliseconds;

        #endregion

        #region Properties

        /// <summary>
        ///     Flush interval in hours
        /// </summary>
        [XmlAttribute("hours")]
        public int Hours
        {
            get => _hours;
            set => _hours = value;
        }


        /// <summary>
        ///     Flush interval in minutes
        /// </summary>
        [XmlAttribute("minutes")]
        public int Minutes
        {
            get => _minutes;
            set => _minutes = value;
        }


        /// <summary>
        ///     Flush interval in seconds
        /// </summary>
        [XmlAttribute("seconds")]
        public int Seconds
        {
            get => _seconds;
            set => _seconds = value;
        }


        /// <summary>
        ///     Flush interval in milliseconds
        /// </summary>
        [XmlAttribute("milliseconds")]
        public int Milliseconds
        {
            get => _milliseconds;
            set => _milliseconds = value;
        }


        /// <summary>
        ///     Get the flush interval value
        /// </summary>
        [XmlIgnore]
        public long Interval { get; private set; } = CacheModel.NO_FLUSH_INTERVAL;

        #endregion
    }
}