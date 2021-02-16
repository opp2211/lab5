using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace lab1
{

    class Brute
    {
        public delegate void BruteHandler(string pass, double speed, TimeSpan time);
        public event BruteHandler OnNewPassWasFound;

        public int MaxLength { get; set; } = 1;
        public bool Numbers { get; set; } = true;
        public bool Lower { get; set; } = true;
        public bool Upper { get; set; } = true;
        public bool Symb { get; set; } = true;

        public string Filename { get; set; }

        bool work = false;
        string alph = "";
        string curr_value = "";

        public void FullBrute_SetUp()
        {
            alph = "";

            if (Numbers == true)
                alph += "0123456789";
            if (Lower == true)
                alph += "abcdefghijklmnopqrstuvwxyz";
            if (Upper == true)
                alph += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (Symb == true)
                alph += "!\"№;%?\'()*+,-./:;<=>?@[\\]^_`{|}~";
        }

        public void DicBrute_Start(string pass)
        {
            Thread thread = new Thread(() =>
            {
                double speed = 0;
                DateTime dts = DateTime.Now;
                DateTime dt1 = dts;
                DateTime dt2;
                work = true;
                int i = 0;
                using (StreamReader sr = new StreamReader(Filename))
                {
                    while (work)
                    {
                        curr_value = Convert(sr.ReadLine());
                        if (curr_value == pass || curr_value == null)
                            work = false;
                        i++;
                        if (i >= 1000)
                        {
                            dt2 = DateTime.Now;
                            speed = i / (dt2 - dt1).TotalSeconds;
                            dt1 = dt2;
                            i = 0;
                        }
                        OnNewPassWasFound?.Invoke(curr_value, speed, DateTime.Now.Subtract(dts));
                    }
                }

            });
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();
        }

        string Convert(string orig)
        {
            try
            {
                string RusKey = "Ё!\"№;%:?*()_+ЙЦУКЕНГШЩЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ё1234567890-=йцукенгшщзхъ\\фывапролджэячсмитьбю. ";
                string EngKey = "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./ ";

                string s = "";
                for (int i = 0; i < orig.Length; i++)
                {
                    s += EngKey[RusKey.IndexOf(orig[i])];
                }
                return s;
            }
            catch
            {
                return null;
            }
        }
        public void FullBrute_Start(string pass)
        {
            Thread thread = new Thread(() => 
            {
                double speed = 0;
                DateTime dts = DateTime.Now;
                DateTime dt1 = dts;
                DateTime dt2;
                work = true;
                FullBrute_SetUp();
                int i = 0;
                foreach (var p in GetCombinations(alph.ToCharArray(), MaxLength))
                {
                    if (!work)
                        break;
                    curr_value = p;
                    if (curr_value == pass)
                        work = false;
                    i++;
                    if (i >= 1000)
                    {
                        dt2 = DateTime.Now;
                        speed = i / (dt2 - dt1).TotalSeconds;
                        dt1 = dt2;
                        i = 0;
                    }

                    OnNewPassWasFound?.Invoke(curr_value, speed, DateTime.Now.Subtract(dts));
                }
            });
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();

        }
        private static IEnumerable<string> GetCombinations(char[] chars, int maxLength)
        {
            if (maxLength <= 0)
                yield break;

            foreach (var c in chars)
            {
                yield return c.ToString();

                foreach (var child in GetCombinations(chars, maxLength - 1))
                    yield return c + child;
            }
        }

        public void Stop()
        {
            work = false;
        }

    }
}
