using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace toLowerVariants
{
    class Program
    {
        static void Main(string[] args)
        {

            //var maintainList = new List<string> { "M", "KB" };
            if (args.Length < 1)
            {
                Console.WriteLine("Please enter the path of test file when running test!");
            }
            else
            {
                string filename = args[0];
                Console.WriteLine($"Testing cases in {filename}");
                List<string> toConvert = new List<string>(System.IO.File.ReadAllLines(filename));

                Convert3_ConvertAll(toConvert, 100);
                Convert3_CheckBeforeConvert(toConvert, 100);
                Console.WriteLine("\n");
                Convert3_TestMatches(toConvert, 100);
                Convert3_TestMatchesAndCount(toConvert, 100);
                Convert3_TestMatchesAndIsMatch(toConvert, 100);
            }
            Console.ReadKey();
        }

        public static void ApplyReverse(int idx, char[] str, string value)
        {
            if (idx + value.Length > str.Length)
                throw new ArgumentException("Index out of range.");
            for (int i = 0; i < value.Length; ++i)
            {
                str[idx + i] = value[i];
            }
        }

        public static void ApplyReverse2(int idx, ref StringBuilder str, string value)
        {
            if (idx + value.Length > str.Length)
                throw new ArgumentException("Index out of range.");
            for (int i = 0; i < value.Length; ++i)
            {
                str[idx + i] = value[i];
            }
        }


        public static void Convert3_ConvertAll(List<string> toConvert, int times)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            var patternRegex = new Regex(@"(?<=(\s|\b\d+))" + allPattern + @"\b", RegexOptions.Compiled);
            var numIn = 0;
            for (int i = 0; i < times; ++i)
            {
                foreach (var str in toConvert)
                {
                    var matches = patternRegex.Matches(str);
                    var resChar = new StringBuilder(str.ToLower());
                    foreach (Match m in matches)
                    {
                        ApplyReverse2(m.Index, ref resChar, m.Value);
                    }

                    var resString = resChar.ToString();
                }
            }
            sw.Stop();
            Console.WriteLine($"convert3, convert all, takes time: {sw.Elapsed}s");
        }


        public static void Convert3_CheckBeforeConvert(List<string> toConvert, int times)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            var patternRegex = new Regex(@"(?<=(\s|\b\d+))" + allPattern + @"\b", RegexOptions.Compiled);
            var numIn = 0;
            for (int i = 0; i < times; ++i)
            {
                foreach (var str in toConvert)
                {
                    var matches = patternRegex.Matches(str);
                    var resString = str.ToLower();

                    if (matches.Count > 0)
                    {
                        var resChar = new StringBuilder(resString);
                        foreach (Match m in matches)
                        {
                            ApplyReverse2(m.Index, ref resChar, m.Value);
                        }
                        resString = resChar.ToString();
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"convert3, check matches.Count before convert, takes time: {sw.Elapsed}s");
        }

        public static void Convert3_TestMatches(List<string> toConvert, int times)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            var patternRegex = new Regex(@"(?<=(\s|\b\d+))" + allPattern + @"\b", RegexOptions.Compiled);
            var numIn = 0;
            for (int i = 0; i < times; ++i)
            {
                foreach (var str in toConvert)
                {
                    var matches = patternRegex.Matches(str);
                }
            }
            sw.Stop();
            Console.WriteLine($"convert3, only matches, no conversion, takes time: {sw.Elapsed}s");
        }


        public static void Convert3_TestMatchesAndCount(List<string> toConvert, int times)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            var patternRegex = new Regex(@"(?<=(\s|\b\d+))" + allPattern + @"\b", RegexOptions.Compiled);
            var numIn = 0;
            for (int i = 0; i < times; ++i)
            {
                foreach (var str in toConvert)
                {
                    var matches = patternRegex.Matches(str);
                    var count = matches.Count;
                }
            }
            sw.Stop();
            Console.WriteLine($"convert3, only matches and matches.Count, no conversion, takes time: {sw.Elapsed}s");
        }

        public static void Convert3_TestMatchesAndIsMatch(List<string> toConvert, int times)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            var patternRegex = new Regex(@"(?<=(\s|\b\d+))" + allPattern + @"\b", RegexOptions.Compiled);
            var numIn = 0;
            for (int i = 0; i < times; ++i)
            {
                foreach (var str in toConvert)
                {
                    var matches = patternRegex.Matches(str);
                    var ismatch = patternRegex.IsMatch(str);
                }
            }
            sw.Stop();
            Console.WriteLine($"convert3, only matches and ismatch, no conversion, takes time: {sw.Elapsed}s");
        }


        public static void Convert4(List<string> toConvert, List<string> maintainList)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            var patternRegex = @"(?<=(\s|\b\d+))" + allPattern + @"\b";
            for (int i = 0; i < 1000; ++i)
            {
                foreach (var str in toConvert)
                {

                    var res = str.ToLower().ToCharArray();
                    string result = Regex.Replace(str, patternRegex, m => m.Value.ToLower());

                    for (int idx = 0; idx < str.Length; ++idx)
                    {
                        if (result[idx] != str[idx])
                        {
                            res[idx] = str[idx];
                        }
                    }
                    var resString = new String(res);
                    //Console.WriteLine($"{resString}\n");
                }
            }
            sw.Stop();
            Console.WriteLine($"convert4, takes time: {sw.Elapsed}");
        }
        public static void Convert5(List<string> toConvert, List<string> maintainList)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            var patternRegex = new Regex(@"(?<=(\s|\b\d+))" + allPattern + @"\b", RegexOptions.Compiled);
            for (int i = 0; i < 1000; ++i)
            {
                foreach (var str in toConvert)
                {

                    var matches = patternRegex.Matches(str);
                    var res = new StringBuilder(str.ToLower());
                    var temp = str;
                    foreach (Match m in matches)
                    {
                        temp = temp.Replace(m.Value, m.Value.ToLower());
                    }
                    // Final gen
                    for (int idx = 0; idx < str.Length; ++idx)
                    {
                        if (temp[idx] != str[idx])
                        {
                            res[idx] = str[idx];
                        }
                    }
                    var resString = res.ToString();

                }
            }
            sw.Stop();
            Console.WriteLine($"convert5, takes time: {sw.Elapsed}");
        }
    }
}
