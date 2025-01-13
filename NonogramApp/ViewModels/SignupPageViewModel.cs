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
//using static Java.Util.Jar.Attributes;

namespace NonogramApp.ViewModels
{
    public class SignupPageViewModel:ViewModelBase
    {
        private NonogramService service;
        private readonly IServiceProvider serviceProvider;
        public SignupPageViewModel(NonogramService service, IServiceProvider serviceProvider)
        {
            this.service = service;
            SignupCommand = new Command(OnSignup);
            CancelCommand = new Command(OnCancel);
            UploadPhotoCommand = new Command(OnUploadPhoto);
            PhotoURL = service.GetDefaultProfilePhotoUrl();
            LocalPhotoPath = "";
            DisplayNameError = "Name is required";
            EmailError = "Email is required";
            PasswordError = "Password must be at least 4 characters long and contain letters and numbers";
            errorMsg = "";
        }
        private string displayName;
        private int? userId { get; set; }
        private string? displayNameError;
        private string email;
        private string password;
        private string? passwordError;
        private string errorMsg;
        public string ErrorMsg { get => errorMsg; set { errorMsg = value; OnPropertyChanged("ErrorMsg"); } }
        private string emailError;
        public string EmailError { get => emailError; set { emailError = value; OnPropertyChanged("EmailError"); } }
        private bool showDisplayNameError;
        public bool ShowDisplayNameError { get => showDisplayNameError; set { showDisplayNameError = value; OnPropertyChanged("ShowNameError"); } }
        private bool showEmailError;
        public bool ShowEmailError { get => showEmailError; set { showEmailError = value; OnPropertyChanged("ShowEmailError"); } }
        private bool showPasswordError;
        public bool ShowPasswordError { get => showPasswordError; set { showPasswordError = value; OnPropertyChanged("ShowPasswordError"); } }
        private string photoURL;
        public string PhotoURL { get => photoURL; set { photoURL = value; OnPropertyChanged("PhotoURL"); } }
        private string localPhotoPath;
        public string LocalPhotoPath { get => localPhotoPath; set { localPhotoPath = value; OnPropertyChanged("LocalPhotoPath"); } }
        //This method open the file picker to select a photo
        // הוספת אובייקט ממחלקת השירותים שיוכל להפעיל את הפונקציות במחלקה
        public ICommand CancelCommand { get; set; }
        public ICommand SignupCommand { get; set; }
        public Command UploadPhotoCommand { get; }
        public string DisplayName
        {
            get
            {
                return displayName;
            }
            set
            {
                displayName = value;
                OnPropertyChanged(nameof(DisplayName));
                // בדיקת תקינות שם המשתמש
                if (!string.IsNullOrEmpty(DisplayName))
                {
                    if (char.IsDigit(DisplayName[0]))
                    {
                        DisplayNameError = "Display name can't start with a number";
                        OnPropertyChanged(nameof(DisplayName));
                    }
                }
            }
        }
        public string DisplayNameError
        {
            get
            {
                return displayNameError;
            }
            set
            {
                displayNameError = value;
                OnPropertyChanged(nameof(DisplayNameError));
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
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(DisplayNameError));
                if (string.IsNullOrEmpty(password))
                {
                    PasswordError = ""; // איפוס השגיאה אם השדה ריק
                }
                else
                {
                    if (password != null)
                    {
                        bool IsPasswordOk = IsValidPassword(password);
                        if (!IsPasswordOk)
                        {
                            PasswordError = "Password must contain an Uppercast letter and a number";
                        }
                    }
                }
            }
        }
        private async void OnUploadPhoto()
        {
            try
            {
                var result = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Please select a photo",
                });

                if (result != null)
                {
                    // The user picked a file
                    this.LocalPhotoPath = result.FullPath;
                    this.PhotoURL = result.FullPath;
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void UpdatePhotoURL(string virtualPath)
        {
            Random r = new Random();
            PhotoURL = service.GetImagesBaseAddress() + virtualPath + "?v=" + r.Next();
            LocalPhotoPath = "";
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
        public string PasswordError
        {
            get { return passwordError; }
            set
            {
                passwordError = value;
                OnPropertyChanged(nameof(PasswordError));
                //OnPropertyChanged(nameof(CanRegister));
            }
        }
        private void OnCancel()
        {
            DisplayName = "";
            Email = "";
            Password = "";
            // Navigate to the Register View page
            ((App)Application.Current).MainPage.Navigation.PopAsync();
        }
        private void ValidateName()
        {
            this.ShowDisplayNameError = string.IsNullOrEmpty(DisplayName);
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
        public async void OnSignup()
        {
            ValidateName();
            ValidateEmail();
            ValidatePassword();
            if (!ShowDisplayNameError && !ShowEmailError && !ShowPasswordError)
            {
                //Create a new AppUser object with the data from the registration form
                var newUser = new PlayerDTO
                {
                    DisplayName = DisplayName,
                    Email = Email,
                    Password = Password,
                    IsAdmin = false
                };

                //Call the Register method on the proxy to register the new user
                InServerCall = true;
                newUser = await service.Signup(newUser);
                InServerCall = false;

                //If the registration was successful, navigate to the login page
                if (newUser != null)
                {
                    //UPload profile imae if needed
                    if (!string.IsNullOrEmpty(LocalPhotoPath))
                    {
                        await service.LoginAsync(new LoginInfo { Email = newUser.Email, Password = newUser.Password });
                        PlayerDTO? updatedUser = await service.UploadProfileImage(LocalPhotoPath);
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
            else if (ShowEmailError)
            {
                ErrorMsg = EmailError;
            }
            else if (ShowPasswordError)
            {
                ErrorMsg = PasswordError;
            }
            else if (ShowDisplayNameError)
            {
                ErrorMsg = DisplayNameError;
            }
        }
    }
}
