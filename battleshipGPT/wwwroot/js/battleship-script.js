let gameArrow = document.querySelector('svg');
const playGroundEnemy = document.querySelector('.enemyField');
const playGroundPlayer = document.querySelector('.playerField');
const cells = document.querySelectorAll('.cell');
document.addEventListener('DOMContentLoaded', function () {
  
    playGroundEnemy.addEventListener('click', handleClickEvent);
});

function handleClickEvent(event) {
    if (getPlayerTurn()) {
        const clickedCell = event.target;
        if (!clickedCell.classList.contains('occupied')) {
            if (playGroundEnemy.contains(clickedCell)) {
                const row = clickedCell.getAttribute('data-row');
                const col = clickedCell.getAttribute('data-col');

                playerMove(col, row); 

                console.log(`Игрок кликнул на ряд ${row} и на ячейку ${col}!`);
            }
        }
    }
}

function rotateArrow(status) {
    if (status) {
        gameArrow.classList.remove('rotated');
    }
    else {
        gameArrow.classList.add('rotated');
    }
}