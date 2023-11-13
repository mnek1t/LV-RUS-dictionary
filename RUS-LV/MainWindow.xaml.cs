using System.Windows;
using LV_RUS.Presenters;
using RUS_LV.Windows;
using System.IO;
using System.Diagnostics;
using System;

namespace LV_RUS
{
    public sealed partial class MainWindow : Window // main window
    {
        private static Presenter_Dictionary presenter; 
        public string DictionaryPATH { get; private set; } = "Dictionary.txt"; // file for stroing latvian and russian words
        public string RussianWordsPATH { get; private set; } = "RussianWords.txt"; //file for storing only russian words
        public string LatvianWordsPATH { get; private set; } = "LatvianWords.txt"; // file for storing only latvian words
        private StreamWriter writer;
        private string key; // variable for writing a latvian word to  Dictionary.txt and LatvianWords.txt files
        private string value;// variable for writing a russian word to  Dictionary.txt and RussianWords.txt files;
        private static bool isCheckLv;
        public MainWindow()
        {
            InitializeComponent();
            CreationFile();
            CorrectnessButton.IsEnabled = false;
            presenter = new Presenter_Dictionary(this);
            //close access to files
            File.SetAttributes(DictionaryPATH, FileAttributes.ReadOnly);
            File.SetAttributes(RussianWordsPATH, FileAttributes.ReadOnly);
            File.SetAttributes(LatvianWordsPATH, FileAttributes.ReadOnly);
        }
        private void GetAccessFile() // Here we get access for programm to write info to the file
        {
            FileAttributes attributes1 = File.GetAttributes(DictionaryPATH); //get ALL attrinbutes from file Dictionary.txt
            attributes1 &= ~FileAttributes.ReadOnly; //by a bitwise AND operator assign only inversed(~) ReadOnly
            File.SetAttributes(DictionaryPATH, attributes1); //apply attributes

            FileAttributes attributes2 = File.GetAttributes(DictionaryPATH);
            attributes2 &= ~FileAttributes.ReadOnly;
            File.SetAttributes(RussianWordsPATH, attributes2);

            FileAttributes attributes3 = File.GetAttributes(DictionaryPATH);
            attributes3 &= ~FileAttributes.ReadOnly;
            File.SetAttributes(LatvianWordsPATH, attributes3);
        }
        private bool IsLatvianInput(string text)
        {
            if(text!=null)
            {
                foreach (char c in text)// we check each character of the string 
                {
                    return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c == 'ž') || (c == 'č') || (c == 'ķ') || (c == 'š');
                }
                return false;
            }
            else
            {
                return false;

            }
        } // if user inputs word in latvian
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
        private void CreationFile()
        {
            if (!File.Exists(DictionaryPATH))
            {
                writer = new StreamWriter(DictionaryPATH);
            }
            if (!File.Exists(RussianWordsPATH))
            {
                writer = new StreamWriter(RussianWordsPATH);
            }
            if (!File.Exists(LatvianWordsPATH))
            {
                writer = new StreamWriter(LatvianWordsPATH);
            }

        } // creates files if thy do not exist
        // Handling button clicks
        private void AddButton_Click(object sender, RoutedEventArgs e) //add word and its translation to the dictionary
        {
            
            AddingWindow taskWindow = new AddingWindow(OutputField, key, value); // window for users input
            taskWindow.ShowDialog(); // ShowDialog(), stop execution below after closing taskWindow
            CorrectnessButton.IsEnabled = false;
            OutputField.FontSize = 62;
            GetAccessFile();
            if (OutputField.Text != "Слово уже существует!" )
            {
                try // Write to file if word-key does not exist already
                {
                    key = taskWindow.ReturnWordsToFile().Item1;
                    value = taskWindow.ReturnWordsToFile().Item2;
                    using (writer = File.AppendText(DictionaryPATH))
                    {
                        writer.WriteLine($"{key} - {value}");
                    }
                    using (writer = File.AppendText(RussianWordsPATH))
                    {
                        writer.WriteLine(value);
                    }
                    using (writer = File.AppendText(LatvianWordsPATH))
                    {
                        writer.WriteLine(key);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
                finally
                { //close access to files
                    File.SetAttributes(DictionaryPATH, FileAttributes.ReadOnly); 
                    File.SetAttributes(RussianWordsPATH, FileAttributes.ReadOnly);
                    File.SetAttributes(LatvianWordsPATH, FileAttributes.ReadOnly);
                }
            }  
        } 
        private void RemoveButton_Click(object sender, RoutedEventArgs e)//remove word and its translation from the dictionary
        {
            RemovingWindow removeWindow = new RemovingWindow(OutputField, presenter); // window for users input
            removeWindow.ShowDialog();
            CorrectnessButton.IsEnabled = false;
            OutputField.FontSize = 62;
            GetAccessFile();

            try
            { //Firstly clear file:
                File.WriteAllText(DictionaryPATH, string.Empty);
                File.WriteAllText(RussianWordsPATH, string.Empty);
                File.WriteAllText(LatvianWordsPATH, string.Empty);
                //Write to file new information without deleted word -key and -value 
                using (writer = File.AppendText(RussianWordsPATH))
                {
                    foreach (string item in Presenter_Dictionary.russianLinesList)
                    {
                        writer.WriteLine(item);
                    }
                }
                using (writer = File.AppendText(LatvianWordsPATH))
                {
                    foreach (string item in Presenter_Dictionary.latvianLinesList)
                    {
                        writer.WriteLine(item);
                    }
                }
                using (writer = File.AppendText(DictionaryPATH))
                {
                    for (int i = 0; i < Presenter_Dictionary.russianLinesList.Count; i++)
                    {
                        writer.WriteLine($"{Presenter_Dictionary.latvianLinesList[i]} - {Presenter_Dictionary.russianLinesList[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            { 
                File.SetAttributes(DictionaryPATH, FileAttributes.ReadOnly);
                File.SetAttributes(RussianWordsPATH, FileAttributes.ReadOnly);
                File.SetAttributes(LatvianWordsPATH, FileAttributes.ReadOnly);
            }
            
        }
        private void GenerateButton_Click(object sender, RoutedEventArgs e) // means to generate a random word from dictionary
        {
            OutputField.FontSize = 62;
            OutputField.Text = presenter.Get(false);
            isCheckLv = true;
            EnterField.Text = string.Empty;
            CorrectnessButton.IsEnabled = true; //the idea is to genetrate word and if user input something, make Checker button active
        }
        private void CorrectnessButton_Click(object sender, RoutedEventArgs e) //enable only after generating a word
        {
            if (isCheckLv)
            {
                if (IsLatvianInput(EnterField.Text))
                {
                    OutputField.FontSize = 62;
                    OutputField.Text = presenter.Check(EnterField.Text, OutputField.Text, isCheckLv);
                    EnterField.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Необходимо ввести значение на латышском в поле снизу!", "Внимание!");
                }
            }
            else
            {
                if (IsRussianInput(EnterField.Text))
                {
                    OutputField.FontSize = 62;
                    OutputField.Text = presenter.Check(EnterField.Text, OutputField.Text, isCheckLv);
                    EnterField.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Необходимо ввести значение на русском в поле снизу!", "Внимание!");
                }
            }
        }
        private void FindWordButton_Click(object sender, RoutedEventArgs e) //find a word entered in the enterField if such exists in dictionary 
        {
            OutputField.FontSize = 62;
            OutputField.Text = string.Empty;
            if (IsLatvianInput(EnterField.Text))
            {
                OutputField.Text = presenter.Get(EnterField.Text);
                CorrectnessButton.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Необходимо ввести значение на латышском в поле внизу!", "Внимание!");
            }
            
        }
        private void GetDictionaryButton_Click(object sender, RoutedEventArgs e) //open Dictionary.txt
        {
            try
            {
                Process.Start("notepad.exe", DictionaryPATH); // Open notepad for watching the Dictionary.txt file
                OutputField.FontSize = 52;
                OutputField.Text = "Словарь открыт в новом окне";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            
        }

        private void GenerateAnotherLanguage_Click(object sender, RoutedEventArgs e)
        {
            OutputField.FontSize = 62;
            OutputField.Text = presenter.Get(true);
            EnterField.Text = string.Empty;
            isCheckLv = false;
            CorrectnessButton.IsEnabled = true;
        }
    }
}
