using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Crawler
{
    class Crawler
    {
        // Declare an event handler.
        public event EventHandler<LinkFoundEventArgs> LinkFound;

        public void Crawl(string url)
        {
            try
            {
                // -- Establish connection --
                // WebRequest object - request
                // WebResponse object - response
                // StreamReader object to get response stream - reader
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string html = reader.ReadToEnd();
                    List<string> links = GetLinks(html);

                    // Recursive call to 'Crawl()' on each link
                    foreach (var link in links)
                        Crawl(link);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private List<string> GetLinks(string html)
        {
            List<string> links = new List<string>();

            // 'html' is a long string,
            // we need to find start and end index for each URL inside that string to extract the address.
            int startIndex = 0, endIndex = 0;

            while (true)
            {
                // Find start index of a link
                startIndex = html.IndexOf("\"https://", endIndex + 1);
                if (startIndex == -1)
                    break;

                // Find end index of a link
                endIndex = html.IndexOf("\"", startIndex + 1);
                if (endIndex == -1)
                    break;

                // Get the link
                string link = html.Substring(startIndex + 1, endIndex - startIndex - 1);

                // Add the link to the list of links if it does not exist in it.
                if(!links.Contains(link))
                    links.Add(link);

                // Invoke an event when a link was found 
                LinkFound?.Invoke(this, new LinkFoundEventArgs(link));
            }

            return links;
        }
    }
}
