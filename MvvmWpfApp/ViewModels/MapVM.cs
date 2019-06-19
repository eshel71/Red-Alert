using MvvmWpfApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using BE;
using Microsoft.Maps.MapControl.WPF;
using MvvmWpfApp.Annotations;
using BL;

namespace MvvmWpfApp.ViewModels
{
    public class MapVM : INotifyPropertyChanged
    {

        private readonly IBl _bl = new FactoryBl().GetInstance();

        public MapVM()
        {


            MapModel = new MapModel();
            MapModel.PropertyChanged += (sender, args) =>
            {
                OnPropertyChanged("event in mapVm");
            };


            ReportList = new ObservableCollection<Report>();


            LocationList.CollectionChanged += (sender, args) =>
            {
                OnPropertyChanged(nameof(LocationList));
            };

            GetReports();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += CheckNewReports;
            worker.RunWorkerAsync();
        }



        public void OnKmeansActivate()
        {
            if (K_ == 0) // will restore the location list
            {
                // restore the location list
                kmeansLocationList = null;
                OnPropertyChanged(nameof(LocationList));
                return;
            }

            if (K_ < 0) // inappropriate value
                return;

            kmeansLocationList = null; // thus the algorithm will run on new locationList and not on the prev kmeansLocationList
            kmeansLocationList = MapModel.RunKmeans(LocationList, K_);
            OnPropertyChanged(nameof(LocationList)); // replace the list with the new one
        }

        public MapModel MapModel { get; set; }
        public ObservableCollection<Event> Events
        {
            get { return new ObservableCollection<Event>(MapModel.Events); }
            set { MapModel.Events = new List<Event>(value); }
        }


        public ObservableCollection<Report> ReportList { get; set; }
        public async void GetReports()
        {
            ReportList = new ObservableCollection<Report>(await _bl.GetReportsAsync());
            OnPropertyChanged(nameof(ReportList));
        }


        private ObservableCollection<Pushpin> kmeansLocationList; // for restoring after kmeans
        public ObservableCollection<Pushpin> LocationList
        {
            get
            {
                if (kmeansLocationList != null)
                {
                    return new ObservableCollection<Pushpin>(kmeansLocationList);
                }

                return new ObservableCollection<Pushpin>(
                  ReportList.Select(r => new Pushpin()
                  {
                      Location = new Location(r.Latitude, r.Longitude),
                  }));
            }
        }


        private int K_;
        public int K
        {
            get { return K_; }
            set { K_ = value; }
        }


        private void CheckNewReports(object sender, DoWorkEventArgs e)
        {
            int prevCount = -1;
            while (true)
            {
                if (ReportList != null && prevCount == -1) // only once
                    prevCount = ReportList.Count();

                GetReports();

                if (ReportList.Count() != prevCount) // for loading the new data
                {
                    prevCount = ReportList.Count();
                    kmeansLocationList = null;
                    OnPropertyChanged(nameof(LocationList));
                }
                Thread.Sleep(5000);
            }
        }



        #region property changed definitions

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
