using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameJam
{
    public static class BackgroundGenerator
    {
        private static string[] tileNames = 
        { "T01B","T01M", "T01R",
            "T02B","T02M", "T02R",
            "T03B","T03M", "T03R",
            "T04B","T04M", "T04R",
            "T05B","T05M", "T05R",
            "T06B","T06M", "T06R",
            "T07B","T07M", "T07R",
            "T08B","T08M", "T08R",
            "T09B","T09M", "T09R",
        };

        public static Dictionary<string, TileRef> Generate()
        {
            List<string> names = new List<string>(tileNames);
            Dictionary<string, TileRef> map = new Dictionary<string, TileRef>();

            Random rand = new Random();
            int count = 1;
            int rowCount = 0;
            int colCount = 0;

            for (int x = 1; x <= 3; x++)
            {
                for (int i = 0; i <= 2; i++)
                {
                    var returnedNames = from n in names
                               where n.Contains(count.ToString())
                               select n;

                    string name = returnedNames.ToList()[rand.Next(0, returnedNames.Count())];
                
                    if (!string.IsNullOrEmpty(name))
                    {
                        map.Add(name, new TileRef(rowCount, colCount));
                        names.Remove(name);
                        colCount++;
                        count++;
                    }      
                }

                rowCount++;
                colCount = 0;
            }

            return map;
        }
    }
}
