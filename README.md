# Othello

### Presentation

Othello est un jeu de société à plateau de 64 cases où deux joueurs se partagent 64 pions bicolores noirs d'un coté et blancs de l'autre. Le but étant tout au long de la partie que les joueurs aient le plus possible de leurs pions respectifs. Pour plus de détails sur les règles, suivez ce liens : http://www.ffothello.org/othello/regles-du-jeu/

### Intelligence Artificielle

Dans le cadre d'un projet de Bachelor à la He-Arc de Neuchâtel, il nous a été demandé d'implémenter une intelligence artificielle permettant de jouer une partie d'Othello contre une autre personne (humaine) ou encore simplement contre une autre intelligence artificielle. De ce fait nous avons utilisé une fonction AlphaBeta qui permet de déterminer quel est le meilleur coup suivant à jouer en utilisant une autre fonction d'évaluation qui permet de donner un poid à un coup de jeu.

Ces fonctions seront revues avec quelques explications et commentaires dans les chapitres suivants afin de mieux comprendre notre implementation. Nous concentrerons nos explications sur la fonction d'évalutation et celle définissant les règles du jeu de l'Othello car la fonction AlphaBeta n'est en fait qu'une fonction AlphaBeta standard.
