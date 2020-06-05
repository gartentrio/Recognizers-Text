using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Newtonsoft.Json;

namespace Recognizer
{
    public static class Program
    {
        // Use English for the Recognizers culture
        private static string _defaultCulture = Culture.German;

        public static void Main(string[] args)
        {
            // ShowIntro();

            if (args.Length == 3)
            {
                var lang = args[0];
                var type = args[1];
                var input = args[2];

                if (lang == "english")
                {
                    _defaultCulture = Culture.English;
                }
                else if (lang == "german")
                {
                    _defaultCulture = Culture.German;
                }
                else
                {
                    _defaultCulture = args[0];
                }

                var results = ParseAll(input, _defaultCulture, type);

                var output = results.ToList();

                if (output.Count == 0)
                {
                    Console.WriteLine("[]");
                }
                else
                {
                    Console.WriteLine("[");
                    Console.WriteLine(string.Join(",", output.Select(result =>
                        JsonConvert.SerializeObject(result, Formatting.Indented))));
                    Console.WriteLine("]");
                }
            }
            else
            {
                Console.WriteLine("Need 3 Arguments: language, type, input");
            }
        }

        /// <summary>
        /// Parse query with all recognizers.
        /// </summary>
        private static IEnumerable<ModelResult> ParseAll(string query, string culture, string type)
        {
            if (type == "number")
            {
                return MergeResults(NumberRecognizer.RecognizeNumber(query, culture));
            }

            if (type == "datetime")
            {
                return MergeResults(DateTimeRecognizer.RecognizeDateTime(query, culture));
            }

            return MergeResults(NumberWithUnitRecognizer.RecognizeDimension(query, culture));
        }

        private static IEnumerable<ModelResult> MergeResults(params List<ModelResult>[] results)
        {
            return results.SelectMany(o => o);
        }
    }
}