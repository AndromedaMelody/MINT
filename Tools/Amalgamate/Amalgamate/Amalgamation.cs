using System;
using System.Collections.Generic;
using System.IO;

namespace Amalgamate
{
    public class Amalgamation : Object
    {
        public string project;
        public string target;
        public HashSet<string> sources;
        public HashSet<string> includePaths;
        public bool verbose;
        public string sourcePath;
        public HashSet<string> includedFiles;

        public Amalgamation()
        {
            this.project = "Mouri's Internal NT API Collections (MINT)";
            this.target = "MINT.h";
            this.sources = new() { "phnt_windows.h", "phnt.h" };
            this.includePaths = new() { "." };
            this.verbose = true;
            this.sourcePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\..\..\Source";
            this.includedFiles = new();
            return;
        }

        public string ActualPath(string filePath)
        {
            if (!Path.IsPathRooted(filePath))
            {
                return Path.Combine(this.sourcePath, filePath);
            }
            else
            {
                return Path.GetFullPath(filePath);
            }
        }

        public string FindIncludedFile(string filePath, string sourceDir)
        {
            HashSet<string> searchDirs = new(this.includePaths);
            if (sourceDir != null)
            {
                searchDirs.Add(sourceDir);
            }
            foreach (string dir in searchDirs)
            {
                string path = Path.Combine(dir, filePath);
                if (File.Exists(ActualPath(path)))
                {
                    return path;
                }
            }
            return null;
        }

        public void Generate()
        {
            string amalgamation = $"// {DateTime.Now.ToString()}";
            amalgamation += Environment.NewLine;
            if (this.verbose)
            {
                Console.WriteLine("Config:");
                Console.WriteLine($"    target        = {this.target}");
                Console.WriteLine($"    working_dir   = {Environment.CurrentDirectory}");
                foreach (string item in this.includePaths)
                {
                    Console.WriteLine($"    include_paths = {item}");
                }
            }
            Console.WriteLine("Creating amalgamation:");
            foreach (string path in this.sources)
            {
                Console.WriteLine($"  - processing '{path}'");
                TranslationUnit t = new(path, this, true);
                amalgamation += t.content;
                amalgamation += Environment.NewLine;
            }
            try
            {
                File.WriteAllText(this.target, amalgamation, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("... done!");
            Console.WriteLine();
            if (this.verbose)
            {
                Console.WriteLine("Files processed:");
                foreach (var item in this.sources)
                {
                    Console.WriteLine($"    {this.sources}");
                }
                Console.WriteLine("Files included:");
                foreach (string item in this.includedFiles)
                {
                    Console.WriteLine($"     {this.includedFiles}");
                }
                Console.WriteLine();
            }
            return;
        }
    }
}
