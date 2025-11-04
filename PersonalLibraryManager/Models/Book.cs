using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalLibraryManager.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Book title is required")]
        [StringLength(255)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        [StringLength(255)]
        public string Author { get; set; }

        [StringLength(50)]
        public string ISBN { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int? Rating { get; set; }

        [StringLength(2000)]
        public string Review { get; set; }

        [Required]
        public ReadingStatus Status { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? DateCompleted { get; set; }

        // Foreign Key
        [Required]
        public string UserId { get; set; }

        // Navigation property
        public ApplicationUser User { get; set; }
    }

    public enum ReadingStatus
    {
        ToRead = 1,
        Reading = 2,
        Completed = 3
    }
}