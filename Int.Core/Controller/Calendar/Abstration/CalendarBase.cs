//
//  CalendarBase.cs
//
//  Author:
//       Songurov Fiodor <f.songurov@software-dep.net>
//
//  Copyright (c) 2016 (c) BSS
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Collections.Generic;

namespace Int.Core.Controller.Calendar.Abstration
{
    public class HandlerCalendarChangeDateArgs
    {
        internal readonly DateTime Date;

        public HandlerCalendarChangeDateArgs(DateTime dateChange)
        {
            Date = dateChange;
        }
    }

    public class DateInfoCell
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public int LineGroupGrid { get; set; }
        public int NrOfWeek { get; set; }
        public bool Selected { get; set; }
    }

    public class CalendarBase : ICalendar
    {
        private DateTime _currentMonth;

        protected CalendarBase(DateTime date)
        {
            CurrentMonth = date;
        }

        public DayOfWeek NameDay { get; set; }

        public IList<DateInfoCell> CurrentMonthDay { get; set; } = new List<DateInfoCell>();
        public IList<DateInfoCell> NextMonthDay { get; set; } = new List<DateInfoCell>();
        public IList<DateInfoCell> PreviousMonthDay { get; set; } = new List<DateInfoCell>();

        public IList<int> OrdinDay => FirstDay
            ? new List<int>
            {
                1,
                2,
                3,
                4,
                5,
                6,
                0
            }
            : new List<int>
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6
            };

        public bool FirstDay { get; set; }

        public DateTime CurrentMonth
        {
            get { return _currentMonth; }
            set
            {
                _currentMonth = value;
                PreviousMonth = _currentMonth.AddMonths(-1);
                NextMonth = _currentMonth.AddMonths(1);
            }
        }

        public DateTime NextMonth { get; set; }
        public DateTime PreviousMonth { get; set; }
    }
}