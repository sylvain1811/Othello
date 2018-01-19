using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class ContextScore : INotifyPropertyChanged
    {
        private string whiteScore;
        public string WhiteScore
        {
            get { return whiteScore; }
            set
            {
                whiteScore = value;
                NotifyPropertyChanged("WhiteScore");
            }
        }
        private string blackScore;
        public string BlackScore
        {
            get { return blackScore; }
            set
            {
                blackScore = value;
                NotifyPropertyChanged("BlackScore");
            }
        }

        public ContextScore(int w, int b)
        {
            BlackScore = "" + b;
            WhiteScore = "" + w;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string nomPropriete)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
        }
    }
}
