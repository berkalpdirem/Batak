using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Batak
{
    public static class BatakMethods
    {
        public static Player[] CreateDeck()
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

            //Player Creations
            Player player0 = new Player();
            Player player1 = new Player();
            Player player2 = new Player();
            Player player3 = new Player();
            Player[] PlayerArray = new Player[] { player0, player1, player2 , player3};
            
            // Fiiling in Player Classes
            Random rnd = new Random();
            for (int playerNo = 0; playerNo < 4; playerNo++)
            {
                //Player Name:
                PlayerArray[playerNo].Name = "player" + playerNo.ToString();
                //Player NextPlayerName:
                PlayerArray[playerNo].NextPlayerName = "player" + (playerNo + 1).ToString();
                if (PlayerArray[playerNo].NextPlayerName == "player4")
                {
                    PlayerArray[playerNo].NextPlayerName = "player0";
                }
                //Players CardList's :
                for (int i = 0; i < 13; i++)
                {
                    //Select a random card from cardDeck and send player hand
                    int drawnCardNo = rnd.Next(0, cardDeck.Count);
                    Cards drawnCard = cardDeck[drawnCardNo];
                    PlayerArray[playerNo].CardList.Add(drawnCard);

                    //Determination cards ownerships
                    string cardOwnership = PlayerArray[playerNo].Name;
                    drawnCard.Ownership = cardOwnership;

                    //Remove drawn card from cardDeck for not-duplicitaion
                    cardDeck.RemoveAt(drawnCardNo);
                }
            }
            return PlayerArray;
        }

        /// <summary>
        /// Sorting the cards received by the players from largest to smallest in clusters
        /// </summary>
        /// <param name="ListToSort"></param>
        public static void SortingList(List<Cards> ListToSort)
        {

            //Creating sub list for sorting by cards type
            List<Cards> tempListClubs = new List<Cards>();
            List<Cards> tempListHearts = new List<Cards>();
            List<Cards> tempListSpades = new List<Cards>();
            List<Cards> tempListDiamonds = new List<Cards>();


            for (int i = 0; i < ListToSort.Count; i++)
            {
                if (ListToSort[i].Type == "Club")
                {
                    tempListClubs.Add(ListToSort[i]);
                    tempListClubs.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));
                }
                else if (ListToSort[i].Type == "Heart")
                {
                    tempListHearts.Add(ListToSort[i]);
                    tempListHearts.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));
                }
                else if (ListToSort[i].Type == "Spade")
                {
                    tempListSpades.Add(ListToSort[i]);
                    tempListSpades.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));
                }
                else if (ListToSort[i].Type == "Diamond")
                {
                    tempListDiamonds.Add(ListToSort[i]);
                    tempListDiamonds.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));
                }
            }
            // Mergiing created sublist and clear all of them
            List<List<Cards>> subLists = new List<List<Cards>>();
            subLists.Add(tempListClubs);
            subLists.Add(tempListHearts);
            subLists.Add(tempListSpades);
            subLists.Add(tempListDiamonds);

            ListToSort.Clear();
            foreach (List<Cards> subList in subLists)
            {
                foreach (Cards card in subList)
                {
                    ListToSort.Add(card);
                }
                subList.Clear();
            }
        }

        /// <summary>
        /// Create Card's picture box for MainMenu
        /// </summary>
        /// <param name="PanelName"></param>
        /// <param name="ListName"></param>
        public static void Visualization(Control PanelName, List<Cards> ListName, EventHandler picture_Click)
        {   
            for (int i = 0; i < ListName.Count; i++)
            {
                //Cards visualizations 
                PictureBox picture = new PictureBox();

                picture.Image = ListName[i].Image;

                picture.Size = picture.Image.Size;
                picture.Location = new Point(20 * i, 50);
                PanelName.Controls.Add(picture);
                picture.BringToFront();

                //Cards Events;
                picture.Enabled = false;

                picture.Click += new EventHandler(picture_Click);
                //picture.DoubleClick += new EventHandler(picture_doubleClick);
            }
        }

        /// <summary>
        /// Allows players to play in clockwise order
        /// </summary>
        public static void startRound(Player[] PlayerArray, List<Cards> MidCards , string startingPlayer)
        {
            //lblTest.Text = (midCards.Count).ToString();
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
                //CardpPlayingRules(panelPlayer0);
                startingPlayer = "player1";
                if (MidCards.Count == 4)
                {
                    EvaluationMidCards();
                }
            }
            else if (startingPlayer == "player1")
            {
                lblPlayerOrder.Text = "player1";
                //CardpPlayingRules(panelPlayer1);
                //yzCardPlay("player1");
                startingPlayer = "player2";
                if (MidCards.Count == 4)
                {
                    //EvaluationMidCards();
                }
            }
            else if (startingPlayer == "player2")
            {
                lblPlayerOrder.Text = "player2";
                //CardpPlayingRules(panelPlayer2);
                //yzCardPlay("player2");
                startingPlayer = "player3";
                if (MidCards.Count == 4)
                {
                    EvaluationMidCards();
                }
            }
            else if (startingPlayer == "player3")
            {
                lblPlayerOrder.Text = "player3";
                //CardpPlayingRules(panelPlayer3);
                //yzCardPlay("player3");
                startingPlayer = "player0";
                if (MidCards.Count == 4)
                {
                    EvaluationMidCards();
                }
            }
        }
    }

}

