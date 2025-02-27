namespace Common.AppSettings
{
    public interface IAppSettings
    {
        string? DBConnectionString { get; set; }
        string? AppVersion { get; set; }
        IAuthSettings AuthSettings { get; set; }
    }

    public interface IAuthSettings
    {
        string UIAppUrl { get; set; }
        string UIClientId { get; set; }
        string UIClientSecret { get; set; }
        string Issuer { get; set; }
        string SymmetricSecurityKey { get; set; }
    }
}
