using System;
using System.ComponentModel.DataAnnotations;

namespace LTS.Candela.API.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int TranslationCredits { get; set; }
    }
}
