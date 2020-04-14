namespace MWMS.Services.Maintenance.InfrastructureLayer.Util
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
