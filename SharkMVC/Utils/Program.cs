using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    class Program
    {
        static void Main(string[] args)
        {
            var Games = GetUsedGames();

            
        }

        private static DataTable GetUsedGames()
        {
            var usedGames = new DataTable();
            string urlAddress = "http://www.public.gr/cat/gaming/used-games/ps4-used/?No=0";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                //File.WriteAllText(@"C:\Projects\shark\myhtml.html", data);
                //data = File.ReadAllText(@"C:\Projects\shark\myhtml.html");

                var index = data.IndexOf("<div class=\"ui-table-cell filter-label\">");
                var noOfUsedGames = data.Substring(index, data.Length - index - 1);
                index = noOfUsedGames.IndexOf("</div>");
                noOfUsedGames = noOfUsedGames.Substring(0, index);
                index = noOfUsedGames.IndexOf("<div>");
                noOfUsedGames = noOfUsedGames.Substring(index, noOfUsedGames.Length - index);
                noOfUsedGames = noOfUsedGames.Split('>')[1].ToString().Split(' ')[0].ToString();
                if (Int32.TryParse(noOfUsedGames, out int noOfUsedGamesInt))
                    Console.WriteLine(noOfUsedGamesInt);
                else
                    Console.WriteLine("String could not be parsed.");

                int pages = (noOfUsedGamesInt / 18) + ((noOfUsedGamesInt % 18) > 0 ? 1 : 0);
                for (int i = 0; i < pages; i++)
                {
                    urlAddress = "http://www.public.gr/cat/gaming/used-games/ps4-used/?No=" + i * 18;

                    request = (HttpWebRequest)WebRequest.Create(urlAddress);
                    response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        receiveStream = response.GetResponseStream();
                        readStream = null;

                        if (response.CharacterSet == null)
                        {
                            readStream = new StreamReader(receiveStream);
                        }
                        else
                        {
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        }

                        data = readStream.ReadToEnd();

                        var productListIndexStart = data.IndexOf("<div class=\"product-list\">");
                        var productListIndexEnd = data.IndexOf("<hr>");
                        var UsedGames = data.Substring(productListIndexStart, data.Length - productListIndexEnd);
                    }
                }
                response.Close();
                readStream.Close();
            }

            return usedGames;
        }
    }
}
