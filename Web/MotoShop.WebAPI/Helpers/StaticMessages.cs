using Newtonsoft.Json;

namespace MotoShop.WebAPI.Helpers
{
    public class StaticMessages
    {
        public static string SomethingWentWrong = _serialize("Something went wrong while trying to complete the action. Try again");

        public static string InvalidLoginProvider = _serialize( "Invalid or unsupported External login provider");
        public static string AlreadyIs(string type) => _serialize( $"User already is a {type}");
        public static string Created(string type) => _serialize( $"{type} successfully created");
        public static string NotFound(string type) => _serialize($"Cannot find any available {type}");
        public static string NotFound(string type, string data, object value) => _serialize($"{type} with {data} of {value.ToString()}  was not found");
        public static string WasNull(string data) => _serialize($"{data} was null");
        public static string Taken(string data) => _serialize( $"{data} is already taken");
        public static string InvalidSignInData(string prefix) => _serialize($"Invalid {prefix} or password ");
        public static string NotExist(string type, string data, object value) => _serialize( $"{type} with {data} of {value} does not exist!!");
        public static string FailedToUpdate(string data) =>_serialize( $"Something went wrong while trying to update the {data}");
        public static string Deleted(string data) => _serialize($"-{data} was successfully deleted");
        public static string Updated(string data) => _serialize($"-{data} was successfully updated");
        public static string Empty(string text) => _serialize(text);

        private static string _serialize(string value) => JsonConvert.SerializeObject(value);
    }
}
