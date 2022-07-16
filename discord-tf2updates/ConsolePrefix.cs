using System;
namespace discordtf2updates
{
    class CustomConsole
    {
        public static void CustomWriteLine(string msg)
        {
            Console.WriteLine(String.Format("{0} {1}", DateTime.Now.ToString("HH:mm:ss"), msg));
        }
    }
}