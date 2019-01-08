using System.Collections.Generic;

namespace IranianMusic.Instruments.Consumer1.Tests
{
    internal class ResponseVM
    {
        public string message { get; set; }
        public string result { get; set; }
        public Dictionary<string,string> data { get; set; }
    }
}