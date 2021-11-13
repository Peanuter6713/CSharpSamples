using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SliderDemo.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

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

        //public event RoutedPropertyChangedEventHandler<double> SliderValueChanged;


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
            if (!File.Exists(FileHelper.DefaultFilePath))
            {
                FileHelper.CreateFile(FileHelper.DefaultFilePath);
                DefaultInit();
            }
            else
            {
                ReadFile(FileHelper.DefaultFilePath);
            }

            //this.SettingChecked = SettingEnum.Two;
            //this.SliderValueChanged += Show;
        }
 
        private void DefaultInit()
        {
            this.SettingChecked = SettingEnum.One;
            this.Colors1 = 1;
            this.Gamma1 = 1;
            this.Brightness1 = 1;
            this.Colors2 = 2;
            this.Gamma2 = 2;
            this.Brightness2 = 2;
        }

        private void ReadFile(string file)
        {
            try
            {
                JObject jObject = null;
                using (StreamReader streamReader = File.OpenText(file))
                {
                    using(JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                    {
                        jObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    }
                }

                this.SettingChecked = (SettingEnum)(int)jObject[FileHelper.SettingType];
                this.Colors1 = (double)jObject[FileHelper.Color1];
                this.Gamma1 = (double)jObject[FileHelper.Gamma1];
                this.Brightness1 = (double)jObject[FileHelper.Brightness1];
                this.Colors2 = (double)jObject[FileHelper.Color2];
                this.Gamma2 = (double)jObject[FileHelper.Gamma2];
                this.Brightness2 = (double)jObject[FileHelper.Brightness2];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
