using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Batak
{

    public partial class MainMenu : Form
    {
        public betPage betPageDialog = new betPage();

        public MainMenu()
        {
            InitializeComponent();
            betPageDialog.mainMenuPage = this;
        }
        //Global variables and Lists ---------------------------------------------------------------------------------------
        public string startingPlayer;
        public List<Cards> midCards = new List<Cards>();
        public Player[] PlayerArray;

        private void btn_NewGame_Click(object sender, EventArgs e)
        {
            PlayerArray = BatakMethods.CreateDeck();
            Panel[] PanelArray = new Panel[] { panelPlayer0, panelPlayer1, panelPlayer2, panelPlayer3 };
            //All Player's Hands Are Sorted and Visualized
            for (int i = 0; i < 4; i++)
            {
                PlayerArray[i].RelatedPanel = PanelArray[i];
                BatakMethods.SortingList(PlayerArray[i].CardList);
                BatakMethods.Visualization(PanelArray[i], PlayerArray[i].CardList, picture_Click);
            }
            //Players' Bets Are Taken
            betPageDialog.ShowDialog();


        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        public void picture_Click(object sender, EventArgs e)
        {
            PictureBox cardPicturebox = (sender as PictureBox);
            Cards relatedCard = cardPicturebox.Image.Tag as Cards;

            cardPicturebox.Location = new Point(midCards.Count * 20, 0);
            for (int i = 0; i < 4; i++)
            {
                if (relatedCard.Ownership == PlayerArray[i].Name)
                {
                    PlayerArray[i].CardList.Remove(relatedCard);
                }
            }
            midCards.Add(relatedCard);
            panel_Mid.Controls.Add(cardPicturebox);
            cardPicturebox.BringToFront();
            cardPicturebox.Enabled = false;

            //visualizationAllCards();
            //Check Round Ending 
            //if (midCards.Count < 4)
            //{
            //    startRound();
            //}
            //else
            //{
            //    EvaluationMidCards();
            //}
        }
    }
}