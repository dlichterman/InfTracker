using FactionLib;
using InfTracker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<FactionSystemInf> lst = new ObservableCollection<FactionSystemInf>();

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            SetColors();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            utclbl.Content = DateTime.UtcNow.ToString();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            lst.Clear();
            btnGo.IsEnabled = false;
            GetFactionInfo gfi = new GetFactionInfo();
            var progressIndicator = new Progress<int>(ReportProgress);
            lst = new ObservableCollection<FactionSystemInf>(await gfi.GetFactionList(tbFactionName.Text, progressIndicator));
            lv.ItemsSource = lst;
            progbar.Value = 0;
            btnGo.IsEnabled = true;
        }

        void ReportProgress(int value)
        {
            //Update the UI to reflect the progress value that is passed back.
            progbar.Value = value;
        }

        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            //lv.ItemContainerStyle.Triggers.Clear();
            SettingsWindow w = new SettingsWindow();
            w.Owner = this;
            w.ShowDialog();
            SetColors();
        }

        private void SetColors()
        {
            var set = InfTracker.Properties.Settings.Default;

            this.Resources["okBG"] = new SolidColorBrush(set.okBackgroundColor);
            this.Resources["okFG"] = new SolidColorBrush(set.okForegroundColor);
            this.Resources["oldBG"] = new SolidColorBrush(set.oldBackgroundColor);
            this.Resources["oldFG"] = new SolidColorBrush(set.oldForergoundColor);
            this.Resources["badBG"] = new SolidColorBrush(set.badBackgroundColor);
            this.Resources["badFG"] = new SolidColorBrush(set.badForegroundColor);
            this.Resources["reallyBadBG"] = new SolidColorBrush(set.reallyBadBackgroundColor);
            this.Resources["reallyBadFG"] = new SolidColorBrush(set.reallyBadForegroundColor);
        }
    }

}
