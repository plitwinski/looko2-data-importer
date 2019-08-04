using CommandDotNet;
using CommandDotNet.Attributes;
using System;

namespace LookO2.Importer.Console.Cli.Arguments
{
    public class StartImportArgs : IArgumentModel
    {
        [Option(ShortName = "s")]
        public DateTime StartDate { get; set; }

        [Option(ShortName = "e")]
        public DateTime EndDate { get; set; }
    }
}
