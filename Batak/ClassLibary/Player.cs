using System.Collections.Generic;
using System.Windows.Forms;

namespace Batak
{
    public class Player
    {
        public string Name = string.Empty;

        public string NextPlayerName = string.Empty;

        public int Score;

        public int GenelScore;

        public List<Cards> CardList;

        public Panel RelatedPanel;

        public Player()
        {
             CardList = new List<Cards>();
        }

    }

}

