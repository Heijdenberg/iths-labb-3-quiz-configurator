using iths_labb_3_quiz_configurator.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace iths_labb_3_quiz_configurator.ViewModels;

internal class QuestionPackViewModel : ViewModelBase
{
    private readonly QuestionPack _model;

    public QuestionPackViewModel(QuestionPack model)
    {
        _model = model;
        Questions = new ObservableCollection<QuestionViewModel>( _model.Questions.Select(q => new QuestionViewModel(q)) );
        Questions.CollectionChanged += Questions_CollectionChanged;
    }

    public QuestionPack Model { get => _model; }

    private void Questions_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            foreach (QuestionViewModel q in e.NewItems) _model.Questions.Add(q.Question);

        if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            foreach (QuestionViewModel q in e.OldItems) _model.Questions.Remove(q.Question);

        if (e.Action == NotifyCollectionChangedAction.Replace && e.OldItems != null && e.NewItems != null)
            _model.Questions[e.OldStartingIndex] = ((QuestionViewModel)e.NewItems[0]).Question;

        if (e.Action == NotifyCollectionChangedAction.Reset)
            _model.Questions.Clear();
    }

    public string Name
    {
        get => _model.Name;
        set
        {
            _model.Name = value;
            RaisePropertyChanged();
        }
    }

    public Difficulty Difficulty
    {
        get => _model.Difficulty;
        set
        {
            _model.Difficulty = value;
            RaisePropertyChanged();
        }
    }

    public int TimeLimitInSeconds
    {
        get => _model.TimeLimitInSeconds;
        set
        {
            _model.TimeLimitInSeconds = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<QuestionViewModel> Questions { get; set; }
}
