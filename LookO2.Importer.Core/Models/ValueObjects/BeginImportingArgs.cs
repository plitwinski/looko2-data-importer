using System;

namespace LookO2.Importer.Core.Models.ValueObjects
{
    public struct BeginImportingArgs
    {
        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public BeginImportingArgs(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
