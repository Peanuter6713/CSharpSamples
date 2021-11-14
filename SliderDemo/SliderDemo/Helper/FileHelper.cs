using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SliderDemo.Helper
{
    class FileHelper
    {
        public const string DefaultFilePath = "E:\\ColorAdjust.json";
        //public static string DefaultFilePath
        //{
        //    get
        //    {
        //        var filePath = ConfigurationManager.AppSettings["DefaultFilePath"];
        //        return filePath ?? "E:\\ColorAdjust.json";
        //    }
        //}

        public const string SettingType = "SettingType";
        public const string Color1 = "Color1";
        public const string Gamma1 = "Gamma1";
        public const string Brightness1 = "Brightness1";
        public const string Color2 = "Color2";
        public const string Gamma2 = "Gamma2";
        public const string Brightness2 = "Brightness2";

        public static void CreateFile(string file)
        {

            try
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(SettingType, "1");
                keyValues.Add(Color1, "1");
                keyValues.Add(Gamma1, "1");
                keyValues.Add(Brightness1, "1");
                keyValues.Add(Color2, "2");
                keyValues.Add(Gamma2, "2");
                keyValues.Add(Brightness2, "2");

                string jsonContent = JsonConvert.SerializeObject(keyValues);

                using (FileStream fileStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter streamWriter = new(fileStream, Encoding.UTF8))
                    {
                        streamWriter.Write(jsonContent);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    }
}
