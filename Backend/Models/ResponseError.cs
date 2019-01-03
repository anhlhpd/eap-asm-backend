using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ResponseError
    {
        public ResponseError(string message, int statusCode)
        {
            this.message = message;
            this.statusCode = statusCode;
        }
        [NotMapped]
        public string message { get; set; }
        [NotMapped]
        public int statusCode { get; set; }
    }
}
