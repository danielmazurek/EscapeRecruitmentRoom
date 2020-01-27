using Autofac;

using EscapeRecruitmentRoom.Core.Logic.Account;
using EscapeRecruitmentRoom.Presentation.View;
using EscapeRecruitmentRoom.Presentation.ViewModel;

using Xunit;

namespace EscapeRecruitmentRoom.Tests
{
    public class LoginTests
    {
        [Fact]
        public void ShouldProperlyResolveLoginService()
        {
            // given
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(MainViewModel).Assembly);
            var container = builder.Build();

            // when
            var service = container.Resolve<ILoginService>();

            // then
            Assert.NotNull(service);
        }

        [Fact]
        public void ShouldAuthorizeBaseUser()
        {
            // given
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(MainViewModel).Assembly);
            var container = builder.Build();
            var service = container.Resolve<ILoginService>();
            string login = "Code Byte";
            string password = "It's a trap!";

            // when
            bool logged = service.Authorize(login, password);

            // then
            Assert.True(logged);
        }

        [Fact]
        public void MustNotLoginPasswordWithLowerCases()
        {
            // given
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(MainViewModel).Assembly);
            var container = builder.Build();
            var service = container.Resolve<ILoginService>();
            var loginNavigator = container.Resolve<IViewNavigator>();
            var loginViewModel = new LoginViewModel(service, loginNavigator) { LoginText = "Code Byte", PasswordText = "it's a trap!" };

            // when
            loginViewModel.Login.Execute(null);

            // then
            Assert.Equal("Invalid input", loginViewModel.Response);
        }

        [Fact]
        public void MustLoginCorretlyLoginNameWitLowerCases()
        {
            // given
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(MainViewModel).Assembly);
            var container = builder.Build();
            var service = container.Resolve<ILoginService>();
            string login = "code byte";
            string password = "It's a trap!";

            // when
            bool logged = service.Authorize(login, password);

            // then
            Assert.True(logged);
        }

        [Fact]
        public void MustNotLoginCredentialsNotPassed()
        {
            // given
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(MainViewModel).Assembly);
            var container = builder.Build();
            var service = container.Resolve<ILoginService>();
            var loginNavigator = container.Resolve<IViewNavigator>();
            var loginViewModel = new LoginViewModel(service, loginNavigator) { LoginText = string.Empty, PasswordText = string.Empty };

            // when
            loginViewModel.Login.Execute(null);

            // then
            Assert.Equal("Please provide credentials", loginViewModel.Response);
        }

        [Fact]
        public void MustNotLoginPasswordNotProvided()
        {
            // given
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(MainViewModel).Assembly);
            var container = builder.Build();
            var service = container.Resolve<ILoginService>();
            var loginNavigator = container.Resolve<IViewNavigator>();
            var loginViewModel = new LoginViewModel(service, loginNavigator) { LoginText = "Code Byte", PasswordText = string.Empty };

            // when
            loginViewModel.Login.Execute(null);

            // then
            Assert.Equal("Password was not provided", loginViewModel.Response);
        }

        [Fact]
        public void MustNotLoginUsernameNotProvided()
        {
            // given
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(MainViewModel).Assembly);
            var container = builder.Build();
            var service = container.Resolve<ILoginService>();
            var loginNavigator = container.Resolve<IViewNavigator>();
            var loginViewModel = new LoginViewModel(service, loginNavigator) { LoginText = string.Empty, PasswordText = "It's a trap!" };

            // when
            loginViewModel.Login.Execute(null);

            // then
            Assert.Equal("Login was not provided", loginViewModel.Response);
        }

        [Fact]
        public void MustNotLoginWrongUsername()
        {
            // given
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(MainViewModel).Assembly);
            var container = builder.Build();
            var service = container.Resolve<ILoginService>();
            var loginNavigator = container.Resolve<IViewNavigator>();
            var loginViewModel = new LoginViewModel(service, loginNavigator) { LoginText = "Test", PasswordText = "It's a trap!" };

            // when
            loginViewModel.Login.Execute(null);

            // then
            Assert.Equal("Invalid input", loginViewModel.Response);
        }

        [Fact]
        public void MustNotLoginWrongPassword()
        {
            // given
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(MainViewModel).Assembly);
            var container = builder.Build();
            var service = container.Resolve<ILoginService>();
            var loginNavigator = container.Resolve<IViewNavigator>();
            var loginViewModel = new LoginViewModel(service, loginNavigator) { LoginText = "Code Byte", PasswordText = "Test" };

            // when
            loginViewModel.Login.Execute(null);

            // then
            Assert.Equal("Invalid input", loginViewModel.Response);
        }


    }
}
