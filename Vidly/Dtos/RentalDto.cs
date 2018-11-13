using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.Dtos
{
    public class RentalDto
    {
        public int Id { get; set; }

        [Required]
        public CustomerDto Customer { get; set; }

        [Required]
        public MovieDto Movie { get; set; }

        public DateTime DateRented { get; set; }

        public DateTime? DateReturned { get; set; }
        public bool Active { get; set; }
    }
}