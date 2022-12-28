using System.Threading.Tasks;

namespace HatsuneMiku;

internal class Program
{
    // Restrict perms
    private static async Task Main(string[] args)
    {
        using (HatsuneMikuBot bot = await HatsuneMikuBot.CreateAsync())
            await bot.RunAsync();
    }
}