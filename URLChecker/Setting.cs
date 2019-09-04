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
        public static void f_save_settings(string FileName, string base_Hash, int i_0, int i_1, int i_3, int i_5, int i_7, int i_9)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileName, false))
            {
                file.WriteLine(base_Hash);
                file.WriteLine(i_0.ToString());
                file.WriteLine(i_1.ToString());
                file.WriteLine(i_3.ToString());
                file.WriteLine(i_5.ToString());
                file.WriteLine(i_7.ToString());
                file.WriteLine(i_9.ToString());
            }
        }

        //считывание настроек, чтобы продолжать с места в котором закончили
        public static void f_load_settings(string FileName, out string base_Hash, out int i_0, out int i_1, out int i_3, out int i_5, out int i_7, out int i_9)
        {
            using (StreamReader fs = new StreamReader(FileName))
            {
                base_Hash = fs.ReadLine();
                i_0 = Convert.ToInt32(fs.ReadLine());
                i_1 = Convert.ToInt32(fs.ReadLine());
                i_3 = Convert.ToInt32(fs.ReadLine());
                i_5 = Convert.ToInt32(fs.ReadLine());
                i_7 = Convert.ToInt32(fs.ReadLine());
                i_9 = Convert.ToInt32(fs.ReadLine());
            }
        }
    }
}
