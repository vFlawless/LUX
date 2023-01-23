using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LUX
{
    internal class LoadPopup
    {
        private List<Payload> planetsList;
        
        Form form = new Form();

        public LoadPopup(List<Payload> PlanetsList)
        {
            this.planetsList = PlanetsList;
        }

        public void Pupup()
        {
            string currentSort = "average d"; // highest multiplier // amount d|a 
            char arrowDown = '\u2193';
            char arrowUp = '\u2191';
            
            form.Size = new Size(form.Width * 3, form.Height * 2);
            form.Cursor = Cursors.IBeam;
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.ReadOnly = true;
            richTextBox.HideSelection = true;
            richTextBox.DetectUrls = true;
            richTextBox.Font = new Font("Consolas", 10);

            richTextBox.LinkClicked += RichTextBox_LinkClicked;
            richTextBox.MouseMove += RichTextBox_MouseMove;
            richTextBox.MouseClick += RichTextBox_MouseClick;

            //form.Controls.Add(richTextBox);
            Print(richTextBox, currentSort);

            form.ShowDialog();
        }

        private async void RichTextBox_MouseClick(object sender, EventArgs e)
        {
            RichTextBox richTextBox = (RichTextBox)sender;
            Point clickPosition = richTextBox.PointToClient(Cursor.Position); // get the position of the mouse click in the RichTextBox's client area
            int charIndex = richTextBox.GetCharIndexFromPosition(clickPosition); // get the index of the character under the mouse cursor
            if (charIndex < 200)
            {
                string word = GetClickedWord(richTextBox, charIndex);
                string currentSort = ""; 
                string arrowDown = "\u2193";
                string arrowUp = "\u2191";

                if (word == "amount" || word == "amount" + arrowUp)
                {
                    currentSort = "amount d";
                }
                else if (word == "amount" + arrowDown)
                {
                    currentSort = "amount a";
                }

                if (word == "average" || word == "highest" || word == "multiplier" || word == "multiplier" + arrowUp || word == "multiplier" + arrowDown)
                {
                    int index = charIndex;
                    string prevWord = "";
                    if (word == "multiplier" || word == "multiplier" + arrowUp || word == "multiplier" + arrowDown)
                    {
                        do
                        {
                            index--;
                        }
                        while (index > 0 && !char.IsWhiteSpace(richTextBox.Text[index - 1]));
                        index--;
                        prevWord = GetClickedWord(richTextBox, index);
                    }
                    else
                    {
                        prevWord = word;
                    }

                    if (prevWord == "average" || prevWord == "highest")
                    {

                        if(word != "multiplier" && word != "multiplier" + arrowUp && word != "multiplier" + arrowDown)
                        {
                            do
                            {
                                index++;
                            }
                            while (index > 0 && !char.IsWhiteSpace(richTextBox.Text[index - 1]));
                            index++;
                            word = GetClickedWord(richTextBox, index);
                        }


                        if (word == "multiplier" || word == "multiplier" + arrowUp)
                        {
                            currentSort = (prevWord == "average") ? "average d" : "highest d";
                        }
                        else if (word == "multiplier" + arrowDown)
                        {
                            currentSort = (prevWord == "average") ? "average a" : "highest a";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(currentSort))
                {
                    Print(richTextBox, currentSort);
                }
            }
        }

        private string GetClickedWord(RichTextBox richTextBox, int charIndex)
        {
            int startIndex = charIndex;
            while (startIndex > 0 && !char.IsWhiteSpace(richTextBox.Text[startIndex - 1]))
            {
                startIndex--;
            }

            int length = charIndex - startIndex;
            while (charIndex < richTextBox.Text.Length && !char.IsWhiteSpace(richTextBox.Text[charIndex]))
            {
                charIndex++;
                length++;
            }

            return richTextBox.Text.Substring(startIndex, length);
        }



        private void Coloring(RichTextBox richTextBox)
        {
            char arrowDown = '\u2193';
            char arrowUp = '\u2191';

            RichTextBox tempBox = richTextBox;
            form.Controls.Remove(richTextBox);
            
            int index = tempBox.Find("average multiplier: 1"); // Color the average Multipliers 
            while (index >= 0)
            {
                index += "average multiplier: ".Length;
                int length2 = 7;
                string y = "";

                for (int i = 0; i < length2; i++)
                {
                    y += tempBox.Text[index + i];
                }
                // Select the word and make it bold
                tempBox.SelectionStart = index;
                tempBox.SelectionLength = length2;

                float number = float.Parse(y);
                if (number >= 100000) number /= 100000;

                if (number < 1.003)
                {
                    tempBox.SelectionColor = Color.Red;
                }
                else if (number < 1.007)
                {
                    tempBox.SelectionColor = Color.Orange;
                }

                else if (number < 1.010)
                {
                    tempBox.SelectionColor = Color.YellowGreen;
                }
                else
                {
                    tempBox.SelectionColor = Color.Green;
                }
                tempBox.SelectionFont = new Font(tempBox.SelectionFont, tempBox.SelectionFont.Style | FontStyle.Bold);

                // Find the next occurrence of the word
                index = tempBox.Find("average multiplier: 1", index, tempBox.TextLength, RichTextBoxFinds.None);

            }

            //Get the first average Multiplier (Sorting)
            index = tempBox.Find("average multiplier"); // Color the average Multiplier
            if (index < 250)
            {
                if (tempBox.Text[index + "average multiplier".Length] == arrowUp)
                {
                    int length3 = "average multiplier".Length; 
                    tempBox.SelectionStart = index;
                    tempBox.SelectionLength = length3;
                    tempBox.SelectionFont = new Font(tempBox.SelectionFont, tempBox.SelectionFont.Style | FontStyle.Bold | FontStyle.Underline);
                }
                else if (tempBox.Text[index + "average multiplier".Length] == arrowDown)
                {
                    int length3 = "average multiplier".Length;
                    tempBox.SelectionStart = index;
                    tempBox.SelectionLength = length3;
                    tempBox.SelectionFont = new Font(tempBox.SelectionFont, tempBox.SelectionFont.Style | FontStyle.Bold | FontStyle.Underline);
                }
                else
                {
                    int length2 = "average multiplier".Length;

                    tempBox.SelectionStart = index;
                    tempBox.SelectionLength = length2;
                    tempBox.SelectionFont = new Font(tempBox.SelectionFont, tempBox.SelectionFont.Style | FontStyle.Bold);
                }


                index = tempBox.Find("highest multiplier"); // Color the highest Multiplier
                if (tempBox.Text[index + "highest multiplier".Length] == arrowUp)
                {
                    int length3 = "highest multiplier".Length;
                    tempBox.SelectionStart = index;
                    tempBox.SelectionLength = length3;
                    tempBox.SelectionFont = new Font(tempBox.SelectionFont, tempBox.SelectionFont.Style | FontStyle.Bold | FontStyle.Underline);
                }
                else if (tempBox.Text[index + "average multiplier".Length] == arrowDown)
                {
                    int length3 = "highest multiplier".Length;
                    tempBox.SelectionStart = index;
                    tempBox.SelectionLength = length3;
                    tempBox.SelectionFont = new Font(tempBox.SelectionFont, tempBox.SelectionFont.Style | FontStyle.Bold | FontStyle.Underline);
                }
                else
                {
                    int length2 = "highest multiplier".Length;

                    tempBox.SelectionStart = index;
                    tempBox.SelectionLength = length2;
                    tempBox.SelectionFont = new Font(tempBox.SelectionFont, tempBox.SelectionFont.Style | FontStyle.Bold);
                }


                index = tempBox.Find("amount"); // Color the amount
                if (tempBox.Text[index + "amount".Length] == arrowUp)
                {
                    int length3 = "amount".Length;
                    tempBox.SelectionStart = index;
                    tempBox.SelectionLength = length3;
                    tempBox.SelectionFont = new Font(tempBox.SelectionFont, tempBox.SelectionFont.Style | FontStyle.Bold | FontStyle.Underline);
                }
                else if (tempBox.Text[index + "amount".Length] == arrowDown)
                {
                    int length3 = "amount".Length;
                    tempBox.SelectionStart = index;
                    tempBox.SelectionLength = length3;
                    tempBox.SelectionFont = new Font(tempBox.SelectionFont, tempBox.SelectionFont.Style | FontStyle.Bold | FontStyle.Underline);
                }
                else
                {
                    int length2 = "amount".Length;

                    tempBox.SelectionStart = index;
                    tempBox.SelectionLength = length2;
                    tempBox.SelectionFont = new Font(tempBox.SelectionFont, tempBox.SelectionFont.Style | FontStyle.Bold);
                }
            }

            tempBox.SelectionLength = 0;
            
            form.Controls.Add(tempBox);
        }

        private void Print(RichTextBox richTextBox, string currentSort)
        {
            string x = "";
            string arrowDown = "\u2193";
            string arrowUp = "\u2191";
            string averageMultiplier = "average multiplier";
            string highestMultiplier = "highest multiplier";
            string amount = "amount";

            // Sort the planetsList based on the currentSort variable
            SortPlanets(currentSort);

            // Add the header to the string
            switch (currentSort)
            {
                case "average d":
                    x += "".PadRight(8) + $"{averageMultiplier}{arrowDown}".PadRight(30) + $"{highestMultiplier}".PadRight(55) + $"{amount}" + "\n\n\n";
                    break;
                case "average a":
                    x += "".PadRight(8) + $"{averageMultiplier}{arrowUp}".PadRight(30) + $"{highestMultiplier}".PadRight(55) + $"{amount}" + "\n\n\n";
                    break;
                case "highest d":
                    x += "".PadRight(8) + $"{averageMultiplier}".PadRight(30) + $"{highestMultiplier}{arrowDown}".PadRight(55) + $"{amount}" + "\n\n\n";
                    break;
                case "highest a":
                    x += "".PadRight(8) + $"{averageMultiplier}".PadRight(30) + $"{highestMultiplier}{arrowUp}".PadRight(55) + $"{amount}" + "\n\n\n";
                    break;
                case "amount d":
                    x += "".PadRight(8) + $"{averageMultiplier}".PadRight(30) + $"{highestMultiplier}".PadRight(55) + $"{amount}{arrowDown}" + "\n\n\n";
                    break;
                case "amount a":
                    x += "".PadRight(8) + $"{averageMultiplier}".PadRight(30) + $"{highestMultiplier}".PadRight(55) + $"{amount}{arrowUp}" + "\n\n\n";
                    break;
            }

            for (int i = 0; i < planetsList.Count; i++)
            {
                x += (i + 1).ToString().PadRight(8) + planetsList[i].URLSubstring + "\n" + "".PadRight(8) +
                     $"average multiplier: {planetsList[i].avrgMultiplier:0.00000}".PadRight(30);
                string y = $"highest multiplier: ";
                for (int j = 0; j < planetsList[i].highestMultiplier.Count; j++)
                {
                    y += $"{planetsList[i].highestMultiplier[j]:0.0000}   ";
                }
                x += y.PadRight(55);
                x += $"{planetsList[i].amount:n0} fishes caught" + "\n\n";
            }

            richTextBox.Text = x;
            Coloring(richTextBox);
        }

        private void SortPlanets(string currentSort)
        {
            switch (currentSort)
            {
                case "average d":
                    planetsList.Sort((x, y) => y.avrgMultiplier.CompareTo(x.avrgMultiplier));
                    break;
                case "average a":
                    planetsList.Sort((x, y) => x.avrgMultiplier.CompareTo(y.avrgMultiplier));
                    break;
                case "highest d":
                    planetsList.Sort((x, y) => y.highestMultiplier[0].CompareTo(x.highestMultiplier[0]));
                    break;
                case "highest a":
                    planetsList.Sort((x, y) => x.highestMultiplier[0].CompareTo(y.highestMultiplier[0]));
                    break;
                case "amount d":
                    planetsList.Sort((x, y) => y.amount.CompareTo(x.amount));
                    break;
                case "amount a":
                    planetsList.Sort((x, y) => x.amount.CompareTo(y.amount));
                    break;
            }
        }


        private void RichTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            RichTextBox richTextBox = (RichTextBox)sender;
            Point clickPosition = richTextBox.PointToClient(Cursor.Position); // get the position of the mouse click in the RichTextBox's client area
            int charIndex = richTextBox.GetCharIndexFromPosition(clickPosition); // get the index of the character under the mouse cursor
            if (charIndex < 150)
            {
                int startIndex = charIndex; // set the start index of the word to the index of the clicked character

                while (startIndex > 0 && !char.IsWhiteSpace(richTextBox.Text[startIndex - 1]))
                {
                    // if the character to the left of the clicked character is not a white space, move the start index to the left
                    startIndex--;
                }
                int length = charIndex - startIndex; // set the length of the word to the difference between the start index and the end index
                while (charIndex < richTextBox.Text.Length && !char.IsWhiteSpace(richTextBox.Text[charIndex]))
                {
                    // if the character to the right of the clicked character is not a white space, move the end index to the right
                    charIndex++;
                    length++;
                }

                if (length > 0) // if the length of the word is greater than 0, show a message box
                {
                    // "average multiplier"; // highest multiplier // amount ˅ ^
                    string arrowDown = "\u2193";
                    string arrowUp = "\u2191";

                    string word = richTextBox.Text.Substring(startIndex, length); // get the word that was clicked

                    if (word == "average" || word == "highest" || word == "amount" || word == "multiplier" || word == "multiplier" + arrowDown || word == "multiplier" + arrowUp ||
                    word == "multiplier" + arrowDown || word == "amount" + arrowDown || word == "amount" + arrowUp)
                    {
                        richTextBox.Cursor = Cursors.Hand;
                    }
                }
                return;
            }
            richTextBox.Cursor = Cursors.IBeam;
        }

        private static void RichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = e.LinkText, UseShellExecute = true });
        }
    }
}
