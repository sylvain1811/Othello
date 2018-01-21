# Othello

Projet d'intelligence artificielle du jeu Othello, avec un implementation graphique. 
Ces projets sont réalisés en C#.

## Cadre

### Presentation

Othello est un jeu de société à plateau de 64 cases où deux joueurs se partagent 64 pions bicolores noirs d'un coté et blancs de l'autre. Le but étant tout au long de la partie que les joueurs aient le plus possible de leurs pions respectifs. Pour plus de détails sur les règles, suivez ce liens : http://www.ffothello.org/othello/regles-du-jeu/

### Intelligence Artificielle

Dans le cadre d'un projet de Bachelor à la He-Arc de Neuchâtel, il nous a été demandé d'implémenter une intelligence artificielle permettant de jouer une partie d'Othello contre une autre personne (humaine) ou encore simplement contre une autre intelligence artificielle. De ce fait nous avons utilisé une fonction AlphaBeta qui permet de déterminer quel est le meilleur coup suivant à jouer en utilisant une autre fonction d'évaluation qui permet de donner un poid à un coup de jeu.

Ces fonctions seront revues avec quelques explications et commentaires dans les chapitres suivants afin de mieux comprendre notre implementation. Nous concentrerons nos explications sur la fonction d'évalutation et celle définissant les règles du jeu de l'Othello car la fonction AlphaBeta n'est en fait qu'une fonction AlphaBeta standard.

## Implementation IA

### Fonction CheckOrPlay

### Fonction Eval

La fonction d'évaluation permet de donner un poids aux coups qui sont possibles de jouer. Ce poids permettra de définir quel sera le meilleur coup à jouer. Afin de déterminer ce dernier, il faut d'abord comprendre quels critères permettent de faire gagner une partie d'Othello. 

Dans notre cas, nous allons nous intéresser à quatre critères heuristiques qui aideront à déterminer le poids d'un coup : 

- La parité des pions
- La mobilité
- Les coins capturés
- La stabilité

Au départ, les premières intelligences artificielles pour Othello se basaient sur le fait de gagner le plus de pions possibles à chaque tour. Malheureusement, ce n'était pas une bonne idée car un pion peut potentiellement retourner jusqu'à 18 autres pions et donc renverser le cours de la partie. C'est pour cela qu'on se concentrera plutôt sur le gain de quelques pions plus important que vouloir tous les prendre (on priviligera toujours les pions qui sont stables). 

Pour cela, on calcule premièrement la différence de pions entre le joueur à maximiser et celui à minimiser. Ce critère correspond à la parité des pions. On détermine ce critère à l'aide du code ci-dessous.

```c#
//Récupère le nombre de pions des joueurs à maximiser et à minimiser
maxPlayerCoin = isWhite ? board.GetWhiteScore() : board.GetBlackScore();
minPlayerCoin = isWhite ? board.GetBlackScore() : board.GetWhiteScore();

//Retourne une valeur entre -100 et 100 du rapport du nombre de pion à maximiser sur le total des deux joueurs
if (maxPlayerCoin > minPlayerCoin){ 
   coinParity = (100 * maxPlayerCoin)/(maxPlayerCoin + minPlayerCoin);
}else if (maxPlayerCoin < minPlayerCoin){
   coinParity = -(100 * minPlayerCoin)/(maxPlayerCoin + minPlayerCoin);
}
```
Le second critère est celui de la mobilité, il y a deux type de mobilité dans le cas du jeu Othello. La mobilité actuel qui correspond aux nombre de coups possibles à un instant t du jeu. Et la mobilité potentielle qui elle correspond aux nombres de coup qui seront possibles. Nous utiliserons, nous, une légère variante qui fait le rapport du nombre de coups possibles à joueur pour le joueur à maximiser par le nombre de coups possibles à jouer des deux joueurs. Ce rapport, comme le précédent retourne une valeur entre -100 et 100. 

Le troisième critère fait certainement parti des critères les plus important. Il donne un poids arbitraire mais stratégique à chacune des cases du damier. Plus le poids de la case est grande, plus on souhaiterais se diriger vers l'une de ces cases pour y poser notre pion. Ces cases sont les coins, car une fois qu'un de nos pions est posé sur un coin, il ne pourra plus jamais être retourné. C'est pour cela que les cases autour des coins ne sont pas spécialement des cases où nous aimerions poser nos pions car ils seront rapidement retourner et donneront un chemin aux coins. Afin de donner un poids à nos cases, nous utiliserons une matrice comme ci-dessous.

```c#
//Initialisation de la matrice de valeurs pour valoriser les coins. 
int[,] mat_corner_weight = new int[,] {{20, -10, 1, 1, 1, 1, -10, 20},
                                       {-10, -7, 1, 1, 1, 1, -7, -10},
                                       {1, 1, 1, 1, 1, 1, 1, 1},
                                       {1, 1, 1, 1, 1, 1, 1, 1},
                                       {1, 1, 1, 1, 1, 1, 1, 1},
                                       {1, 1, 1, 1, 1, 1, 1, 1},
                                       {-10, -7, 1, 1, 1, 1, -7, -10},
                                       {20, -10, 1, 1, 1, 1, -10, 20}};
``` 

On additionne la valeur des cases de cette matrice si un de nos pions se trouve sur la case correspondante. On calcul ces sommes pour les deux joueurs afin d'en suite également créer un même rapport que les deux derniers critères. 

Pour finir, 

## Sources

https://kartikkukreja.wordpress.com/2013/03/30/heuristic-function-for-reversiothello/
https://courses.cs.washington.edu/courses/cse573/04au/Project/mini1/RUSSIA/Final_Paper.pdf
