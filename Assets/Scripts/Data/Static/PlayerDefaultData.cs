using System.Collections.Generic;
using Util;

namespace Data.Static
{
    public class PlayerDefaultData
    {
        public readonly Dictionary<string, float> Data;

        public PlayerDefaultData(string csvText)
        {
            Data = CsvReader.ReadData(csvText);
        }
    }
}