using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialLibraryMVC.Models
{
    public class UserFavorites
    {
        [Key]
        public int Id { get; set; }
        public string User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual IdentityUser? User { get; set; }

        public long ISBN_13 { get; set; }
        [ForeignKey("ISBN_13")]
        public Book Books { get; set; }
    }
}
