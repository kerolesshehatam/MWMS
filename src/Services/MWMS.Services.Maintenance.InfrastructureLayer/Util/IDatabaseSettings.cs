namespace MWMS.Services.Maintenance.InfrastructureLayer.Util
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
