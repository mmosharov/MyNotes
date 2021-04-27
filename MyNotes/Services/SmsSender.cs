using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options; 
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MyNotes.Services
{
    public class SmsSender
    {

        private string twilioAccountSid;
        private string twilioAuthToken;
        private string twilioFromNumber;

        public SmsSender(IOptions<IApiConfiguration> apiConfiguration)
        {
            
            twilioAccountSid = apiConfiguration.Value.TwilioAccountSid;
            twilioAuthToken = apiConfiguration.Value.TwilioAuthToken;
            twilioFromNumber = apiConfiguration.Value.TwilioFromNumber;
            
            TwilioClient.Init(twilioAccountSid, twilioAuthToken);

        }

        public void Send(string toNumber, string text)
        {

            var message = MessageResource.Create(
                body: text,
                from: new Twilio.Types.PhoneNumber(twilioFromNumber),
                to: new Twilio.Types.PhoneNumber(toNumber)
            );

        }
    }
}