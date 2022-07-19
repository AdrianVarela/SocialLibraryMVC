using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLibraryMVC.Models
{
    public class Review
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public string User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual ApplicationUser User { get; set; }

	    public string? Text_review {get;set; }
        [Required]
	    public int Rating { get; set; }
        public long Isbn_13 { get; set; }
        [ForeignKey("book_id")]
	    public Book Books { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        public virtual Review Reviews { get; set; }
    }
}

