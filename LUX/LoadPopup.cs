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
            print(richTextBox, currentSort);

            form.ShowDialog();
        }

        private async void RichTextBox_MouseClick(object sender, EventArgs e)
        {
            RichTextBox richTextBox = (RichTextBox)sender;
            Point clickPosition = richTextBox.PointToClient(Cursor.Position); // get the position of the mouse click in the RichTextBox's client area
            int charIndex = richTextBox.GetCharIndexFromPosition(clickPosition); // get the index of the character under the mouse cursor
            if (charIndex < 250)
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
                    string word = richTextBox.Text.Substring(startIndex, length); // get the word that was clicked
                    string currentSort = "average multiplier"; // highest multiplier // amount
                    string arrowDown = "\u2193";
                    string arrowUp = "\u2191";

                    if (word == "amount" || word == "amount" + arrowUp)
                    {
                        currentSort = "amount d";
                        print(richTextBox, currentSort);
                    }
                    else if (word == "amount" + arrowDown)
                    {
                        currentSort = "amount a";
                        print(richTextBox, currentSort);
                    }

                    if(word == "average")
                    {
                        charIndex = startIndex + length + 1;
                        startIndex = charIndex; // set the start index of the word to the index of the clicked character

                        while (startIndex > 0 && !char.IsWhiteSpace(richTextBox.Text[startIndex - 1]))
                        {
                            // if the character to the left of the clicked character is not a white space, move the start index to the left
                            startIndex--;
                        }

                        length = charIndex - startIndex; // set the length of the word to the difference between the start index and the end index

                        while (charIndex < richTextBox.Text.Length && !char.IsWhiteSpace(richTextBox.Text[charIndex]))
                        {
                            // if the character to the right of the clicked character is not a white space, move the end index to the right
                            charIndex++;
                            length++;
                        }
                        word = richTextBox.Text.Substring(startIndex, length);

                        if (word == "multiplier" || word == "multiplier" + arrowUp)
                        {
                            currentSort = "average d";
                            print(richTextBox, currentSort);
                        }
                        else if (word == "multiplier" + arrowDown)
                        {
                            currentSort = "average a";
                            print(richTextBox, currentSort);
                        }
                    }

                    if (word == "highest")
                    {
                        charIndex = startIndex + length + 1;
                        startIndex = charIndex; // set the start index of the word to the index of the clicked character

                        while (startIndex > 0 && !char.IsWhiteSpace(richTextBox.Text[startIndex - 1]))
                        {
                            // if the character to the left of the clicked character is not a white space, move the start index to the left
                            startIndex--;
                        }

                        length = charIndex - startIndex; // set the length of the word to the difference between the start index and the end index

                        while (charIndex < richTextBox.Text.Length && !char.IsWhiteSpace(richTextBox.Text[charIndex]))
                        {
                            // if the character to the right of the clicked character is not a white space, move the end index to the right
                            charIndex++;
                            length++;
                        }
                        word = richTextBox.Text.Substring(startIndex, length);

                        if (word == "multiplier" || word == "multiplier" + arrowUp)
                        {
                            currentSort = "highest d";
                            print(richTextBox, currentSort);
                        }
                        else if (word == "multiplier" + arrowDown)
                        {
                            currentSort = "highest a";
                            print(richTextBox, currentSort);
                        }
                    }

                    if (word == "multiplier" || word == "multiplier" + arrowUp)
                    {
                        charIndex = startIndex - 2;
                        startIndex = charIndex; // set the start index of the word to the index of the clicked character

                        while (startIndex > 0 && !char.IsWhiteSpace(richTextBox.Text[startIndex - 1]))
                        {
                            // if the character to the left of the clicked character is not a white space, move the start index to the left
                            startIndex--;
                        }

                        length = charIndex - startIndex; // set the length of the word to the difference between the start index and the end index

                        while (charIndex < richTextBox.Text.Length && !char.IsWhiteSpace(richTextBox.Text[charIndex]))
                        {
                            // if the character to the right of the clicked character is not a white space, move the end index to the right
                            charIndex++;
                            length++;
                        }
                        word = richTextBox.Text.Substring(startIndex, length);
                        if (word == "highest")
                        {
                            currentSort = "highest d";
                            print(richTextBox, currentSort);
                        }
                        else if (word == "average")
                        {
                            currentSort = "average d";
                            print(richTextBox, currentSort);
                        }
                    }
                    else if (word == "multiplier" + arrowDown)
                    {
                        charIndex = startIndex - 2;
                        startIndex = charIndex; // set the start index of the word to the index of the clicked character

                        while (startIndex > 0 && !char.IsWhiteSpace(richTextBox.Text[startIndex - 1]))
                        {
                            // if the character to the left of the clicked character is not a white space, move the start index to the left
                            startIndex--;
                        }

                        length = charIndex - startIndex; // set the length of the word to the difference between the start index and the end index

                        while (charIndex < richTextBox.Text.Length && !char.IsWhiteSpace(richTextBox.Text[charIndex]))
                        {
                            // if the character to the right of the clicked character is not a white space, move the end index to the right
                            charIndex++;
                            length++;
                        }
                        word = richTextBox.Text.Substring(startIndex, length);
                        if (word == "highest")
                        {
                            currentSort = "highest a";
                            print(richTextBox, currentSort);
                        }
                        else if (word == "average")
                        {
                            currentSort = "average a";
                            print(richTextBox, currentSort);
                        }
                    }
                }
            }
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

        private void print(RichTextBox richTextBox, string currentSort)
        {
            string x = "";
            string arrowDown = "\u2193";
            string arrowUp = "\u2191";
            
            if (currentSort == "average d")
            {
                x += "".PadRight(8) + $"average multiplier{arrowDown}".PadRight(30) + "highest multiplier".PadRight(55) + "amount" + "\n\n\n";
                planetsList.Sort((x, y) => y.avrgMultiplier.CompareTo(x.avrgMultiplier));
            }
            else if (currentSort == "average a")
            {
                x += "".PadRight(8) + $"average multiplier{arrowUp}".PadRight(30) + "highest multiplier".PadRight(55) + "amount" + "\n\n\n";
                planetsList.Sort((x, y) => x.avrgMultiplier.CompareTo(y.avrgMultiplier));
            }
            else if (currentSort == "highest d")
            {
                x += "".PadRight(8) + "average multiplier".PadRight(30) + $"highest multiplier{arrowDown}".PadRight(55) + "amount" + "\n\n\n";
                planetsList.Sort((x, y) => y.highestMultiplier[0].CompareTo(x.highestMultiplier[0]));
            }
            else if (currentSort == "highest a")
            {
                x += "".PadRight(8) + "average multiplier".PadRight(30) + $"highest multiplier{arrowUp}".PadRight(55) + "amount" + "\n\n\n";
                planetsList.Sort((x, y) => x.highestMultiplier[0].CompareTo(y.highestMultiplier[0]));
            }
            else if (currentSort == "amount d")
            {
                x += "".PadRight(8) + "average multiplier".PadRight(30) + "highest multiplier".PadRight(55) + "amount" + arrowDown + "\n\n\n";
                planetsList.Sort((x, y) => y.amount.CompareTo(x.amount));
            }
            else if (currentSort == "amount a")
            {
                x += "".PadRight(8) + "average multiplier".PadRight(30) + "highest multiplier".PadRight(55) + "amount" + arrowUp + "\n\n\n";
                planetsList.Sort((x, y) => x.amount.CompareTo(y.amount));
            }

            for (int i = 0; i < planetsList.Count; i++)
            {
                x += (i + 1).ToString().PadRight(8) + planetsList[i].URLSubstring + "\n" + "".PadRight(8) +
                     $"average multiplier: {planetsList[i].avrgMultiplier:0.00000}".PadRight(30);
                string y = $"highest multiplier: "; 
                for(int j = 0; j < planetsList[i].highestMultiplier.Count; j++)
                {
                    y += $"{planetsList[i].highestMultiplier[j]:0.0000}   ";
                }
                x += y.PadRight(55);
                x += $"{planetsList[i].amount:n0} fishes caught" + "\n\n";
            }

            richTextBox.Text = x;
            Coloring(richTextBox);
        }

        private void RichTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            RichTextBox richTextBox = (RichTextBox)sender;
            Point clickPosition = richTextBox.PointToClient(Cursor.Position); // get the position of the mouse click in the RichTextBox's client area
            int charIndex = richTextBox.GetCharIndexFromPosition(clickPosition); // get the index of the character under the mouse cursor
            if (charIndex < 200)
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
