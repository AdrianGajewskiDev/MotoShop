namespace MotoShop.WebAPI.Helpers
{
    public class StaticMessages
    {
        public static string SomethingWentWrong = "Something went wrong while trying to complete the action. Try again";

        public static string InvalidLoginProvider = "Invalid or unsupported External login provider";
        public static string AlreadyIs(string type) => $"User already is a {type}";
        public static string Created(string type) => $"{type} successfully created";
        public static string NotFound(string type) => $"Cannot find any available {type}";
        public static string NotFound(string type, string data, object value) => $"{type} with {data} of {value.ToString()}  was not found";
        public static string WasNull(string data) => $"{data} was null";
        public static string Taken(string data) => $"{data} is already taken";
        public static string InvalidSignInData(string prefix) => $"Invalid {prefix} or password ";
        public static string NotExist(string type, string data, object value) => $"{type} with {data} of {value} does not exist!!";
        public static string FailedToUpdate(string data) => $"Something went wrong while trying to update the {data}";
        public static string Deleted(string data) => $"-{data} was successfully deleted";
    }
}
