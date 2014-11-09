using System;

namespace GameJam
{
#if WINDOWS || XBOX
    static class Program
    {

        /// The main entry point for the application.

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }

        // do this stuff on exit
        static void OnProcessExit(object sender, EventArgs e)
        {
            //Console.WriteLine("I'm out of here");
        }
    }
#endif
}

