using System;
using System.Collections.Generic;
using System.Threading;

namespace Y.ASIS.Common.Manager
{
    public class TimerManager
    {
        private static TimerManager instance;

        public static TimerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimerManager();
                }
                return instance;
            }
        }

        private readonly Dictionary<string, Schedule> schedules;

        private TimerManager()
        {
            schedules = new Dictionary<string, Schedule>();
        }

        public void AddSchedule(Action action, TimeSpan timeSpan)
        {
            string name = Guid.NewGuid().ToString().Replace("-", "");
            AddSchedule(name, action, timeSpan);
        }

        public void AddSchedule(string name, Action action, TimeSpan timeSpan)
        {
            if (schedules.TryGetValue(name, out Schedule schedule)
                && schedule.Interval == (int)timeSpan.TotalMilliseconds)
            {
                schedule.Actions.Add(action);
            }
            schedules.Add(name, new Schedule(name, timeSpan, action));
        }

        public bool GetSchedule(string name, out Schedule schedule)
        {
            return schedules.TryGetValue(name, out schedule);
        }

        public void RemoveSchedule(string name)
        {
            if (schedules.TryGetValue(name, out Schedule schedule))
            {
                schedules.Remove(name);
                schedule.Timer.Dispose();
            }
        }
    }

    public class Schedule
    {
        public Schedule(string name, TimeSpan timeSpan, Action action)
        {
            Name = name;
            Interval = (int)timeSpan.TotalMilliseconds;
            Actions = new List<Action>()
            {
                action
            };
            Timer = new Timer(ExecuteActions, null, 0, Interval);
        }

        public string Name { get; set; }

        public Timer Timer { get; private set; }

        public int Interval { get; private set; }

        public List<Action> Actions { get; private set; }


        private void ExecuteActions(object _)
        {
            Actions.ForEach(i =>
            {
                try
                {
                    i.Invoke();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
    }
}
