using System;

namespace StateApples
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                string[] names = new string[] { "Masha", "Vitya", "Lena" };
                StateAppleControl stateAppleControl = new StateAppleControl(names, 0.3);
                stateAppleControl.LaunchUsers();
                while (true)
                {
                    stateAppleControl.ControlWrite();
                    switch (stateAppleControl.ControlRead())
                    {
                        case Status.DeleteData:
                            {
                                stateAppleControl.DeleteData();
                                break;
                            }
                        case Status.PickApples:
                            {
                                stateAppleControl.PickApples();
                                break;
                            }
                        case Status.ReceivApples:
                            {
                                stateAppleControl.ReceivApples();
                                break;
                            }
                        case Status.WriteData:
                            {
                                stateAppleControl.WriteData();
                                break;
                            }
                        case Status.Exit:
                            {
                                stateAppleControl.Exit();
                                Environment.Exit(0);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
            else
            {
                UserApple userApple = new UserApple(args[0]);
                Console.WriteLine("Приветствую {0}", userApple.Name);
                while (true)
                {
                    switch ((Status)PipeDataExchange.ClientGetValue(userApple.Name))
                    {
                        case Status.DeleteData:
                            {
                                userApple.DeleteData();
                                break;
                            }
                        case Status.PickApples:
                            {
                                userApple.PickApples();
                                break;
                            }
                        case Status.ReceivApples:
                            {
                                userApple.ReceivApples();
                                break;
                            }
                        case Status.WriteData:
                            {
                                userApple.WriteData();
                                break;
                            }
                        case Status.Exit:
                            {
                                Environment.Exit(0);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }
    }
}
