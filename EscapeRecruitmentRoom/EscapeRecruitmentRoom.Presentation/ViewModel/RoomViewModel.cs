using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using EscapeRecruitmentRoom.Core.Logic;
using EscapeRecruitmentRoom.Core.Logic.Account;
using EscapeRecruitmentRoom.Core.Logic.Game;
using EscapeRecruitmentRoom.Core.Model;
using EscapeRecruitmentRoom.Presentation.View;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EscapeRecruitmentRoom.Presentation.ViewModel
{
    public class RoomViewModel : ViewModelBase, INavigatedTo
    {
        private readonly ILoginService _loginService;
        private readonly IViewNavigator _navigator;
        public IGameManager Manager { get; }

        public IReadOnlyCollection<IReadOnlyCollection<Tile>> Tiles { get; set; }

        private string _commandText;
        private string _message = "Welcome ";

        public string CommandText
        {
            get => _commandText;
            set => Set(ref _commandText, value);
        }

        public string Title => Manager.Title;

        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        public RelayCommand Left { get; }
        public RelayCommand Right { get; }
        public RelayCommand Up { get; }
        public RelayCommand Down { get; }
        public RelayCommand Restart { get; }
        public RelayCommand Logout { get; }
        public RelayCommand Parse { get; }
        public RelayCommand Copy { get; }
        public RelayCommand Solution { get; }

        public RoomViewModel(IGameManager manager, ILoginService loginService, IViewNavigator navigator)
        {
            _loginService = loginService;
            _navigator = navigator;
            Manager = manager;
            manager.StartGame();

            Tiles = Manager.GameState.Tiles;

            Left = new RelayCommand(() => Go(Direction.Left));

            Right = new RelayCommand(() => Go(Direction.Right));

            Up = new RelayCommand(() => Go(Direction.Up));

            Down = new RelayCommand(() => Go(Direction.Down));

            Restart = new RelayCommand(() => RestartImpl());

            Parse = new RelayCommand(() =>
            {
                var commands = CommandText?.Split(new []{ "#" }, StringSplitOptions.RemoveEmptyEntries);
                if (commands?.Any() == true)
                {
                    foreach (string command in commands)
                    {
                        CommandParser.ParseAndRun(command, Manager);
                    }
                    CommandText = null;
                    Tiles = Manager.GameState.Tiles;
                    this.RaisePropertyChanged(nameof(Tiles));
                    this.RaisePropertyChanged(nameof(Title));
                }
            });

            Logout = new RelayCommand(() => _navigator.NavigateTo(View.Login));

            Copy = new RelayCommand(() => Clipboard.SetText(string.Join("#", Manager.GameState.Commands)));

            Solution = new RelayCommand(() =>
            {
                Go(Direction.Down);
                Go(Direction.Right);
                Go(Direction.Down);
                Go(Direction.Down);
                Go(Direction.Down);
                Go(Direction.Right);
                Go(Direction.Right);
                ExecuteAutomatedCommand("K8 Move Down");
                ExecuteAutomatedCommand("I9 Move Right");
                ExecuteAutomatedCommand("J9 Move Right");
                ExecuteAutomatedCommand("K9 Move Right");
                Go(Direction.Down);
                Go(Direction.Right);
                Go(Direction.Right);
                Go(Direction.Right);
                for (int i=0; i<8; i++)
                {
                    Go(Direction.Left);
                }
                ExecuteAutomatedCommand("D9 Move Up");
                Go(Direction.Left);
                Go(Direction.Left);
                Go(Direction.Left);
                for (int i=0; i<5; i++)
                {
                    Go(Direction.Up);
                }
                Go(Direction.Left);
                Go(Direction.Right);
                Go(Direction.Down);
                Go(Direction.Down);
                Go(Direction.Right);
                ExecuteAutomatedCommand("D6 Move Down");
                Go(Direction.Right);
                ExecuteAutomatedCommand("E6 Move Down");
                Go(Direction.Right);
                ExecuteAutomatedCommand("F6 Move Up");
                ExecuteAutomatedCommand("F5 Move Left");
                for (int i=0; i<4; i++)
                {
                    Go(Direction.Up);
                }
                ExecuteAutomatedCommand("E1 Move Left");
                ExecuteAutomatedCommand("D1 Move Left");
                Go(Direction.Right);
                ExecuteAutomatedCommand("G2 Move Down");
                ExecuteAutomatedCommand("G3 Move Left");
                ExecuteAutomatedCommand("F3 Move Left");
                ExecuteAutomatedCommand("E3 Move Up");
                ExecuteAutomatedCommand("E2 Move Up");
                Go(Direction.Left);
                ExecuteAutomatedCommand("E1 Move Left");
                ExecuteAutomatedCommand("D1 Move Left");
                Go(Direction.Up);
                Go(Direction.Left);
                ExecuteAutomatedCommand("C1 Move Left");
                Go(Direction.Left);
                Go(Direction.Left);
                Go(Direction.Left);
                for (int i=0; i<4; i++)
                {
                    Go(Direction.Right);
                }
                for (int i=0; i<4; i++)
                {
                    Go(Direction.Down);
                }
                for (int i=0; i<5; i++)
                {
                    Go(Direction.Right);
                }
                ExecuteAutomatedCommand("J6 Move Right");
                ExecuteAutomatedCommand("K6 Move Up");
                ExecuteAutomatedCommand("K5 Move Right");
                Go(Direction.Up);
                Go(Direction.Right);
                ExecuteAutomatedCommand("L3 PutTNT");
                ExecuteAutomatedCommand("L3 FireUP");
                ExecuteAutomatedCommand("L5 Move Right");
                Go(Direction.Right);
                ExecuteAutomatedCommand("M3 Move Left");
                ExecuteAutomatedCommand("M5 Move Up");
                ExecuteAutomatedCommand("M4 Move Up");
                ExecuteAutomatedCommand("M3 FireUp");
                Go(Direction.Down);
                Go(Direction.Right);
                for (int i=0; i<4; i++)
                {
                    Go(Direction.Up);
                }
            });
        }

        void ExecuteAutomatedCommand(string inputCommand)
        {
            CommandParser.ParseAndRun(inputCommand, Manager);
            Tiles = Manager.GameState.Tiles;
            this.RaisePropertyChanged(nameof(Tiles));
            this.RaisePropertyChanged(nameof(Title));

        }

        private void Go(Direction direction)
        {
            try
            {
                Manager.Go(Manager.HeroTile.Code, direction);
                this.RaisePropertyChanged(nameof(Title));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
        }

        private void RestartImpl()
        {
            Manager.Restart();
            Tiles = Manager.GameState.Tiles;
            this.RaisePropertyChanged(nameof(Tiles));
           
        }

        public void Update()
        {
            Message = "Welcome " + $"{_loginService.UserName}!";
        }
    }
}
