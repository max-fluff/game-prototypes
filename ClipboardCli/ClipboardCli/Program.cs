using System;
using System.Linq;
using WinFormsCli;

namespace ClipboardCli
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var pathToPng = args.Single();
            
            ClipboardWrapper.SetClipboard(pathToPng);
        }
    }
}