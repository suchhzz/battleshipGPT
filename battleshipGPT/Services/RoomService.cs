using battleshipGPT.Models.GameModels;
using battleshipGPT.Models.MainModels;
using battleshipGPT.Models.PlayersModel;
using battleshipGPT.Services;

namespace battleshipGPT.Serives
{
    public class RoomService
    {
        public RoomService()
        {
            rooms.Add(new Room
            {
                Player = new Player
                {
                    PlayerShips = new List<ShipModel>
                {
                    new ShipModel { Coords = new List<Coordinates> { new Coordinates { X = 0, Y = 0 } }, Deck = 1, DeckRemaining = 1, Destroyed = false, Horizontal = true },

                    new ShipModel { Coords = new List<Coordinates>
                    {
                        new Coordinates { X = 6, Y = 7 },
                        new Coordinates { X = 7, Y = 7 }
                    }, Deck = 2, DeckRemaining = 2, Destroyed = false, Horizontal = true },

                    new ShipModel { Coords = new List<Coordinates>
                    {
                        new Coordinates { X = 2, Y = 4 },
                        new Coordinates { X = 2, Y = 5 },
                        new Coordinates { X = 2, Y = 6 }
                    }, Deck = 3, DeckRemaining = 1, Destroyed = false, Horizontal = false },

                    new ShipModel { Coords = new List<Coordinates>
                    {
                        new Coordinates { X = 6, Y = 3 },
                        new Coordinates { X = 7, Y = 3 },
                        new Coordinates { X = 8, Y = 3 },
                        new Coordinates { X = 9, Y = 3 }
                    }, Deck = 4, DeckRemaining = 4, Destroyed = false, Horizontal = true }
                }
                },
                enemy = new Enemy
                {
                    EnemyShips = new List<ShipModel>
                    {
                        new ShipModel { Coords = new List<Coordinates> { new Coordinates { X = 5, Y = 7 } }, Deck = 1, DeckRemaining = 1, Destroyed = false, Horizontal = true },

                    new ShipModel { Coords = new List<Coordinates>
                    {
                        new Coordinates { X = 7, Y = 2 },
                        new Coordinates { X = 7, Y = 3 }
                    }, Deck = 2, DeckRemaining = 2, Destroyed = false, Horizontal = false },

                    new ShipModel { Coords = new List<Coordinates>
                    {
                        new Coordinates { X = 1, Y = 1 },
                        new Coordinates { X = 2, Y = 1 },
                        new Coordinates { X = 3, Y = 1 }
                    }, Deck = 3, DeckRemaining = 3, Destroyed = false, Horizontal = true },

                    new ShipModel { Coords = new List<Coordinates>
                    {
                        new Coordinates { X = 0, Y = 6 },
                        new Coordinates { X = 0, Y = 7 },
                        new Coordinates { X = 0, Y = 8 },
                        new Coordinates { X = 0, Y = 9 }
                    }, Deck = 4, DeckRemaining = 4, Destroyed = false, Horizontal = false }
                    },

                    EnemyHitCoordinates = new List<Coordinates> { new Coordinates { X = 2, Y = 4 }, new Coordinates { X = 2, Y = 5 } },
                    AvailableCoordinates = setAvailableCoordinates(),
                    UsedCoordinates = new List<Coordinates> { new Coordinates { X = 2, Y = 4 }, new Coordinates { X = 2, Y = 5 }, new Coordinates { X = 1, Y = 4 }, new Coordinates { X = 2, Y = 3 } },
                    EnemyShipsRemaining = 4
                },

                RoomId = Guid.NewGuid()

            });
        }
        public List<Room> rooms = new List<Room>();

        public void CreateRoom(Player player, Guid roomId)
        {
            rooms.Add(new Room
            {
                Player = player,
                RoomId = roomId,
                enemy = new Enemy
                {
                    EnemyShips = new List<ShipModel>(),
                    UsedCoordinates = new List<Coordinates>(),
                    AvailableCoordinates = setAvailableCoordinates(),
                    EnemyHitCoordinates = new List<Coordinates>(),
                    EnemyShipsRemaining = 0
                }
            }) ;
        }

        private List<Coordinates> setAvailableCoordinates()
        {
            List<Coordinates> available = new List<Coordinates>();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    available.Add(new Coordinates { X = j, Y = i });
                }
            }

            return available;
        }

        public Room GetRoomById(Guid roomId)
        {
            return rooms.FirstOrDefault(r => r.RoomId == roomId);
        }

        public Room GetTestRoom()
        {
            return rooms[0];
        }
    }
}
