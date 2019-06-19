using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Media;
using MvvmWpfApp.ViewModels;
using Microsoft.Win32;
using Microsoft.Maps.MapControl.WPF;

namespace MvvmWpfApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel { get; set; }
        private bool isAnalysisMode;

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel = new MainViewModel();
            DataContext = MainViewModel;

            ReportFormView.ReportFormVm = MainViewModel.NewReportFormVm;
            MapView.MapVm = MainViewModel.MapVm;
            Title = "Control Panel";
            kmeansTB.Text = "0";
            isAnalysisMode = false;
        }


        private void SelectedTabChange(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            GridCursor.Margin = new Thickness((150 * index), 0, 0, 0);
            if (index == 0)
            {

                isAnalysisMode = false;
                if (isContact)
                    player.Stop();
                #region toShow
                ReportFormView.Visibility = Visibility.Visible;
                MapView.Visibility = Visibility.Visible;
                kmeansButton.Visibility = Visibility.Visible;
                kmeansTB.Visibility = Visibility.Visible;
                #endregion

                #region toHide
                goToMap_button.Visibility = Visibility.Collapsed;
                AnalysisText.Visibility = Visibility.Collapsed;
                contactsText.Visibility = Visibility.Collapsed;
                emergencyImage.Visibility = Visibility.Collapsed;
                GalleryList.Visibility = Visibility.Collapsed;
                addPhoto_button.Visibility = Visibility.Collapsed;
                AnalysisResult_TB.Visibility = Visibility.Collapsed;
                AnalysisResult_card.Visibility = Visibility.Collapsed;
                goodGif.Visibility = Visibility.Collapsed;
                notGoodGif.Visibility = Visibility.Collapsed;
                textBlockChangeRange.Visibility = Visibility.Collapsed;
                textBoxNewRange.Visibility = Visibility.Collapsed;
                callButton.Visibility = Visibility.Collapsed;
                #endregion

            }
            else if (index == 1)
            {

                isAnalysisMode = false;
                if (isContact)
                    player.Stop();
                #region toShow
                GalleryList.Visibility = Visibility.Visible;
                addPhoto_button.Visibility = Visibility.Visible;
                #endregion

                #region toHide
                goToMap_button.Visibility = Visibility.Collapsed;
                AnalysisText.Visibility = Visibility.Collapsed;
                contactsText.Visibility = Visibility.Collapsed;
                emergencyImage.Visibility = Visibility.Collapsed;
                kmeansTB.Visibility = Visibility.Collapsed;
                kmeansButton.Visibility = Visibility.Collapsed;
                ReportFormView.Visibility = Visibility.Collapsed;
                MapView.Visibility = Visibility.Collapsed;
                AnalysisResult_TB.Visibility = Visibility.Collapsed;
                AnalysisResult_card.Visibility = Visibility.Collapsed;
                goodGif.Visibility = Visibility.Collapsed;
                notGoodGif.Visibility = Visibility.Collapsed;
                textBlockChangeRange.Visibility = Visibility.Collapsed;
                textBoxNewRange.Visibility = Visibility.Collapsed;
                callButton.Visibility = Visibility.Collapsed;


                #endregion
            }
            else if (index == 2)
            {
                isAnalysisMode = true;
                if (isContact)
                    player.Stop();
                #region toShow
                goToMap_button.Visibility = Visibility.Visible;
                AnalysisText.Visibility = Visibility.Visible;
                AnalysisResult_TB.Visibility = Visibility.Visible;


                #endregion

                #region toHide
                contactsText.Visibility = Visibility.Collapsed;
                emergencyImage.Visibility = Visibility.Collapsed;
                kmeansTB.Visibility = Visibility.Collapsed;
                kmeansButton.Visibility = Visibility.Collapsed;
                ReportFormView.Visibility = Visibility.Collapsed;
                MapView.Visibility = Visibility.Collapsed;
                GalleryList.Visibility = Visibility.Collapsed;
                addPhoto_button.Visibility = Visibility.Collapsed;
                goodGif.Visibility = Visibility.Collapsed;
                notGoodGif.Visibility = Visibility.Collapsed;
                callButton.Visibility = Visibility.Collapsed;

                #endregion
            }
            else if (index == 3)
            {
                isAnalysisMode = false;

                #region toShow
                contactsText.Visibility = Visibility.Visible;
                emergencyImage.Visibility = Visibility.Visible;
                callButton.Visibility = Visibility.Visible;

                #endregion

                #region toHide
                goToMap_button.Visibility = Visibility.Collapsed;
                AnalysisText.Visibility = Visibility.Collapsed;
                kmeansTB.Visibility = Visibility.Collapsed;
                kmeansButton.Visibility = Visibility.Collapsed;
                ReportFormView.Visibility = Visibility.Collapsed;
                MapView.Visibility = Visibility.Collapsed;
                GalleryList.Visibility = Visibility.Collapsed;
                addPhoto_button.Visibility = Visibility.Collapsed;
                AnalysisResult_TB.Visibility = Visibility.Collapsed;
                AnalysisResult_card.Visibility = Visibility.Collapsed;
                goodGif.Visibility = Visibility.Collapsed;
                notGoodGif.Visibility = Visibility.Collapsed;
                textBlockChangeRange.Visibility = Visibility.Collapsed;
                textBoxNewRange.Visibility = Visibility.Collapsed;
                #endregion
            }

        }


        private void KmeansButton_OnClick(object sender, EventArgs e)
        {
            MainViewModel.ActivateKmeans(MainViewModel.MapVm);
        }

        private void AddPhoto_OnClick(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png"
            };

            if (op.ShowDialog() == true)
            {
                Image image = new Image
                {
                    Source = (new ImageSourceConverter()).ConvertFromString((new Uri(op.FileName)).ToString()) as ImageSource
                };
                GalleryList.Items.Add(image);
            }
        }


        private string leftMouseClick_Text = "", rightMouseClick_Text = "";
        private Location leftLoc = null, rightLoc = null;

        private void OnMapLeftClick(object sender, MouseButtonEventArgs e)
        {
            if (!isAnalysisMode)
                return;

            Point mousePosition = e.GetPosition((UIElement)sender);
            Location pinLocation = MapView.BingMap.ViewportPointToLocation(mousePosition);
            leftLoc = pinLocation;
            leftMouseClick_Text = "Point 1: (";
            leftMouseClick_Text += pinLocation.Latitude.ToString();
            leftMouseClick_Text += " , ";
            leftMouseClick_Text += pinLocation.Longitude.ToString();
            leftMouseClick_Text += ")\n";

            CalcAnalysisResult();
        }

        private void OnMapRightClick(object sender, MouseButtonEventArgs e)
        {
            if (!isAnalysisMode)
                return;

            Point mousePosition = e.GetPosition((UIElement)sender);
            Location pinLocation = MapView.BingMap.ViewportPointToLocation(mousePosition);
            rightLoc = pinLocation;
            rightMouseClick_Text = "Point 2: (";
            rightMouseClick_Text += pinLocation.Latitude.ToString();
            rightMouseClick_Text += " , ";
            rightMouseClick_Text += pinLocation.Longitude.ToString();
            rightMouseClick_Text += ")\n";

            CalcAnalysisResult();
        }

        double distance = 0;
        private void CalcAnalysisResult()
        {
            if (leftLoc == null || rightLoc == null || !isAnalysisMode)
                return;

            double lon1 = leftLoc.Longitude, lat1 = leftLoc.Latitude;
            double lon2 = rightLoc.Longitude, lat2 = rightLoc.Latitude;
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);
            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            distance = Math.Round(angle * RADIUS, 2);
            AnalysisResult_TB.Text = leftMouseClick_Text + rightMouseClick_Text;
            AnalysisResult_TB.Text += "\nThe distance is: " + distance + " Km";

            UpdateGifstatus();


            textBlockChangeRange.Visibility = Visibility.Visible;
            textBoxNewRange.Visibility = Visibility.Visible;
            goToMap_button.Visibility = Visibility.Visible;
            AnalysisText.Visibility = Visibility.Visible;
            MapView.Visibility = Visibility.Collapsed;
            AnalysisResult_card.Visibility = Visibility.Visible;

            leftLoc = null;
            rightLoc = null;
        }

        const double RADIUS = 6378.16; // radius of earth

        private void PowerOffClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void OnRangeChanged(object sender, TextChangedEventArgs e)
        {
            if (!isAnalysisMode)
                return;

            UpdateGifstatus();
        }

        private void UpdateGifstatus()
        {
            string range = textBoxNewRange.Text;

            int num;
            if (range == "")
                return;
            num = int.Parse(range);

            if (distance < num)
            {
                goodGif.Visibility = Visibility.Visible;
                notGoodGif.Visibility = Visibility.Collapsed;
            }
            else
            {
                goodGif.Visibility = Visibility.Collapsed;
                notGoodGif.Visibility = Visibility.Visible;
            }
        }
        SoundPlayer player;
        bool isContact = false;
        private void callClick(object sender, RoutedEventArgs e)
        {
            isContact = true;
            player = new SoundPlayer("C:/Users/Ariel Eshel/אריאל/קורסים/סמסטר א/הנדסת מערכות חלונות/להגשה/windows-system-project-mvvm/MvvmWpfApp/images/siren sound.wav");
            player.PlayLooping();
        }



        private void GoToMap_button_Click(object sender, RoutedEventArgs e)
        {
            goToMap_button.Visibility = Visibility.Collapsed;
            AnalysisText.Visibility = Visibility.Collapsed;
            AnalysisResult_card.Visibility = Visibility.Collapsed;
            MapView.Visibility = Visibility.Visible;
            goodGif.Visibility = Visibility.Collapsed;
            notGoodGif.Visibility = Visibility.Collapsed;
            textBlockChangeRange.Visibility = Visibility.Collapsed;
            textBoxNewRange.Visibility = Visibility.Collapsed;
        }

        public static double Radians(double x)
        {
            return x * Math.PI / 180;
        }

    }
}