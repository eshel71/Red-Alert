using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    internal class DalImp : IDal
    {
        #region Event methods

        /// <summary>
        /// add new event to the table
        /// </summary>
        /// <param name="_event"> the new event </param>
        /// <exception>throw exception if the id already exist</exception>
        public async void AddEvent(Event _event)
        {
            if (_event.Id != 0 && GetEvent(_event.Id) != null)
            {
                throw new Exception("the event already exist");
            }
            using (var db = new ProjectContext())
            {
                db.Events.Add(_event);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// remove event by event id
        /// if not found do nothing
        /// </summary>
        /// <param name="id">the event id to remove</param>
        public void RemoveEvent(int id)
        {
            var eventToRemove = GetEvent(id);
            if (eventToRemove == null) return;
            using (var db = new ProjectContext())
            {
                db.Events.Remove(eventToRemove);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// update the event by find the old one with the id
        /// and replace with the new one
        /// </summary>
        /// <param name="_event">the new event to replace</param>
        /// <exception>throw exception if the event id to update not found</exception>
        public void UpdateEvent(Event _event)
        {
            if (GetEvent(_event.Id) == null)
                throw new Exception("the event to update not found");
            using (var db = new ProjectContext())
            {
                db.Entry(_event);
                db.Events.AddOrUpdate(_event);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// get the all events or events by conditions
        /// </summary>
        /// <param name="predicate">the condition predicat</param>
        /// <returns>list of the relevant events</returns>
        public List<Event> GetEvents(Predicate<Event> predicate = null)
        {
            List<Event> events;
            using (var db = new ProjectContext())
            {
                if (predicate != null)
                {
                    events = (from _event in db.Events
                              where predicate(_event)
                              select _event).ToList();
                }
                else
                {
                    events = (from _event in db.Events
                              select _event).ToList();
                }
            }
            return events;
        }

        /// <summary>
        /// get the all events or events by conditions asynchronously
        /// </summary>
        /// <param name="predicate">the condition predicat</param>
        /// <returns>list of the relevant events</returns>
        public async Task<List<Event>> GetEventsAsync(Predicate<Event> predicate = null)
        {
            List<Event> events;
            using (var db = new ProjectContext())
            {
                events = await db.Events.ToListAsync();
                if (predicate != null)
                    events = events.Where((@event => predicate(@event))).ToList();
            }
            return events;
        }

        /// <summary>
        /// get event by id
        /// </summary>
        /// <param name="id">the event id</param>
        /// <returns>the event</returns>
        public Event GetEvent(int? id)
        {
            Event _event;
            using (var db = new ProjectContext())
            {
                _event = db.Events.SingleOrDefault(e => e.Id == id);
            }
            return _event;
        }

        #endregion

        #region Report methods

        /// <summary>
        /// add new report to the table
        /// </summary>
        /// <param name="report"> the new event </param>
        /// <exception>throw exception if the id already exist</exception>
        public async Task<Report> AddReport(Report report)
        {
            if (report.Id != null && GetReport(report.Id) != null)
            {
                throw new Exception("the report already exist");
            }
            try
            {
                Report resReport = new Report();
                using (var db = new ProjectContext())
                {
                    resReport = db.Reports.Add(report);
                    if (report.Event.Id != 0)
                    {
                        resReport.Event.Reports.Add(resReport);
                        db.Events.Attach(resReport.Event);
                    }
                    await db.SaveChangesAsync();
                }
                if (resReport.Id != null) return resReport;
                throw new Exception("id not updated wen the report saved");
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// remove report by event id
        /// if not found do nothing
        /// </summary>
        /// <param name="id">the report id to remove</param>
        public void RemoveReport(int? id)
        {
            var reportToRemove = GetReport(id);
            if (reportToRemove == null) return;
            using (var db = new ProjectContext())
            {
                db.Reports.Remove(reportToRemove);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// update the report by find the old one with the id
        /// and replace with the new one
        /// </summary>
        /// <param name="report">the new report to replace</param>
        /// <exception>throw exception if the report id to update not found</exception>
        public void UpdateReport(Report report)
        {
            if (GetReport(report.Id) == null)
                throw new Exception("the report to update not found");
            using (var db = new ProjectContext())
            {
                db.Entry(report);
                db.Reports.AddOrUpdate(report);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// get the all reports or reports by conditions
        /// </summary>
        /// <param name="predicate">the condition predicat</param>
        /// <returns>list of the relevant reports</returns>
        public List<Report> GetReports(Predicate<Report> predicate = null)
        {
            List<Report> reports;
            using (var db = new ProjectContext())
            {
                reports = db.Reports.Include(report => report.Event).ToList();
                if (predicate != null)
                    reports = reports.Where(r => predicate(r)).ToList();
            }
            return reports;
        }

        /// <summary>
        /// get the all reports or reports by conditions asynchronously
        /// </summary>
        /// <param name="predicate">the condition predicat</param>
        /// <returns>list of the relevant reports</returns> 
        public async Task<List<Report>> GetReportsAsync(Predicate<Report> predicate = null)
        {
            List<Report> reports;
            using (var db = new ProjectContext())
            {
                reports = await db.Reports.Include(report => report.Event).ToListAsync();
                if (predicate != null)
                    reports = reports.Where(r => predicate(r)).ToList();
            }
            return reports;
        }

        /// <summary>
        /// get report by id
        /// </summary>
        /// <param name="id">the report id</param>
        /// <returns>the report</returns>
        public Report GetReport(int? id)
        {
            Report report;
            using (var db = new ProjectContext())
            {
                report = db.Reports.SingleOrDefault(r => r.Id == id);
            }
            return report;
        }

        #endregion

        #region Explosions

        public async Task<List<Explosion>> GetExplosions(Predicate<Explosion> predicate = null)
        {
            List<Explosion> explosions;
            using (var db = new ProjectContext())
            {
                explosions = await db.Explosions.Include(report => report.Event).ToListAsync();
                if (predicate != null)
                    explosions = explosions.Where(r => predicate(r)).ToList();
            }
            return explosions;
        }

        public async Task<Explosion> GetExplosionByEventId(int? eventId)
        {
            Explosion explosion;
            using (var db = new ProjectContext())
            {
                explosion = await db.Explosions.SingleOrDefaultAsync(exp => exp.Event.Id == eventId);
            }
            return explosion;
        }

        public async Task<Explosion> AddExplosion(Explosion explosion)
        {
            if (explosion.Id != null && GetExplosions(exp => exp.Id == explosion.Id) != null)
            {
                throw new Exception("the explosion already exist");
            }
            try
            {
                using (var db = new ProjectContext())
                {
                    if (explosion.Event.Id != 0)
                    {
                        explosion.Event.Explosions = null;
                        explosion.Event.Reports = null;
                        db.Explosions.Add(explosion);
                        db.Events.Attach(explosion.Event);
                    }
                    else
                    {
                        throw new Exception("explosion has no event");
                    }
                    await db.SaveChangesAsync(); // waits for "SaveChangesAsync" to be completed
                }
                if (explosion.Id != null) return explosion;
                throw new Exception("id not updated wen the explosion saved");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + $"in Dalimp AddExplosion with the explosion: {explosion}");
            }
        }

        public async void UpdateExplosion(Explosion explosion)
        {
            var findExplosion = await GetExplosions(exp => exp.Id == explosion.Id);
            if (findExplosion == null || findExplosion.Count == 0)
                throw new Exception("the explosion to update not found");
            using (var db = new ProjectContext())
            {
                db.Entry(explosion);
                db.Explosions.AddOrUpdate(explosion);
                await db.SaveChangesAsync();
            }
        }

        public async void RemoveExplosion(int? explosionId)
        {
            using (var db = new ProjectContext())
            {
                var explosionToRemove = db.Explosions.Where(exp => exp.Id == explosionId).ToList();
                if (explosionToRemove.Count == 0) return;
                db.Explosions.Remove(explosionToRemove.First());
                await db.SaveChangesAsync();
            }
        }

        #endregion
    }
}