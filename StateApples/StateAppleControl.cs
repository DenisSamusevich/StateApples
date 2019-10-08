using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace StateApples
{
    class StateAppleControl
    {
        double Tax { get; set; }
        int TaxApples { get; set; }
        int ReceivApplesAllUsers { get; set; }
        UserApple[] UsersApples { get; set; }
        ProcessStartInfo[] ProcessStartInfoUser { get; set; }

        public StateAppleControl(string[] names, double tax)
        {
            Tax = tax;
            UsersApples = new UserApple[names.Length];
            ProcessStartInfoUser = new ProcessStartInfo[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                UsersApples[i] = new UserApple(names[i]);
                ProcessStartInfoUser[i] = new ProcessStartInfo(Environment.CurrentDirectory +@"\StateApples.exe", names[i]);
            }
        }
        public void LaunchUsers()
        {
            for (int i = 0; i < UsersApples.Length; i++)
            {
                Process p = Process.Start(ProcessStartInfoUser[i]);
            }
        }
        public void ControlWrite()
        {
            Console.WriteLine("1 - Очистить данные,\n2 - Собрать яблоки с пользователей,\n3 - Отдать яблоки с вычетом налогов,\n4 - Записать данные,\n5 - Выйти из программы,");
            Console.Write("Введите значение:");
        }
        public Status ControlRead()
        {
            string ControlRead = Console.ReadLine();
            return (Status)Convert.ToInt32(ControlRead);
        }
        public void DeleteData()
        {
            Thread[] threadDataExchanges = new Thread[UsersApples.Length];
            for (int i = 0; i < UsersApples.Length; i++)
            {
                UsersApples[i].DeleteData();
                threadDataExchanges[i] = new Thread(() => PipeDataExchange.ServerForwardValue(UsersApples[i].Name, (byte)Status.DeleteData));
                threadDataExchanges[i].Start();
                Thread.Sleep(1000);
            }
            ThreadWaiting(threadDataExchanges);
            Console.WriteLine("Данные успешно очищены");
        }
        public void PickApples()
        {
            Thread[] threadDataExchanges = new Thread[UsersApples.Length];
            for (int i = 0; i < UsersApples.Length; i++)
            {
                threadDataExchanges[i] = new Thread(() => PipeDataExchange.ServerForwardValue(UsersApples[i].Name, (byte)Status.PickApples));
                threadDataExchanges[i].Start();
                Thread.Sleep(1000);
            }
            ThreadWaiting(threadDataExchanges);
            for (int i = 0; i < UsersApples.Length; i++)
            {
                threadDataExchanges[i] = new Thread(() => UsersApples[i].SetPickApples(PipeDataExchange.ServerGetValue(UsersApples[i].Name)));
                threadDataExchanges[i].Start();
                Thread.Sleep(1000);
            }
            ThreadWaiting(threadDataExchanges);
            Console.WriteLine("Яблоки собраны");
        }
        public void ReceivApples()
        {
            TaxСollection();
            Console.WriteLine("Налог собран");
            Thread[] threadDataExchanges = new Thread[UsersApples.Length];
            for (int i = 0; i < UsersApples.Length; i++)
            {
                threadDataExchanges[i] = new Thread(() => PipeDataExchange.ServerForwardValue(UsersApples[i].Name, (byte)Status.ReceivApples));
                threadDataExchanges[i].Start();
                Thread.Sleep(1000);
            }
            ThreadWaiting(threadDataExchanges);
            for (int i = 0; i < UsersApples.Length; i++)
            {
                UsersApples[i].SetReceivApples((byte)ReceivApplesAllUsers);
                threadDataExchanges[i] = new Thread(() => PipeDataExchange.ServerForwardValue(UsersApples[i].Name, (byte)ReceivApplesAllUsers));
                threadDataExchanges[i].Start();
                Thread.Sleep(1000);
            }
            ThreadWaiting(threadDataExchanges);
            Console.WriteLine("Яблоки переданы");
        }
        public void WriteData()
        {
            Thread[] threadDataExchanges = new Thread[UsersApples.Length];
            for (int i = 0; i < UsersApples.Length; i++)
            {
                threadDataExchanges[i] = new Thread(() => PipeDataExchange.ServerForwardValue(UsersApples[i].Name, (byte)Status.WriteData));
                threadDataExchanges[i].Start();
                Thread.Sleep(1000);
            }
            ThreadWaiting(threadDataExchanges);
            FileInfo File = File = new FileInfo(Environment.CurrentDirectory + string.Format(@"\" + DateTime.Now.Year.ToString("D4") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + ".txt"));
            FileStream fileStream = File.Open(FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            for (int i = 0; i < UsersApples.Length; i++)
            {
                streamWriter.WriteLine(UsersApples[i].FileWriteLineUserData());
            }
            streamWriter.WriteLine();
            streamWriter.Close();
            Console.WriteLine("Данные записаны в файл");
        }
        public void Exit()
        {
            Thread[] threadDataExchanges = new Thread[UsersApples.Length];
            for (int i = 0; i < UsersApples.Length; i++)
            {
                threadDataExchanges[i] = new Thread(() => PipeDataExchange.ServerForwardValue(UsersApples[i].Name, (byte)Status.Exit));
                threadDataExchanges[i].Start();
                Thread.Sleep(1000);
            }
            ThreadWaiting(threadDataExchanges);
        }
        static void ThreadWaiting(Thread[] thread)
        {
            bool flagThreadSleep = true;
            while (flagThreadSleep)
            {
                for (int i = 0; i < thread.Length; i++)
                {
                    if (thread[i].IsAlive)
                    {
                        Console.WriteLine("Ожидаем выполнения программы");
                        Thread.Sleep(3000);
                        break;
                    }
                    else
                    {
                        if (i == thread.Length - 1)
                        {
                            flagThreadSleep = false;
                        }
                    }
                }
            }
        }
        void TaxСollection()
        {
            byte a = 5;
            double s = a;
            var g = a / s;
            int AllApples = 0;
            for (int i = 0; i < UsersApples.Length; i++)
            {
                AllApples += UsersApples[i].PickedApples;
            }
            TaxApples = (int)Math.Round((double)AllApples * Tax);
            int remainder = (AllApples - TaxApples) % UsersApples.Length;
            TaxApples += remainder;
            ReceivApplesAllUsers = (AllApples - TaxApples) / UsersApples.Length;
        }
    }
}
