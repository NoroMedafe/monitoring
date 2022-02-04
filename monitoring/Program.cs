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
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Приложение не найдено, производится поиск приложения");
                Console.ForegroundColor = ConsoleColor.White;
                bool chek = int.TryParse(freq, out int _freq);
                if (!chek)
                {
                    Console.WriteLine("Вы ввели символы, а не цифры или ничего не ввели. Программа будет закрыта, после нажатия любой клавиши");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                Timer(_freq);

            }
            if (proc != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Приложение найдено, запущен таймер жизни"); Console.ForegroundColor = ConsoleColor.White;
                Timer(Convert.ToInt32(freq));

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
        private static void Timer(int tik)
        {
            sTimer = new System.Timers.Timer(tik * 10000);
            sTimer.Elapsed += OnTimedEvent;
            sTimer.AutoReset = true;
            sTimer.Enabled = true;
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Приложение не найдено, производится поиск приложения"); Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            if (proc != null)
            {
                int _timeoflife; bool chek =int.TryParse(timeoflife, out _timeoflife);
                if (!chek)
                {
                    Console.WriteLine("Вы ввели символы, а не цифры или ничего не ввели. Программа будет закрыта, после нажатия любой клавиши");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                DateTime date = proc.StartTime;
                DateTime now = DateTime.Now;
                TimeSpan ts = now - date;
                Console.WriteLine($"Программа работает {ts.Minutes} минут и {ts.Seconds} секунд");
                _timeoflife = _timeoflife - ts.Minutes;
                if (_timeoflife <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Программа убита"); Console.ForegroundColor = ConsoleColor.White;
                    proc.Kill();
                }
                else
                {
                    Console.WriteLine($"Программе осталось жить:{_timeoflife}");
                }
            }
        }
    }
}
