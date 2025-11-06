using iths_labb_3_quiz_configurator.Models;
using iths_labb_3_quiz_configurator.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace iths_labb_3_quiz_configurator.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();

		private QuestionPackViewModel _activePack;
        private ViewModelBase _activeView;
        private readonly IWindowServices _windowService;

        public MainWindowViewModel(IWindowServices windowService)
        {
            _windowService = windowService;
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            MenuViewModel = new MenuViewModel(this);
            var pack = new QuestionPack("Mina frågor");
            ActivePack = new QuestionPackViewModel(pack);
            Packs.Add(ActivePack);
            ActivePack.Questions.Add(new QuestionViewModel($"Vad är 1+1", "2", "3", "1", "4"));
            ActivePack.Questions.Add(new QuestionViewModel($"Vad heter sveriges huvudstad?", "Stockholm", "Oslo", "London", "Göteborg"));
            ActiveView = ConfigurationViewModel;
        }
        public QuestionPackViewModel ActivePack
		{
			get => _activePack;
			set { 
				_activePack = value;
				RaisePropertyChanged();
                PlayerViewModel?.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
			}
		}
        public ViewModelBase ActiveView
        {
            get => _activeView;
            set
            {
                _activeView = value;
                RaisePropertyChanged();
            }
        }
        public PlayerViewModel? PlayerViewModel { get; }
        public ConfigurationViewModel? ConfigurationViewModel { get; }
        public MenuViewModel? MenuViewModel { get; }

        public bool CanOpenNewPack(object? arg)
        {
            return true;
        }
        public void OpenNewPack(object? obj)
        {
            NewPackViewModel newPackViewModel = new NewPackViewModel(this);
            _windowService.ShowDialog(newPackViewModel);
        }

        public bool CanCreateNewPack(object? arg)
        {
            return true;
        }
        public void CreateNewPack(object? obj)
        {
            var newPack = new QuestionPackViewModel(new QuestionPack("test"));
            Packs.Add(newPack);
            ActivePack = newPack;
        }
        public bool CanShowPlayer(object? arg)
        {
            return true; 
        }
        public void ShowPlayer(object? obj)
        {
            if (PlayerViewModel != null) ActiveView = PlayerViewModel;
        }
        public bool CanShowConfiguration(object? arg)
        {
            return true;
        }
        public void ShowConfiguration(object? obj)
        {
            if (ConfigurationViewModel != null) ActiveView = ConfigurationViewModel;
        }
    }
}
