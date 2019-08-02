using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LookO2.Importer.Persistance.Entitites
{
    public class MeterReadingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public double PM1 { get; set; }

        public double PM25 { get; set; }

        public double PM10 { get; set; }

        public int DeviceId { get; set; }

        public double? Hcho { get; set; }

        public double? Temerature { get; set; }

        public double? Humidity { get; set; }

        public MeterDeviceEntity Device { get; set; }
    }
}
