using System.ComponentModel;

namespace Kursovaya;

[QueryProperty(nameof(Index), "index")]
public partial class NotePage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _index;
    public int Index
    {
        get => _index;
        set
        {
            if (_index != value)
            {
                _index = value;
                // Не вызываем LoadNote() здесь!
                OnPropertyChanged(nameof(Index));
            }
        }
    }

    private string _noteTitle = "Заметка";
    public string NoteTitle
    {
        get => _noteTitle;
        set
        {
            if (_noteTitle != value)
            {
                _noteTitle = value;
                OnPropertyChanged(nameof(NoteTitle));
            }
        }
    }

    private string _noteContent = "";
    public string NoteContent
    {
        get => _noteContent;
        set
        {
            if (_noteContent != value)
            {
                _noteContent = value;
                OnPropertyChanged(nameof(NoteContent));
            }
        }
    }

    public NotePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Загружаем текст заметки только при первом открытии страницы
        if (Index >= 0 && Index < MainPage.Notes.Count)
        {
            NoteTitle = MainPage.Notes[Index].Title;
            NoteContent = MainPage.Notes[Index].Content;
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (Index >= 0 && Index < MainPage.Notes.Count)
        {
            MainPage.Notes[Index] = (NoteTitle, NoteContent);
        }
        await Shell.Current.GoToAsync("..");
    }

    void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}