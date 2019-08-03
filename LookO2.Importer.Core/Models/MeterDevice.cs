using System;

namespace LookO2.Importer.Core.Models
{
    public class MeterDevice
    {
        public string Id { get; }
        public string Name { get; }

        public MeterDevice(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public MeterDevice(ReadOnlySpan<char> id, ReadOnlySpan<char> name)
        {
            Id = id.ToString();
            Name = name.ToString();
        }
    }
}
