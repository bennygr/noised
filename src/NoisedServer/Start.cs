using Noised.Core;

namespace Noised.Server
{
    public class Start
    {
        public static int Main(string[] args)
        {
            //Starting noised
            return new CoreStarter(args).Start();
        }
    }
}
