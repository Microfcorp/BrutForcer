using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace BrutForce
{
    class Program
    {
        const string InFile = "in.txt";
        const string Array = "array.txt";
        const string OutFile = "out.txt";

        static bool IsReplZv = true;
        static ulong res = 0;
        static ulong mzxvar = 0;
        static AsyncFile file = new AsyncFile(OutFile);
        static void Main(string[] args)
        {
            Console.WriteLine("Брутфорссер V1.0");

            IsReplZv = args.Contains("-i");
            List<string> Outs = new List<string>();
            string InText = File.ReadAllText(InFile);
            var buf = File.ReadAllLines(Array);
            Outs.Add(InText);
            ulong variant = (ulong)Math.Pow(buf.Length, InText.Count(tmp => tmp == '*'));

            Console.WriteLine("Входная маска " + InText);
            Console.WriteLine("Символов для перебора " + buf.Length);
            Console.WriteLine("Доступно корректных вариантов " + variant);
            Console.WriteLine("Начать");
            Console.ReadLine();
            file.ReplaceFile();
            var starttime = DateTime.Now;

            Thread t = new Thread(() => GetBrut(buf, InText, (o, e) => { res++; file.AddText(o.ToString()); /*Outs.Add(o.ToString());*/ }));
            t.Start();
            //Thread.Sleep(100);
            Console.WriteLine();

            //var l = th.Where(tmp => !tmp.IsAlive).ToArray();
            while (res + 5 < variant || (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.C))
            {
                Console.SetCursorPosition(0, 7);
                var pr = (int)(Math.Round((double)mzxvar / (double)variant, 2) * 100);
                Console.Write("Прогресс " + pr + "% (" + mzxvar + ")");
                //if (pr == 100) break;
            }
            Console.WriteLine();
            Console.WriteLine();

            //if (!IsReplZv) Outs.RemoveAll(tmp => tmp.Contains('*'));
            var endtime = DateTime.Now;

            Console.WriteLine("Найдено " + mzxvar + " вариантов (включая части маски)");
            Console.WriteLine("Записано " + res + " вариантов");
            Console.WriteLine("Время выполнения " + (endtime - starttime).ToString());

            Console.ReadLine();
        }

        static object __lockObj = new object();
        static List<Thread> th = new List<Thread>();

        static void GetBrut(string[] buffer, string intext, EventHandler eh)
        {

            for (int ia = 0; ia < buffer.Length; ia++) //перебираем каждый символ замены
            {
                var i = ia;
                //Thread m = new Thread(() =>
                //{
                var str = Replace(intext, "*", buffer[i]);
                if (IsReplZv || (!IsReplZv && !str.Contains('*')))
                {
                    /*Thread qt = new Thread(() => eh.Invoke(str, null));
                    qt.Start();*/
                    eh.Invoke(str, null);
                }

                if (str.Contains('*'))
                {
                    Thread th = new Thread(() => GetBrut(buffer, str, eh));
                    th.Start();                    
                    //GetBrut(buffer, str, eh);
                }
                Thread.Sleep(100);
                mzxvar++;                  
                //});
                //th.Add(m);
                //m.IsBackground = true;
                //m.Start();
            }
        }

        static string Replace(string original, string find, string replace)
        {
            string substring = find;
            int i = original.IndexOf(substring);
            string result = original.Remove(i, substring.Length).Insert(i, replace);
            return result;
        }
    }
}
