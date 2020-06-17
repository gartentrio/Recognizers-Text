using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Choice;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Sequence;
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
            return type switch
            {
                "number" => NumberRecognizer.RecognizeNumber(query, culture),
                "ordinal" => NumberRecognizer.RecognizeOrdinal(query, culture),
                "percentage" => NumberRecognizer.RecognizePercentage(query, culture),
                "numberrange" => NumberRecognizer.RecognizeNumberRange(query, culture),
                "age" => NumberWithUnitRecognizer.RecognizeAge(query, culture),
                "currency" => NumberWithUnitRecognizer.RecognizeCurrency(query, culture),
                "dimension" => NumberWithUnitRecognizer.RecognizeDimension(query, culture),
                "temperature" => NumberWithUnitRecognizer.RecognizeTemperature(query, culture),
                "datetime" => DateTimeRecognizer.RecognizeDateTime(query, culture),
                "phonenumber" => SequenceRecognizer.RecognizePhoneNumber(query, culture),
                "ipaddress" => SequenceRecognizer.RecognizeIpAddress(query, culture),
                "mention" => SequenceRecognizer.RecognizeMention(query, culture),
                "hashtag" => SequenceRecognizer.RecognizeHashtag(query, culture),
                "email" => SequenceRecognizer.RecognizeEmail(query, culture),
                "url" => SequenceRecognizer.RecognizeURL(query, culture),
                "guid" => SequenceRecognizer.RecognizeGUID(query, culture),
                "boolean" => ChoiceRecognizer.RecognizeBoolean(query, culture),
                _ => new List<ModelResult>()
            };
        }

        private static IEnumerable<ModelResult> MergeResults(params List<ModelResult>[] results)
        {
            return results.SelectMany(o => o);
        }
    }
}