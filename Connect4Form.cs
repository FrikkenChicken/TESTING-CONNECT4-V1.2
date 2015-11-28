using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TESTING_CONNECT4
{
    public partial class Connect4Form : Form
    {
        public Connect4Form()
        {
            InitializeComponent();
        }

        //Variables
        public int spacing = 3;     //not sure yet...
        public int width = 7;       //number of columns of blank spaces
        public int height = 7;      //number of rows of blank spaces
        public int[,] gameArray;    // To create a game array
        public int currentPlayer = 1;      // To hold what player's turn it is (player 1 or 2)

        /*private void createGameArray()
        {
            int arrayRow = 0;
            int arrayColumn = 0;

            for (int count = 1; count < width * height; count++)
            {
                // Set that postion of the array to 0
                gameArray[arrayRow, arrayColumn] = 0;

                // Count itterations
                int itterationCount = 1;

                // Follow the grid
                if (itterationCount < width)
                {
                    arrayColumn++;
                }
                else if (itterationCount == width)
                {
                    itterationCount = 1;
                    arrayRow++;
                    arrayColumn = 0;
                }
            }

            MessageBox.Show(gameArray[width, height].ToString());
        }*/

        private void startButton_Click(object sender, EventArgs e)
        {
            //Make buttons go away
            titleLabel.Visible = false;
            startButton.Visible = false;
            optionsButton.Visible = false;
            quitButton.Visible = false;

            //Variables for making the game board
            int row = 0;        //number in 'row' part of label name
            int column = 0;     //number in 'column' part of label name

            int initialx = 5;         //initial column (x) value of blank space
            int initialy = 5;         //initial row (y) value of blank space
            int x = initialx;         //column value, set to column initial value
            int y = initialy;         //row value, set to initial row value

            int columnCounter = 0;     //Stores what column the program is at
            int lblSize = 30;          //Size of the blank space
            int btnSize = lblSize + 4;

            //Create panel and add propreties
            Panel gameBoardPanel = new Panel();
            gameBoardPanel.BackColor = Color.Black;
            gameBoardPanel.Location = new Point(initialx, initialy + btnSize + spacing);
            gameBoardPanel.Size = new Size(width * lblSize + (width+1) * spacing, height * lblSize + (height+1) * spacing);

            //Resize form
            this.Size = new Size(width * lblSize + (width + 1) * 2 + 16 + 2 * 5, height * lblSize + (height + 1) * 2 + 38 + 2 * 5);

            //Create buttons procedure
            for (int countButton = 1; countButton <= width; countButton++)
            {
                //Create buttons and format them
                Button btncreate = new Button();
                btncreate.Size = new Size(btnSize, btnSize);
                btncreate.Location = new Point(x+spacing/2, y);
                btncreate.Tag = countButton;

                // Add event handlers to the buttons
                btncreate.Click += new EventHandler(button_Click);
                btncreate.MouseEnter += new EventHandler(buttonHoverEnter);
                btncreate.MouseLeave += new EventHandler(buttonHoverLeave);

                this.Controls.Add(btncreate);
                btncreate.Text = countButton.ToString();
                btncreate.Name = countButton + "Button";

                //Change position of next button
                x += lblSize + spacing;
            }

            //Set position of y to under buttons
            y = spacing;

            //set x position to the spacing
            x = spacing;

            //Create label procedure
            for (int countLabel = 1; countLabel <= width * height; countLabel++)
            {
                //Label formatting
                Label lblcreate = new Label();
                lblcreate.BackColor = Color.White;
                lblcreate.AutoSize = false;
                lblcreate.Size = new Size(lblSize, lblSize);
                lblcreate.Location = new Point(x, y);
                gameBoardPanel.Controls.Add(lblcreate);
                lblcreate.Name = "R" + row + "C" + column + "Label";

                //Count up
                columnCounter++;              
                
                //Find when to switch rows and do so
                if (columnCounter < width)
                {
                    column++;
                    x += lblSize + spacing;
                }
                else if (columnCounter == width)
                {
                    columnCounter = 0;
                    row++;
                    y += lblSize + spacing;
                    column = 0;
                    x = spacing;
                }
            }

            //Add panel to form
            this.Controls.Add(gameBoardPanel);

            //resize the from
            this.Size = new Size(gameBoardPanel.Width + 16 + 10, btnSize + gameBoardPanel.Height + spacing + 39 + 10);
        }

        protected void button_Click(object Sender, EventArgs e)
        {
            Button buttonClicked = Sender as Button;

            if (buttonClicked == null) // To be safe
                return;

            int buttonTagNumber = 1; //To count up

            while ((int)buttonClicked.Tag != buttonTagNumber)
            {
                buttonTagNumber++;
            }

            //call the placeToken Method with the array column number (0 to width-1)
            placeToken(buttonTagNumber-1);
        }

        private void switchPlayer()
        {
            if (currentPlayer == 1)
            {
                currentPlayer = 2;
            }
            else
            {
                currentPlayer = 1;
            }
        }

        private void buttonHoverEnter(object sender, EventArgs e)
        {
            Button buttonHoveredOver = sender as Button;

            if (buttonHoveredOver == null) // To be safe
                return;

            if (currentPlayer == 1)
                buttonHoveredOver.BackColor = Color.FromArgb(255, 128, 128);
            else
                buttonHoveredOver.BackColor = Color.FromArgb(255, 255, 128);
        }
        private void buttonHoverLeave(object sender, EventArgs e)
        {
            Button buttonHoveredOver = sender as Button;

            if (buttonHoveredOver == null) // To be safe
                return;

            buttonHoveredOver.BackColor = DefaultBackColor;

        }
        
        private void placeToken(int columnNumber)
        {
            // Create a countdown int
            int countDown = height - 1;

            //Find the label with this specific row and column
            Label lblFind = this.Controls.Find(("R" + countDown + "C" + columnNumber + "Label").ToString(), true).FirstOrDefault() as Label;

            while (lblFind.BackColor != Color.White)
            {
                countDown--;
                lblFind = this.Controls.Find(("R" + countDown + "C" + columnNumber + "Label").ToString(), true).FirstOrDefault() as Label;
                if (countDown < 0)
                    break;
            }
            if (countDown < 0)
            {
                MessageBox.Show("Column " + (columnNumber + 1) + " is full!");
            }
            else
            {
                if (currentPlayer == 1)
                    lblFind.BackColor = Color.Red;
                else
                    lblFind.BackColor = Color.Yellow;
                switchPlayer();
            }
            //Check for connect4
            findConnect4(1, columnNumber);
        }

        private void findConnect4(int tokenRow, int tokenColumn)
        {

        }
    }
}
