using DotnetUssdSample.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace DotnetUssdSample.Controllers;

[ApiController]
[Route("[controller]")]
public class UssdController : ControllerBase
{
    private readonly ILogger<UssdController> _logger;
    private readonly IMemoryCache _memoryCache;

    public UssdController(ILogger<UssdController> logger, IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    [HttpPost(Name = "Handle")]
    public UssdResponseDto Handle([FromBody] UssdRequestDto requestBody)
    {
        var response = new UssdResponseDto
        {
            Msisdn = requestBody.Msisdn,
            UserID = requestBody.UserID,
            SessionID = requestBody.SessionID
        };

        if (requestBody.NewSession)
        {
            response.Message = "Welcome to Arkesel Voting Portal. Please vote for your favourite service from Arkesel" +
                               "\n1. SMS" +
                               "\n2. Voice" + "\n3. Email" + "\n4. USSD" + "\n5. Payments";
            response.ContinueSession = true;

            // Keep track of the USSD state of the user and their session
            var currentState = new UssdState(
                requestBody.SessionID,
                requestBody.Msisdn,
                requestBody.UserData,
                requestBody.Network,
                requestBody.NewSession,
                response.Message,
                1,
                1);

            _memoryCache.TryGetValue(requestBody.SessionID, out List<UssdState> userResponseTracker);

            if (userResponseTracker == null)
            {
                userResponseTracker = new List<UssdState>();
            }
            
            userResponseTracker.Add(currentState);

            _memoryCache.Set(requestBody.SessionID, userResponseTracker, TimeSpan.FromMinutes(2));
        }
        else
        {
            var lastResponse = _memoryCache.Get<List<UssdState>>(requestBody.SessionID).Last();

            if (lastResponse.Level == 1)
                switch (requestBody.UserData)
                {
                    case "1":
                    {
                        response.Message = "For SMS which of the features do you like best?" +
                                           "\n1. From File" +
                                           "\n2. Quick SMS" + "\n\n #. Next Page";
                        response.ContinueSession = true;

                        // Keep track of the USSD state of the user and their session
                        var currentState = new UssdState(
                            requestBody.SessionID,
                            requestBody.Msisdn,
                            requestBody.UserData,
                            requestBody.Network,
                            requestBody.NewSession,
                            response.Message,
                            2,
                            1);

                        _memoryCache.TryGetValue(requestBody.SessionID, out List<UssdState> userResponseTracker);

                        userResponseTracker.Add(currentState);

                        _memoryCache.Set(requestBody.SessionID, userResponseTracker, TimeSpan.FromMinutes(2));

                        break;
                    }
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    {
                        response.Message = "Thank you for voting!";
                        response.ContinueSession = false;
                        break;
                    }
                    default:
                    {
                        response.Message = "Bad choice!";
                        response.ContinueSession = false;
                        break;
                    }
                }

            if (lastResponse.Level == 2)
            {
                List<string> possibleChoices = new() {"1", "2", "3", "4"};
                if (lastResponse.Part == 1 && requestBody.UserData == "#")
                {
                    response.Message = "For SMS which of the features do you like best?" +
                                       "\n3. Bulk SMS" +
                                       "\n\n*. Go Back" + "\n#. Next Page";
                    response.ContinueSession = true;

                    // Keep track of the USSD state of the user and their session
                    var currentState = new UssdState(
                        requestBody.SessionID,
                        requestBody.Msisdn,
                        requestBody.UserData,
                        requestBody.Network,
                        requestBody.NewSession,
                        response.Message,
                        2,
                        2);

                    _memoryCache.TryGetValue(requestBody.SessionID, out List<UssdState> userResponseTracker);

                    userResponseTracker.Add(currentState);

                    _memoryCache.Set(requestBody.SessionID, userResponseTracker, TimeSpan.FromMinutes(2));
                }
                else if (lastResponse.Part == 2 && requestBody.UserData == "#")
                {
                    response.Message = "For SMS which of the features do you like best?" +
                                       "\n4. SMS To Contacts" +
                                       "\n\n*. Go Back";
                    response.ContinueSession = true;

                    // Keep track of the USSD state of the user and their session
                    var currentState = new UssdState(
                        requestBody.SessionID,
                        requestBody.Msisdn,
                        requestBody.UserData,
                        requestBody.Network,
                        requestBody.NewSession,
                        response.Message,
                        2,
                        3);

                    _memoryCache.TryGetValue(requestBody.SessionID, out List<UssdState> userResponseTracker);

                    userResponseTracker.Add(currentState);

                    _memoryCache.Set(requestBody.SessionID, userResponseTracker, TimeSpan.FromMinutes(2));
                }
                else if (lastResponse.Part == 3 && requestBody.UserData == "*")
                {
                    response.Message = "For SMS which of the features do you like best?" +
                                       "\n3. Bulk SMS" +
                                       "\n\n*. Go Back" + "\n#. Next Page";
                    response.ContinueSession = true;

                    // Keep track of the USSD state of the user and their session
                    var currentState = new UssdState(
                        requestBody.SessionID,
                        requestBody.Msisdn,
                        requestBody.UserData,
                        requestBody.Network,
                        requestBody.NewSession,
                        response.Message,
                        2,
                        2);

                    _memoryCache.TryGetValue(requestBody.SessionID, out List<UssdState> userResponseTracker);

                    userResponseTracker.Add(currentState);

                    _memoryCache.Set(requestBody.SessionID, userResponseTracker, TimeSpan.FromMinutes(2));
                }
                else if (lastResponse.Part == 2 && requestBody.UserData == "*")
                {
                    response.Message = "For SMS which of the features do you like best?" +
                                       "\n1. From File" +
                                       "\n2. Quick SMS" + "\n\n #. Next Page";
                    response.ContinueSession = true;

                    // Keep track of the USSD state of the user and their session
                    var currentState = new UssdState(
                        requestBody.SessionID,
                        requestBody.Msisdn,
                        requestBody.UserData,
                        requestBody.Network,
                        requestBody.NewSession,
                        response.Message,
                        2,
                        1);

                    _memoryCache.TryGetValue(requestBody.SessionID, out List<UssdState> userResponseTracker);

                    userResponseTracker.Add(currentState);

                    _memoryCache.Set(requestBody.SessionID, userResponseTracker, TimeSpan.FromMinutes(2));
                }
                else if (possibleChoices.Contains(requestBody.UserData ?? ""))
                {
                    response.Message = "Thank you for voting!";
                    response.ContinueSession = false;
                }
                else
                {
                    response.Message = "Bad choice!";
                    response.ContinueSession = false;
                }
            }
        }

        return response;
    }
}