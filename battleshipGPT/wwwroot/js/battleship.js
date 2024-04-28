let gameArrow;
document.addEventListener('DOMContentLoaded', function () {
  const playGroundEnemy = document.querySelector('.enemyField');
  const playGroundPlayer = document.querySelector('.playerField');
  gameArrow = document.querySelector('svg');
    let playerTurn = true;

  playGroundEnemy.addEventListener('click', function(event) {
    if (getPlayerTurn()) {
      const clickedCell = event.target;
      if (playGroundEnemy.contains(clickedCell)) {
        const row = clickedCell.getAttribute('data-row');
          const col = clickedCell.getAttribute('data-col');

          console.log("player turn: " + getPlayerTurn());

          console.log("selected cell color : " + clickedCell.style.backgroundColor);


          playerMove(col, row);

          console.log(`Игрок кликнул на ряд ${row} и на ячейку ${col}!`);

      }
    }
  });

    


  playGroundPlayer.addEventListener('click', function(event) {
    if (!playerTurn) {
      const clickedCell = event.target;
      if (playGroundPlayer.contains(clickedCell)) {
        const row = clickedCell.getAttribute('data-row');
        const col = clickedCell.getAttribute('data-col');
        console.log(`Противник кликнул на ряд ${row} и на ячейку ${col}!`);


      }
    }
  });

});

function rotateArrow(status) {
    if (status) {
        gameArrow.classList.remove('rotated');
    }
    else {
        gameArrow.classList.add('rotated');
    }
}