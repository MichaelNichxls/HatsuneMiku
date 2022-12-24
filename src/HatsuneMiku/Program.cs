using System.IO;
using System.Threading.Tasks;

namespace HatsuneMiku
{
    internal class Program
    {
        public static string SolutionDirectory { get; } = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.Parent!.FullName;
        public static string ProjectDirectory { get; } = Path.Combine(SolutionDirectory, $@"src\{nameof(HatsuneMiku)}\");

        // Restrict perms
        private static async Task Main(string[] args)
        {
            using (Bot bot = new())
                await bot.RunAsync();
        }
    }
}