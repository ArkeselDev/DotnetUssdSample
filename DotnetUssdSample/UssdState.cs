namespace DotnetUssdSample;

public record UssdState(
    string SessionID,
    string Msisdn,
    string UserData,
    string Network,
    bool NewSession,
    string Message,
    int Level,
    int Part
);