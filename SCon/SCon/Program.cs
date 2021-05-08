using System;
using System.IO.Ports;
using System.Threading;

namespace SCon
{
    class MainClass
    {
        static SerialPort com = new SerialPort();
        static string portlist = "";

        public static void Main(string[] args)
        {
            if (com.IsOpen)
            {
                com.Close();
            }
            Console.WriteLine("Welcome to SCom Serial Port Terminal.");
            Console.WriteLine("Be sure to run app as root.");
            Console.WriteLine();

            Console.WriteLine("Available ports:");
            foreach (string pn in SerialPort.GetPortNames())
            {
                char c = pn[9];
                if (char.IsDigit(c) && pn.Contains("/dev/tty"))
                {

                }
                else
                {
                    Console.WriteLine(pn);
                    portlist += pn + "-";
                }
            }
            //Console.WriteLine(portlist);
            Console.WriteLine();

            getPort();

            int i = 9600;
            Console.WriteLine("(Def= {0})", i);
            Console.Write("Baud rate: ");
            try
            {
                i = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid value. Selecting: " + i);
            }
            com.BaudRate = i;

            com.Open();
            /*
            try 
            {
                com.Open();
            }
            catch (System.IO.IOException) 
            {
                Console.WriteLine("Port not available or wrong baud rate.");
                Environment.Exit(0);
            }*/

            Console.Clear();
            Thread ta = new Thread(new ThreadStart(Read));
            Thread tb = new Thread(new ThreadStart(Write));
            ta.Start();
            tb.Start();
        }
        static void Read() 
        {
            while (true)
            {
                try 
                {
                    Console.Write(com.ReadExisting());
                }
                catch (System.IO.IOException) 
                {
                    Console.Write("Port disconnected. Press ENTER to exit...");
                    Console.Read();
                    Environment.Exit(0);
                }
            }
        }
        static void Write() 
        {
            while (true)
            {
                com.WriteLine(Console.ReadLine());
            }
        }
        static void getPort() 
        {
            Console.Write("Port name: ");
            string pname = Console.ReadLine();
            string pname2 = pname + "-";
            //Console.WriteLine(pname);
            //Console.WriteLine(portlist.Contains(pname));
            if (portlist.Contains(pname2))
            {
                com.PortName = pname;
                Console.WriteLine();
            }
            else
            {
                Console.Write("Enter a valid name. ");
                getPort();
            }
        }
    }
}
