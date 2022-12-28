using System.Threading.Tasks;

namespace HatsuneMiku;

internal class Program
{
    // Restrict perms
    private static async Task Main(string[] args)
    {
        using (Bot bot = await Bot.CreateAsync())
            await bot.RunAsync();
    }
}