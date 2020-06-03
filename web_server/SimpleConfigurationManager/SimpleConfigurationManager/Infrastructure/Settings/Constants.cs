namespace SimpleConfigurationManager.Infrastructure.Settings
{
    public class Constants
    {
        /// <summary>
        /// Route prefix for application.
        /// </summary>
        public const string RoutePrefix = "SCM";

        /// <summary>
        /// Default duration of cache response in seconds.
        /// </summary>
        public const int CacheDuration = 30;

        public static readonly string CONNECTION_STRING = "Server=DESKTOP-L5OCQBJ;Database=ScmDb;Trusted_Connection=True;";
    }
}
