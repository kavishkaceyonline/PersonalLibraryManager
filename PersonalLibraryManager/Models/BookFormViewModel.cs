using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalLibraryManager.Models
{
    public class BookFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the book title")]
        [StringLength(255)]
        [Display(Name = "Book Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter the author name")]
        [StringLength(255)]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [StringLength(50)]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars")]
        [Display(Name = "Rating (1-5 stars)")]
        public int? Rating { get; set; }

        [StringLength(2000, ErrorMessage = "Review cannot exceed 2000 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Personal Review")]
        public string Review { get; set; }

        [Required(ErrorMessage = "Please select a reading status")]
        [Display(Name = "Reading Status")]
        public ReadingStatus Status { get; set; }

        [Display(Name = "Date Completed")]
        [DataType(DataType.Date)]
        public DateTime? DateCompleted { get; set; }

        public string Heading { get; set; }
    }
}