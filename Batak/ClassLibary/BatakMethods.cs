using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Batak
{
    public static class BatakMethods
    {
        public static Player[] _PlayerArray;
        public static string _startingPlayer;

        public static List<Cards> _midCards;
        public static Panel _PanelMid;
        
        public static Label _lblPlayerOrder;
        public static Label _lblSpacialType;

        public static Label _lblPlayer0Score;
        public static Label _lblPlayer1Score;
        public static Label _lblPlayer2Score;
        public static Label _lblPlayer3Score;

        public static PictureBox _testMidPanelWinner;



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
             _PlayerArray = new Player[] { player0, player1, player2, player3 };

            // Fiiling in Player Classes
            Random rnd = new Random();
            for (int playerNo = 0; playerNo < 4; playerNo++)
            {
                //Player Name:
                _PlayerArray[playerNo].Name = "player" + playerNo.ToString();
                //Player NextPlayerName:
                _PlayerArray[playerNo].NextPlayerName = "player" + (playerNo + 1).ToString();
                if (_PlayerArray[playerNo].NextPlayerName == "player4")
                {
                    _PlayerArray[playerNo].NextPlayerName = "player0";
                }
                //Players CardList's :
                for (int i = 0; i < 13; i++)
                {
                    //Select a random card from cardDeck and send player hand
                    int drawnCardNo = rnd.Next(0, cardDeck.Count);
                    Cards drawnCard = cardDeck[drawnCardNo];
                    _PlayerArray[playerNo].CardList.Add(drawnCard);

                    //Determination cards ownerships
                    string cardOwnership = _PlayerArray[playerNo].Name;
                    drawnCard.Ownership = cardOwnership;

                    //Remove drawn card from cardDeck for not-duplicitaion
                    cardDeck.RemoveAt(drawnCardNo);
                }
            }
            return _PlayerArray;
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
        public static void Visualization(Control PanelName, List<Cards> ListName, EventHandler RelatedEvent)
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

                picture.Click += new EventHandler(RelatedEvent);
                //picture.DoubleClick += new EventHandler(picture_doubleClick);
            }
        }

        /// <summary>
        /// Allows players to play in clockwise order
        /// </summary>
        public static void startRound()
        {
            //lblTest.Text = (_midCards.Count).ToString();
            //Disable all cards for next player playing
            
            //Whoever's turn is to enable their respective cards according to the game rules.

            foreach (Player RelatedPlayer in _PlayerArray)
            {
                if (RelatedPlayer.Name == _startingPlayer)
                {
                    foreach (PictureBox CardsImage in RelatedPlayer.RelatedPanel.Controls)
                    {
                        CardsImage.Enabled = false;
                    }
                    _lblPlayerOrder.Text = RelatedPlayer.Name;

                    //CardpPlayingRules(RelatedPlayer.RelatedPanel);
                    if (RelatedPlayer.Name != "player0")
                    {
                        //yzCardPlay(RelatedPlayer.Name);
                    }
                    _startingPlayer = RelatedPlayer.NextPlayerName;
                    if (_midCards.Count == 4)
                    {
                        MessageBox.Show("Round Bitti");
                        EvaluationMidCards();
                    }
                }
            }
        }

        /// <summary>
        /// Card comparasion at mid panel for round winner.
        /// </summary>
        /// <param name="midCards"></param>
        /// <returns></returns>
        public static Cards cardComparasionListCards(List<Cards> RelatedCardList)
        {
            string spacialType = _lblSpacialType.Text;
            Cards winnerCard = RelatedCardList[0];
            foreach (Cards relatedCard in RelatedCardList)
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
        public static void EvaluationMidCards()
        {
            // When the round is over, the enabled property of all cards is doing false
            foreach (Player relatedPlayer in _PlayerArray)
            {
                foreach (PictureBox cardsImages in relatedPlayer.RelatedPanel.Controls)
                {
                    cardsImages.Enabled = false;
                }
            }
            //Identification of the winner in the round
            Cards winnerCard = cardComparasionListCards(_midCards);
            //testMidPanelWinner.Image = winnerCard.Image;
            _startingPlayer = winnerCard.Ownership;
            if (_startingPlayer == "player0")
            {
                _lblPlayer0Score.Text = ((Convert.ToInt32(_lblPlayer0Score.Text)) + 1).ToString();
            }
            else if (_startingPlayer == "player1")
            {
                _lblPlayer1Score.Text = ((Convert.ToInt32(_lblPlayer1Score.Text)) + 1).ToString();
            }
            else if (_startingPlayer == "player2")
            {
                _lblPlayer2Score.Text = ((Convert.ToInt32(_lblPlayer2Score.Text)) + 1).ToString();
            }
            else if (_startingPlayer == "player3")
            {
                _lblPlayer3Score.Text = ((Convert.ToInt32(_lblPlayer3Score.Text)) + 1).ToString();
            }

            //---------------------------Mid winner test-----------------------------------

            _testMidPanelWinner.Image = winnerCard.Image;
            MessageBox.Show("Round bitti");
            _testMidPanelWinner.Image = null;
            //------------------------------------------------------------------------------

            _midCards.Clear();
            _PanelMid.Controls.Clear();
            //Checks game is over
            int player0Score = Convert.ToInt32(_lblPlayer0Score.Text);
            int player1Score = Convert.ToInt32(_lblPlayer1Score.Text);
            int player2Score = Convert.ToInt32(_lblPlayer2Score.Text);
            int player3Score = Convert.ToInt32(_lblPlayer3Score.Text);

            if ((player0Score + player1Score + player2Score + player3Score) == 13)
            {
                MessageBox.Show("Game Over");
            }
            else
            {
                startRound();
            }


        }

    }

}

