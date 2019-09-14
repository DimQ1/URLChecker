using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLChecker
{
    public class LocalHash
    {
        //блок массивов
        //1-й столбец формируется по алфавиту с маленьких, потом цифры, после снова алфавит большие буквы
        static string[] a_s0 = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                              "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
                              "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

        //2-й столбец зависимоть не найдена, повторяются цифры и буквы (нижний регистр a-f    !!!!!)
        static string[] a_s1 = { "a", "b", "c", "d", "e", "f",
                              "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"};

        //3-й столбец 61 символово повторяются, потом изменяются по алфавиту, потом регистру, потом цифры
        static string[] a_s2 = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                              "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
                              "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

        //4 - й столбец зависимоть не найдена, повторяются цифры(10символов) и буквы(нижний регистр a-f(6символов))
        static string[] a_s3 = { "a", "b", "c", "d", "e", "f",
                              "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"};

        //5 - й столбец, большая буква алфавита(изменяется через неопределённый период примерно(3781)(3789)  !!!!!
        // string[] a_s4 = ПОСТОЯННО    !!!!!   - может несколько соседних символов (3781)(3789) не такие большие числа;

        // 6 - й столбец зависимоть не найдена, повторяются цифры(в большей степени) и буквы(нижний регистр a - f)
        static string[] a_s5 = { "a", "b", "c", "d", "e", "f",
                              "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"};

        //7 - й столбец изменяется по алфавиту через не определенный период(период большой) - w ==> x
        // string[] a_s6 = ПОСТОЯННО    !!!!!;

        //8 - й столбец зависимоть не найдена, повторяются цифры и буквы(нижний регистр a - f)
        static string[] a_s7 = { "a", "b", "c", "d", "e", "f",
                              "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"};

        //9 - й столбец - n
        // string[] a_s8 = ПОСТОЯННО    !!!!!;

        //10 - й столбец зависимоть не найдена, повторяются цифры и буквы(нижний регистр a - f)
        static string[] a_s9 = { "a", "b", "c", "d", "e", "f",
                              "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"};

        public static async Task CheckUrlWithHash(string base_Hash, string[] arStr)
        {
            await Task.Run(async () =>
            {
                LocalBruteForce localBruteForce = new LocalBruteForce(1000);
                var urls = new Stack<string>();     //стэк для хранения урлов

                string s_Url = "";

                if (!string.IsNullOrEmpty(base_Hash) || (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/settings_Local.txt")))
                {

                    //если файл настроек не существует
                    if (!File.Exists(System.IO.Directory.GetCurrentDirectory() + "/settings_Local.txt"))
                    {
                        Setting.f_save_settings(System.IO.Directory.GetCurrentDirectory() + "/settings_Local.txt", base_Hash, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    }

                    using (StreamWriter sw = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + "/mutationString_Local.txt", false, System.Text.Encoding.Default))
                    {
                        int i0_start = 0;              //!!!!! начали сразу с "A"   нет ерунда
                        int i0_end = 0;
                        int i0_delta = 0;

                        int i2 = 0;     //!!!!! временно

                        int i1_start = 0;
                        int i2_start = 0;
                        int i3_start = 0;
                        int i5_start = 0;
                        int i7_start = 0;
                        int i9_start = 0;

                        Setting.f_load_settings(System.IO.Directory.GetCurrentDirectory() + "/settings_Local.txt", out base_Hash, out i0_start, out i0_end, out i0_delta, out i1_start, out i2_start, out i3_start, out i5_start, out i7_start, out i9_start);

                        // здесь постоянные символы на входе будут известны
                        string a_s2 = base_Hash.Substring(2, 1);          //не постоянное, но пока СЧИТАЕМ постоянным, потом для красоты 1 вниз и 1 вверх
                        string a_s4 = base_Hash.Substring(4, 1);
                        string a_s6 = base_Hash.Substring(6, 1);
                        string a_s8 = base_Hash.Substring(8, 1);

                        //для первого символа ищем базу
                        Int32 base_s0_index = 0;                        //номер в массиве базового символа для первой позиции
                        Int32 delta_s0 = arStr.Length / 2;              //половина диапазона который проверяем, для первой позиции
                        for (Int32 i = 0; i < a_s0.Length; i++) { if (a_s0[i] == base_Hash.Substring(0, 1)) { base_s0_index = i; continue; } }

                        
                        string s_mut = "";
                        for (int i0 = base_s0_index - delta_s0/*i0_start*/; i0 < base_s0_index + delta_s0/*a_s0.Length*/; i0++)                 //здесь перебирать не все, 7 вверх и 7 вниз
                        {
                            for (int i1 = i1_start; i1 < a_s1.Length; i1++)
                            {
                                //for (int i2 = i2_start; i2 < a_s2.Length; i1++)         //здесь перебирать не все, одну вверх и одну вниз
                                //{
                                for (int i3 = i3_start; i3 < a_s3.Length; i3++)
                                {
                                    for (int i5 = i5_start; i5 < a_s5.Length; i5++)
                                    {
                                        for (int i7 = i7_start; i7 < a_s7.Length; i7++)
                                        {
                                            for (int i9 = i9_start; i9 < a_s9.Length; i9++)
                                            {

                                                string a_si0 = "";
                                                if (i0 < base_s0_index)
                                                {
                                                    a_si0 = a_s0[i0 + 8];
                                                }
                                                if (i0 > base_s0_index)
                                                {
                                                    a_si0 = a_s0[base_s0_index - (i0 - base_s0_index)];
                                                }
                                                if (i0 == base_s0_index) { break; }
                                                                                               
                                                s_mut = a_si0/*a_s0[i0]*/ + a_s1[i1] + a_s2/*константа*/ + a_s3[i3] + a_s4/*константа*/ + a_s5[i5] + a_s6/*константа*/ + a_s7[i7] + a_s8/*константа*/ + a_s9[i9];

                                                sw.WriteLine(s_mut);
                                                sw.Flush();

                                                if (urls.Count == 0)
                                                {
                                                    s_Url = base_Hash;
                                                }            //это проверочный существующий url
                                                else
                                                {
                                                    s_Url = s_mut;
                                                }

                                                if (urls.Count < 1000)
                                                {
                                                    urls.Push(s_Url);
                                                }
                                                else
                                                {
                                                    await localBruteForce.StartBruteForce(urls, arStr);
                                                }



                                                //сохраняем в файл настроек прогресс
                                                Setting.f_save_settings(System.IO.Directory.GetCurrentDirectory() + "/settings_Local.txt", base_Hash, i0, i0_end, i0_delta, i1, i2, i3, i5, i7, i9);
                                            }
                                            i9_start = 0;
                                        }
                                        i7_start = 0;
                                    }
                                    i5_start = 0;
                                }
                                i3_start = 0;
                                //}
                                //i2_start = 0;
                            }
                            i1_start = 0;
                        }
                        i0_start = 0;
                    }
                }

            });

        }
    }
}
