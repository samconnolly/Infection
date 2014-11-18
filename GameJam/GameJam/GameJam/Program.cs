using System;
using System.Windows.Forms;

namespace GameJam
{

#if WINDOWS || XBOX
        
    static class Program
    {

        /// The main entry point for the application.

        // check if there's an error, chuck it out if so...

        static void Main(string[] args)
        {
            //try
            //{
                AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Whoops! An error has occured...." + ex.ToString());
            //}
        }
    

        // do this stuff on exit
        static void OnProcessExit(object sender, EventArgs e)
        {
            //Console.WriteLine("I'm out of here");
        }
    }
#endif
}

