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
        // IdentityUser is required when working with identities (users)
        [ForeignKey("User_id")]
        public virtual IdentityUser? User { get; set; }
        [Display(Name = "Text Review")]
	    public string? Text_review {get;set; }
        [Required]
        // Rating should be 1 through 5
        [Range(1,5)]
	    public int Rating { get; set; }
        public long Isbn_13 { get; set; }
        [ForeignKey("Isbn_13")]
	    public virtual Book? Books { get; set; }
    }

}

