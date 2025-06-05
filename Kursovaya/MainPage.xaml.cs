namespace Kursovaya;

public partial class MainPage : ContentPage
{
    public static List<(string Title, string Content, DateTime Created)> Notes = new();

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
            var dateLabel = new Label
            {
                Text = note.Created.ToString("dd.MM.yy"),
                FontSize = 13,
                TextColor = Color.FromArgb("#FFAAAAAA"),
                HorizontalTextAlignment = TextAlignment.End
            };
            var timeLabel = new Label
            {
                Text = note.Created.ToString("HH:mm:ss"),
                FontSize = 12,
                TextColor = Color.FromArgb("#FFAAAAAA"),
                HorizontalTextAlignment = TextAlignment.End
            };

            var infoStack = new VerticalStackLayout
            {
                Spacing = 2,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Children = { dateLabel, timeLabel },
                WidthRequest = 60 // фиксированная ширина для даты и времени
            };

            var titleLabel = new Label
            {
                Text = note.Title,
                FontSize = 18,
                TextColor = Color.FromArgb("#FFFAFAFA"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                LineBreakMode = LineBreakMode.TailTruncation, // "..." если не помещается
                MaxLines = 2 // максимум две строки
            };

            var grid = new Grid
            {
                ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            },
                VerticalOptions = LayoutOptions.Center,
            };
            grid.Add(titleLabel, 0, 0);
            grid.Add(infoStack, 1, 0);

            var frame = new Frame
            {
                CornerRadius = 20,
                BackgroundColor = Color.FromArgb("#FF383838"),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 4),
                Content = grid
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
        Notes.Add(("Заметка", "", DateTime.Now));
        int index = Notes.Count - 1;
        await Shell.Current.GoToAsync($"{nameof(NotePage)}?index={index}");
    }

    public void UpdateNote(int index, string title, string content)
    {
        if (index >= 0 && index < Notes.Count)
        {
            Notes[index] = (title, content, Notes[index].Created);
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

    private Frame CreateNote(string title, int index)
    {
        var created = Notes[index].Created;
        var dateLabel = new Label
        {
            Text = created.ToString("dd.MM.yy"),
            FontSize = 13,
            TextColor = Color.FromArgb("#FFAAAAAA"),
            HorizontalTextAlignment = TextAlignment.End
        };
        var timeLabel = new Label
        {
            Text = created.ToString("HH:mm:ss"),
            FontSize = 12,
            TextColor = Color.FromArgb("#FFAAAAAA"),
            HorizontalTextAlignment = TextAlignment.End
        };

        var infoStack = new VerticalStackLayout
        {
            Spacing = 2,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center,
            Children = { dateLabel, timeLabel },
            WidthRequest = 60 // фиксированная ширина для даты и времени
        };

        var titleLabel = new Label
        {
            Text = title,
            FontSize = 18,
            TextColor = Color.FromArgb("#FFFAFAFA"),
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            LineBreakMode = LineBreakMode.TailTruncation,
            MaxLines = 2
        };

        var grid = new Grid
        {
            ColumnDefinitions =
        {
            new ColumnDefinition(GridLength.Star),
            new ColumnDefinition(GridLength.Auto)
        },
            VerticalOptions = LayoutOptions.Center,
        };
        grid.Add(titleLabel, 0, 0);
        grid.Add(infoStack, 1, 0);

        var frame = new Frame
        {
            CornerRadius = 20,
            BackgroundColor = Color.FromArgb("#FF383838"),
            Padding = new Thickness(20),
            Margin = new Thickness(0, 0, 0, 4),
            Content = grid
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