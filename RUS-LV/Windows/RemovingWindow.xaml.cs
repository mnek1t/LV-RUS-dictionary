using LV_RUS.Presenters;
using System.Windows;
using System.Windows.Controls;

namespace RUS_LV.Windows
{
    public sealed partial class RemovingWindow : Window
    {
        private Presenter_Dictionary presenter;
        private TextBox output;
        public RemovingWindow(TextBox outputField, Presenter_Dictionary presenter)
        { // initialize variables
            output = outputField; // we get an access to private TextBox to write a value there
            this.presenter = presenter; // we do not create a new object of presenter to lose read info from files, so we assign reference
            InitializeComponent();
        }
        private bool IsLatvianInput(string text)  // if user entered in latvian 
        {
            if (text != null)
            {
                foreach (char c in text) // we check each character of the string
                {
                    return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
                }
                return false;
            }
            else
            {
                return false;

            }
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsLatvianInput(LvInputTF.Text))
            {
                MessageBox.Show("Введите слова на латышском!", "Внимание!");
            }
            else
            {
                output.Text = presenter.Remove(LvInputTF.Text);
                this.Close();
            }
        }
    }
}
