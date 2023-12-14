using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Registration.API.Builder;
using Registration.API.Domains;
using Registration.API.Extensions;
using Registration.API.Repositories.Abstration;
using Registration.API.Services.Abstration;
using Registration.API.Validations;
using Registration.API.ViewModels;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;

namespace Registration.API.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHelperService _helperService;
        private readonly IMapper _mapper;
        private readonly HttpContext _httpContext;

        public UserService(IUserRepository userRepository, IHelperService helperService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _helperService = helperService;
            _mapper = mapper;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<(string, RegistrationApiError)> ChangeEmail(UserEmailModel userEmailModel, CancellationToken cancellationToken)
        {
            string? error;
            try
            {
                //Validation
                UserEmailValidation validationRules = new UserEmailValidation();
                ValidationResult validationResult = validationRules.Validate(userEmailModel);

                if (!validationResult.IsValid)
                {
                    string errorMessage = string.Empty;
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        errorMessage += validationFailure.ErrorMessage;
                        break;
                    }
                    return (string.Empty, new RegistrationApiError(errorMessage));
                }

                if (userEmailModel.OldEmailAddress.Equals(userEmailModel.NewEmailAddress))
                {
                    error = "Both EmailAddress Are Same";
                    return (string.Empty, new RegistrationApiError(error));
                }

                var userdata = await _userRepository.GetUserEmailAddress(userEmailModel.OldEmailAddress, cancellationToken);
                if (userdata is null)
                {
                    error = "Old EmailAddress Doesn't Match";
                    return (string.Empty, new RegistrationApiError(error));
                }


                var newEmailUserData = await _userRepository.GetUserEmailAddress(userEmailModel.NewEmailAddress, cancellationToken);
                if (newEmailUserData is not null)
                {
                    error = "New EmailAddress Is Already Exists. Please Try with different EmailAddress";
                    return (string.Empty, new RegistrationApiError(error));
                }
                userdata.EmailAddress = userEmailModel.NewEmailAddress;
                var IsEmailUpdated = await _userRepository.UpdateEmailAddress(userdata, cancellationToken);
                if (IsEmailUpdated)
                {
                    return ("EmailAddress Updated Successfully", new RegistrationApiError(string.Empty));
                }
                return (string.Empty, new RegistrationApiError("Error while updating emailaddress"));

            }
            catch (Exception ex)
            {
                error = "UserService ChangeEmail API Error : " + ex.Message;
                return (string.Empty, new RegistrationApiError(error));
            }
        }

        public async Task<(string, RegistrationApiError)> ChangePassword(UserPasswordModel userPasswordModel, CancellationToken cancellationToken)
        {
            string? error;
            try
            {
                //Validation
                UserPasswordValidation validationRules = new UserPasswordValidation();
                ValidationResult validationResult = validationRules.Validate(userPasswordModel);

                if (!validationResult.IsValid)
                {
                    string errorMessage = string.Empty;
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        errorMessage += validationFailure.ErrorMessage;
                        break;
                    }
                    return (string.Empty, new RegistrationApiError(errorMessage));
                }

                //Getting UserId
                var userId = _httpContext.User.GetUserId();

                //Getting UserData

                var userData = await _userRepository.GetUserById(userId, cancellationToken);
                if (userData is null)
                {
                    error = "User Record Not Found";
                    return (string.Empty, new RegistrationApiError(error));
                }

                //Checking for Old Password
                var isPasswordVerify = _helperService.VerifyPassword(userData.Password, userData.PasswordSalt, userPasswordModel.OldPassword);
                if (!isPasswordVerify)
                {
                    error = "Old Password Doesn't Match";
                    return (string.Empty, new RegistrationApiError(error));
                }

                // EncryptPassword
                var (password, salt) = _helperService.PasswordSalt(userPasswordModel.NewPassword);
                userData.Password = password;
                userData.PasswordSalt = salt;

                var isPasswordUpdate = await _userRepository.UpdatePassword(userData, cancellationToken);
                if (isPasswordUpdate)
                {
                    return ("Password Updated Successfully", new RegistrationApiError(string.Empty));
                }
                return (string.Empty, new RegistrationApiError("Error while updating password"));
            }
            catch (Exception ex)
            {
                error = "UserService ChangePassword API Error : " + ex.Message;
                return (string.Empty, new RegistrationApiError(error));
            }
        }

        public async Task<(UserDisplayModel, RegistrationApiError)> CreateUser(UserAddModel userAddModel, CancellationToken cancellationToken)
        {
            string? error;
            try
            {
                //Validation
                UserValidation validationRules = new UserValidation();
                ValidationResult validationResult = validationRules.Validate(userAddModel);
                if (!validationResult.IsValid)
                {
                    var errorMessage = "";
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        errorMessage += validationFailure.ErrorMessage;
                        break;
                    }
                    return (new NullUserDisplayModel(), new RegistrationApiError(errorMessage));
                }

                //CheckingExistByEmail
                var checkExistsByEmail = await _userRepository.GetUserEmailAddress(userAddModel.EmailAddress, cancellationToken);
                if (checkExistsByEmail != null)
                {
                    error = "Oops Email Address Already Exists";
                    return (new NullUserDisplayModel(), new RegistrationApiError(error));
                }

                //EncryptPassword
                var (password, salt) = _helperService.PasswordSalt(userAddModel.Password);
                var adduser = UserBuilder.Convert(userAddModel, salt, password);

                //Inserting UserData
                var isUserInserted = await _userRepository.AddUserData(adduser, cancellationToken);
                if (!isUserInserted)
                {
                    error = "Error While Adding User";
                    return (new NullUserDisplayModel(), new RegistrationApiError(error));
                }

                //Getting User Data
                var userData = await _userRepository.GetUserById(adduser.Id, cancellationToken);
                userData.Password = userAddModel.Password;
                var user = _mapper.Map<UserDisplayModel>(userData);
                return (user, new RegistrationApiError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "UserService CreateUser API Error : " + ex.Message;
                return (new NullUserDisplayModel(), new RegistrationApiError(error));
            }

        }

        public async Task<(List<UserDisplayModel>, RegistrationApiError)> GetAllUsers(CancellationToken cancellationToken)
        {
            string? error;
            try
            {
                var usersData = await _userRepository.GetAllUsers(cancellationToken);
                if (usersData is null || usersData.Count <= 0)
                {
                    error = "No Users Record Found";
                    return (new NullUserListDisplayModel(), new RegistrationApiError(error));
                }
                var users = _mapper.Map<List<UserDisplayModel>>(usersData);
                return (users, new RegistrationApiError(string.Empty));
            }
            catch (Exception ex)
            {

                error = "UserService GetAllUsers API Error : " + ex.Message;
                return (new NullUserListDisplayModel(), new RegistrationApiError(error));
            }
        }

        public async Task<(UserDisplayModel, RegistrationApiError)> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            string? error;
            try
            {
                var userData = await _userRepository.GetUserById(id, cancellationToken);
                if (userData is null)
                {
                    error = "No User Record Found";
                    return (new NullUserDisplayModel(), new RegistrationApiError(error));
                }
                var user = _mapper.Map<UserDisplayModel>(userData);
                return (user, new RegistrationApiError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "UserService GetUserById API Error : " + ex.Message;
                return (new NullUserDisplayModel(), new RegistrationApiError(error));
            }

        }

        public async Task<(string, RegistrationApiError)> Login(UserLoginModel userLogin, CancellationToken cancellationToken)
        {
            string? error;
            try
            {
                //Validation
                LoginValidation validationRules = new LoginValidation();
                ValidationResult validationResult = validationRules.Validate(userLogin);
                if (!validationResult.IsValid)
                {
                    string errorMessage = string.Empty;
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        errorMessage += validationFailure.ErrorMessage;
                    }
                    return (string.Empty, new RegistrationApiError(errorMessage));
                }

                var userData = await _userRepository.GetUserEmailAddress(userLogin.EmailAddress);
                if (userData is null)
                {
                    error = "EmailAddress Not Found";
                    return (string.Empty, new RegistrationApiError(error));
                }
                var isPasswordVerify = _helperService.VerifyPassword(userData.Password, userData.PasswordSalt, userLogin.Password);

                if (!isPasswordVerify)
                {
                    error = "Invaid Password";
                    return (string.Empty, new RegistrationApiError(error));
                }

                //Generate Token
                var token = _helperService.Authenticate(userData);
                return (token, new RegistrationApiError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "UserService Login API Error " + ex.Message;
                return (string.Empty, new RegistrationApiError(error));
            }
        }

        public async Task<(string, RegistrationApiError)> RemoveUser(Guid id, CancellationToken cancellationToken)
        {
            string? error;
            try
            {
                //Gettting User Data
                var userData = await _userRepository.GetUserById(id, cancellationToken);
                if (userData is null)
                {
                    error = "User Not Found By Id : " + id;
                    return (string.Empty, new RegistrationApiError(error));
                }

                var user = await _userRepository.RemoveUser(userData, cancellationToken);
                if (user)
                {
                    return ("User Removed Successfully", new RegistrationApiError(string.Empty));
                }
                return (string.Empty, new RegistrationApiError("Error while removing user"));
            }
            catch (Exception ex)
            {
                error = "UserService RemoveUser API Error " + ex.Message;
                return (string.Empty, new RegistrationApiError(error));
            }
        }

        public async Task<(string, RegistrationApiError)> SendOTP(string emailAddress, CancellationToken cancellationToken)
        {
            string? error;
            try
            {
                var userData = await _userRepository.GetUserEmailAddress(emailAddress, cancellationToken);
                if (userData is null)
                {
                    error = "User Record Not Found";
                    return (string.Empty, new RegistrationApiError(error));
                }

                var otp = _helperService.GenerateRandomOtp();
                var addOTPModel = new AddOTP()
                {
                    UserId = userData.Id,
                    Otp = otp
                };

                var userOTPModel = _mapper.Map<UserOtp>(addOTPModel);
                bool IsOTPInserted = await _userRepository.AddingOTP(userOTPModel, cancellationToken);
                if (IsOTPInserted)
                {
                    await _helperService.SendEmail(userData.EmailAddress, "OTP verification", "Dear " + userData.Name + ", \nHere is the OTP for your account login is : " + otp);
                    return ("OTP Sended Successfully", new RegistrationApiError(string.Empty));
                }
                return (string.Empty, new RegistrationApiError("Error While Sending OTP"));
            }
            catch (Exception ex)
            {
                error = "UserService SendOTP API Error : " + ex.Message;
                return (string.Empty, new RegistrationApiError(error));
            }
        }

        public async Task<(UserDisplayModel, RegistrationApiError)> UpdateUser(Guid id, UserUpdateModel userUpdateModel, CancellationToken cancellationToken)
        {
            string? error;
            try
            {
                //Validation
                UpdateUserValidation validationRules = new UpdateUserValidation();
                ValidationResult validationResult = validationRules.Validate(userUpdateModel);
                if (!validationResult.IsValid)
                {
                    var errorMessage = "";
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        errorMessage += validationFailure.ErrorMessage;
                        break;
                    }
                    return (new NullUserDisplayModel(), new RegistrationApiError(errorMessage));
                }

                //Getting User and Mapping it
                var userData = await _userRepository.GetUserById(id, cancellationToken);
                if (userData is null)
                {
                    error = "User Not Found By Id : " + id;
                    return (new NullUserDisplayModel(), new RegistrationApiError(error));
                }
                var updatedUser = _mapper.Map<UserUpdateModel, User>(userUpdateModel, userData);

                //Updating User Data
                var IsUserUpdated = await _userRepository.UpdateUser(updatedUser, cancellationToken);

                if (!IsUserUpdated)
                {
                    error = "Error while updating user";
                    return (new NullUserDisplayModel(), new RegistrationApiError(error));
                }
                var user = _mapper.Map<UserDisplayModel>(updatedUser);
                return (user, new RegistrationApiError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "UserService UpdateUser API Error : " + ex.Message;
                return (new NullUserDisplayModel(), new RegistrationApiError(error));
            }
        }

        public async Task<(string, RegistrationApiError)> VerifyOTP(string userEmailAddress, int OTP, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                var userData = await _userRepository.GetUserEmailAddress(userEmailAddress, cancellationToken);
                var userOTPData = await _userRepository.GetUserOTP(userData, cancellationToken);
                if (userData is null)
                {
                    error = "User Record Not Found";
                    return (string.Empty, new RegistrationApiError(error));
                }
                var datediff = DateTime.Now.Subtract(userOTPData.SendDateTime);
                if (datediff.TotalSeconds <= 90 && userOTPData.Otp.Equals(OTP))
                {
                    return ("OTP Verified Successfully", new RegistrationApiError(string.Empty));
                }
                else
                {
                    error = "OTP Not Verified";
                    return (string.Empty, new RegistrationApiError(error));
                }
            }
            catch (Exception ex)
            {
                error = "UserService VerifyOtp API Error : " + ex.Message;
                return (string.Empty, new RegistrationApiError(error));
            }

        }
    }
}
