using iths_labb_3_quiz_configurator.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iths_labb_3_quiz_configurator.ViewModels;

class MenuViewModel
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public MenuViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        OpenNewPackDialogCommand = new DelegateCommand(_mainWindowViewModel.OpenNewPack, _mainWindowViewModel.CanOpenNewPack);
        CahngeActivePackCommand = new DelegateCommand(CahngeActivePack, CanCahngeActivePack);
        ShowPlayerCommand = new DelegateCommand(_mainWindowViewModel.ShowPlayer, _mainWindowViewModel.CanShowPlayer);
        ShowConfigurationCommand = new DelegateCommand(_mainWindowViewModel.ShowConfiguration, _mainWindowViewModel.CanShowConfiguration);
    }
    public DelegateCommand OpenNewPackDialogCommand { get; }
    public DelegateCommand ShowPlayerCommand {  get; }
    public DelegateCommand ShowConfigurationCommand {  get; }
    public DelegateCommand CahngeActivePackCommand { get; }
    public ObservableCollection<QuestionPackViewModel> Packs => _mainWindowViewModel.Packs;

    public QuestionPackViewModel ActivePack
    {
        get => _mainWindowViewModel.ActivePack;
        set => _mainWindowViewModel.ActivePack = value;
    }

    public bool CanCahngeActivePack(object? arg)
    {
        return true;
    }
    public void CahngeActivePack(object obj)
    {
        if (obj is QuestionPackViewModel pack) ActivePack = pack;
    }
}
