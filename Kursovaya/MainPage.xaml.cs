namespace Kursovaya;

public partial class MainPage : ContentPage
{
    public static List<(string Title, string Content)> Notes = new();

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshNotes();
        SearchEntry.Text = "";
    }

    public void RefreshNotes()
    {
        NotesStack.Children.Clear();
        for (int i = 0; i < Notes.Count; i++)
        {
            var note = Notes[i];
            var frame = new Frame
            {
                CornerRadius = 20,
                BackgroundColor = Color.FromArgb("#FF383838"),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 4),
                Content = new Label
                {
                    Text = note.Title,
                    FontSize = 18,
                    TextColor = Color.FromArgb("#FFFAFAFA")
                }
            };
            int index = i;
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                await Shell.Current.GoToAsync($"{nameof(NotePage)}?index={index}");
            };
            frame.GestureRecognizers.Add(tapGesture);
            NotesStack.Children.Add(frame);
        }
    }

    private async void AddNoteButton_Clicked(object? sender, EventArgs e)
    {
        Notes.Add(("Заметка", ""));
        int index = Notes.Count - 1;
        await Shell.Current.GoToAsync($"{nameof(NotePage)}?index={index}");
    }

    public void UpdateNote(int index, string title, string content)
    {
        if (index >= 0 && index < Notes.Count)
        {
            Notes[index] = (title, content);
            RefreshNotes();
        }
    }

    private void SearchEntry_Completed(object sender, EventArgs e)
    {
        string searchText = SearchEntry.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(searchText))
        {
            RefreshNotes(); // Показываем все заметки
        }
        else
        {
            var filtered = Notes
                .Select((note, idx) => (note, idx))
                .Where(x => x.note.Title == searchText)
                .ToList();
            NotesStack.Children.Clear();
            foreach (var (note, index) in filtered)
            {
                var frame = CreateNote(note.Title, index);
                NotesStack.Children.Add(frame);
            }
        }
    }

    // Универсальный метод для создания блока заметки без кнопки удаления
    private Frame CreateNote(string title, int index)
    {
        var frame = new Frame
        {
            CornerRadius = 20,
            BackgroundColor = Color.FromArgb("#FF383838"),
            Padding = new Thickness(20),
            Margin = new Thickness(0, 0, 0, 4),
            Content = new Label
            {
                Text = title,
                FontSize = 18,
                TextColor = Color.FromArgb("#FFFAFAFA")
            }
        };
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) =>
        {
            await Shell.Current.GoToAsync($"{nameof(NotePage)}?index={index}");
        };
        frame.GestureRecognizers.Add(tapGesture);
        return frame;
    }

}