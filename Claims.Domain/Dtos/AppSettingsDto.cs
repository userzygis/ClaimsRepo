namespace Claims.Domain.Dtos
{
    public class AppSettingsDto
    {
        public CosmosDbSettings CosmosDb { get; set; }
        public ConnectionStringsSettings ConnectionStrings { get; set; }
    }

    public class CosmosDbSettings
    {
        public string Account { get; set; }
        public string Key { get; set; }
        public string DatabaseName { get; set; }
        public string ClaimContainerName { get; set; }
        public string CoverContainerName { get; set; }
    }

    public class ConnectionStringsSettings
    {
        public string DefaultConnection { get; set; }
    }
}
