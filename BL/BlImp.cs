using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    internal class BlImp : IBl
    {
        private IDal _dal = new FactoryDal().GetInstance();

        #region Events

        public void AddEvent(Event _event)
        {
            _dal.AddEvent(_event);
        }

        public void RemoveEvent(int id)
        {
            _dal.RemoveEvent(id);
        }

        public void UpdateEvent(Event _event)
        {
            _dal.UpdateEvent(_event);
        }

        public List<Event> GetEvents(Predicate<Event> predicate = null)
        {
            try
            {
                if (_dal.GetEvents(predicate).Count != 0)
                {
                    return _dal.GetEvents(predicate);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\nget event in blimp");
            }
            return new List<Event>();
        }

        public Task<List<Event>> GetEventsAsync(Predicate<Event> predicate = null)
        {
            return _dal.GetEventsAsync(predicate);
        }

        public Event GetEvent(int? id)
        {
            return _dal.GetEvent(id);
        }
        #endregion

        #region Reports

        public Task<Report> AddReport(Report report)
        {
            List<Event> events = (from e in GetEvents()
                                  where e.StartTime <= report.Time.AddMinutes(10) &&
                                  e.EndTime >= report.Time.AddMinutes(-10)
                                  select e).ToList();

            if (events.Count == 1)
            {
                if (report.Time < events[0].StartTime)
                {
                    events[0].StartTime = report.Time;
                    UpdateEvent(events[0]);
                }
                else if (report.Time > events[0].EndTime)
                {
                    events[0].EndTime = report.Time;
                    UpdateEvent(events[0]);
                }
                report.Event = events[0];
            }
            else if (events.Count > 1)
            {
                Event closestEvent = events[0]; // base case
                double tmpInterval, minInterval = Math.Abs((report.Time - closestEvent.StartTime).TotalMinutes);

                foreach (Event ev in events) // check closest start time
                {
                    tmpInterval = Math.Abs((report.Time - ev.StartTime).TotalMinutes);
                    if (tmpInterval < minInterval)
                    {
                        minInterval = tmpInterval;
                        closestEvent = ev;
                    }
                }

                foreach (Event ev in events) // check closest end time
                {
                    tmpInterval = Math.Abs((report.Time - ev.EndTime).TotalMinutes);
                    if (tmpInterval < minInterval)
                    {
                        minInterval = tmpInterval;
                        closestEvent = ev;
                    }
                }

                report.Event = closestEvent;
            }
            else
            {
                report.Event = new Event(report.Time) { StartTime = report.Time };
            }

            var res = _dal.AddReport(report);
            UpdateExplosions(report);

            return res;
        }


        private async void UpdateExplosions(Report newReport)
        {
            Event _event = newReport.Event;
            var explotions = await _dal.GetExplosions(e => e.Event.Id == _event.Id);

            //remove the last approximate explosions
            foreach (var explotion in explotions.ToArray())
            {
                _dal.RemoveExplosion(explotion.Id);
            }
            _event.Reports = await _dal.GetReportsAsync((report => report.Event.Id == _event.Id));
            _event.Reports.Add(newReport);
            int averageExplosions = (int)_event.Reports.Average(r => r.NumOfExplosions);
        }


        public void RemoveReport(int id)
        {
            _dal.RemoveReport(id);
        }

        public void UpdateReport(Report report)
        {
            _dal.UpdateReport(report);
        }

        public List<Report> GetReports(Predicate<Report> predicate = null)
        {
            return _dal.GetReports(predicate);
        }

        public Task<List<Report>> GetReportsAsync(Predicate<Report> predicate = null)
        {
            return _dal.GetReportsAsync(predicate);
        }

        public Report GetReport(int? id)
        {
            return _dal.GetReport(id);
        }
        #endregion

        #region Explosion

        public Task<Explosion> AddExplosion(Explosion explosion)
        {
            return _dal.AddExplosion(explosion);
        }

        public void RemoveExplosion(int id)
        {
            _dal.RemoveExplosion(id);
        }

        public void UpdateExplosion(Explosion explosion)
        {
            _dal.UpdateExplosion(explosion);
        }

        public Task<List<Explosion>> GetExplosions(Predicate<Explosion> predicate = null)
        {
            return _dal.GetExplosions(predicate);
        }

        public Explosion GetExplosion(int? id)
        {
            return _dal.GetExplosions(x => x.Id == id).Result.FirstOrDefault();
        }

        #endregion

        public List<object[]> RunKmeans(double[][] observations, int k)
        {
            return new KmeansAlgo(observations, k).Run();
        }

    }
}
