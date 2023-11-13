using System;
using System.Collections.Generic;
using System.Linq;

namespace LV_RUS.Models
{
    sealed class Model_Dictionary
    {
        private Dictionary<string, string> dictionary = null;// stroring a dictionary
        private List<string> keys = null; // storing 1st language words
        private List<string> values = null;//storing its translation words
        public Model_Dictionary(Dictionary<string,string> dictionary,List<string> latvianWords, List<string> russianWords)
        { //initialize collections
            this.dictionary = dictionary;
            this.keys = latvianWords;
            this.values = russianWords;
        }
        public string this[string index] // indexer for reading a value from dictionary 
        {
            get { return dictionary[index]; }
        }
        public KeyValuePair<string, string> FirstOrDefault(Func <KeyValuePair<string,string>, bool> predicate)//"Overriding LINQ method FirstOrDefault"
        {
            return dictionary.FirstOrDefault(predicate);
        }
        public string AddWord(string key, string value) // add word
        {   // in this case we write logic because in presenter and adding window we write new word to string, it is a value type, it does not
            //influence on Model_Dictionary collections
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                keys.Add(key);
                values.Add(dictionary[key]);
                return $"Слово {key} добавлено!";
            }
            else
            {
                return "Слово уже существует!";
            }
        }
        public string DeleteWord(string key) // delete word
        { //there is no logic, because constructor takes reference types
            return $"Слово {key} удалено";
        }
        public string GetRandomWord(bool change)
        {
            if (change == true)
            {
                if (dictionary.Count > 0)
                {
                    int index = new Random().Next(0, dictionary.Count); // initialize index with random value from 0 to last value of dictionary
                    return keys[index];
                }
                else
                {
                    return null;
                }
            }
            else
            {


                if (dictionary.Count > 0)
                {
                    int index = new Random().Next(0, dictionary.Count); // initialize index with random value from 0 to last value of dictionary
                    return dictionary[keys[index]];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
