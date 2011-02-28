using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace LongRunningHttpRequest.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = HttpWebRequest.Create("http://longrunning.local/foo.SampleAsync");

            request.Method = "GET";

            var response = request.GetResponse();

            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            while(stream.CanRead)
            {
                System.Console.Write(reader.ReadLine());
            }
        }
    }
}
