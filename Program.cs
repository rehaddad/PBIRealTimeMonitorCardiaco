using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonitorCardiacoRealTime
{
    class Program
    {
        //private static string realTimePushURL = "https://api.powerbi.com/beta/6ecd73d4-43af-45c5-a552-97627e83c4ab/datasets/ba990c8a-4ef6-40a4-8620-fdbc68096cde/rows?key=%2FveE50MPU%2FgzAdsVxOhz4J65A3IFSuhkUdxEXrHGpKzEAstX%2BioNq4%2BWKDEiD3u1viOqOQns1%2FQdcWDe4%2BjG%2BA%3D%3D";
        private static string realTimePushURL = "https://api.powerbi.com/beta/8c07f23c-e1b4-48a9-83fe-31b66589cc67/datasets/290ffc9b-f1b9-418b-9722-658d6827f1fe/rows?key=FDE58YVhyLidMLaJfNJsLsuDLz8BHv1SlOQar8kQDKfLcD6tP0ZhFKYP234nJ9pKbP5ToEHO1sUBl5t5gFIPNg%3D%3D";

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    // Declare values that we're about to send
                    String currentTime = DateTime.UtcNow.ToString();
                    Random r = new Random();
                    int currentValue = r.Next(60, 190);

                    // Send POST request to the push URL
                    // Uses the WebRequest sample code as documented here: https://msdn.microsoft.com/en-us/library/debx8sh9(v=vs.110).aspx
                    WebRequest request = WebRequest.Create(realTimePushURL);
                    request.Method = "POST";
                    // The data should be in JSON format
                    //string postData = $"[{{ 'ts': '{currentTime}', 'value':{currentValue} }}]";
                    string postData = String.Format("[{{ \"ts\": \"{0}\", \"value\":{1} }}]", currentTime, currentValue);
                    Console.WriteLine($"Dados enviados via POST: {postData}");

                    // Prepare request for sending
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = byteArray.Length;

                    // Get the request stream.
                    Stream dataStream = request.GetRequestStream();

                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    // Close the Stream object.
                    dataStream.Close();

                    // Get the response.
                    WebResponse response = request.GetResponse();

                    // Display the status.
                    Console.WriteLine($"Service response: {((HttpWebResponse)response).StatusCode}");

                    // Get the stream containing content returned by the server.
                    dataStream = response.GetResponseStream();

                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);

                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();

                    // Display the content.
                    Console.WriteLine(responseFromServer);

                    // Clean up the streams.
                    reader.Close();
                    dataStream.Close();
                    response.Close();

                    // Wait 1 second before sending
                    System.Threading.Thread.Sleep(1000);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
