using System;
using System.IO;
using System.Net;
using System.Text;

namespace MonitorCardiacoRealTime
{
    class Program
    {
        private static string realTimePushURL = "https://api.powerbi.com/beta/5ecd43d4-43af-45c5-a552-97627e83c4ab/datasets/13afbc87-08f9-41e6-a73f-06676d797c79/rows?key=sjBEapK%2FtFKSfFN3wm4QHzMVNANAtc41fm2n736PN39lglALd9SVjjIx7HfgcYv5DlNeclMqg73gEKc5QHCvLA%3D%3D"; 

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    // Declare values that we're about to send
                    String currentTime = DateTime.UtcNow.ToString();
                    Random r = new Random();
                    int currentValue = r.Next(50, 200);
                    int min = r.Next(1, 50);
                    int max = r.Next(200,250);

                    // Send POST request to the push URL
                    // Uses the WebRequest sample code as documented here: https://msdn.microsoft.com/en-us/library/debx8sh9(v=vs.110).aspx
                    WebRequest request = WebRequest.Create(realTimePushURL);
                    request.Method = "POST";
                    // The data should be in JSON format
                    //string postData = $"[{{ 'ts': '{currentTime}', 'value':{currentValue} }}]";
                    string postData = String.Format(
                "[{{ \"ts\": \"{0}\", \"value\":{1}, \"max\":{2}, \"min\":{3} }}]", 
                currentTime, currentValue, max, min);
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
