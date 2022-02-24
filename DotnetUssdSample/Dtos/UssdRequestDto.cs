namespace DotnetUssdSample.Dtos;

public class UssdRequestDto
{
    public string SessionID { get; set; }
    public string UserID { get; set; }
    public bool NewSession { get; set; }
    public string Msisdn { get; set; }
    public string? UserData { get; set; }
    public string Network { get; set; }
}