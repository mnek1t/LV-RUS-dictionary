using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LV_RUS.Presenters;

namespace LV_RUS
{
    public sealed partial class AddingWindow : Window
    {
        public delegate bool Predicate(string text);
        private Predicate lang1;
        private Presenter_Dictionary presenter;
        private TextBox outputField;
        public KeyValuePair<string,string> KeyValue { get; private set; }
        //public string Key { get; private set; }
        //public string Value { get; private set; }
        public AddingWindow(TextBox outputField, Predicate lang1)
        { // initialize variables
            this.outputField = outputField; // we get an access to private TextBox to write a value there
            presenter = new Presenter_Dictionary(this);
            InitializeComponent();
            this.lang1 = lang1;
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
            if (!IsRussianInput(RUInput.Text) || !lang1(LVInput.Text))
            {
                MessageBox.Show("Введите слова на русском и на латышском соответсвенно!", "Внимание!");        
            }
            else
            {
                outputField.Text = presenter.Add(LVInput.Text.ToLower(), RUInput.Text.ToLower());
                KeyValue = new KeyValuePair<string, string>(LVInput.Text, RUInput.Text);
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
