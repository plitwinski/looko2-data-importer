using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LookO2.Importer.Core
{
    public class ArchivedFileUrlGenerator : IEnumerator<string>
    {
        private readonly IEnumerator<DateTime> dateRange;

        public ArchivedFileUrlGenerator(DateTime startDate, DateTime endDate)
        {
            dateRange = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                                      .Select(offset => startDate.AddDays(offset))
                                      .GetEnumerator();
        }

        public string Current
        {
            get
            {
                if(dateRange.Current.Year < DateTime.Now.Year)
                    return $"http://looko2.com/Archives/{dateRange.Current.Year}/Archives_{dateRange.Current.ToString("yyyy-MM-dd")}.csv";
                return $"http://looko2.com/Archives/Archives_{dateRange.Current.ToString("yyyy-MM-dd")}.csv";

            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
            => dateRange.Dispose();

        public bool MoveNext()
            => dateRange.MoveNext();

        public void Reset()
            => dateRange.Reset();
    }
}
