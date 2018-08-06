//
//  CalendarBuilder.cs
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
using System.Linq;
using Int.Core.Controller.Calendar.Abstration;
using Int.Core.Extensions;

namespace Int.Core.Controller.Calendar
{
    public class CalendarBuilder : Builder
    {
        private DateTime _firstDayCurrent;

        private DateTime _firstDayNext;

        private DateTime _firstDayPrev;
        private int _firstIndexDay;

        private int _firstIndexDayCurrentMonth;
        private int _groupItem;
        private DateTime _lastDayCurrent;
        private DateTime _lastDayNext;
        private DateTime _lastDayPrev;

        private IList<DateTime> _listDateTimeCalendar;

        public IList<DateTime> ListDateTimeCalendar
        {
            get { return _listDateTimeCalendar; }
            set { _listDateTimeCalendar = value?.Select(x => x.Date).Distinct().ToList(); }
        }

        public event EventHandler<HandlerCalendarChangeDateArgs> ChangeDateRaise;
        public event EventHandler<int> OnYearChanging;

        public ICalendar GetCalendar() => Calendar;

        public void AddMonth(int month)
        {
            var yearBeforeAdd = Calendar.CurrentMonth.Year;
            Calendar.CurrentMonth = Calendar.CurrentMonth.AddMonths(month);
            if (yearBeforeAdd != Calendar.CurrentMonth.Year)
                OnYearChanging?.Invoke(this, Calendar.CurrentMonth.Year);
            CreateCalendar3Month();
        }

        public void AddYear(int year)
        {
            Calendar.CurrentMonth = Calendar.CurrentMonth.AddYears(year);
            if (year != 0)
                OnYearChanging?.Invoke(this, Calendar.CurrentMonth.Year);
            CreateCalendar3Month();
        }

        public void AddDate(DateTime dateModification)
        {
            var yearBeforeAdd = Calendar.CurrentMonth.Year;
            Calendar.CurrentMonth = dateModification;
            if (yearBeforeAdd != Calendar.CurrentMonth.Year)
                OnYearChanging?.Invoke(this, Calendar.CurrentMonth.Year);
            CreateCalendar3Month();
        }

        private void CreateCalendar3Month()
        {
            _firstDayCurrent = Calendar.CurrentMonth.FirstDayOfMonth();
            _firstIndexDayCurrentMonth = (int)_firstDayCurrent.DayOfWeek;

            _firstIndexDay = _firstIndexDayCurrentMonth;

            _lastDayCurrent = Calendar.CurrentMonth.LastDayOfMonth();

            _firstDayPrev = Calendar.PreviousMonth.FirstDayOfMonth();
            _lastDayPrev = Calendar.PreviousMonth.LastDayOfMonth();

            _firstDayNext = Calendar.NextMonth.FirstDayOfMonth();
            _lastDayNext = Calendar.NextMonth.LastDayOfMonth();

            ClearCollection();
            CollectCurrentMonth();

            ChangeDateRaise?.Invoke(this, new HandlerCalendarChangeDateArgs(Calendar.CurrentMonth));
        }


        private void CollectCurrentMonth()
        {
            _groupItem = 0;

            for (DateTime[] day = { _firstDayCurrent }; day[0] <= _lastDayCurrent; day[0] = day[0].AddDays(1))
            {
                var selected = _listDateTimeCalendar?.ToList().Exists(x => x.Date == day[0].Date) ?? false;

                Calendar.CurrentMonthDay.Add(ModificationOrdin(new DateInfoCell(), day[0], _groupItem, selected));

                Counter(day[0]);
            }

            if (Calendar.CurrentMonthDay.Count(x => x.LineGroupGrid == 0) != 7)
                CollectPrevMonth();

            CollectNextMonth();
        }

        private DateInfoCell ModificationOrdin(DateInfoCell model, DateTime day, int line, bool selected)
        {
            model.Day = day.Day;
            model.Month = day.Month;
            model.Year = day.Year;
            model.LineGroupGrid = line;
            model.Selected = selected;

            if (Calendar.FirstDay)
                switch ((int)day.DayOfWeek)
                {
                    case 1:
                        model.NrOfWeek = 0;
                        break;
                    case 2:
                        model.NrOfWeek = 1;
                        break;
                    case 3:
                        model.NrOfWeek = 2;
                        break;
                    case 4:
                        model.NrOfWeek = 3;
                        break;
                    case 5:
                        model.NrOfWeek = 4;
                        break;
                    case 6:
                        model.NrOfWeek = 5;
                        break;
                    case 0:
                        model.NrOfWeek = 6;
                        break;
                }
            else
                model.NrOfWeek = (int)day.DayOfWeek;

            return model;
        }

        private void CollectPrevMonth()
        {
            if (Calendar.FirstDay)
                _firstIndexDayCurrentMonth = _firstIndexDay == 0 ? 6 : _firstIndexDay - 1;
            else
                _firstIndexDayCurrentMonth = _firstIndexDay == 0 ? 6 : _firstIndexDay;


            for (DateTime[] day = { _lastDayPrev }; day[0] >= _firstDayPrev; day[0] = day[0].AddDays(-1))
            {
                var selected = _listDateTimeCalendar?.ToList().Exists(x => x.Date == day[0].Date) ?? false;

                if (_firstIndexDayCurrentMonth > 0)
                    Calendar.PreviousMonthDay.Add(ModificationOrdin(new DateInfoCell(), day[0], 0, selected));

                _firstIndexDayCurrentMonth -= 1;
            }
        }

        private void CollectNextMonth()
        {
            var completeArray = 42 - (Calendar.CurrentMonthDay.Count + Calendar.PreviousMonthDay.Count);

            for (DateTime[] day = { _firstDayNext }; day[0] <= _lastDayNext; day[0] = day[0].AddDays(1))
            {
                if (0 > completeArray) continue;
                var selected = _listDateTimeCalendar?.ToList().Exists(x => x.Date == day[0].Date) ?? false;

                Calendar.NextMonthDay.Add(ModificationOrdin(new DateInfoCell(), day[0], _groupItem, selected));

                Counter(day[0]);
            }
        }

        private void Counter(DateTime day)
        {
            if (Calendar.FirstDay != true)
            {
                if ((int)day.DayOfWeek != 6) return;
                _groupItem++;
            }
            else if (day.DayOfWeek != 0)
            {
                return;
            }
            _groupItem++;
        }

        private void ClearCollection()
        {
            Calendar.PreviousMonthDay.Clear();
            Calendar.CurrentMonthDay.Clear();
            Calendar.NextMonthDay.Clear();
        }

        public override void BuildCalendar(CalendarBase calendars)
        {
            Calendar = calendars;
            CreateCalendar3Month();
        }
    }
}