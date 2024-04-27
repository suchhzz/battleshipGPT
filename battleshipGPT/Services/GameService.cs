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

            room.enemy.UsedCoordinates.Add(new Coordinates { X = x, Y = y });
            room.enemy.AvailableCoordinates.Remove(new Coordinates { X = x, Y = y });

        }

        public HitPointModel GetHitCoord(Room room, int x, int y)
        {
            var hitPoint = new HitPointModel
            {
                HitCoords = new Coordinates { X = x, Y = y },
                isHit = false,
                BorderCoords = new List<Coordinates>()
            };

            var hitShip = ShipHitCheck(room.enemy.EnemyShips, x, y);

            if (hitShip != null)
            {
                hitPoint.isHit = true;

                if (hitShip.Destroyed)
                {
                    hitPoint.BorderCoords = GetBorderCoords(hitShip);

                    room.enemy.EnemyShipsRemaining--;
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


        private ShipModel ShipHitCheck(List<ShipModel> enemyShips, int x, int y)
        {
            foreach (var ship in enemyShips)
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

        public void EnemySetPoint(Room room)
        {
            bool setPoint = false;

            while (!setPoint)
            {
                if (room.enemy.EnemyHitCoordinates.Count != 0)
                {
                    var newEnemyCoords = generateRandomDirection(room.enemy.EnemyHitCoordinates);

                    if (room.enemy.EnemyHitCoordinates.Count == 1)
                    {
                        if (checkBorder(newEnemyCoords) && checkShipBorders(room, newEnemyCoords))
                        {
                            setPoint = true;
                        }
                    }
                }
            }
        }

        private Coordinates generateRandomDirection(List<Coordinates> coordinates)
        {
            var rnd = new Random();

            var enemyCoordinates = new Coordinates();

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
            return userCoords.Contains(coords);
        }
        private bool availableCoordinate(Room room, Coordinates coords) // свободна ли ячейка
        {
            return !room.enemy.UsedCoordinates.Contains(coords);
        }

        private bool checkBorder(Coordinates coords)
        {
            return coords.X <= 9 && coords.X >= 0 && coords.Y <= 9 && coords.X >= 0;
        }

        private bool checkShipBorders(Room room, Coordinates coords)
        {
            return !(room.enemy.UsedCoordinates.FirstOrDefault(c => c.X == coords.X && c.Y == coords.Y) != null);
        }
    }
}
