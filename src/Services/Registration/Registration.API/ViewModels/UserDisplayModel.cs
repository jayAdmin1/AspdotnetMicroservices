namespace Registration.API.ViewModels
{
    public class UserDisplayModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
    }
    public class NullUserDisplayModel : UserDisplayModel { }
    public class NullUserListDisplayModel : List<UserDisplayModel> { }
}
