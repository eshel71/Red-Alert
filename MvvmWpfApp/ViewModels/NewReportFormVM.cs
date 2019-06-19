
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BE;
using MvvmWpfApp.Annotations;
using MvvmWpfApp.Models;
using MvvmWpfApp.Utils;

namespace MvvmWpfApp.ViewModels
{
    public class NewReportFormVM : INotifyPropertyChanged
    {
        public NewReportFormVM()
        {
            FormModel = new NewReportFormModel();
            reportModel = FormModel.Report.Clone() as Report;
            AddReportCommand = new RelayCommand<NewReportFormModel>(formModel =>
            {
                if (reportModel.Name == "" || reportModel.Address == null || reportModel.NumOfExplosions == 0)
                    return;

                formModel.Report = reportModel.Clone() as Report;
                Report = new Report();
                formModel.AddReport();
            },
                //if have more condition to add report 
                report =>
                {
                    return report != null;
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public NewReportFormModel FormModel { get; set; }

        private RelayCommand<NewReportFormModel> _addReportCommand;
        public RelayCommand<NewReportFormModel> AddReportCommand
        {
            get { return _addReportCommand; }
            set
            {
                _addReportCommand = value;
                OnPropertyChanged();
            }
        }


        private Report reportModel;
        public Report Report
        {
            get { return reportModel; }
            set
            {
                OnPropertyChanged();
                reportModel = value;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
