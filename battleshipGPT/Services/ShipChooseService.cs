using battleshipGPT.Models.GameModels;
using battleshipGPT.Models.MainModels;
using System.Reflection.Metadata.Ecma335;

namespace battleshipGPT.Services
{
    public class ShipChooseService
    {
        public void SetShip(Room room, int headX, int headY, int deck, bool horizontal)
        {
            ShipModel ship = new ShipModel();

            ship.Deck = deck;
            ship.DeckRemaining = deck;
            ship.Horizontal = horizontal;

            ship.Coords = CreateShip(headX, headY, ship);

            room.userShips.Add(ship);
        }

        private List<Coordinates> CreateShip(int headX, int headY, ShipModel ship)
        {
            List<Coordinates> newCoords = new List<Coordinates>(); 

            for (int i = 0; i < ship.Deck; i++)
            {
                newCoords.Add(new Coordinates { X = headX, Y = headY });

                if (ship.Horizontal)
                {
                    headX++;
                }
                else
                {
                    headY++;
                }
            }
            return newCoords;
        }

        public List<ShipModel> CreateEnemyShips(Room room)
        {
            List<ShipModel> enemyShips = new List<ShipModel>();

            enemyShips.Add(PlaceShip(room, 4));
            enemyShips.Add(PlaceShip(room, 3));
            enemyShips.Add(PlaceShip(room, 3));
            enemyShips.Add(PlaceShip(room, 2));
            enemyShips.Add(PlaceShip(room, 2));
            enemyShips.Add(PlaceShip(room, 2));
            enemyShips.Add(PlaceShip(room, 1));
            enemyShips.Add(PlaceShip(room, 1));
            enemyShips.Add(PlaceShip(room, 1));
            enemyShips.Add(PlaceShip(room, 1));

            room.enemyShipsRemaining = 10;

            return enemyShips;
        }

        private ShipModel PlaceShip(Room room, int deck)
        {
            ShipModel ship = new ShipModel();

            ship.HitCoords = new List<Coordinates>();

            var rnd = new Random();

            bool placed = false;

            ship.Deck = ship.DeckRemaining = deck;

            while (!placed)
            {
                ship.Coords = new List<Coordinates> 
                { new Coordinates
                    {
                        X = rnd.Next(0, 10),
                        Y = rnd.Next(0, 10)
                    } 
                };
                ship.Horizontal = rnd.Next(0, 2) == 0;
                
                if (CheckPlaygroundBorder(ship))
                {
                    ship.Coords = CreateShip(ship.Coords[0].X, ship.Coords[0].Y, ship);

                    if (CheckOtherShipsBorder(ship, room))
                    {
                        placed = true;
                    }
                }

            }

            return ship;
        }

        private bool CheckPlaygroundBorder(ShipModel ship)
        {
            if (ship.Horizontal)
            {
                return ship.Coords[0].X + ship.Deck - 1 <= 9;
            }
            else
            {
                return ship.Coords[0].Y + ship.Deck - 1 <= 9;
            }
        }

        private bool CheckOtherShipsBorder(ShipModel ship, Room room)
        {
            int currentX;
            int currentY;

            for (int i = 0; i < ship.Deck; i++)
            {
                currentX = ship.Coords[i].X;
                currentY = ship.Coords[i].Y;

                if (CompareShips(currentX, currentY, room)) { return false; }
                if (CompareShips(currentX, currentY - 1, room)) { return false; }
                if (CompareShips(currentX, currentY + 1, room)) { return false; }
                if (CompareShips(currentX - 1, currentY, room)) { return false; }
                if (CompareShips(currentX + 1, currentY, room)) { return false; }
                if (CompareShips(currentX - 1, currentY - 1, room)) { return false; }
                if (CompareShips(currentX + 1, currentY + 1, room)) { return false; }
                if (CompareShips(currentX + 1, currentY - 1, room)) { return false; }
                if (CompareShips(currentX + 1, currentY - 1, room)) { return false; }
            }

            return true;
        }

        private bool CompareShips(int x, int y, Room room)
        {
            for (int i = 0; i < room.enemyShips.Count; i++)
            {
                for (int j = 0; i < room.enemyShips[i].Coords.Count; j++)
                {
                    if (room.enemyShips[i].Coords[j].X == x && room.enemyShips[i].Coords[j].Y == y)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
