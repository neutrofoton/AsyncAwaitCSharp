using System;
namespace AsyncAwaitCSharp
{
    public class WebsiteDataModel
    {
        public WebsiteDataModel(string url, string data)
        {
            this.Url = url;
            this.Data = data;
        }

        public string Url { get; private set; }
        public string Data { get; private set; }

        public override string ToString()
        {
            return $"{ this.Url } downloaded { this.Data.Length } characters long. { Environment.NewLine }";
        }
    }
}
