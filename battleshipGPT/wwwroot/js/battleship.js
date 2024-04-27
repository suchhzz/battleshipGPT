document.addEventListener('DOMContentLoaded', function() {
  const playGroundEnemy = document.querySelector('.enemyField');
  const playGroundPlayer = document.querySelector('.playerField');
  const gameArrow = document.querySelector('svg')
  let shooted = false;

  playGroundEnemy.addEventListener('click', function(event) {
    if (!shooted) {
      const clickedCell = event.target;
      if (playGroundEnemy.contains(clickedCell)) {
        const row = clickedCell.getAttribute('data-row');
        const col = clickedCell.getAttribute('data-col');
        console.log(`Игрок кликнул на ряд ${row} и на ячейку ${col}!`);
        shooted = true;
        gameArrow.classList.remove('rotated')
      }
    }
  });

  playGroundPlayer.addEventListener('click', function(event) {
    if (shooted) {
      const clickedCell = event.target;
      if (playGroundPlayer.contains(clickedCell)) {
        const row = clickedCell.getAttribute('data-row');
        const col = clickedCell.getAttribute('data-col');
        console.log(`Противник кликнул на ряд ${row} и на ячейку ${col}!`);
        shooted = false;
        gameArrow.classList.add('rotated')
      }
    }
  });

});
