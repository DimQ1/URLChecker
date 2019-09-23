using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace URLChecker
{
    public class HashSettings
    {
        public int i1_start;
        public int i3_start;
        public int i5_start;
        public int i7_start;
        public int i9_start;

        public string base_Hash;
        public int i0_start;
        public int i0_delta;
        public int i0_end;


        public HashSettings()
        {
            i1_start = 0;
            i3_start = 0;
            i5_start = 0;
            i7_start = 0;
            i9_start = 0;

            base_Hash = "";
            i0_start = 0;
            i0_delta = 0;
            i0_end = 0;
        }


        public void initByInputHash(string base_Hash_, int i0_delta_, int i0_start_)
        {
            base_Hash = base_Hash_;

            i0_start = i0_start_ - i0_delta_;
            i0_end = i0_start_ + i0_delta_;
            i0_delta = i0_delta_;

            i1_start = 0;
            i3_start = 0;
            i5_start = 0;
            i7_start = 0;
            i9_start = 0;
        }
        

        public void initByLoadSettingsFromFile(string filePath_settings)
        {
            HashSettings hs = JsonConvert.DeserializeObject<HashSettings>(File.ReadAllText(filePath_settings));

            base_Hash = hs.base_Hash;

            i0_start = hs.i0_start;
            i0_end = hs.i0_end;
            i0_delta = hs.i0_delta;

            i1_start = hs.i1_start;
            i3_start = hs.i3_start;
            i5_start = hs.i5_start;
            i7_start = hs.i7_start;
            i9_start = hs.i9_start;

            /*
            using (StreamReader fs = new StreamReader(filePath_settings))
            {
                base_Hash = fs.ReadLine();

                i0_start = Convert.ToInt32(fs.ReadLine());
                i0_end = Convert.ToInt32(fs.ReadLine());
                i0_delta = Convert.ToInt32(fs.ReadLine());

                i1_start = Convert.ToInt32(fs.ReadLine());
                i3_start = Convert.ToInt32(fs.ReadLine());
                i5_start = Convert.ToInt32(fs.ReadLine());
                i7_start = Convert.ToInt32(fs.ReadLine());
                i9_start = Convert.ToInt32(fs.ReadLine());
            }
            */
        }


        public void SaveProgress(string filePath_settings)
        {
            /*
            using (StreamWriter file = new StreamWriter(filePath_settings, false))
            {
                file.WriteLine(base_Hash);

                file.WriteLine(i0_start.ToString());
                file.WriteLine(i0_end.ToString());
                file.WriteLine(i0_delta.ToString());

                file.WriteLine(i1_start.ToString());
                file.WriteLine(i3_start.ToString());
                file.WriteLine(i5_start.ToString());
                file.WriteLine(i7_start.ToString());
                file.WriteLine(i9_start.ToString());
            }
            */

            File.WriteAllText(filePath_settings, JsonConvert.SerializeObject(this));
        }



    }
}



