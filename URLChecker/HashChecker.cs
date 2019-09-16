using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLChecker
{
    public class HashChecker
    {

        public delegate void StateHandler(string message);
        // Событие, возникающее при выводе денег
        public static event StateHandler Status;

        private static string _settingPath = Directory.GetCurrentDirectory() + "/settings.txt";
        private static string _progressPath = Directory.GetCurrentDirectory() + "/mutationString.txt";

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

        public static Hash GetStartHash(string base_Hash)
        {
            //если файл настроек не существует
            if (File.Exists(_settingPath))
            {
                return Setting.f_load_settings(_settingPath);
            }

            return new Hash() { BaseHash = base_Hash };
        }

        public static async Task CheckUrlsWithHash(string base_Hash)
        {
            var startHash = GetStartHash(base_Hash);

            var hashsForCheck = await GetHashs(startHash);

            HttpBruteForce httpBruteForce = new HttpBruteForce(20000, "https://anonfile.com/");

            while (hashsForCheck.Count > 0)
            {
                Status?.Invoke($"{DateTime.Now} начало обработки {hashsForCheck.Count} хэшей");
                var lastHash = hashsForCheck.Peek();

                var firstHash = hashsForCheck.Last();

                await httpBruteForce.StartBruteForce(hashsForCheck)
                    .ContinueWith((Task t) =>
                    {
                        Status?.Invoke(message: $"{DateTime.Now} все хэши c '{firstHash}' по '{lastHash}' обработаны");
                    });

                startHash = GetStartHash(base_Hash);
                hashsForCheck = await GetHashs(startHash);
            }
        }

        public static async Task<Stack<string>> GetHashs(Hash base_Hash = null, int MaxCount = 2000)
        {
            return await Task.Run(() =>
              {
                  var urls = new Stack<string>();     //стэк для хранения урлов
                  string calculatedHash = "";

                  if (base_Hash != null)
                  {
                      using (StreamWriter sw = new StreamWriter(_progressPath, false, Encoding.Default))
                      {
                          int i0_start = base_Hash.i0;              //!!!!! начали сразу с "A"   нет ерунда
                          int i1_start = base_Hash.i1;
                          int i3_start = base_Hash.i3;
                          int i5_start = base_Hash.i5;
                          int i7_start = base_Hash.i7;
                          int i9_start = base_Hash.i9;

                          // здесь постоянные символы на входе будут известны
                          string a_s2 = base_Hash.BaseHash.Substring(2, 1);
                          string a_s4 = base_Hash.BaseHash.Substring(4, 1);
                          string a_s6 = base_Hash.BaseHash.Substring(6, 1);
                          string a_s8 = base_Hash.BaseHash.Substring(8, 1);

                          StringBuilder s_mut = new StringBuilder();
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
                                                  s_mut.Clear();
                                                  s_mut.Append(a_s0[i0]);
                                                  s_mut.Append(a_s2);
                                                  s_mut.Append(a_s3[i3]);
                                                  s_mut.Append(a_s4);
                                                  s_mut.Append(a_s5[i5]);
                                                  s_mut.Append(a_s6);
                                                  s_mut.Append(a_s7[i7]);
                                                  s_mut.Append(a_s8);
                                                  s_mut.Append(a_s9[i9]);

                                                  sw.WriteLine(s_mut.ToString());

                                                  if (urls.Count == 0)
                                                  {
                                                      calculatedHash = base_Hash.BaseHash;
                                                  }            //это проверочный существующий url
                                                  else
                                                  {
                                                      calculatedHash = s_mut.ToString();
                                                  }

                                                  if (urls.Count < MaxCount)
                                                  {
                                                      urls.Push(calculatedHash);
                                                  }
                                                  else
                                                  {
                                                      //сохраняем в файл настроек прогресс
                                                      base_Hash.SetParametrs(i0, i1, i3, i5, i7, i9);
                                                      Setting.f_save_settings(_settingPath, base_Hash);
                                                      return urls;
                                                  }
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

                      return urls;
                  }

                  return null;

              });

        }
    }
}
