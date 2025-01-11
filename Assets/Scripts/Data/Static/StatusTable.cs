using System.Collections.Generic;
using Util;

namespace Data.Static
{
    public class StatusTable
    {
        public StatusTable(string csvText)
        {
            Table = CsvReader.ReadMap(csvText);
        }

        public readonly Dictionary<string, Dictionary<string, float>> Table;
    }
}