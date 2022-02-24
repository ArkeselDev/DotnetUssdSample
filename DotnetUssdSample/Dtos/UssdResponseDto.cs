namespace DotnetUssdSample.Dtos;

public class UssdResponseDto
{
    public string SessionID { get; set; }
    public string UserID { get; set; }
    public bool ContinueSession { get; set; }
    public string Msisdn { get; set; }
    public string Message { get; set; }
}