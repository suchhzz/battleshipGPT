document.addEventListener('DOMContentLoaded', function() {
  const playGroundEnemy = document.querySelector('.enemyField');
  const playGroundPlayer = document.querySelector('.playerField');
  const gameArrow = document.querySelector('svg')
    let playerTurn = true;

  playGroundEnemy.addEventListener('click', function(event) {
    if (playerTurn) {
      const clickedCell = event.target;
      if (playGroundEnemy.contains(clickedCell)) {
        const row = clickedCell.getAttribute('data-row');
          const col = clickedCell.getAttribute('data-col');

          if (clickedCell.style.backgroundColor !== 'rgb(110, 110, 110)') {
              playerTurn = false;
          }
          console.log("player turn: " + playerTurn);

          console.log("selected cell color : " + clickedCell.style.backgroundColor);


          playerMove(col, row);

          console.log(`Игрок кликнул на ряд ${row} и на ячейку ${col}!`);

          



          if (playerTurn) {
              gameArrow.classList.remove('rotated')
          }

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
        playerTurn = true;
          gameArrow.classList.add('rotated');


      }
    }
  });

});
