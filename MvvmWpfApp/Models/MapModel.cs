using BE;
using BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MvvmWpfApp.Annotations;
using System.Collections.ObjectModel;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Media;

namespace MvvmWpfApp.Models
{
    public class MapModel : INotifyPropertyChanged
    {
        private readonly IBl _bl = new FactoryBl().GetInstance();

        private List<Event> _events = new List<Event>();
        public List<Event> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                OnPropertyChanged();
            }
        }

        public MapModel()
        {
            GetEvents();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += CheckNewEvents;
            worker.RunWorkerAsync();
        }

        private void CheckNewEvents(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (true)
            {
                GetEvents();
                Thread.Sleep(5000);
            }
        }

        public async void GetEvents()
        {
            if (Events.Count == 0)
            {
                Events = _bl.GetEvents();
            }
            else
            {
                var allEvents = await _bl.GetEventsAsync();
                Events.AddRange(allEvents.Where(e => !Events.Exists(_e => _e.Id == e.Id)));
                OnPropertyChanged("event in mapModel");
            }
        }

        public async Task<IEnumerable<Report>> GetReports(int eventId)
        {
            return await _bl.GetReportsAsync(r => r.Event.Id == eventId);
        }

        public ObservableCollection<Pushpin> RunKmeans(ObservableCollection<Pushpin> locationList, int k)
        {
            double[][] observations = new double[locationList.Count()][];

            int i = 0;
            foreach (var pushPin in locationList)
            {
                observations[i++] = new double[] { pushPin.Location.Latitude, pushPin.Location.Longitude };
            }

            List<object[]> kmeansResult = _bl.RunKmeans(observations, k);


            // gathering the new points
            ObservableCollection<Pushpin> result = new ObservableCollection<Pushpin>();
            foreach (var obj in kmeansResult)
            {
                double latitude = Convert.ToDouble(obj[0]);
                double longitude = Convert.ToDouble(obj[1]);
                string name = Convert.ToString(obj[2]);
                int centerNumber = Convert.ToInt32(obj[3]);

                result.Add(new Pushpin()
                {
                    Location = new Location(latitude, longitude),
                    Content = name,
                    Background = new SolidColorBrush(GetClusterColor(centerNumber))
                });
            }

            return result;

        }

        // set the color of a pin by centerNumber
        public Color GetClusterColor(int centerNumber)
        {
            switch (centerNumber)
            {
                case 0:
                    return Colors.Red;

                case 1:
                    return Colors.Orange;

                case 2:
                    return Colors.Green;

                case 3:
                    return Colors.Blue;

                case 4:
                    return Colors.Black;

                default:
                    return Colors.Red;

            }
        }


        #region propertyChanged 

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
