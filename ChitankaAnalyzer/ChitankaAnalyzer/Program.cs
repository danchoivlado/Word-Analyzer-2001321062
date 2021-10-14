using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ChitankaAnalyzer
{
    class Program
    {
        private static string text;
        private static string[] wordsArray;

        static void Main(string[] args)
        {
            text = ReadText();
            wordsArray = GetWordsFromText();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            WordAnalyzer(); // Or WordAnalyzerParallel() to test parallel version 
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }

        static void WordAnalyzerParallel()
        {
            List<Thread> threads = getTheadsList();

            foreach (var t in threads) t.Join();
        }

        static List<Thread> getTheadsList()
        {
            List<Thread> threads = new List<Thread>();

            Thread PrintNumberOfWordsThread = new Thread(PrintNumberOfWords);
            threads.Add(PrintNumberOfWordsThread);
            PrintNumberOfWordsThread.Start();

            Thread PrintShortestWordThread = new Thread(PrintShortestWord);
            threads.Add(PrintShortestWordThread);
            PrintShortestWordThread.Start();

            Thread PrintLongestWordThread = new Thread(PrintLongestWord);
            threads.Add(PrintLongestWordThread);
            PrintLongestWordThread.Start();

            Thread PrintAverageWordLengthThread = new Thread(PrintAverageWordLength);
            threads.Add(PrintAverageWordLengthThread);
            PrintAverageWordLengthThread.Start();

            Thread PrintFiveMostCommonWordsThread = new Thread(PrintFiveMostCommonWords);
            threads.Add(PrintFiveMostCommonWordsThread);
            PrintFiveMostCommonWordsThread.Start();

            Thread PrintFiveLeastCommonWordsThread = new Thread(PrintFiveLeastCommonWords);
            threads.Add(PrintFiveLeastCommonWordsThread);
            PrintFiveLeastCommonWordsThread.Start();

            return threads;
        }

        static void WordAnalyzer()
        {
            PrintNumberOfWords();
            PrintShortestWord();
            PrintLongestWord();
            PrintAverageWordLength();
            PrintFiveMostCommonWords();
            PrintFiveLeastCommonWords();
        }

        static string[] GetWordsFromText()
        {
            return text.Split(new char[] {' ', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
        }

        static string ReadText()
        {
            return System.IO.File.ReadAllText(
                @"C:\Users\gotin\source\repos\ChitankaAnalyzer\ChitankaAnalyzer/book.txt");
        }

        static void PrintNumberOfWords()
        {
            Console.WriteLine("Number of words: " + wordsArray.Length);
            Console.WriteLine();
        }

        static void PrintShortestWord()
        {
            string shortestWord = wordsArray.OrderBy(word => word.Length).First();
            Console.WriteLine($"The shortest word is {shortestWord}");
            Console.WriteLine();
        }

        static void PrintLongestWord()
        {
            string longestWord = wordsArray.OrderByDescending(word => word.Length).First();
            Console.WriteLine($"The longest word is {longestWord}");
            Console.WriteLine();
        }

        static void PrintAverageWordLength()
        {
            int allWordsLength = wordsArray.Sum(word => word.Length);
            int numberOfWords = wordsArray.Length;
            Console.WriteLine($"Average word length is {allWordsLength/numberOfWords}");
            Console.WriteLine();
        }

        static void PrintFiveMostCommonWords()
        {
            var wordGroups = wordsArray.GroupBy(word => word)
                .Select(group => new { group.Key, Count = group.Count() })
                .OrderByDescending(group => group.Count);

            Console.WriteLine("Five most common words are");
            foreach (var wordGroup in wordGroups.Take(5))
            {
                Console.WriteLine($"Word: {wordGroup.Key} repeated {wordGroup.Count} times");
            }

            Console.WriteLine();
        }

        static void PrintFiveLeastCommonWords()
        {
            var wordGroups = wordsArray.GroupBy(word => word)
                .Select(group => new { group.Key, Count = group.Count() })
                .OrderBy(group => group.Count);

            Console.WriteLine("Five least common words are");
            foreach (var wordGroup in wordGroups.Take(5))
            {
                Console.WriteLine($"Word: {wordGroup.Key} repeated {wordGroup.Count} times");
            }

            Console.WriteLine();
        }
    }
}
