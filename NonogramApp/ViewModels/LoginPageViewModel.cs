using Microsoft.Win32;
using NonogramApp.Models;
using NonogramApp.Services;
using NonogramApp.ViewModels;
using NonogramApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace NonogramApp.ViewModels;

public class LoginPageViewModel : ViewModelBase
{
    private NonogramService service;
    private IServiceProvider serviceProvider;
    public LoginPageViewModel(NonogramService service, IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.service = service;
        LoginCommand = new Command(OnLogin);
        RegisterCommand = new Command(OnRegister);
        CancelCommand = new Command(OnCancel);
        email = "";
        password = "";
        displayName="";
        InServerCall = false;
        errorMsg = "";
    }
    private string email;
    public string Email { get => email; set { if (email != value) { email = value; OnPropertyChanged(nameof(Email)); } } }
    private string password;
    public string Password { get => password; set { if (password != value) { password = value; OnPropertyChanged(nameof(Password)); } } }
    private string errorMsg;
    public string ErrorMsg { get => errorMsg; set { if (errorMsg != value) { errorMsg = value; OnPropertyChanged(nameof(ErrorMsg)); } } }
    private string displayName;
    public string DisplayName { get => displayName; set { if (displayName != value) { displayName = value; OnPropertyChanged(nameof(displayName)); } } }
    public ICommand LoginCommand { get; set; }
    public ICommand RegisterCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    private async void OnLogin()
    {
        //Choose the way you want to blobk the page while indicating a server call
        InServerCall = true;
        ErrorMsg = "";
        //Call the server to login
        LoginInfo loginInfo = new LoginInfo { Email = this.Email, Password = this.Password };
        PlayerDTO? u = await this.service.LoginAsync(loginInfo);

        InServerCall = false;

        //Set the application logged in user to be whatever user returned (null or real user)
        ((App)Application.Current).LoggedInUser = u;
        if (u == null)
        {
             ErrorMsg = "Email and password do not match";
        }
        else
        {
            ErrorMsg = "";
            //Navigate to the main page
            AppShell shell = serviceProvider.GetService<AppShell>();
            ((App)Application.Current).MainPage = shell;
            
        }
    }
    private void OnRegister()
    {
        ErrorMsg = "";
        Email = "";
        Password = "";
        // Navigate to the Register View page
        ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<SignupPage>());
    }
    private void OnCancel()
    {
        DisplayName = "";
        Email = "";
        Password = "";
        // Navigate to the Register View page
        ((App)Application.Current).MainPage.Navigation.PopAsync();
    }
}