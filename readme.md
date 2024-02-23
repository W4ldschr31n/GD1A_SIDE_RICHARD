
# Prototype : The Devastation

<p>Vous vous réveillez au milieu d'un désert sans savoir comment ni pourquoi. Explorez l'environnement et rassemblez des indices pour comprendre ce qui ce passe !</p>

## Lancement du jeu

<p>Ouvrez le projet avec Unity (de préférence version 2022.3.17), chargez la scène "RealGame" et cliquez sur Play.</p>

<p>N'hésitez pas à jouer avec l'éditeur pour déplacer le personnage principal et accéder à des séquences de jeu plus rapidement.</p>

## Contrôles

| Action      | Clavier/souris        | Manette (XBox)  |
|-------------|-----------------------|-----------------|
| Déplacement | ZQSD/Touches fléchées | Joystick gauche |
| Sauter      | Barre espace          | Bouton A        |
| Attaquer    | Click gauche          | Bouton X        |
| Utiliser    | E                     | Bouton B        |
| Recommencer | R                     | Bouton Start    |

## Graphismes

<p>Les élements graphiques peuvent être trouvés dans les dossiers suivants:</p>

- Personnage : /Assets/Player
- Ennemi : /Assets/Enemy
- Collectibles : /Assets/Sprites/Collectibles (le grappin n'est pas présent dans le jeu)
- Barre de vie : /Assets/UI
- Éléments de décor : /Assets/Sprites/Background
- Plateformes : Assets/Tiles (les 3 fichiers "tilemap_X")

<p>Des éléments supplémentaires de recherche graphique peuvent être trouvés dans le dossier /Conception</p>

## Programmation gameplay

- Contrôles : le clavier et la manette peuvent être utilisés
- Indicateur de vie : coeurs en haut à gauche de l'écran, quand le joueur subit des dégâts, il ne peut pas en prendre plus pendant un court laps de temps et devient grisé
- Ennemis : endommagent le joueur au contact, patrouillent entre deux points, peuvent être tués avec une attaque
- Action déplacement : rebond sur les murs (wall jump)
- Altération déplacement : surfaces glissantes et collantes (dans la 3e partie du jeu)
- Obstacles mortels : les pics
- Power-up : le fouet qui permet ensuite d'attaquer
- Scrolling : la caméra suit le joueur
- Parallaxe : dans le désert, les divers éléments du décor

## Améliorations souhaitées et problèmes connus

- Finir d'implémenter le grappin, il est utilisable en activant le script "Grappin" sur l'objet DynamicPlayer puis en faisant click droit sur les plateformes en jeu
- Rendre moins visibles les murs du désert
- Remanier la différenciation entre sol et murs. Il arrive souvent de faire un wall jump non désiré en grimpant des marches
- Pouvoir injecter des données dans les tile asset directement plutôt que de faire une tilemap par type de tile asset (sol glissant etc.)
- Étoffer graphiquement les deuxième et troisième parties du jeu