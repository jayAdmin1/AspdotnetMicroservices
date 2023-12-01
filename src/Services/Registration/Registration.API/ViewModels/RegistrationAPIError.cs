namespace Registration.API.ViewModels
{
    public class RegistrationApiError
    {
        public string Message { get; set; }
        public RegistrationApiError(string message)
        {
            Message = message;
        }
    }
}
