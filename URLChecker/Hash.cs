﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLChecker
{
    public class Hash
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
        // string[] a_s2 = ПОСТОЯННО    !!!!!    тоже условно;

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

        public static async Task CheckUrlWithHash(string base_Hash)
        {
            await Task.Run(async () =>
            {
                HttpBruteForce httpBruteForce = new HttpBruteForce(1000);
                var urls = new Stack<string>();     //стэк для хранения урлов

                //string s_html = "";
                string s_Url = "";

                if (!string.IsNullOrEmpty(base_Hash) || (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/settings.txt")))
                {

                    //если файл настроек не существует
                    if (!File.Exists(System.IO.Directory.GetCurrentDirectory() + "/settings.txt"))
                    {
                        Setting.f_save_settings(System.IO.Directory.GetCurrentDirectory() + "/settings.txt", base_Hash, 0, 0, 0, 0, 0, 0);
                    }

                    using (StreamWriter sw = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + "/mutationString.txt", false, System.Text.Encoding.Default))
                    {
                        int i0_start = 0;              //!!!!! начали сразу с "A"   нет ерунда
                        int i1_start = 0;
                        int i3_start = 0;
                        int i5_start = 0;
                        int i7_start = 0;
                        int i9_start = 0;

                        Setting.f_load_settings(System.IO.Directory.GetCurrentDirectory() + "/settings.txt", out base_Hash, out i0_start, out i1_start, out i3_start, out i5_start, out i7_start, out i9_start);

                        // здесь постоянные символы на входе будут известны
                        string a_s2 = base_Hash.Substring(2, 1);
                        string a_s4 = base_Hash.Substring(4, 1);
                        string a_s6 = base_Hash.Substring(6, 1);
                        string a_s8 = base_Hash.Substring(8, 1);

                        string s_mut = "";
                        for (int i0 = i0_start; i0 < a_s0.Length; i0++)
                        {
                            for (int i1 = i1_start; i1 < a_s1.Length; i1++)
                            {
                                for (int i3 = i3_start; i3 < a_s3.Length; i3++)
                                {
                                    for (int i5 = i5_start; i5 < a_s5.Length; i5++)
                                    {
                                        for (int i7 = i7_start; i7 < a_s7.Length; i7++)
                                        {
                                            for (int i9 = i9_start; i9 < a_s9.Length; i9++)
                                            {
                                                s_mut = a_s0[i0] + a_s1[i1] + a_s2/*константа*/ + a_s3[i3] + a_s4/*константа*/ + a_s5[i5] + a_s6/*константа*/ + a_s7[i7] + a_s8/*константа*/ + a_s9[i9];

                                                sw.WriteLine(s_mut);
                                                sw.Flush();

                                                if (urls.Count == 0)
                                                {
                                                    s_Url = "https://anonfile.com/" + base_Hash;
                                                }            //это проверочный существующий url
                                                else
                                                {
                                                    s_Url = "https://anonfile.com/" + s_mut;
                                                }

                                                if (urls.Count < 1000)
                                                {
                                                    urls.Push(s_Url);
                                                }
                                                else
                                                {
                                                    await httpBruteForce.StartBruteForce(urls);
                                                }

                                                //s_html = getHtmlPageUsingWC("", s_Url);       //пока отключили ПО ПРОИЗВОДИТЕЛЬНОСТИ превосходит многопоточность
                                                //if (s_html == "null") { f_save_log("log_WebRequest.log", s_Url + " -- " + "null"); }       //пока отключили
                                                //else { f_save_log("log_WebRequest.log", s_Url + " -- " + "Ok"); }

                                                //myThread t = new myThread("Thread" + i7.ToString() + i9.ToString(), s_Url);   //хлам

                                                //сохраняем в файл настроек прогресс
                                                Setting.f_save_settings(System.IO.Directory.GetCurrentDirectory() + "/settings.txt", base_Hash, i0, i1, i3, i5, i7, i9);
                                            }
                                            i9_start = 0;
                                        }
                                        i7_start = 0;
                                    }
                                    i5_start = 0;
                                }
                                i3_start = 0;
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
