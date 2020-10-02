using System.Text.Json;

namespace MotoShop.WebAPI.Configurations.Policy
{
    public class JsonCustomNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// Converts first letter of string to uppercase
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string ConvertName(string name)
        {
            char[] word = name.ToCharArray();

            for (int i = 0; i < word.Length; i++)
            {
                if (i == 0)
                    word[i] = char.ToUpper(word[i]);
            }

            return new string(word);
        }
    }
}
