using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MvvmWpfApp.ViewModels;

namespace MvvmWpfApp
{
    /// <summary>
    /// Interaction logic for MapUC.xaml
    /// </summary>
    public partial class MapView : UserControl
    {

        public MapView()
        {
            InitializeComponent();
            BingMap.Height = SystemParameters.PrimaryScreenHeight * 0.80;
            BingMap.Width = SystemParameters.PrimaryScreenWidth * 0.70;
        }

        public static readonly DependencyProperty MapVmProperty = DependencyProperty.Register(
            "MapVm", typeof(MapVM), typeof(MapView), new PropertyMetadata(default(MapVM)));


        public MapVM MapVm
        {
            get { return (MapVM)GetValue(MapVmProperty); }
            set
            {
                SetValue(MapVmProperty, value);
                value.PropertyChanged += PropertyChanged;
                DataContext = MapVm;
            }
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(() => DataContext = MapVm);
        }


    }
}
