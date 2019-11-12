using System.Collections.Generic;

namespace SayHelloApp.Models
{
    public class ServiceDetailsModel
    {
        public string ServiceName { get; set; }
        
        public string Host { get; set; }
        
        public string Port { get; set; }

        public List<KeyValuePair<string, string>> IncomingHeaders { get; set; }
    }
}