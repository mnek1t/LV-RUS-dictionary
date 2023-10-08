using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using LV_RUS.Models;

namespace LV_RUS.Presenters
{
    public sealed class Presenter_Dictionary
    {
        private static Dictionary<string, string> vardnica = null; // stroe latvian and russian words
        private Window view = null;
        private string[] russianLinesArray = null; //for storing info from file
        private string[] latvianLinesArray = null;
        //TODO : russianLinesList and latvianLinesList have to be PRIVATE!
        public static List<string> russianLinesList = null; // for easier manipulation with storing words
        public static List<string> latvianLinesList = null;
        private Model_Dictionary dictionary = null;
        public Presenter_Dictionary(Window view)
        {
            this.view = view;
            ReadInformation();
            dictionary = new Model_Dictionary(vardnica, latvianLinesList, russianLinesList);
        }
        private void ReadInformation() // read word from files to collections
        {
            if (view is MainWindow)
            { // Initialize collections
                MainWindow mainWindowView = (MainWindow)view;
                russianLinesList = new List<string>();
                latvianLinesList = new List<string>();
                vardnica = new Dictionary<string, string>();
                russianLinesArray = File.ReadAllLines(mainWindowView.RussianWordsPATH); // reading
                latvianLinesArray = File.ReadAllLines(mainWindowView.LatvianWordsPATH);
                if (russianLinesArray.Length == latvianLinesArray.Length) // emulation of successfull reading (each key finds each value)
                { // TO TRY: reorganize emulatation
                    if (russianLinesArray.Length == 0) // If files are empty:
                    {
                        WriteInformation(mainWindowView);
                    }
                    else
                    { // fill collections by file content
                        for (int i = 0; i < russianLinesArray.Length; i++)
                        {
                            russianLinesList.Add(russianLinesArray[i]);
                            latvianLinesList.Add(latvianLinesArray[i]);
                            string value = russianLinesArray[i];
                            string key = latvianLinesArray[i];
                            vardnica[key] = value;
                        }
                    }

                }
            }
        }
        private void WriteInformation(MainWindow view) // adding default key and value to the collections sveiki - привет
        {
            russianLinesList.Add("привет");
            latvianLinesList.Add("sveiki");
            vardnica[latvianLinesList[0]] = russianLinesList[0];
            try
            {
                using (StreamWriter writer = File.AppendText(view.DictionaryPATH))
                {
                    writer.WriteLine($"{latvianLinesList[0]} - {russianLinesList[0]}");
                }
                using (StreamWriter writer = File.AppendText(view.RussianWordsPATH))
                {
                    writer.WriteLine(russianLinesList[0]);
                }
                using (StreamWriter writer = File.AppendText(view.LatvianWordsPATH))
                {
                    writer.WriteLine(latvianLinesList[0]);
                }
            }
            catch (Exception ex)
            {
                russianLinesList.Clear();
                latvianLinesList.Clear();
                vardnica.Clear();
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            
        } 
        public (string, string, string) Add(string key, string value) // Adding words to collections
        {
            return (dictionary.AddWord(key.ToLower(), value.ToLower()),key,value);
        }
        public string Remove(string key)// Removing words from collections
        { //There is no need apeal to model dictionary, because changing Presenter_Dictionary collections, we change Model_Dictionary collections
          //as we fetch them as a parameters to Model_Dictionary constructor in 23th line
            if (vardnica.ContainsKey(key))
            {
                russianLinesList.Remove(vardnica[key]);
                latvianLinesList.Remove(key);
                vardnica.Remove(key);
                return dictionary.DeleteWord(key.ToLower());
            }
            return $"Слова {key} не найдено";
        }
        public string Get(bool change) //get word for GenerateButton
        {
            return dictionary.GetRandomWord(change);
        }
        public string Get(string key) // get word for FindWordButton
        {
            if (vardnica.ContainsKey(key))
            {
                return dictionary[key.ToLower()];
            }
            return "Слово в словаре отсутвует";
        }
        public string Check(string key, string value, bool lvCheck) // check user input
        {
            string message = " ";
            if (lvCheck)
            {
                try
                {
                    if (dictionary[key.ToLower()] == value.ToLower())
                    {
                        message = "Correct";
                    }
                }
                catch (Exception)
                {
                    message = "Incorrect";
                }
            }
            else
            {      
                if (dictionary[value.ToLower()] == key.ToLower())
                {
                    message = "Correct";
                }
                else
                {
                    message = "Incorrect";
                }
            }
            return message;
        }
    }
}
