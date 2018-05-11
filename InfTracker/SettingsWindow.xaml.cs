using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InfTracker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        #region old
        private Color oldbg;
        public Color oldBG
        {
            get { return oldbg; }
            set
            {
                oldbg = value;
                OnPropertyChanged();
                OnPropertyChanged("oldBGBrush");
            }
        }
        public SolidColorBrush oldBGBrush
        {
            get { return new SolidColorBrush(oldBG); }
        }
        private Color oldfg;
        public Color oldFG
        {
            get { return oldfg; }
            set
            {
                oldfg = value;
                OnPropertyChanged();
                OnPropertyChanged("oldFGBrush");
            }
        }
        public SolidColorBrush oldFGBrush
        {
            get { return new SolidColorBrush(oldFG); }
        }
        #endregion
        #region ok
        private Color okbg;
        public Color okBG
        {
            get { return okbg; }
            set
            {
                okbg = value;
                OnPropertyChanged();
                OnPropertyChanged("okBGBrush");
            }
        }
        public SolidColorBrush okBGBrush
        {
            get { return new SolidColorBrush(okbg); }
        }
        private Color okfg;
        public Color okFG
        {
            get { return okfg; }
            set
            {
                okfg = value;
                OnPropertyChanged();
                OnPropertyChanged("okFGBrush");
            }
        }
        public SolidColorBrush okFGBrush
        {
            get { return new SolidColorBrush(okfg); }
        }
        #endregion
        #region reallybad
        private Color reallybadbg;
        public Color reallybadBG
        {
            get { return reallybadbg; }
            set
            {
                reallybadbg = value;
                OnPropertyChanged();
                OnPropertyChanged("reallybadBGBrush");
            }
        }
        public SolidColorBrush reallybadBGBrush
        {
            get { return new SolidColorBrush(reallybadbg); }
        }
        private Color reallybadfg;
        public Color reallybadFG
        {
            get { return reallybadfg; }
            set
            {
                reallybadfg = value;
                OnPropertyChanged();
                OnPropertyChanged("reallybadFGBrush");
            }
        }
        public SolidColorBrush reallybadFGBrush
        {
            get { return new SolidColorBrush(reallybadfg); }
        }
        #endregion
        #region bad
        private Color badbg;
        public Color badBG
        {
            get { return badbg; }
            set
            {
                badbg = value;
                OnPropertyChanged();
                OnPropertyChanged("badBGBrush");
            }
        }
        public SolidColorBrush badBGBrush
        {
            get { return new SolidColorBrush(badbg); }
        }
        private Color badfg;
        public Color badFG
        {
            get { return badfg; }
            set
            {
                badfg = value;
                OnPropertyChanged();
                OnPropertyChanged("badFGBrush");
            }
        }
        public SolidColorBrush badFGBrush
        {
            get { return new SolidColorBrush(badfg); }
        }
        #endregion


        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;

            //Init defaults
            var set = Properties.Settings.Default;

            oldBG = set.oldBackgroundColor;
            oldFG = set.oldForergoundColor;
            okBG = set.okBackgroundColor;
            okFG = set.okForegroundColor;
            reallybadBG = set.reallyBadBackgroundColor;
            reallybadFG = set.reallyBadForegroundColor;
            badBG = set.badBackgroundColor;
            badFG = set.badForegroundColor;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var set = Properties.Settings.Default;
            oldBG = set.oldBackgroundColor;
            oldFG = set.oldForergoundColor;
            okBG = set.okBackgroundColor;
            okFG = set.okForegroundColor;
            reallybadBG = set.reallyBadBackgroundColor;
            reallybadFG = set.reallyBadForegroundColor;
            badBG = set.badBackgroundColor;
            badFG = set.badForegroundColor;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var set = Properties.Settings.Default;
            set.oldBackgroundColor = oldBG;
            set.oldForergoundColor = oldFG;
            set.okBackgroundColor = okBG;
            set.okForegroundColor = okFG;
            set.reallyBadBackgroundColor = reallybadBG;
            set.reallyBadForegroundColor = reallybadFG;
            set.badBackgroundColor = badBG;
            set.badForegroundColor = badFG;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btnResetDefaults_Click(object sender, RoutedEventArgs e)
        {
            var set = Properties.Settings.Default;
            set.oldBackgroundColor = Colors.Silver;
            set.oldForergoundColor = Colors.White;
            set.okBackgroundColor = Colors.PaleGreen;
            set.okForegroundColor = Colors.Black;
            set.reallyBadBackgroundColor = Colors.Brown;
            set.reallyBadForegroundColor = Colors.White;
            set.badBackgroundColor = Colors.LightPink;
            set.badForegroundColor = Colors.Black;
            Properties.Settings.Default.Save();
            btnReset_Click(null, null);
        }
    }
}
