using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLibraryMVC.Models
{
    public class Book
    {

        [Required]
        public string Title { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }
        [Display(Name = "Author")]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author? Authors { get; set; }
        public Genre Genre { get; set; }
        public int PublishYear { get; set; }
        [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "ISBN-10 should be 10 integers long")]
        //
        public long? ISBN_10 { get; set; }
        // ISBN_13 should be 13 integers long, no longer and no shorter
        [RegularExpression(@"^[0-9]{13}$", ErrorMessage = "ISBN-13 should be 13 integers long")]
        [Required]
        [Key]
        // This option is so that the database doesn't generate its own ISBN_13 each time an entry is added, and lets the user add in the ISBN_13
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ISBN_13 { get; set; }
        public byte[]? Cover { get; set; }
    }

    // Certain genres have spaces, thus the display name should be different from the internal name
    public enum Genre
    {
        Fantasy, [Display(Name = "Science Fiction")]Science_Fiction, Dystopian, Action, Adventure, Mystery, Horror,
        Thriller, [Display(Name = "Historical Fiction")] Historical_Fiction, Romance, [Display(Name="Young Adult")]Young_Adult, Children,
        Autobiography, Biography, Cooking, Art, [Display(Name = "Self Help")]Self_Help, Motivational, Health, History, Hobbies, Families, Humor,
        Business, Law, Politics, Religion, Education, Travel, [Display(Name = "True Crime")]True_Crime
    }
}