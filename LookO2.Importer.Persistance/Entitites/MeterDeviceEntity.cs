using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LookO2.Importer.Persistance.Entitites
{
    public class MeterDeviceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string DeviceId { get; set; }

        public string Name { get; set; }
    }
}
