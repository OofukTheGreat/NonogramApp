﻿//using NonogramApp.Models;
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

    //public NonogramService service;
    //private readonly IServiceProvider serviceProvider;

    //public ICommand LoginCommand { get; set; }
    //public ICommand GoToRegisterCommand { get; set; }
    //private string email;
    //public string Email
    //{
    //    get => email;
    //    set
    //    {
    //        if (email != value)
    //        {
    //            email = value;
    //            OnPropertyChanged(nameof(Email));
    //            // can add more logic here like email validation etc.
    //            // can add error message property and set it here
    //        }
    //    }
    //}
    //private string password;
    //public string Password
    //{
    //    get => password;
    //    set
    //    {
    //        if (password != value)
    //        {
    //            password = value;
    //            OnPropertyChanged(nameof(Password));
    //            // can add more logic here like email validation etc.
    //            // can add error message property and set it here
    //        }
    //    }
    //}

    public LoginPageViewModel()
    {
        //this.LoginCommand = new Command(OnLogin);
        //this.GoToRegisterCommand = new Command(OnRegisterClicked);
    }

    //public async void OnLogin()
    //{
    //    LoginInfo info = new LoginInfo
    //    {
    //        Email = this.Email,
    //        Password = this.Password
    //    };
    //    User user = await service.Login(info);
    //    if (user != null)
    //    {
    //        if (user.UserType == "3")
    //        {
    //            var businessesPage = serviceProvider.GetRequiredService<BusinessesPage>();
    //            await App.Current.MainPage.Navigation.PushAsync(businessesPage);
    //        }
    //        if (user.UserType == "2")
    //        {
    //            //Navigate to the main page
    //            App.Current.MainPage = new MainPage();
    //        }
    //        //Navigate to the main page
    //        //App.Current.MainPage = new MainPage();
    //        // ÷áìú äãó ãøê ä-DI åðéååè àìéå
    //        //var businessesPage = serviceProvider.GetRequiredService<BusinessesPage>();
    //        //await App.Current.MainPage.Navigation.PushAsync(businessesPage);
    //    }

    //    else
    //    {
    //        Debug.WriteLine("Login failed");
    //    }
    //}

    //private async void OnRegisterClicked()
    //{
    //    // ÷áìú äãó ãøê ä-DI åðéååè àìéå
    //    var registrationPage = serviceProvider.GetRequiredService<RegistrationPage>();
    //    await App.Current.MainPage.Navigation.PushAsync(registrationPage);
    //}





}