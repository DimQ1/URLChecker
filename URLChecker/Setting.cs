using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLChecker
{
    public class Setting
    {
        //сохранение настроек, чтобы продолжать с места в котором закончили
        public static void f_save_settings(string FileName, Hash base_Hash)
        {
            File.WriteAllText(FileName, JsonConvert.SerializeObject(base_Hash));
        }

        //считывание настроек, чтобы продолжать с места в котором закончили
        public static Hash f_load_settings(string FileName)
        {
           return JsonConvert.DeserializeObject<Hash>(File.ReadAllText(FileName));
        }
    }
}
