using battleshipGPT.Models.GameModels;
using battleshipGPT.Models.MainModels;

namespace battleshipGPT.Services
{
    public class GameService
    {
        public void SetPoint(Room room, int x, int y)
        {
            var selectedShip = ShipHitCheck(room.enemy.EnemyShips, x, y);

            if (selectedShip != null)
            {
                if (selectedShip.Destroyed)
                {
                    room.enemy.EnemyShipsRemaining--;
                }
            }
        }

        public HitPointModel GetHitCoord(Room room, int x, int y, bool isPlayerMoves)
        {
            var hitPoint = new HitPointModel
            {
                HitCoords = new Coordinates { X = x, Y = y },
                isHit = false,
                BorderCoords = new List<Coordinates>()
            };

            ShipModel hitShip = new ShipModel();

            if (isPlayerMoves)
            {
                hitShip = ShipHitCheck(room.enemy.EnemyShips, x, y);
            }
            else
            {
                hitShip = ShipHitCheck(room.Player.PlayerShips, x, y);
            }

            if (hitShip != null)
            {
                hitPoint.isHit = true;

                if (!isPlayerMoves)
                {
                    room.enemy.EnemyHitCoordinates.Add(new Coordinates { X = x, Y = y });
                }

                if (hitShip.Destroyed)
                {
                    hitPoint.BorderCoords = GetBorderCoords(hitShip);

                    if (isPlayerMoves)
                    {
                        room.enemy.EnemyShipsRemaining--;
                    }
                    else
                    {
                        room.Player.PlayerShipsRemaining--;

                        room.enemy.UsedCoordinates.AddRange(hitPoint.BorderCoords);
                        room.enemy.UsedCoordinates = room.enemy.UsedCoordinates.Distinct().ToList();
                        
                        foreach (var usedCoords in hitPoint.BorderCoords)
                        {
                            room.enemy.AvailableCoordinates.Remove(usedCoords);
                        }

                        room.enemy.EnemyHitCoordinates.Clear();
                    }
                }
            }


            return hitPoint;
        }

        private List<Coordinates> GetBorderCoords(ShipModel ship)
        {
            List<Coordinates> borderCoords = new List<Coordinates>();

            Coordinates upperLeftBorder = new Coordinates { X = ship.Coords[0].X, Y = ship.Coords[0].Y };
            Coordinates lowerRightBorder = new Coordinates { X = ship.Coords[ship.Coords.Count - 1].X, Y = ship.Coords[ship.Coords.Count - 1].Y };

            if (upperLeftBorder.X - 1 >= 0)
            {
                upperLeftBorder.X--;
            }
            if (upperLeftBorder.Y - 1 >= 0)
            {
                upperLeftBorder.Y--;
            }

            if (lowerRightBorder.X + 1 <= 9)
            {
                lowerRightBorder.X++;
            }
            if (lowerRightBorder.Y + 1 <= 9)
            {
                lowerRightBorder.Y++;
            }

            for (int i = upperLeftBorder.Y; i <= lowerRightBorder.Y; i++)
            {
                for (int j = upperLeftBorder.X; j <= lowerRightBorder.X; j++)
                {
                    if (!ShipContainsCoords(ship, j, i))
                    {
                        borderCoords.Add(new Coordinates { X = j, Y = i });
                    }
                }
            }

            return borderCoords;
        }

        private bool ShipContainsCoords(ShipModel ship, int x, int y)
        {
            foreach (var coords in ship.Coords)
            {
                if (coords.X == x && coords.Y == y)
                {
                    return true;
                }
            }
            return false;
        }


        private ShipModel ShipHitCheck(List<ShipModel> checkShips, int x, int y)
        {
            foreach (var ship in checkShips)
            {
                foreach (var coords in ship.Coords)
                { 
                    if (coords.X == x && coords.Y == y)
                    {
                        ship.DeckRemaining--;

                        if (ship.DeckRemaining == 0)
                        {
                            ship.Destroyed = true;
                        }

                        return ship;
                    }
                }
            }
            return null;
        }

        public Coordinates EnemySetPoint(Room room)
        {
            bool setPoint = false;

            Coordinates enemyRandomCoord = new Coordinates();

            while (!setPoint)
            {
                if (room.enemy.EnemyHitCoordinates.Count != 0)
                {
                    enemyRandomCoord = generateRandomDirection(room.enemy.EnemyHitCoordinates);

                    if (checkBorder(enemyRandomCoord) && checkShipBorders(room, enemyRandomCoord))
                    {
                        setPoint = true;
                    }
                    
                }
                else
                {
                    var rnd = new Random();

                    enemyRandomCoord = room.enemy.AvailableCoordinates[rnd.Next(0, room.enemy.AvailableCoordinates.Count)];

                    setPoint = true;
                }
            }
            room.enemy.UsedCoordinates.Add(enemyRandomCoord);
            room.enemy.AvailableCoordinates.Remove(enemyRandomCoord);

            return enemyRandomCoord;
        }

        private ShipModel checkEnemyHit(Coordinates checkCoords, List<ShipModel> shipsList)
        {
            foreach (var ship in shipsList)
            {
                foreach (var coord in ship.Coords)
                {
                    if (coord.X == checkCoords.X && coord.Y == checkCoords.Y)
                    {
                        ship.DeckRemaining--;

                        if (ship.DeckRemaining == 0)
                        {
                            ship.Destroyed = true;
                        }

                        return ship;
                    }
                }
            }

            return null;
        }

        private Coordinates generateRandomDirection(List<Coordinates> coordinates)
        {
            var rnd = new Random();

            var enemyCoordinates = new Coordinates { X = coordinates[0].X, Y = coordinates[0].Y };

            bool horizontal = true;
            bool direction = true;

            if (coordinates.Count == 1)
            {
                horizontal = rnd.Next(0, 2) == 0;

                if (horizontal)
                {
                    direction = rnd.Next(0, 2) == 0;

                    if (direction)
                    {
                        enemyCoordinates.X--;
                    }
                    else
                    {
                        enemyCoordinates.X++;
                    }
                }
                else
                {
                    direction = rnd.Next(0, 2) == 0;

                    if (direction)
                    {
                        enemyCoordinates.Y--;
                    }
                    else
                    {
                        enemyCoordinates.Y++;
                    }
                }
            }
            else
            {
                bool border = true;

                if (coordinates[0].X == coordinates[1].X)
                {
                    enemyCoordinates.X = coordinates[0].X;
                    enemyCoordinates.Y = coordinates[0].Y;

                    direction = rnd.Next(0, 2) == 0;

                    if (direction)
                    {
                        while (border)
                        {
                            enemyCoordinates.Y++;

                            if (!compareCoordinates(enemyCoordinates, coordinates))
                            {
                                border = false;
                            }
                        }
                    }
                    else
                    {
                        while (border)
                        {
                            enemyCoordinates.Y--;

                            if (!compareCoordinates(enemyCoordinates, coordinates))
                            {
                                border = false;
                            }
                        }
                    }
                }
                else
                {
                    enemyCoordinates.X = coordinates[0].X;
                    enemyCoordinates.Y = coordinates[0].Y;

                    direction = rnd.Next(0, 2) == 0;

                    if (direction)
                    {
                        while (border)
                        {
                            enemyCoordinates.X++;

                            if (!compareCoordinates(enemyCoordinates, coordinates))
                            {
                                border = false;
                            }
                        }
                    }
                    else
                    {
                        while (border)
                        {
                            enemyCoordinates.X--;

                            if (!compareCoordinates(enemyCoordinates, coordinates))
                            {
                                border = false;
                            }
                        }
                    }
                }
            }

            return enemyCoordinates;
        }
        private bool compareCoordinates(Coordinates coords, List<Coordinates> userCoords)
        {
            return userCoords.FirstOrDefault(c => c.X == coords.X && c.Y == coords.Y) != null;
        }
        private bool availableCoordinate(Room room, Coordinates coords) // свободна ли ячейка
        {
            return !room.enemy.UsedCoordinates.Contains(coords);
        }

        private bool checkBorder(Coordinates coords)
        {
            return coords.X <= 9 && coords.X >= 0 && coords.Y <= 9 && coords.Y >= 0;
        }

        private bool checkShipBorders(Room room, Coordinates coords)
        {
            return !(room.enemy.UsedCoordinates.FirstOrDefault(c => c.X == coords.X && c.Y == coords.Y) != null);
        }
    }
}
