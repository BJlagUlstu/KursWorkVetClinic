using System.ComponentModel.DataAnnotations;

namespace KursWorkVetClinicDatabaseImplements.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Password { get; set; }
    }
}