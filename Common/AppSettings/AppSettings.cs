namespace Common.AppSettings
{
    public class AppSettings : IAppSettings
    {
        public string? DBConnectionString { get; set; }
        public string? AppVersion { get; set; }
        public IAuthSettings AuthSettings { get; set; } = new AuthSettings();
    }

    public class AuthSettings: IAuthSettings
    {
        public string UIAppUrl { get; set; } = null!;
        public string UIClientId { get; set; } = null!;
        public string UIClientSecret { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string SymmetricSecurityKey { get; set; } = null!;
    }
}
