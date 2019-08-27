using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public struct CardData
        {
            public string name;
            public int mana, strength, health;
        }

        public List<CardData> oldCardDatas = new List<CardData>();
        public List<CardData> newCardDatas = new List<CardData>();

        public Form1()
        {
            InitializeComponent();
        }

        //XmlReader reader;
        

        private void Form1_Load(object sender, EventArgs e)
        {
            oldCardDatas = ParseSoT("C:/XMLs/oldTest.xml");
            newCardDatas = ParseSoT("C:/XMLs/newTest.xml");
            CompareSoTs(oldCardDatas, newCardDatas);
        }

        public void CompareSoTs(List<CardData> oldSoT, List<CardData> newSoT)
        {
            textBox1.AppendText("sot length difference: " + (oldSoT.Count - newSoT.Count) + System.Environment.NewLine);
            for (int i = 0; i < oldSoT.Count; i++)
            {
                bool healthDiff = false;
                bool strDiff = false;
                bool manaDiff = false;
                if (oldSoT[i].health != newSoT[i].health)
                {
                    healthDiff = true;
                }
                if (oldSoT[i].strength != newSoT[i].strength)
                {
                    strDiff = true;
                }
                if (oldSoT[i].mana != newSoT[i].mana)
                {
                    manaDiff = true;
                }

                if (healthDiff || strDiff || manaDiff)
                {
                    textBox1.AppendText(oldSoT[i].name + System.Environment.NewLine);
                    if (manaDiff)
                        textBox1.AppendText("Mana cost changed from " + oldSoT[i].mana + " to " + newSoT[i].mana + System.Environment.NewLine);
                    if (healthDiff || strDiff)
                        textBox1.AppendText("Stats changed from " + oldSoT[i].strength + "/" + oldSoT[i].health + " to " + newSoT[i].strength + "/" + newSoT[i].health + System.Environment.NewLine);
                }
            }
        }

        public List<CardData> ParseSoT (string location)
        {
            List<CardData> tempCardData = new List<CardData>();

            // Create an XML reader for this file.
            using (XmlReader reader = XmlReader.Create(location))
            {
                CardData currentCardData = new CardData();
                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case "Name":
                                // Next read will contain text.
                                if (reader.Read())
                                {
                                    if (reader.Value.Trim() != currentCardData.name)
                                    {
                                        currentCardData = new CardData();
                                        currentCardData.name = reader.Value.Trim();

                                        tempCardData.Add(currentCardData);
                                    }

                                }
                                break;
                                
                            case "Mana":
                                if (reader.Read())
                                {
                                    int.TryParse(reader.Value.Trim(), out currentCardData.mana);
                                    
                                }
                                break;

                            case "Attack":
                                if (reader.Read())
                                {
                                    int.TryParse(reader.Value.Trim(), out currentCardData.strength);
                                }

                                break;
                            case "Health":
                                if (reader.Read())
                                {
                                    int.TryParse(reader.Value.Trim(), out currentCardData.health);

                                    tempCardData.Add(currentCardData);
                                }
                                break;
                        }
                    }
                }
            }

            return tempCardData;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
