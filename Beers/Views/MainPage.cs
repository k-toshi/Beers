using System;
using System.Linq;
using System.Collections.Generic;

using Xamarin.Forms;

using Beers.Models;
using Beers.Views;
using Beers.Controllers;

namespace Beers
{
    public class MainPage : TabbedPage
    {
        Page loginPage, loginUserInfoPage, menuPage, aboutPage = null;

        public MainPage()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    loginUserInfoPage = new NavigationPage(new LoginUserInfoPage())
                    {
                        Title = "UserInfo"
                    };

                    loginPage = new NavigationPage(new LoginPage(this))
                    {
                        Title = "Login"
                    };

                    menuPage = new NavigationPage(new MenuPage())
                    {
                        Title = "Menu"
                    };

                    aboutPage = new NavigationPage(new AboutPage())
                    {
                        Title = "About"
                    };

                    aboutPage.Icon = "tab_about.png";
                    menuPage.Icon = "tab_feed.png";
                    loginUserInfoPage.Icon = "tab_user.png";
                    loginPage.Icon = "tab_user.png";

                    break;

                default:
                    
                    loginUserInfoPage = new LoginUserInfoPage()
                    {
                        Title = "UserInfo"
                    };

                    loginPage = new LoginPage(this)
                    {
                        Title = "Login"
                    };

                    menuPage = new MenuPage()
                    {
                        Title = "Menu"
                    };

                    aboutPage = new AboutPage()
                    {
                        Title = "About"
                    };
                    break;
            }

            Children.Add(aboutPage);
            Children.Add(menuPage);

            SetLoginPage();

            Title = Children[0].Title;
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetLoginPage();
        }

        public void SetLoginPage()
        {
            if(UserController.IsLogin)
            {
                Children.Remove(loginPage);
                Children.Add(loginUserInfoPage);
            }
            else
            {
                Children.Remove(loginUserInfoPage);
                Children.Add(loginPage);
            }
        }
    }
}
