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
    public class AsyncFile : IDisposable
    {
        Thread th;
        AutoResetEvent ev = new AutoResetEvent(false);
        List<string> Waiting = new List<string>();
        private bool isDispose = false;
        //StreamWriter f;
        string fil;

        public AsyncFile(string file, int timeout = 100)
        {
            //f = new StreamWriter(file);
            fil = file;
            /*th = new Thread(() => 
            {                
                while (!isDispose)
                {
                    if(Waiting.Count > 0)
                    {
                        for (int i = 0; i < Waiting.Count; i++)
                        {
                            //f.WriteLine(Waiting[i]);
                            File.AppendAllLines(file, new string[] { Waiting[i] });
                            Waiting.RemoveAt(i);
                            //Thread.Sleep(timeout);                           
                        }
                        //Waiting.Clear();
                    }
                }
                //f.Close();
            });
            //th.IsBackground = true;
            th.Start();*/
        }

        public void AddText(string Text)
        {
            //Waiting.Add(Text);
            lock (this)
            {
                File.AppendAllLines(fil, new string[] { Text });
            }
            
        }
        public void ReplaceFile()
        {
            //f.Close();
            File.WriteAllText(fil, "");
            //f = new StreamWriter(fil);
        }

        public bool IsDispose { get => isDispose; set => isDispose = value; }

        public void Dispose()
        {
            isDispose = true;
        }
    }
}
