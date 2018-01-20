using System;
using System.ComponentModel;
using System.Timers;

namespace Othello
{
    class ContextPlayers : INotifyPropertyChanged
    {
        public Timer TimerWhite { get; set; }
        public Timer TimerBlack { get; set; }
        private int elapsedWhite;
        private int elapsedBlack;
        public int ElapsedWhite
        {
            get { return elapsedWhite; }
            set
            {
                elapsedWhite = value;
                StrTimerWhite = $"{value} s";
            }
        }
        public int ElapsedBlack
        {
            get { return elapsedBlack; }
            set
            {
                elapsedBlack = value;
                StrTimerBlack = $"{value} s";
            }
        }
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
            ElapsedWhite = 0;
            ElapsedBlack = 0;
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
