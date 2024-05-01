document.addEventListener("DOMContentLoaded", function () {
    const ships = document.querySelectorAll('.ship');
    const cells = document.querySelectorAll('.cell');
    const playgroundTable = document.getElementById("playground");
    const playButt = document.querySelector('.playButt')
    const rotateShipButt = document.querySelector('.rotateShip')
    const infoForRotation = document.querySelector('.infoForRotation')
    let selectedShip = null;
    let fourCellShipCount = 0;
    let threeCellShipCount = 0;
    let twoCellShipCount = 0;
    let oneCellShipCount = 0;
    let currentDeck = 0;
    let placedShipCount = 0;
    const totalShipCount = 9;
    let rotate = false;



    // чек ли разместил все корабли
    function checkAllShipsPlaced() {
        if (placedShipCount === totalShipCount) {
            console.log('Игрок разместил все корабли!');
            playButt.style.display = 'block'

        }
    }


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

            this.classList.add('occupied');
            resetColouredCells();
            checkAllShipsPlaced()

            let currentRow = row;
            let currentCol = col;

            try {
                // Проверка установки корабля за границами поля
                for (let i = 0; i < currentDeck; i++) {
                    opportunityPlaceShip = true;
                    if ((!rotate && i + col > 9) || (rotate && i + row > 9)) {
                        console.log('Корабль нельзя так поставить!!');
                        opportunityPlaceShip = false;
                        this.classList.remove('occupied');
                        if (selectedShip == fourCellShipCount) {

                            fourCellShipCount - 1
                            placedShipCount - 1

                        } else if (selectedShip.classList.contains('three-cell-ship')) {

                            threeCellShipCount - 1
                            placedShipCount - 1

                        } else if (selectedShip == twoCellShipCount) {

                            twoCellShipCount - 1
                            placedShipCount - 1

                        } else if (selectedShip == oneCellShipCount) {

                            oneCellShipCount - 1
                            placedShipCount - 1
                        }
                        break;
                    }

                }


                if (opportunityPlaceShip) {


                    // установка максимум кораблей одного типа
                    if (selectedShip.classList.contains('four-cell-ship')) {

                        if (fourCellShipCount < 1) {
                            fourCellShipCount++;
                            placedShipCount++;
                            console.log(`Корабль поставлен на ряду ${row}, и в ячейке ${col}`);
                            console.log('кол-во палуб: ' + currentDeck);


                        }
                        else {
                            console.log('Больше нельзя поставить данный тип кораблей');
                            this.classList.remove('occupied');
                            return;
                        }
                    }
                    else if (selectedShip.classList.contains('three-cell-ship')) {
                        if (threeCellShipCount < 2) {
                            threeCellShipCount++;
                            placedShipCount++;
                            console.log(`Корабль поставлен на ряду ${row}, и в ячейке ${col}`);
                            console.log('кол-во палуб: ' + currentDeck);


                        } else {
                            console.log('Больше нельзя поставить данный тип кораблей');
                            this.classList.remove('occupied');
                            return;
                        }
                    }
                    else if (selectedShip.classList.contains('two-cell-ship')) {
                        if (twoCellShipCount < 3) {
                            twoCellShipCount++;
                            placedShipCount++;
                            console.log(`Корабль поставлен на ряду ${row}, и в ячейке ${col}`);
                            console.log('кол-во палуб: ' + currentDeck);


                        } else {
                            console.log('Больше нельзя поставить данный тип кораблей');
                            this.classList.remove('occupied');
                            return;
                        }
                    }
                    else if (selectedShip.classList.contains('one-cell-ship')) {
                        if (oneCellShipCount < 4) {
                            oneCellShipCount++;
                            placedShipCount++;
                            console.log(`Корабль поставлен на ряду ${row}, и в ячейке ${col}`);
                            console.log('кол-во палуб: ' + currentDeck);


                        } else {
                            console.log('Больше нельзя поставить данный тип кораблей');
                            this.classList.remove('occupied');
                            return;
                        }
                    }

                    setPoint(currentCol, currentRow, currentDeck, !rotate);

                    // Логика установки корабля горизонтально

                    if (!rotate) {

                        for (let i = 0; i < currentDeck; i++) {


                            let currentCell = document.createElement('td');
                            currentCell.setAttribute('data-row', currentRow);
                            currentCell.setAttribute('data-col', currentCol);
                            currentCell.classList.add('cellPast');

                            let oldCell = playgroundTable.querySelector(`[data-row="${currentRow}"][data-col="${currentCol}"]`);

                            oldCell.replaceWith(currentCell);

                            oldCell.classList.add('occupied');

                            currentCol++;




                        }
                        // Логика установки корабля вертикально
                    } else {
                        for (let i = 0; i < currentDeck; i++) {


                            let currentCell = document.createElement('td');
                            currentCell.setAttribute('data-row', currentRow);
                            currentCell.setAttribute('data-col', currentCol);
                            currentCell.classList.add('cellPast');

                            let oldCell = playgroundTable.querySelector(`[data-row="${currentRow}"][data-col="${currentCol}"]`);

                            oldCell.replaceWith(currentCell);


                            oldCell.classList.add('occupied')

                            currentRow++;
                        }
                    }
                }
            } catch (error) {
                console.log(error);
                console.log('Нельзя поставить корабль таким образом!');

            }






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
        ship.addEventListener('click', function (event) {
            event.stopPropagation();
            resetColouredCells();
            lightCells();
            selectedShip = this.cloneNode(true);
            selectedShip.classList.remove('ship');
            selectedShip.classList.add('selected-ship');
            currentDeck = ship.getAttribute('data-decks');



        });
    });

    // переворачивать кораблики
    rotateShipButt.addEventListener('click', () => {
        if (!rotate) {
            rotate = true;
            infoForRotation.textContent = "Корабли ставятся вертикально"
        } else {
            rotate = false;
            infoForRotation.textContent = "Корабли ставятся горизонатально"
        }
    })




    // сброс выбраного корабля на клик за пределами поля
    document.addEventListener('click', function () {
        resetColouredCells();
        selectedShip = null;
    });

});

