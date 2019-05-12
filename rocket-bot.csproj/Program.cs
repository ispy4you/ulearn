using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rocket_bot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var channel = new Channel<Rocket>();
            Random random = new Random(223243);
            var level = LevelsFactory.CreateLevel(random);
            channel.AppendIfLastItemIsUnchanged(level.Rocket, null);
            var bot = new Bot(level.Clone(), channel, 45, 700, random);
            var thread = new Thread(() => bot.Execute()) { IsBackground = true };
            thread.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new GameForm(level.Clone(), channel);
            Application.Run(form);
        }
    }
}
