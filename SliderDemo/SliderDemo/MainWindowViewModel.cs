using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SliderDemo
{
    public enum SettingEnum
    {
        One,
        Two
    }

    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private SettingEnum settingChecked;
        public SettingEnum SettingChecked
        {
            get { return settingChecked; }
            set
            {
                this.settingChecked = value;
                this.RaisePropertyChanged(nameof(SettingChecked));
            }
        }

        #region property of setting1
        private double colors1;
        private double gamma1;
        private double brightness1;

        public double Colors1
        {
            get { return this.colors1; }
            set
            {
                this.colors1 = value;
                this.RaisePropertyChanged(nameof(Colors1));
            }
        }

        public double Gamma1
        {
            get { return this.gamma1; }
            set
            {
                this.gamma1 = value;
                this.RaisePropertyChanged(nameof(Gamma1));
            }
        }

        public double Brightness1
        {
            get { return this.brightness1; }
            set
            {
                this.brightness1 = value;
                this.RaisePropertyChanged(nameof(Brightness1));
            }
        }
        #endregion

        #region property of setting2
        private double colors2;
        private double gamma2;
        private double brightness2;

        public double Colors2
        {
            get { return this.colors2; }
            set
            {
                this.colors2 = value;
                this.RaisePropertyChanged(nameof(Colors2));
            }
        }

        public double Gamma2
        {
            get { return this.gamma2; }
            set
            {
                this.gamma2 = value;
                this.RaisePropertyChanged(nameof(Gamma2));
            }
        }

        public double Brightness2
        {
            get { return this.brightness2; }
            set
            {
                this.brightness2 = value;
                this.RaisePropertyChanged(nameof(Brightness2));
            }
        }
        #endregion

        public MainWindowViewModel()
        {
            this.SettingChecked = SettingEnum.Two;
        }

        private void RaisePropertyChanged([CallerMemberName] string property = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}
