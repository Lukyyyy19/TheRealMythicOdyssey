using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using CsvHelper;


public class DeckManager : MonoBehaviour
{
    [SerializeField] public Card[] allCards;
    [SerializeField] private List<Card> activeDeck;
    private string path;
    private FileStream _currentFile;
    [SerializeField] private string Text;
    public static DeckManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        Init();
        CreateActiveDeck();
    }

    public List<Card> GetActiveDeck()
    {
        return activeDeck;
    }

    void Init()
    {
        path = Application.persistentDataPath + "\\test.txt";
        // File.Delete(path);
        if (!File.Exists(path))
        {
            _currentFile = File.Create(path);
            _currentFile.Close();
        }

        Text = File.ReadAllText(path);
        // else
        // {
        //     _currentFile = File.Open(path, FileMode.Open);
        // }
    }

    void CreateActiveDeck()
    {
        var txtDeck = Text.Split(',');

        activeDeck = allCards.Where(card => txtDeck.Any(s =>
        {
            Int32.TryParse(s, out var x);
            return card.Id == x;
        })).ToList();
    }

    public void AddCardToDeck(int id)
    {
        Text += id.ToString() + ',';
        File.WriteAllText(path, Text);
        CreateActiveDeck();
    }

    public void RemoveCardFromDeck(int id)
    {
        var newtxt = Text.Split(',').Where(x => x != id.ToString()).ToArray();
        Text = String.Join(',', newtxt);
        File.WriteAllText(path, Text);
        CreateActiveDeck();
    }

    private static void AddText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }
}