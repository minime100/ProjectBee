using System;

namespace ProjectBee.Main
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MainEntry game = new MainEntry())
            {
                game.Run();
            }
        }
    }
#endif
}

