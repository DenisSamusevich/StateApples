using System;

namespace StateApples
{
    public class UserApple
    {
        internal string Name { get; private set; }
        internal int PickedApples { get; private set; } = 0;
        internal int ReceivedApples { get; private set; } = 0;

        public UserApple(string name)
        {
            Name = name;
        }
        public void PickApples()
        {
            PickedApples = ConsoleReadLineUserApples();
            PipeDataExchange.ClientForwardValue(Name, (byte)PickedApples);
            Console.WriteLine("Вы собрали {0} и отдали их государству", PickedApples);
        }
        public void SetPickApples(byte apples)
        {
            PickedApples = apples;
        }
        public void ReceivApples()
        {
            ReceivedApples = PipeDataExchange.ClientGetValue(Name);
            Console.WriteLine("Вы получили от государства {0} яблок", ReceivedApples);
        }
        public void SetReceivApples(byte apples)
        {
            ReceivedApples = apples;
        }
        public void DeleteData()
        {
            PickedApples = 0;
            ReceivedApples = 0;
        }
        public static int ConsoleReadLineUserApples()
        {
            int quantity = 0;
            while (true)
            {
                Console.WriteLine("Введите количество собранных вами яблок для государства");
                string stringGuantity = Console.ReadLine();
                if (int.TryParse(stringGuantity, out quantity))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неверное значение");
                }
            }
            return quantity;
        }
        public void WriteData()
        {
            Console.WriteLine("Вас зовут {0,10}, вы собрали {1,4} яблок, получили яблок с вычетом нологов {2,4} шт.", Name, PickedApples, ReceivedApples);
            Console.WriteLine("Эти данные записаны в файл государства");
        }
        public string FileWriteLineUserData()
        {
            string data = string.Format("{0,10}, Cобрал {1,4} яблок, получил яблок с вычетом нологов {2,4} шт.", Name, PickedApples, ReceivedApples);
            Console.WriteLine(data);
            return data;
        }
    }
}
