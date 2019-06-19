using System.ComponentModel;
using MvvmWpfApp.Annotations;
using System.Runtime.CompilerServices;

namespace MvvmWpfApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            _mapVm = new MapVM();
            NewReportFormVm = new NewReportFormVM();
        }

        public static void ActivateKmeans(MapVM obj)
        {
            obj.OnKmeansActivate();
        }

        public int KmeansValue
        {
            get { return _mapVm.K; }
            set { _mapVm.K = value; OnPropertyChanged(nameof(KmeansValue)); }
        }

        #region view models

        private MapVM _mapVm;
        public MapVM MapVm
        {
            get { return _mapVm; }
            set
            {
                _mapVm = value;
                NotifyPropertyChanged();
            }
        }

        private NewReportFormVM _newReportFormVm;
        public NewReportFormVM NewReportFormVm
        {
            get { return _newReportFormVm; }
            set
            {
                _newReportFormVm = value;
                NotifyPropertyChanged();
            }
        }

        #endregion


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
