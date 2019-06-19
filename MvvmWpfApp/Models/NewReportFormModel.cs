using System.Windows;
using BE;
using BL;

namespace MvvmWpfApp.Models
{
    public class NewReportFormModel
    {
        private readonly IBl _bl = new FactoryBl().GetInstance();

        public Report Report { get; set; } = new Report();

        public async void AddReport()
        {
            var res = await _bl.AddReport(Report);
            var message = res != null ?
                $"The Report: {res.Id}\nFrom: {res.Name}\nOn: {res.Time} Saved Successfully!" :
                "Something went wrong when trying to add report!";
            MessageBox.Show(message);
        }

    }
}
