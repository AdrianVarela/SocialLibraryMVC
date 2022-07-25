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
        [RegularExpression(@"^[0-9]{10}$")]
        public long? ISBN_10 { get; set; }
        [RegularExpression(@"^[0-9]{13}$")]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ISBN_13 { get; set; }
        public byte[]? Cover { get; set; }
    }

    public enum Genre
    {
        Fantasy, [Display(Name = "Science Fiction")]Science_Fiction, Dystopian, Action, Adventure, Mystery, Horror,
        Thriller, [Display(Name = "Historical Fiction")] Historical_Fiction, Romance, [Display(Name="Young Adult")]Young_Adult, Children,
        Autobiography, Biography, Cooking, Art, [Display(Name = "Self Help")]Self_Help, Motivational, Health, History, Hobbies, Families, Humor,
        Business, Law, Politics, Religion, Education, Travel, [Display(Name = "True Crime")]True_Crime
    }
}