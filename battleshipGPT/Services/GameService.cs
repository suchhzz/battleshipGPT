using battleshipGPT.Models.GameModels;
using battleshipGPT.Models.MainModels;

namespace battleshipGPT.Services
{
    public class GameService
    {
        public void SetPoint(Room room, int x, int y)
        {
            var selectedShip = IsShipHit(room.enemyShips, x, y);

            if (selectedShip != null)
            {
                room.enemyShipsRemaining--;
            }

            room.enemyUsedCoordinates.Add(new Coordinates { X = x, Y = y });

        }

        private ShipModel IsShipHit(List<ShipModel> enemyShips, int x, int y)
        {
            ShipModel hittedShipModel = null;

            foreach (var ship in enemyShips)
            {
                foreach (var coordinates in ship.Coords)
                {
                    if (coordinates.X == x && coordinates.Y == y)
                    {
                        hittedShipModel = ship;

                        hittedShipModel.DeckRemaining--;

                        if (hittedShipModel.DeckRemaining == 0)
                        {
                            hittedShipModel.Destroyed = true;
                        }
                    }
                }
            }

            return hittedShipModel;
        }

        public void EnemySetPoint(Room room)
        {
            bool setPoint = false;

            while (!setPoint)
            {
                if (room.enemyHitCoordinates.Count != 0)
                {
                    var newEnemyCoords = generateRandomDirection(room.enemyHitCoordinates);

                    if (room.enemyHitCoordinates.Count == 1)
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
        private bool checkUserCoordinates(Room room, Coordinates coords)
        {

        }

        private bool checkBorder(Coordinates coords)
        {
            return coords.X <= 9 && coords.X >= 0 && coords.Y <= 9 && coords.X >= 0;
        }

        private bool checkShipBorders(Room room, Coordinates coords)
        {
            return !(room.usedCoordinates.FirstOrDefault(c => c.X == coords.X && c.Y == coords.Y) != null);
        }
    }
}
