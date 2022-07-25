using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLibraryMVC.Models
{
    // The author should only have basic information, the focus of this is the books and the reviews.
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Biography { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
