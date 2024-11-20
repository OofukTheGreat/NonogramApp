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

namespace NonogramApp.ViewModels
{
    public class SignupPageViewModel:ViewModelBase
    {
        private NonogramService service;
        private readonly IServiceProvider serviceProvider;
        public SignupPageViewModel(NonogramService service, IServiceProvider serviceProvider)
        {
            this.service = service;
            RegistrationCommand = new Command(Register);
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

        public async void Register()
        {
            //    Models.UserDTO user = new Models.UserDTO
            //    {

            //        Email = email,
            //        Password = password,
            //        DisplayName = displayName,
            //    };


            //    // check
            //    int? res = await this.api_service.Register(user);
            //    // אם ההרשמה הצליחה
            //    if (res != null)
            //    {
            //        // בדיקת סוג המשתמש
            //        if (User_Type == "2") // אם המשתמש הוא מוכר
            //        {

            //            // קבלת ה-SellerRegistrationPage וה-ViewModel דרך DI
            //            var sellerRegistrationPage = serviceProvider.GetRequiredService<SellerRegistrationPage>();
            //            var sellerRegistrationViewModel = serviceProvider.GetRequiredService<SellerRegistrationPageViewModel>();

            //            // אתחול ה-ViewModel עם ה-SellerId שנוצר
            //            sellerRegistrationViewModel.Initialize((int)res);

            //            // הגדרת ה-ViewModel כ-BindingContext של הדף
            //            sellerRegistrationPage.BindingContext = sellerRegistrationViewModel;
            //            await App.Current.MainPage.Navigation.PushAsync(sellerRegistrationPage);

            //        }
            //        else if (User_Type == "3") // אם המשתמש הוא קונה
            //        {
            //            // מעבר לדף BusinessesPage
            //            var BusinessesPage = serviceProvider.GetRequiredService<BusinessesPage>();
            //            await App.Current.MainPage.Navigation.PushAsync(BusinessesPage);
            //        }
            //    }
            //    else
            //    {
            //        // טיפול במקרה שההרשמה נכשלה (הודעת שגיאה למשתמש, למשל)
            //        await Application.Current.MainPage.DisplayAlert("שגיאה", "ההרשמה נכשלה, נסה שוב.", "אישור");
            //    }




        }


    }
}
