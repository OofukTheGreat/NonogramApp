using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NonogramApp.Models; 
using NonogramApp.Services;
using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NonogramApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using static Java.Util.Jar.Attributes;

namespace NonogramApp.ViewModels
{
    public class SignupPageViewModel:ViewModelBase
    {
        private NonogramService service;
        private readonly IServiceProvider serviceProvider;
        public SignupPageViewModel(NonogramService service, IServiceProvider serviceProvider)
        {
            this.service = service;
            RegistrationCommand = new Command(Signup);
            CancelCommand = new Command(OnCancel);
            this.serviceProvider = serviceProvider;
        }
        private string displayName;
        private int? userId { get; set; }
        private string? nameError;
        private string email;
        private string password;
        private string? password_error;
        private string user_type;
        private byte[] profilePicture;
        // הוספת אובייקט ממחלקת השירותים שיוכל להפעיל את הפונקציות במחלקה
        public ICommand CancelCommand { get; set; }
        public ICommand SignupCommand { get; set; }
        public string DisplayName
        {
            get
            {
                return displayName;
            }
            set
            {
                displayName = value;
                NameError = ""; // איפוס שגיאת שם המשתמש
                OnPropertyChanged(nameof(DisplayName));
                // בדיקת תקינות שם המשתמש
                if (!string.IsNullOrEmpty(DisplayName))
                {
                    if (char.IsDigit(DisplayName[0]))
                    {
                        NameError = "Display name can't start with a number";
                        OnPropertyChanged(nameof(DisplayName));
                    }
                }
            }
        }
        public string NameError
        {
            get
            {
                return nameError;
            }
            set
            {
                nameError = value;
                OnPropertyChanged(nameof(NameError));
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string? Password
        {
            get { return password; }
            set
            {
                password = value;
                Password_Error = "";
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(NameError));
                if (string.IsNullOrEmpty(password))
                {
                    Password_Error = ""; // איפוס השגיאה אם השדה ריק
                }
                else
                {
                    if (password != null)
                    {
                        bool IsPasswordOk = IsValidPassword(password);
                        if (!IsPasswordOk)
                        {
                            Password_Error = "!!סיסמה חייבת להכיל לפחות אות גדולה אחת ומספר!!";
                        }
                    }
                }
            }
        }
        private bool IsValidPassword(string password)
        {
            bool hasUpperCase = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUpperCase = true;
                }
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }

                if (hasUpperCase && hasDigit)
                {
                    break; // אם מצאנו כבר גם אות גדולה וגם ספרה, אפשר לעצור את הלולאה
                }
            }
            return hasUpperCase && hasDigit;
        }
        public string Password_Error
        {
            get { return password_error; }
            set
            {
                password_error = value;
                OnPropertyChanged(nameof(Password_Error));
                //OnPropertyChanged(nameof(CanRegister));
            }
        }
        public ICommand RegistrationCommand
        {
            get; private set;
        }
        private void OnCancel()
        {
            DisplayName = "";
            Email = "";
            Password = "";
            // Navigate to the Register View page
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<LoginPage>());
        }
        private void ValidateName()
        {
            this.ShowNameError = string.IsNullOrEmpty(Name);
        }
        private void ValidateLastName()
        {
            this.ShowLastNameError = string.IsNullOrEmpty(LastName);
        }
        private void ValidateEmail()
        {
            this.ShowEmailError = string.IsNullOrEmpty(Email);
            if (!ShowEmailError)
            {
                //check if email is in the correct format using regular expression
                if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    EmailError = "Email is not valid";
                    ShowEmailError = true;
                }
                else
                {
                    EmailError = "";
                    ShowEmailError = false;
                }
            }
            else
            {
                EmailError = "Email is required";
            }
        }
        private void ValidatePassword()
        {
            //Password must include characters and numbers and be longer than 4 characters
            if (string.IsNullOrEmpty(password) ||
                password.Length < 4 ||
                !password.Any(char.IsDigit) ||
                !password.Any(char.IsLetter))
            {
                this.ShowPasswordError = true;
            }
            else
                this.ShowPasswordError = false;
        }
        public async void Signup()
        {
            ValidateName();
            ValidateLastName();
            ValidateEmail();
            ValidatePassword();

            if (!ShowNameError && !ShowLastNameError && !ShowEmailError && !ShowPasswordError)
            {
                //Create a new AppUser object with the data from the registration form
                var newUser = new AppUser
                {
                    UserName = Name,
                    UserLastName = LastName,
                    UserEmail = Email,
                    UserPassword = Password,
                    IsManager = false
                };

                //Call the Register method on the proxy to register the new user
                InServerCall = true;
                newUser = await proxy.Register(newUser);
                InServerCall = false;

                //If the registration was successful, navigate to the login page
                if (newUser != null)
                {
                    //UPload profile imae if needed
                    if (!string.IsNullOrEmpty(LocalPhotoPath))
                    {
                        await proxy.LoginAsync(new LoginInfo { Email = newUser.UserEmail, Password = newUser.UserPassword });
                        AppUser? updatedUser = await proxy.UploadProfileImage(LocalPhotoPath);
                        if (updatedUser == null)
                        {
                            InServerCall = false;
                            await Application.Current.MainPage.DisplayAlert("Registration", "User Data Was Saved BUT Profile image upload failed", "ok");
                        }
                    }
                    InServerCall = false;

                    ((App)(Application.Current)).MainPage.Navigation.PopAsync();
                }
                else
                {

                    //If the registration failed, display an error message
                    string errorMsg = "Registration failed. Please try again.";
                    await Application.Current.MainPage.DisplayAlert("Registration", errorMsg, "ok");
                }
            }
        }
    }
}
