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
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 0, 10),
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
}