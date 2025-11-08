using iths_labb_3_quiz_configurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iths_labb_3_quiz_configurator.Command;
using System.Collections.ObjectModel;

namespace iths_labb_3_quiz_configurator.ViewModels
{
    class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private readonly QuestionPackViewModel _activePack;
        private static Random rng = new Random();
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _activePack = _mainWindowViewModel.ActivePack;           
        }

        public string Query {  get; set; }
        public string Alt1 { get; set; }
        public string Alt2 { get; set; }
        public string Alt3 { get; set; }
        public string Alt4 { get; set; }

        private void GameLoop()
        {
            foreach(QuestionViewModel q in _activePack.Questions)
            {
                string Querry = q.Query;
                ObservableCollection<string> alternatives = new();
                alternatives.Add(q.CorrectAnswer);
                alternatives.Add(q.IncorrectAnswer1);
                alternatives.Add(q.IncorrectAnswer2);
                alternatives.Add(q.IncorrectAnswer3);
                Shuffle(alternatives);

                
            }
        }

        public void Shuffle(this ObservableCollection<string> collection)
        {
            var shuffled = collection.OrderBy(_ => rng.Next()).ToList();
            collection.Clear();
            foreach (var item in shuffled)
                collection.Add(item);
        }
    }
}
