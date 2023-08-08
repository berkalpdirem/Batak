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
    public partial class betPage : Form
    {
        public MainMenu mainMenuPage;
        public betPage()
        {
            InitializeComponent();
            
        }
        Random rnd = new Random();
        int player0bet = 0;
        int player1bet = 0;
        int player2bet = 0;
        int player3bet = 0;
        public int bet;
        public string selectedSpacialType;
        public string betWinner;
        


        private void betPage_Load(object sender, EventArgs e)
        {
            mainMenuPage.betPageDialog = this;
            yzBetDetermination();
            
            
        }

        public void betComparasion()
        {
            if (player0bet > player1bet && player0bet > player2bet && player0bet > player3bet)
            {
                betWinner = "player0";
                mainMenuPage.startingPlayer = betWinner;
                bet = player0bet;
            }
            else if (player1bet > player0bet && player1bet > player2bet && player1bet > player3bet)
            {
                betWinner = "player1";
                mainMenuPage.startingPlayer = betWinner;
                bet = player1bet;
            }
            else if (player2bet > player0bet && player2bet > player1bet && player2bet > player3bet)
            {
                betWinner = "player2";
                mainMenuPage.startingPlayer = betWinner;
                bet = player2bet;
            }
            else if (player3bet > player0bet && player3bet > player1bet && player3bet > player2bet)
            {
                betWinner = "player3";
                mainMenuPage.startingPlayer = betWinner;
                bet = player3bet;
            }
        }

        public void yzBetDetermination()
        {
            // will update
            player1bet = rnd.Next(8, 13);
            player1bet = 8;
            lblPlayer1Bet.Text = player1bet.ToString();

            player2bet = rnd.Next(8, 13);
            player2bet = 9;
            lblPlayer2Bet.Text = player2bet.ToString();

            player3bet = rnd.Next(8, 13);
            player3bet = 10;
            lblPlayer3Bet.Text = player3bet.ToString();

            betComparasion();
        }
        #region Button Events

        private void btnConfirmBet_Click(object sender, EventArgs e)
        {
            selectedSpacialType = lblSpacialType.Text;
            mainMenuPage.gbBetSummary.Visible = true;
            mainMenuPage.gbGameInfo.Visible = true;

            mainMenuPage.lblBet.Text = bet.ToString() ;
            mainMenuPage.lblBetWinner.Text = betWinner;
            mainMenuPage.lblPlayerOrder.Text = betWinner;
            mainMenuPage.startingPlayer = betWinner;
            mainMenuPage.lblSpacialType.Text = selectedSpacialType; 
            Close();
        }

        private void btnPass_Click(object sender, EventArgs e)
        {
            player0bet = 0;
            lblPlayer0Bet.Text = player0bet.ToString();
        }

        private void btnSelectBet8_Click(object sender, EventArgs e)
        {
            lblPlayer0Bet.Text = 8.ToString();
            player0bet = 8;
            betComparasion();
        }

        private void btnSelectBet9_Click(object sender, EventArgs e)
        {
            lblPlayer0Bet.Text = 9.ToString();
            player0bet = 9;
            betComparasion();
        }

        private void btnSelectBet10_Click(object sender, EventArgs e)
        {
            lblPlayer0Bet.Text = 10.ToString();
            player0bet = 10;
            betComparasion();
        }

        private void btnSelectBet11_Click(object sender, EventArgs e)
        {
            lblPlayer0Bet.Text = 11.ToString();
            player0bet = 11;
            betComparasion();
        }

        private void btnSelectBet12_Click(object sender, EventArgs e)
        {
            lblPlayer0Bet.Text = 12.ToString();
            player0bet = 12;
            betComparasion();
        }

        private void btnSelectBet13_Click(object sender, EventArgs e)
        {
            lblPlayer0Bet.Text = 13.ToString();
            player0bet = 13;
            betComparasion();
        }

        private void pbClub_Click(object sender, EventArgs e)
        {
            lblSpacialType.Text = "Clubs";
            mainMenuPage.pbSpacialType.Image = Batak.Properties.Resources.clubs;
            
        }

        private void pbHeart_Click(object sender, EventArgs e)
        {
            lblSpacialType.Text = "Heart";
            mainMenuPage.pbSpacialType.Image = Batak.Properties.Resources.hearts;
        }

        private void pbSpade_Click(object sender, EventArgs e)
        {
            lblSpacialType.Text = "Spade";
            mainMenuPage.pbSpacialType.Image = Batak.Properties.Resources.spades;
        }

        private void pbDiamond_Click(object sender, EventArgs e)
        {
            lblSpacialType.Text = "Diamond";
            mainMenuPage.pbSpacialType.Image = Batak.Properties.Resources.dimonds;
        }

        #endregion
    }
}
