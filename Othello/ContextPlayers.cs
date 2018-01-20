using System;
using System.ComponentModel;
using System.Timers;

namespace Othello
{
    class ContextPlayers : INotifyPropertyChanged
    {
        public Timer Timer { get; set; }
        public bool WhiteTurn { get; set; }

        private int elapsedWhite;
        public int ElapsedWhite
        {
            get { return elapsedWhite; }
            set
            {
                elapsedWhite = value;
                // Affichage en minutes secondes
                TimeSpan timeSpan = TimeSpan.FromSeconds(value / 10);
                StrTimerWhite = string.Format("{0:D2}m:{1:D2}s", timeSpan.Minutes, timeSpan.Seconds);
            }
        }

        private int elapsedBlack;
        public int ElapsedBlack
        {
            get { return elapsedBlack; }
            set
            {
                elapsedBlack = value;
                // Affichage en minutes secondes
                TimeSpan timeSpan = TimeSpan.FromSeconds(value / 10);
                StrTimerBlack = string.Format("{0:D2}m:{1:D2}s", timeSpan.Minutes, timeSpan.Seconds);
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
                // Signale que la propriété a changée
                NotifyPropertyChanged("StrTimerWhite");
            }
        }
        public string StrTimerBlack
        {
            get { return strTimerBlack; }
            set
            {
                strTimerBlack = value;
                // Signale que la propriété a changée
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
                // Signale que la propriété a changée
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
                // Signale que la propriété a changée
                NotifyPropertyChanged("BlackScore");
            }
        }

        public ContextPlayers(int whiteScore, int blackScore)
        {
            BlackScore = "" + blackScore;
            WhiteScore = "" + whiteScore;
            ElapsedWhite = 0;
            ElapsedBlack = 0;

            Timer = new Timer
            {
                Interval = 100 // 100ms
            };
            Timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// Incrémentation du temps de réfléxion d'un joueur. Mise à jour automatique de la GUI par binding.
        /// </summary>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (WhiteTurn)
                ElapsedWhite++;
            else
                ElapsedBlack++;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string nomPropriete)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomPropriete));
        }
    }
}
