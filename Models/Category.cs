using System.ComponentModel.DataAnnotations;

namespace ContactPro.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? AppUserId { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        // Virtual Properties for Foreign Keys
        public virtual AppUser? AppUser { get; set; }

        // The line below (see similar virtual property in Contact.cs) will create a "Category Contact" table for us
        public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
    }
}
