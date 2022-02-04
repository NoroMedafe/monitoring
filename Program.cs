using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace monitoring
{
    class Program
    {
        private static System.Timers.Timer sTimer;
        static string procName;
        static string timeoflife;
        static string freq;

        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(args[i]);
            }
            procName = args[0];
            timeoflife = args[1];
            freq = args[2];
            Console.WriteLine("Производится поиск приложения...");
            Process proc = null;
            try
            {
                proc = Process.GetProcessesByName(procName)[0];

            }
            catch (System.IndexOutOfRangeException)
            {
                Console.WriteLine("Приложение не найдено, производится поиск приложения");
                SearchTimer(Convert.ToInt32(freq));

            }
            if (proc != null)
            {
                Console.WriteLine("Приложение найдено, запущен таймер жизни");
                SearchTimer(Convert.ToInt32(freq));

            }
            _key:
           ConsoleKeyInfo key=  Console.ReadKey();
            if (key.Key == ConsoleKey.Q)
            {
                Environment.Exit(0);
            }
            else
            {
                goto _key;
            }

        }
        private static void SearchTimer(int tik)
        {
            sTimer = new System.Timers.Timer(tik * 1000);
            // Hook up the Elapsed event for the timer. 
            sTimer.Elapsed += OnTimedEvent;
            sTimer.AutoReset = true;
            sTimer.Enabled = true;// Create a timer with a two second interval.

        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Process proc = null;
            try
            {
                proc = Process.GetProcessesByName(procName)[0];

            }
            catch (System.IndexOutOfRangeException)
            {
                Console.WriteLine("Приложение не найдено, производится поиск приложения");
                return;
            }

            if (proc != null)
            {
                Console.WriteLine("Пошел тик");

                int _timeoflife = Convert.ToInt32(timeoflife);
                Console.WriteLine("\t"+"Процесс существует");
                DateTime date = proc.StartTime;
                DateTime now = DateTime.Now;
                TimeSpan ts = now - date;
                Console.WriteLine($"Программа работает {ts.Minutes} минут и {ts.Seconds} секунд");
                _timeoflife = _timeoflife - ts.Minutes;
                if (_timeoflife <= 0)
                {
                    Console.WriteLine("Программа убита");

                    proc.Kill();
                }
                else
                {
                    Console.WriteLine("Программе осталось жить: " + (_timeoflife - ts.Minutes));
                }
                Console.WriteLine("тик закончился");

            }
            else
            {
                Console.WriteLine("чето не прокнуло");
                
            }
           


        }
    }
}
