using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.RegularExpressions;


namespace toLowerVariants
{
    class Program
    {
        static void Main(string[] args)
        {
            //CompileAndSaveRegex();
            if (args.Length < 1)
            {
                Console.WriteLine("Please enter the path of test file when running test!");
            }
            else
            {
                string filename = args[0];
                Console.WriteLine($"Testing cases in {filename}");
                List<string> toConvert = new List<string>(System.IO.File.ReadAllLines(filename));

                ToLowerTermSensitive(toConvert, 1000);
                ToLowerTermSensitiveCheckByCount(toConvert, 1000);
                ToLowerTermSensitiveCheckByToList(toConvert, 1000);
                ToLowerTermSensitiveCheckByCountUseAssembledRegex(toConvert, 1000);
            }
            Console.ReadKey();
        }


        public static void ReApplyValue(int idx, ref StringBuilder outString, string value)
        {
            for (int i = 0; i < value.Length; ++i)
            {
                outString[idx + i] = value[i];
            }
        }


        public static void ToLowerTermSensitive(List<string> toConvert, int times)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            var patternRegex = new Regex(@"(?<=(\s|\b\d+))" + allPattern + @"\b", RegexOptions.Compiled);
            for (int i = 0; i < times; ++i)
            {
                foreach (var str in toConvert)
                {
                    var matches = patternRegex.Matches(str);
                    var resChar = new StringBuilder(str.ToLower());
                    foreach (Match m in matches)
                    {
                        ReApplyValue(m.Index, ref resChar, m.Value);
                    }

                    var resString = resChar.ToString();
                }
            }
            sw.Stop();
            Console.WriteLine($"convert without count check, takes time: {sw.Elapsed}s");
        }


        public static void ToLowerTermSensitiveCheckByToList(List<string> toConvert, int times)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            var patternRegex = new Regex(@"(?<=(\s|\b\d+))" + allPattern + @"\b", RegexOptions.Compiled);
            for (int i = 0; i < times; ++i)
            {
                foreach (var str in toConvert)
                {
                    var matches = patternRegex.Matches(str).OfType<Match>().ToList(); ;
                    var resString = str.ToLower();
                    if (matches.Count > 0)
                    {
                        var resChar = new StringBuilder(resString);
                        foreach (Match m in matches)
                        {
                            ReApplyValue(m.Index, ref resChar, m.Value);
                        }
                        resString = resChar.ToString();
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"check by toList before convert, takes time: {sw.Elapsed}s");
        }

        public static void ToLowerTermSensitiveCheckByCount(List<string> toConvert, int times)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            var patternRegex = new Regex(@"(?<=(\s|\b\d+))" + allPattern + @"\b", RegexOptions.Compiled);
            for (int i = 0; i < times; ++i)
            {
                foreach (var str in toConvert)
                {
                    var matches = patternRegex.Matches(str); ;
                    var resString = str.ToLower();

                    if (matches.Count > 0)
                    {
                        var resChar = new StringBuilder(resString);
                        foreach (Match m in matches)
                        {
                            ReApplyValue(m.Index, ref resChar, m.Value);
                        }
                        resString = resChar.ToString();
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"check with matches.Count() before convert, takes time: {sw.Elapsed}s");
        }


        public static void CompileAndSaveRegex()
        {
            string allPattern = @"(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)";
            RegexCompilationInfo TermPattern =new RegexCompilationInfo(@"(?<=(\s|\b\d+))" + allPattern + @"\b",
                                                                        RegexOptions.Compiled,
                                                                        "TermPattern",
                                                                        "Utilities.RegularExpressions",
                                                                        true);
            RegexCompilationInfo[] regexes = { TermPattern };
            AssemblyName assemName = new AssemblyName("RegexLib, Version=1.0.0.1001, Culture=neutral, PublicKeyToken=null");
            Regex.CompileToAssembly(regexes, assemName);
        }

        public static void ToLowerTermSensitiveCheckByCountUseAssembledRegex(List<string> toConvert, int times)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            TermPattern patternRegex = new TermPattern();
            for (int i = 0; i < times; ++i)
            {
                foreach (var str in toConvert)
                {
                    var matches = patternRegex.Matches(str); ;
                    var resString = str.ToLower();

                    if (matches.Count > 0)
                    {
                        var resChar = new StringBuilder(resString);
                        foreach (Match m in matches)
                        {
                            ReApplyValue(m.Index, ref resChar, m.Value);
                        }
                        resString = resChar.ToString();
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"check before convert, and use assembled regex, takes time: {sw.Elapsed}s");
        }
    }
}
