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

        Random rnd = new Random();
        string[] playerList = new string[] { "player0", "player1", "player2", "player3" };

        List<List<Cards>> allPlayerHands = new List<List<Cards>>();
        List<Cards> player0Cards = new List<Cards>();
        List<Cards> player1Cards = new List<Cards>();
        List<Cards> player2Cards = new List<Cards>();
        List<Cards> player3Cards = new List<Cards>();
        List<Cards> midCards = new List<Cards>();

        List<Control> panelList = new List<Control>();
        public string startingPlayer;




        public void CreateDeck()
        {
            //Cards creation;
            string[] cardTypeList = new string[] { "Club", "Spade", "Heart", "Diamond" };
            int[] cardNoList = new int[] { 14, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

            int w = 73; //Card's x Dimensions
            int h = 98; //Card's y Dimensions
            Bitmap deckPic = Batak.Properties.Resources.deckPic;
            List<Cards> cardDeck = new List<Cards>();
            for (int x = 0; x < cardTypeList.Length; x++)
            {
                for (int y = 0; y < cardNoList.Length; y++)
                {
                    Cards card = new Cards();
                    card.Type = cardTypeList[x];
                    card.Value = cardNoList[y];
                    card.Image = deckPic.Clone(new Rectangle(y * w, x * h, w, h), deckPic.PixelFormat);
                    card.Image.Tag = card;
                    cardDeck.Add(card);
                }
            }
            // Deal player hands
            allPlayerHands.Add(player0Cards);
            allPlayerHands.Add(player1Cards);
            allPlayerHands.Add(player2Cards);
            allPlayerHands.Add(player3Cards);
            for (int player = 0; player < 4; player++)
            {
                for (int i = 0; i < 13; i++)
                {
                    //Select a random card from cardDeck and send player hand
                    int drawnCardNo = rnd.Next(0, cardDeck.Count);
                    Cards drawnCard = cardDeck[drawnCardNo];
                    allPlayerHands[player].Add(drawnCard);

                    //Determination cards ownerships
                    string cardOwnership = "player" + player.ToString();
                    drawnCard.Ownership = cardOwnership;

                    //Remove drawn card from cardDeck for not-duplicitaion
                    cardDeck.RemoveAt(drawnCardNo);
                }
            }
        }

        /// <summary>
        /// Sorting the cards received by the players from largest to smallest in clusters
        /// </summary>
        /// <param name="fixingList"></param>
        public void fixHand(List<Cards> fixingList)
        {

            //Creating sub list for sorting by cards type
            List<Cards> tempListClubs = new List<Cards>();
            List<Cards> tempListHearts = new List<Cards>();
            List<Cards> tempListSpades = new List<Cards>();
            List<Cards> tempListDiamonds = new List<Cards>();


            for (int i = 0; i < fixingList.Count; i++)
            {
                if (fixingList[i].Type == "Club")
                {
                    tempListClubs.Add(fixingList[i]);
                    tempListClubs.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));
                }
                else if (fixingList[i].Type == "Heart")
                {
                    tempListHearts.Add(fixingList[i]);
                    tempListHearts.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));
                }
                else if (fixingList[i].Type == "Spade")
                {
                    tempListSpades.Add(fixingList[i]);
                    tempListSpades.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));
                }
                else if (fixingList[i].Type == "Diamond")
                {
                    tempListDiamonds.Add(fixingList[i]);
                    tempListDiamonds.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));
                }
            }
            // Mergiing created sublist and clear all of them
            List<List<Cards>> subLists = new List<List<Cards>>();
            subLists.Add(tempListClubs);
            subLists.Add(tempListHearts);
            subLists.Add(tempListSpades);
            subLists.Add(tempListDiamonds);

            fixingList.Clear();
            foreach (List<Cards> subList in subLists)
            {
                foreach (Cards card in subList)
                {
                    fixingList.Add(card);
                }
                subList.Clear();
            }

        }

        /// <summary>
        /// Create Card's picture box for MainMenu
        /// </summary>
        /// <param name="PanelNo"></param>
        /// <param name="ListName"></param>
        public void Visualization(Control PanelNo, List<Cards> ListName)
        {
            panelList.Add(PanelNo);
            for (int i = 0; i < ListName.Count; i++)
            {
                //Cards visualizations 
                PictureBox picture = new PictureBox();

                picture.Image = allPlayerHands[panelList.Count - 1][i].Image;

                picture.Size = picture.Image.Size;
                picture.Location = new Point(20 * i, 50);
                panelList[panelList.Count - 1].Controls.Add(picture);
                picture.BringToFront();

                //Cards Events;
                picture.Enabled = false;

                picture.Click += new EventHandler(picture_Click);
                //picture.DoubleClick += new EventHandler(picture_doubleClick);
            }
        }

        /// <summary>
        /// Visualization of cards in panels
        /// </summary>
        public void visualizationAllCards()
        {
            if (panelList.Count == 5)
            {
                panelList.Clear();
            }
            Visualization(panelPlayer0, player0Cards);
            Visualization(panelPlayer1, player1Cards);
            Visualization(panelPlayer2, player2Cards);
            Visualization(panelPlayer3, player3Cards);

            Visualization(panel_Mid, midCards);
        }

        public void picture_Click(object sender, EventArgs e)
        {
            PictureBox cardPicturebox = (sender as PictureBox);
            Cards relatedCard = cardPicturebox.Image.Tag as Cards;

            cardPicturebox.Location = new Point(midCards.Count * 20, 0);
            for (int i = 0; i < 4; i++)
            {
                if (relatedCard.Ownership == playerList[i])
                {
                    allPlayerHands[i].Remove(relatedCard);
                }
            }
            midCards.Add(relatedCard);
            panel_Mid.Controls.Add(cardPicturebox);
            cardPicturebox.BringToFront();
            cardPicturebox.Enabled = false;

            //visualizationAllCards();
            //Check Round Ending 
            if (midCards.Count < 4)
            {
                startRound();
            }
            else
            {
                EvaluationMidCards();
            }
        }


        /// <summary>
        /// Changing the enable feature of the relevant panel's controls according to the game rules
        /// </summary>
        /// <param name="selectedPanel"></param>
        public void CardpPlayingRules(Panel relatedPanel)
        {
            //Player can play the first card of the Round as player pleases.
            if (midCards.Count == 0)
            {
                foreach (PictureBox CardsImage in relatedPanel.Controls)
                {
                    CardsImage.Enabled = true;
                }
            }
            //If the hand played is not the first hand of the round, it must be played according to the following rules
            else
            {
                Cards firstPlayedCard = (Cards)midCards[0];
                Cards lastPlayedCard = (Cards)midCards[midCards.Count - 1];


                //------------------------------------------------------Simple Rules-------------------------------------------

                //Obligation to throw a card from the first type of card thrown
                //And obligation to throw the larger card than the first discarded card
                foreach (PictureBox CardsImage in relatedPanel.Controls)
                {
                    Cards selectedCard = (Cards)CardsImage.Image.Tag;
                    if (firstPlayedCard.Type == selectedCard.Type && cardComparasionMidCards(midCards).Value < selectedCard.Value)
                    {
                        CardsImage.Enabled = true;
                    }
                }

                //------------------------------------------------------Complex Rules-------------------------------------------

                //If all cards are disenable, the smaller card of the first throwed type must be discarded.
                if (allCardsDisenable(relatedPanel))
                {
                    foreach (PictureBox CardsImage in relatedPanel.Controls)
                    {
                        Cards selectedCard = (Cards)CardsImage.Image.Tag;
                        if (firstPlayedCard.Type == selectedCard.Type)
                        {
                            CardsImage.Enabled = true;
                        }
                    }
                }

                //If player still can't throw cards, special cards will be activated
                if (allCardsDisenable(relatedPanel))
                {
                    foreach (PictureBox CardsImage in relatedPanel.Controls)
                    {
                        Cards card = (Cards)CardsImage.Image.Tag;
                        if (card.Type == lblSpacialType.Text)
                        {
                            CardsImage.Enabled = true;
                        }
                    }
                }

                //If player doesn't have a special card, player can discard any card player wants.

                if (allCardsDisenable(relatedPanel))
                {
                    foreach (PictureBox CardsImage in relatedPanel.Controls)
                    {
                        CardsImage.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// // Checking the enable property of all cards to be false to related panel
        /// </summary>
        /// <param name="relatedPanel"></param>
        /// <returns></returns>
        public bool allCardsDisenable(Panel relatedPanel)
        {
            bool result = true;
            foreach (PictureBox CardsImage in relatedPanel.Controls)
            {
                if (CardsImage.Enabled)
                {
                    return false;
                }
            }
            return result;
        }

        /// <summary>
        /// The computer chooses which card to play and plays
        /// </summary>
        public void yzCardPlay(string player)
        {
            Random rnd = new Random();

            List<Cards> yzChooseCardsProbabilities = new List<Cards>();
            Cards YzSelectedCard;
            if (player == "player1")
            {

                foreach (PictureBox cardPictureBox in panelPlayer1.Controls)
                {
                    if (cardPictureBox.Enabled)
                    {
                        yzChooseCardsProbabilities.Add((Cards)(cardPictureBox.Image.Tag));
                    }
                }
                YzSelectedCard = yzChooseCardsProbabilities[rnd.Next(0, yzChooseCardsProbabilities.Count)];
                midCards.Add(YzSelectedCard);
                player1Cards.Remove(YzSelectedCard);
                if (midCards.Count < 5)
                {
                    startRound();
                }
            }
            else if (player == "player2")
            {

                foreach (PictureBox cardPictureBox in panelPlayer2.Controls)
                {
                    if (cardPictureBox.Enabled)
                    {
                        yzChooseCardsProbabilities.Add((Cards)(cardPictureBox.Image.Tag));
                    }
                }
                YzSelectedCard = yzChooseCardsProbabilities[rnd.Next(0, yzChooseCardsProbabilities.Count)];
                midCards.Add(YzSelectedCard);
                player2Cards.Remove(YzSelectedCard);
                if (midCards.Count < 5)
                {
                    startRound();
                }
            }
            else if (player == "player3")
            {

                foreach (PictureBox cardPictureBox in panelPlayer3.Controls)
                {
                    if (cardPictureBox.Enabled)
                    {
                        yzChooseCardsProbabilities.Add((Cards)(cardPictureBox.Image.Tag));
                    }
                }
                YzSelectedCard = yzChooseCardsProbabilities[rnd.Next(0, yzChooseCardsProbabilities.Count)];
                midCards.Add(YzSelectedCard);
                player3Cards.Remove(YzSelectedCard);
                if (midCards.Count < 5)
                {
                    startRound();
                }
            }
        }

        /// <summary>
        /// Allows players to play in clockwise order
        /// </summary>
        public void startRound()
        {
            lblTest.Text = (midCards.Count).ToString();
            //Disable all cards for next player playing
            for (int i = 0; i < panelList.Count; i++)
            {
                if (panelList[i].Controls.Count != 0)
                {
                    foreach (PictureBox CardsImage in panelList[i].Controls)
                    {
                        CardsImage.Enabled = false;
                    }
                }

            }
            //Whoever's turn is to enable their respective cards according to the game rules.
            if (startingPlayer == "player0")
            {
                lblPlayerOrder.Text = "player0";
                CardpPlayingRules(panelPlayer0);
                startingPlayer = "player1";
                if (midCards.Count == 4)
                {
                    EvaluationMidCards();
                }
            }
            else if (startingPlayer == "player1")
            {
                lblPlayerOrder.Text = "player1";
                CardpPlayingRules(panelPlayer1);
                yzCardPlay("player1");
                startingPlayer = "player2";
                if (midCards.Count == 4)
                {
                    EvaluationMidCards();
                }
            }
            else if (startingPlayer == "player2")
            {
                lblPlayerOrder.Text = "player2";
                CardpPlayingRules(panelPlayer2);
                yzCardPlay("player2");
                startingPlayer = "player3";
                if (midCards.Count == 4)
                {
                    EvaluationMidCards();
                }
            }
            else if (startingPlayer == "player3")
            {
                lblPlayerOrder.Text = "player3";
                CardpPlayingRules(panelPlayer3);
                yzCardPlay("player3");
                startingPlayer = "player0";
                if (midCards.Count == 4)
                {
                    EvaluationMidCards();
                }
            }
        }

        /// <summary>
        /// Card comparasion at mid panel for round winner.
        /// </summary>
        /// <param name="midCards"></param>
        /// <returns></returns>
        public Cards cardComparasionMidCards(List<Cards> midCards)
        {
            string spacialType = lblSpacialType.Text;
            Cards winnerCard = midCards[0];
            foreach (Cards relatedCard in midCards)
            {
                if (winnerCard.Type == relatedCard.Type && winnerCard.Value < relatedCard.Value)
                {
                    winnerCard = relatedCard;
                }
                else if (winnerCard.Type != relatedCard.Type && winnerCard.Type != spacialType && relatedCard.Type == spacialType)
                {
                    winnerCard = relatedCard;
                }
            }
            return winnerCard;
        }


        /// <summary>
        /// What to do after detecting the winner of the  mid panel
        /// </summary>
        public void EvaluationMidCards()
        {
            // When the round is over, the enabled property of all cards is doing false
            foreach (Control relatedPanel in panelList)
            {
                foreach (PictureBox cardsImages in relatedPanel.Controls)
                {
                    cardsImages.Enabled = false;
                }
            }
            //Identification of the winner in the round
            Cards winnerCard = cardComparasionMidCards(midCards);
            testMidPanelWinner.Image = winnerCard.Image;
            startingPlayer = winnerCard.Ownership;
            if (startingPlayer == "player0")
            {
                lblPlayer0Score.Text = ((Convert.ToInt32(lblPlayer0Score.Text)) + 1).ToString();
            }
            else if (startingPlayer == "player1")
            {
                lblPlayer1Score.Text = ((Convert.ToInt32(lblPlayer1Score.Text)) + 1).ToString();
            }
            else if (startingPlayer == "player2")
            {
                lblPlayer2Score.Text = ((Convert.ToInt32(lblPlayer2Score.Text)) + 1).ToString();
            }
            else if (startingPlayer == "player3")
            {
                lblPlayer3Score.Text = ((Convert.ToInt32(lblPlayer3Score.Text)) + 1).ToString();
            }

            //---------------------------Mid winner test-----------------------------------

            testMidPanelWinner.Image = winnerCard.Image;
            MessageBox.Show("Round bitti");
            testMidPanelWinner.Image = null;
            //------------------------------------------------------------------------------

            midCards.Clear();
            panel_Mid.Controls.Clear();
            //Checks game is over
            int player0Score = Convert.ToInt32(lblPlayer0Score.Text);
            int player1Score = Convert.ToInt32(lblPlayer1Score.Text);
            int player2Score = Convert.ToInt32(lblPlayer2Score.Text);
            int player3Score = Convert.ToInt32(lblPlayer3Score.Text);

            if ((player0Score + player1Score + player2Score + player3Score) == 13)
            {
                MessageBox.Show("Game Over");
            }
            else
            {
                startRound();
            }


        }
        public void clearLists()
        {
            player0Cards.Clear();
            player1Cards.Clear();
            player2Cards.Clear();
            player3Cards.Clear();
            allPlayerHands.Clear();

            panelPlayer0.Controls.Clear();
            panelPlayer1.Controls.Clear();
            panelPlayer2.Controls.Clear();
            panelPlayer3.Controls.Clear();
            panelList.Clear();

        }
      
        private void btn_NewGame_Click(object sender, EventArgs e)
        {
            clearLists();
            CreateDeck();
            foreach (List<Cards> playerhands in allPlayerHands)
            {
                fixHand(playerhands);
            }
            visualizationAllCards();
            
            betPageDialog.ShowDialog();

            startRound();
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }



}