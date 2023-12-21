namespace Core.Infrastructure;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public Authenticate Authenticate { get; set; }
    public ZeebeOptions Zeebe { get; set; }
    public Services Service {  get; set; }
}
public class ConnectionStrings
{
    public string ConnectionString { get; set; }
    public string Monitoring { get; set; }
}

public class Authenticate
{
    public string Secret { get; set; }
    public string RefreshTokenTTL { get; set; }
}
public class ZeebeOptions
{
    public string ModelFilename { get; set; }
    public string ProcessPath { get; set; }
    public string ZeebeGateway { get; set; }
    public string EventHubUrl { get; set; }
}

public class Services
{
    public string SurveyAPI { get; set; }
}