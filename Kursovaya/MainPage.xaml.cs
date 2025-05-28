namespace Kursovaya
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void AddNoteButton_Clicked(object sender, EventArgs e)
        {
            var noteFrame = new Frame
            {
                CornerRadius = 20,
                BackgroundColor = Color.FromArgb("#FF383838"),
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 0, 10),
                Content = new Label
                {
                    Text = "заметка",
                    FontSize = 18,
                    TextColor = Color.FromArgb("#FFFAFAFA")
                }
            };

            NotesStack.Children.Add(noteFrame);
        }
    }

}
