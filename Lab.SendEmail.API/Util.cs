namespace Lab.SendEmail.API
{
    public class Util
    {
        public static bool IsDesenv()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return env == "Development";
        }
    }
}
