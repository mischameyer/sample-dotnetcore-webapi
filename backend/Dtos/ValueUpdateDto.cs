using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos
{
    public class ValueUpdateDto
    {
        public string ValueAsString { get; set; }        
        public int ValueAsInteger { get; set; }
    }
}