using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using MvvmWpfApp.ViewModels;
using QuickType;

namespace MvvmWpfApp.Views
{
    /// <summary>
    /// Interaction logic for NewReportFormView.xaml
    /// </summary>
    public partial class NewReportFormView : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ReportFormVmProperty = DependencyProperty.Register(
            "ReportFormVm", typeof(NewReportFormVM), typeof(NewReportFormView), new PropertyMetadata(default(NewReportFormVM)));

        public NewReportFormVM ReportFormVm
        {
            get { return (NewReportFormVM)GetValue(ReportFormVmProperty); }
            set { SetValue(ReportFormVmProperty, value); }
        }

        public NewReportFormView()
        {
            InitializeComponent();
            InitForm();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ResetForm(object sender, EventArgs e)
        {
            InitForm();
        }

        private void InitForm()
        {
            ReportFormVm = new NewReportFormVM();
            DataContext = ReportFormVm;
            SaveButton.Command = ReportFormVm.AddReportCommand;
            SaveButton.CommandParameter = ReportFormVm.FormModel;
            SaveButton.IsEnabled = false;
            ReportFormVm.PropertyChanged += (sender, args) => InitForm();
        }

        private void SaveEnableCheck(object sender, RoutedEventArgs routedEventArgs)
        {
            int _;
            SaveButton.IsEnabled = AddressTextBox.SelectedLocation != null &&
                                   NameTextBox.Text != "" &&
                                   !(NumOfExplosionsTextBox.Text == "0" ||
                                   !int.TryParse(NumOfExplosionsTextBox.Text, out _));
        }

        private void AddressTextBox_OnSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is AutoCompleteBox) || e.AddedItems.Count == 0) return;
            (DataContext as NewReportFormVM).Report.Address = ((Result)e.AddedItems[0]).Title;
            (DataContext as NewReportFormVM).Report.Latitude = ((Result)e.AddedItems[0])?.Position?[0] ?? 0;
            (DataContext as NewReportFormVM).Report.Longitude = ((Result)e.AddedItems[0])?.Position?[1] ?? 0;
        }


    }
}