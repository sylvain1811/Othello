using System.ComponentModel;
using System.Timers;

namespace Othello
{
    class ContextPlayers : INotifyPropertyChanged
    {
        public Timer TimerWhite { get; }
        public Timer TimerBlack { get; }
        private int elapsedWhite;
        private int elapsedBlack;
        private string strTimerWhite;
        private string strTimerBlack;
        public string StrTimerWhite
        {
            get { return strTimerWhite; }
            set
            {
                strTimerWhite = value;
                NotifyPropertyChanged("StrTimerWhite");
            }
        }
        public string StrTimerBlack
        {
            get { return strTimerBlack; }
            set
            {
                strTimerBlack = value;
                NotifyPropertyChanged("StrTimerBlack");
            }
        }
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

        public ContextPlayers(int w, int b)
        {
            BlackScore = "" + b;
            WhiteScore = "" + w;
            elapsedWhite = 0;
            elapsedBlack = 0;
            StrTimerBlack = "0 s";
            strTimerWhite = "0 s";

            TimerBlack = new Timer
            {
                Interval = 1000
            };
            TimerBlack.Elapsed += TimerBlack_Elapsed;

            TimerWhite = new Timer
            {
                Interval = 1000
            };
            TimerWhite.Elapsed += TimerWhite_Elapsed;
        }

        private void TimerBlack_Elapsed(object sender, ElapsedEventArgs e)
        {
            StrTimerBlack = $"{ elapsedBlack++} s";
        }

        private void TimerWhite_Elapsed(object sender, ElapsedEventArgs e)
        {
            StrTimerWhite = $"{ elapsedWhite++} s";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string nomPropriete)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
        }
    }
}
