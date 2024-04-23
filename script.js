document.addEventListener("DOMContentLoaded", function() {
  const ships = document.querySelectorAll('.ship');
  const cells = document.querySelectorAll('.cell');
  let selectedShip = null;
  let fourCellShipCount = 0;
  let threeCellShipCount = 0;
  let twoCellShipCount = 0;
  let oneCellShipCount = 0;
  let currentDeck = 0;

  // корабль в выбраную ячейку перемещаем
  function moveShipToCell() {
    const row = parseInt(this.getAttribute('data-row'));
    const col = parseInt(this.getAttribute('data-col'));
    

    // Запрет на корабль рядом уже с стоящим кораблем
    const nearbyOccupiedCells = [
      document.querySelector(`[data-row="${row - 1}"][data-col="${col - 1}"]`),
      document.querySelector(`[data-row="${row - 1}"][data-col="${col + 1}"]`),
      document.querySelector(`[data-row="${row + 1}"][data-col="${col - 1}"]`),
      document.querySelector(`[data-row="${row + 1}"][data-col="${col + 1}"]`),
      document.querySelector(`[data-row="${row}"][data-col="${col - 1}"]`),
      document.querySelector(`[data-row="${row}"][data-col="${col + 1}"]`),
      document.querySelector(`[data-row="${row - 1}"][data-col="${col}"]`),
      document.querySelector(`[data-row="${row + 1}"][data-col="${col}"]`),

      
    ];

    
    const cellPastCells = document.querySelectorAll('.cellPast');

    cellPastCells.forEach(cell => {
      const cellRow = parseInt(cell.getAttribute('data-row'));
      const cellCol = parseInt(cell.getAttribute('data-col'));

      nearbyOccupiedCells.push(
        document.querySelector(`[data-row="${cellRow - 1}"][data-col="${cellCol - 1}"]`),
        document.querySelector(`[data-row="${cellRow - 1}"][data-col="${cellCol + 1}"]`),
        document.querySelector(`[data-row="${cellRow + 1}"][data-col="${cellCol - 1}"]`),
        document.querySelector(`[data-row="${cellRow + 1}"][data-col="${cellCol + 1}"]`),
        document.querySelector(`[data-row="${cellRow}"][data-col="${cellCol - 1}"]`),
        document.querySelector(`[data-row="${cellRow}"][data-col="${cellCol + 1}"]`),
        document.querySelector(`[data-row="${cellRow - 1}"][data-col="${cellCol}"]`),
        document.querySelector(`[data-row="${cellRow + 1}"][data-col="${cellCol}"]`)
      );
    });

    const isNearbyOccupied = nearbyOccupiedCells.some(cell => cell && cell.classList.contains('occupied'));

    


    if (selectedShip && !this.classList.contains('occupied') && !isNearbyOccupied) {
      if (selectedShip.classList.contains('four-cell-ship')) {

        if (fourCellShipCount < 1) {
          fourCellShipCount++;
        } 
        else {
          console.log('Больше нельзя поставить данный тип кораблей');
          return;
        }
      }
       else if (selectedShip.classList.contains('three-cell-ship')) {
        if (threeCellShipCount < 2) {
          threeCellShipCount++;
        } else {
          console.log('Больше нельзя поставить данный тип кораблей');
          return;
        }
      } 
      else if (selectedShip.classList.contains('two-cell-ship')) {
        if ( twoCellShipCount < 3) {
          twoCellShipCount++;
        } else {
          console.log('Больше нельзя поставить данный тип кораблей');
          return;
        }
      } 
      else if (selectedShip.classList.contains('one-cell-ship')) {
        if ( oneCellShipCount < 4) {
          oneCellShipCount++;
        } else {
          console.log('Больше нельзя поставить данный тип кораблей');
          return;
        }
      }

      let playgroundTable = document.getElementById("playground");

      let currentRow = row;
      let currentCol = col;

      for (let i = 0; i < currentDeck; i++) {

          let currentCell = document.createElement('td');
          currentCell.setAttribute('data-row', currentRow);
          currentCell.setAttribute('data-col', currentCol);
          currentCell.classList.add('cellPast');

          let oldCell = playgroundTable.querySelector(`[data-row="${currentRow}"][data-col="${currentCol}"]`);

          oldCell.replaceWith(currentCell);

          currentCol++;
        }
        

      console.log(`Корабль поставлен на ряду ${row}, и в ячейке ${col}`);
      this.classList.add('occupied');
      resetColouredCells();
    } 
  }

  // достпуные клетки
  function lightCells() {
    Array.from(cells).forEach(cell => {
      const row = parseInt(cell.getAttribute('data-row'));
      const col = parseInt(cell.getAttribute('data-col'));

      // проверка на зайнятую ячейку рядом с кораблем
      const isNearbyOccupied = (
        Array.from(cells).some(neighbourCell =>
          Math.abs(parseInt(neighbourCell.getAttribute('data-row')) - row) <= 1 &&
          Math.abs(parseInt(neighbourCell.getAttribute('data-col')) - col) <= 1 &&
          neighbourCell.classList.contains('occupied')
        )
      );

      if (!cell.classList.contains('occupied') && !isNearbyOccupied) {
        cell.classList.add('coloured');
        cell.addEventListener('click', moveShipToCell);
      }
    });
  }

  // сброс подсветки
  function resetColouredCells() {
    cells.forEach(cell => {
      cell.classList.remove('coloured');
      cell.removeEventListener('click', moveShipToCell);
    });
  }

  // клик на корабль
  ships.forEach(ship => {
    ship.addEventListener('click', function(event) {
      event.stopPropagation();
      resetColouredCells();
      lightCells();
      selectedShip = this.cloneNode(true);
      selectedShip.classList.remove('ship');
      selectedShip.classList.add('selected-ship');
      currentDeck = ship.getAttribute('data-decks');

    console.log('кол-во палуб: ' + currentDeck);
    });
  });

  // сброс выбраного корабля на клик за пределами поля
  document.addEventListener('click', function() {
    resetColouredCells();
    selectedShip = null;
  });
});

