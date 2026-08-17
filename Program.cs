using System;
using System.Windows.Forms;

namespace FiloYonetimi;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Database.Initialize();
        Application.Run(new MainForm());
    }
}
