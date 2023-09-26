using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class ValueCreateDto
    {

        [Required]
        public string ValueAsString { get; set; }
        public int ValueAsInteger { get; set; }

    }
}