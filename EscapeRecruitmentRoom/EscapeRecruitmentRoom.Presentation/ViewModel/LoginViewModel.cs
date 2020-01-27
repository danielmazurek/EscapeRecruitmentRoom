using EscapeRecruitmentRoom.Core.Logic.Account;
using EscapeRecruitmentRoom.Presentation.View;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EscapeRecruitmentRoom.Presentation.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILoginService _loginService;
        private readonly IViewNavigator _navigator;
        private string _loginText;
        private string _passwordText;
        private string _response;

        public LoginViewModel(ILoginService loginService, IViewNavigator navigator)
        {
            _loginService = loginService;
            _navigator = navigator;
            Login = new RelayCommand(LoginImpl);
        }

        public string LoginText
        {
            get => _loginText;
            set => this.Set(ref _loginText, value);
        }

        public string PasswordText
        {
            get => _passwordText;
            set => this.Set(ref _passwordText, value);
        }

        public string Title => "Please enter credentials";

        public string Response
        {
            get => _response;
            set => Set(ref _response, value);
        }

        public RelayCommand Login { get; }

        private void LoginImpl()
        {
            if(string.IsNullOrEmpty(LoginText) && string.IsNullOrEmpty(PasswordText))
            {
                Response = "Please provide credentials";
            }
            else if(string.IsNullOrEmpty(LoginText))
            {
                Response = "Login was not provided";
            }
            else if (string.IsNullOrEmpty(PasswordText))
            {
                Response = "Password was not provided";
            }
            else if (_loginService.Authorize(LoginText, PasswordText))
            {
                PasswordText = Response = null;
                _navigator.NavigateTo(View.Room);
            }
            else
            {
                Response = "Invalid input";
            }
        }
    }
}
