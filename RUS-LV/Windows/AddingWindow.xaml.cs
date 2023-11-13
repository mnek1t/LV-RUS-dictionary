using System.Windows;
using System.Windows.Controls;
using LV_RUS.Presenters;

namespace LV_RUS
{
    public sealed partial class AddingWindow : Window
    {
        private Presenter_Dictionary presenter;
        private TextBox outputField;
        private string key;
        private string value;
        public AddingWindow(TextBox outputField, string key, string value)
        { // initialize variables
            this.outputField = outputField; // we get an access to private TextBox to write a value there
            this.key = key;
            this.value = value;
            presenter = new Presenter_Dictionary(this);
            InitializeComponent();
        }
        public (string, string) ReturnWordsToFile() // return words
        {
            return (key, value);
        }
        private bool IsLatvianInput(string text) // if user entered in latvian 
        {
            if (text != null)
            {
                foreach (char c in text) // we check each character of the string 
                {
                    return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c == 'ž') || (c == 'č') || (c == 'ķ') || (c == 'š');
                }
                return false;
            }
            else
            {
                return false;

            }
        }
        private bool IsRussianInput(string text) // if user entered in russian
        {
            if (text != null)
            {
                foreach (char c in text) // we check each character of the string
                {
                    return (c >= 'А' && c <= 'Я') || (c >= 'а' && c <= 'я');
                }
                return false;
            }
            else
            {
                return false;

            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRussianInput(RUInput.Text) || !IsLatvianInput(LVInput.Text))
            {
                MessageBox.Show("Введите слова на русском и на латышском соответсвенно!", "Внимание!");        
            }
            else
            { // TO TRY: REDO INITIALIZATION OF KEY AND VALUE NOT USING A TUPLES
                outputField.Text = presenter.Add(LVInput.Text.ToLower(), RUInput.Text.ToLower()).Item1;
                key = presenter.Add(LVInput.Text.ToLower(), RUInput.Text.ToLower()).Item2;
                value = presenter.Add(LVInput.Text.ToLower(), RUInput.Text.ToLower()).Item3;
                this.Close();
            }
        }
    }
}
