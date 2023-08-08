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
            betPageDialog.mainMenu = this;
        }

        //Global variables and Lists ---------------------------------------------------------------------------------------

        Random rnd = new Random();
        string[] playerList = new string[] { "player0", "player1", "player2", "player3" };
        
        List<List<Cards>> allPlayerHands = new List<List<Cards>>();
        List<Cards> player0Cards = new List<Cards>();
        List<Cards> player1Cards = new List<Cards>();
        List<Cards> player2Cards = new List<Cards>();
        List<Cards> player3Cards = new List<Cards>();

        List<Control> panelList = new List<Control>();

        
        #region PrePlayMethods
        public void CreateDeck()
        {
            //Cards creation;
            string[] cardTypeList = new string[] { "Clubs", "Spades", "Hearts", "Diamonds" };
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
                    card.Image= deckPic.Clone(new Rectangle(y * w, x * h, w, h), deckPic.PixelFormat);
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
                if (fixingList[i].Type == "Clubs")
                {
                    tempListClubs.Add(fixingList[i]);
                    tempListClubs.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));
                }
                else if (fixingList[i].Type == "Hearts")
                {
                    tempListHearts.Add(fixingList[i]);
                    tempListHearts.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));
                }
                else if (fixingList[i].Type == "Spades")
                {
                    tempListSpades.Add(fixingList[i]);
                    tempListSpades.Sort((card1, card2) => card2.Value.CompareTo(card1.Value));  
                }
                else if (fixingList[i].Type == "Diamonds")
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


        #endregion

        #region Animation and Vizualition Methods



        /// <summary>
        /// Create Card's picture box for MainMenu
        /// </summary>
        /// <param name="PanelNo"></param>
        /// <param name="ListName"></param>
        public void visualization(Control PanelNo, List<Cards> ListName)
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
                //picture.DoubleClick += new EventHandler(picture_doubleClick);
                //picture.Click += new EventHandler(picture_Click);
            }
        }

        /// <summary>
        /// Visualization of cards in panels
        /// </summary>
        public void visualizationAllCards()
        {

            visualization(panelPlayer0, player0Cards);
            visualization(panelPlayer1, player1Cards);
            visualization(panelPlayer2, player2Cards);
            visualization(panelPlayer3, player3Cards);

            //visualization(panelMid, midCards);
        }


        #endregion

        #region Game Situation

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
        #endregion
        
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
            
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void MainMenu_Load(object sender, EventArgs e)
        {
            

        }
    }
    public class Cards
    {
        public string Type { get; set; }
        public int Value { get; set; }
        public Image Image { get; set; }
        public string Ownership { get; set; }
    }



}
