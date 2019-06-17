using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos
{
    public class ValueCreateDto
    {
        
        [Required]
        public string ValueAsString { get; set; }        
        public int ValueAsInteger { get; set; }
    
    }
}