using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Util
{
    public static class CsvReader
    {
        static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        static char[] TRIM_CHARS = { '\"' };

        public static List<T> ReadList<T>(string csvText) where T : new()
        {
            var list = new List<T>();
            var lines = Regex.Split(csvText, LINE_SPLIT_RE);

            if (lines.Length <= 1) return list;

            var headers = Regex.Split(lines[0], SPLIT_RE);
            for (var i = 1; i < lines.Length; i++)
            {
                var fields = Regex.Split(lines[i], SPLIT_RE);
                if (fields.Length == 0 || fields[0] == "") continue;

                var obj = new T();
                for (var j = 0; j < headers.Length && j < fields.Length; j++)
                {
                    string header = headers[j];
                    string field = fields[j];
                    field = field.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                    PropertyInfo property = typeof(T).GetProperty(header,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                    if (property != null && property.CanWrite)
                    {
                        object value = Convert.ChangeType(field, property.PropertyType);
                        property.SetValue(obj, value);
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        public static T ReadData<T>(string csvText) where T : class, new()
        {
            T obj = new T();
            var lines = Regex.Split(csvText, LINE_SPLIT_RE);

            if (lines.Length <= 1) return null;

            var headers = Regex.Split(lines[0], SPLIT_RE);
            for (var i = 1; i <= 2; i++)
            {
                var fields = Regex.Split(lines[i], SPLIT_RE);
                if (fields.Length == 0 || fields[0] == "") continue;

                for (var j = 0; j < headers.Length && j < fields.Length; j++)
                {
                    string header = headers[j];
                    string field = fields[j];
                    field = field.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                    PropertyInfo property = typeof(T).GetProperty(header,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                    if (property != null && property.CanWrite)
                    {
                        object value = Convert.ChangeType(field, property.PropertyType);
                        property.SetValue(obj, value);
                    }
                }
            }

            return obj;
        }

        public static Dictionary<string, Dictionary<string, float>> ReadMap(string csvText)
        {
            var dictionary = new Dictionary<string, Dictionary<string, float>>();
            var lines = Regex.Split(csvText, LINE_SPLIT_RE);

            if (lines.Length <= 1) return null;

            var headers = Regex.Split(lines[0], SPLIT_RE);



            for (var i = 1; i < lines.Length; i++)
            {
                var fields = Regex.Split(lines[i], SPLIT_RE);
                if (fields.Length == 0 || fields[0] == "") continue;

                var key = fields[0];

                dictionary.TryAdd(key, new Dictionary<string, float>());
                for (var j = 1; j < headers.Length && j < fields.Length; j++)
                {
                    string header = headers[j];
                    string field = fields[j];
                    field = field.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                    dictionary[key].TryAdd(header, float.Parse(field));
                }
            }

            return dictionary;
        }

        public static Dictionary<string, float> ReadData(string csvText)
        {
            var dictionary = new Dictionary<string, float>();
            var lines = Regex.Split(csvText, LINE_SPLIT_RE);

            if (lines.Length <= 1) return null;

            var headers = Regex.Split(lines[0], SPLIT_RE);


            var fields = Regex.Split(lines[1], SPLIT_RE);
            if (fields.Length == 0 || fields[0] == "") return null;

            for (var j = 0; j < headers.Length && j < fields.Length; j++)
            {
                string header = headers[j];
                string field = fields[j];
                field = field.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                dictionary.TryAdd(header, float.Parse(field));
            }

            return dictionary;
        }
    }
}