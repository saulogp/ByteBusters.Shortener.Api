using System.Text;

namespace Core;
public static class ShortenerExtensions
{
    static readonly Random random = new();
    public static string GenerateRandomCode(int length)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder code = new();
        
        for (int i = 0; i < length; i++)
        {
            int index = random.Next(characters.Length);
            code.Append(characters[index]);
        }
        
        return code.ToString();
    }
}