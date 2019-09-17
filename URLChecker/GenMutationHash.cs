﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLChecker
{
    class GenerateMutationHash
    {
        //static public int i0_start;
        static public int i1_start;
        static public int i3_start;
        static public int i5_start;
        static public int i7_start;
        static public int i9_start;

        static public string base_Hash;
        static public int i0_start;
        static public int i0_delta;
        static public int i0_end;


        //блок массивов

        //1-й столбец формируется по алфавиту с маленьких, потом цифры, после снова алфавит большие буквы
        static string[] a_s0 = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                              "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
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


        public GenerateMutationHash(string base_Hash_, int i0_delta_)
        {
            base_Hash = base_Hash_;

            int i0_start_ = 0;
            for (Int32 i = 0; i < a_s0.Length; i++) { if (a_s0[i] == base_Hash_.Substring(0, 1)) { i0_start_ = i; continue; } }
            i0_start = i0_start_ - i0_delta_;
            i0_end = i0_start_ + i0_delta_;
            i0_delta = i0_delta_;

            i1_start = 0;
            i3_start = 0;
            i5_start = 0;
            i7_start = 0;
            i9_start = 0;
        }

        public GenerateMutationHash(string filePath_settings)
        {
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
        }

        public void SaveProgress(string filePath_settings)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath_settings, false))
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
        }


        //корректировка индекса для переходов через пороговые значения (пока нужно только для первого символа)
        static public int orderIndexCicleArr(int index, string[] arr)
        {
            if (index >= arr.Length) { return index - arr.Length; }
            if (index < 0) { return arr.Length - Math.Abs(index); }
            return index;
        }


        //оптимизация выполнения при нахождении хэша
        public void optimizeBruteHash()
        {
            i1_start = 0;
            i3_start = 0;
            i5_start = 0;
            i7_start = 0;
            i9_start = 0;

            i0_start++;
        }
        

        public Stack<string> Next1000Hashs()
        {
            Stack<string> urls = new Stack<string>();

            // здесь постоянные символы
            string a_s2 = base_Hash.Substring(2, 1);          //не постоянное, но пока СЧИТАЕМ постоянным, потом для красоты 1 вниз и 1 вверх
            string a_s4 = base_Hash.Substring(4, 1);
            string a_s6 = base_Hash.Substring(6, 1);
            string a_s8 = base_Hash.Substring(8, 1);

            string s_mut = "";
            int i0 = i0_start, i1 = i1_start, i3 = i3_start, i5 = i5_start, i7 = i7_start, i9 = i9_start;                     //инициализация переменных цикла
            while (i0 < i0_end)                 //здесь перебирать не все, n вверх и n вниз
            {
                while (i1 < a_s1.Length)
                {
                    while (i3 < a_s3.Length)
                    {
                        while (i5 < a_s5.Length)
                        {
                            while (i7 < a_s7.Length)
                            {
                                while (i9 < a_s9.Length)
                                {

                                    string a_si0 = "";
                                    if (i0 < i0_end - i0_delta)
                                    {
                                        a_si0 = a_s0[orderIndexCicleArr((i0 + i0_delta + 1), a_s0)];
                                    }
                                    if (i0 > i0_end - i0_delta)
                                    {
                                        a_si0 = a_s0[orderIndexCicleArr(((i0_end - i0_delta) - (i0 - (i0_end - i0_delta))), a_s0)];
                                    }
                                    if (i0 == i0_end - i0_delta) { break; }


                                    s_mut = a_si0 + a_s1[i1] + a_s2/*константа*/ + a_s3[i3] + a_s4/*константа*/ + a_s5[i5] + a_s6/*константа*/ + a_s7[i7] + a_s8/*константа*/ + a_s9[i9];

                                    
                                    if (urls.Count < 1000)
                                    {
                                        //if (urls.Count == 0) { urls.Push("https://anonfile.com/" + base_Hash); }                    //это проверочный существующий url
                                        urls.Push("https://anonfile.com/" + s_mut);
                                    }
                                    else
                                    {
                                        i0_start = i0; i1_start = i1; i3_start = i3; i5_start = i5; i7_start = i7; i9_start = i9;                     //сохранение переменных цикла
                                        return urls;
                                    }
                                    i9++;
                                }
                                i9 = 0;
                                i7++;
                            }
                            i7 = 0;
                            i5++;
                        }
                        i5 = 0;
                        i3++;
                    }
                    i3 = 0;
                    i1++;
                }
                i1 = 0;
                i0++;
            }
            //i0_start = 0;             //думаем что не нужно, уже завершение

            return urls;        //возвращаем стэе даже если не набралось 1000
        }




    }
}