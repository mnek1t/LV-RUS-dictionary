using LV_RUS.Presenters;
using System.Windows;
using System.Windows.Controls;

namespace RUS_LV.Windows
{
    public sealed partial class RemovingWindow : Window
    {
        public delegate bool Predicate(string text);
        private Presenter_Dictionary presenter;
        private TextBox output;
        private Predicate lang;
        public RemovingWindow(TextBox outputField, Presenter_Dictionary presenter, Predicate _lang)
        { // initialize variables
            output = outputField; // we get an access to private TextBox to write a value there
            this.presenter = presenter; // we do not create a new object of presenter to lose read info from files, so we assign reference
            lang = _lang;
            InitializeComponent();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!lang.Invoke(LvInputTF.Text))
            {
                MessageBox.Show("Введите слова на латышском!", "Внимание!");
            }
            else
            {
                output.Text = presenter.Remove(LvInputTF.Text.ToLower());
                this.Close();
            }
        }

        private void Сancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
