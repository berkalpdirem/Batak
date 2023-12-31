﻿using System;
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
            BatakMethods.mainMenuPage = this;
        }
        //Global variables and Lists ---------------------------------------------------------------------------------------
        public string startingPlayer = string.Empty;
        public List<Cards> midCards = new List<Cards>();
        public Player[] PlayerArray;

        /// <summary>
        /// Sending importand form elements to BatakMethods
        /// </summary>
        public void Postman()
        {
            BatakMethods._PlayerArray = PlayerArray;
            BatakMethods._startingPlayer = startingPlayer;

            BatakMethods._midCards = midCards;
            BatakMethods._PanelMid = panel_Mid;

            BatakMethods._lblPlayerOrder = lblPlayerOrder;
            BatakMethods._lblSpacialType = lblSpacialType;

            List<Label>  ScoreLabelList = new List<Label>() { lblPlayer0Score , lblPlayer1Score , lblPlayer2Score , lblPlayer3Score };
            BatakMethods._ScoreLabelList = ScoreLabelList;
            BatakMethods._lblPlayer0Score = lblPlayer0Score;
            BatakMethods._lblPlayer1Score = lblPlayer1Score;
            BatakMethods._lblPlayer2Score = lblPlayer2Score;
            BatakMethods._lblPlayer3Score = lblPlayer3Score;

            BatakMethods._testMidPanelWinner = testMidPanelWinner;
        }
        private void btn_NewGame_Click(object sender, EventArgs e)
        {
            BatakMethods.clearLists();
            Postman();
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
            Postman();
            //Startted Round
            BatakMethods.startRound();
            //Refreshing the visuals before the player plays
            //for (int i = 0; i < 4; i++)
            //{
            //    PlayerArray[i].RelatedPanel.Controls.Clear();
            //    BatakMethods.Visualization(PanelArray[i], PlayerArray[i].CardList, picture_Click);
            //}
            //panel_Mid.Controls.Clear();
            //BatakMethods.Visualization(panel_Mid,midCards,null);
            //BatakMethods.CardpPlayingRules(PlayerArray[0].RelatedPanel);
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void picture_Click(object sender, EventArgs e)
        {
            PictureBox cardPicturebox = (sender as PictureBox);
            Cards relatedCard = cardPicturebox.Image.Tag as Cards;

            cardPicturebox.Location = new Point(midCards.Count * 20, 50);
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

            //Check Round Ending 
            if (midCards.Count < 4)
            {
                BatakMethods._startingPlayer = "player1";
                BatakMethods.startRound();
            }
            else
            {
                BatakMethods.EvaluationMidCards();
            }
        }

    }
}