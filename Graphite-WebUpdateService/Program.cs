using System.Net;

namespace Graphite_WebUpdateService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // https://raw.githubusercontent.com/Dismalitie/Graphite/main/Graphite/bin/Debug/graphite.exe
            WebClient wc = new WebClient();
            wc.DownloadFile("https://raw.githubusercontent.com/Dismalitie/Graphite/main/Graphite/bin/Debug/graphite.exe", ".\\graphite.exe");
        }
    }
}
