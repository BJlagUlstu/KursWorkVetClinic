
namespace KursWorkVetClinicBusinessLogic.ViewModels
{
    public class UserViewModel
    {
        public int? UserId { get; set; }
        public string Login { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public override string ToString()
        {
            return Fullname;
        }
    }
}