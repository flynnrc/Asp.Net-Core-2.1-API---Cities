using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = "";
        private string _mailFrom = "";
        private string _server = "";

        public void Send(string subject, string message)
        {
            Debug.WriteLine("reached cloud mail service but this is just an example");
        }
    }
}
