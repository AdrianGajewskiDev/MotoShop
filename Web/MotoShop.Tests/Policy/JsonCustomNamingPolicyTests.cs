using MotoShop.WebAPI.Configurations.Policy;
using Xunit;

namespace MotoShop.Tests.Policy
{

    public class JsonCustomNamingPolicyTests
    {
        [Theory]
        [InlineData("string","String")]
        [InlineData("data", "Data")]
        [InlineData("name", "Name")]
        [InlineData("email", "Email")]
        [InlineData("Adress", "Adress")]
        public void Json_Custom_Naming_Policy_Should_Return_Name_With_First_Letter_Uppercase(string lower, string actual)
        {
            string word = lower;

            var converted = new JsonCustomNamingPolicy().ConvertName(word);
            string expected = actual;

            Assert.Equal(expected, converted);
        }

    }
}
