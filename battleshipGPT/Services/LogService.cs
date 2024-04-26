using battleshipGPT.Models.GameModels;

namespace battleshipGPT.Services
{
    public class LogService
    {
        private readonly ILogger<LogService> _logger;
        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;

        }
        private char[,] enemyPlayground = new char[10, 10];
        private char[,] playerPlayground = new char[10, 10];

        public void showField(List<ShipModel> ships, string person)
        {
            if (person == "player")
            {
                fillPlayground(ships, playerPlayground);
            }
            else if (person == "enemy")
            {
                fillPlayground(ships, enemyPlayground);
            }
        }

        private void fillPlayground(List<ShipModel> ships, char[,] playground)
        {

            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    playground[i, j] = '-';
                }
            }

            for (int i = 0; i < ships.Count; i++)
            {
                for (int j = 0; j < ships[i].Coords.Count; j++)
                {
                    playground[ships[i].Coords[j].Y, ships[i].Coords[j].X] = '*';
                }
            }

            showLogPlayground(playground);
        }
        private void showLogPlayground(char[,] playground)
        {
            string logOutput = "";

            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    logOutput += playground[i, j] + " ";
                }
                logOutput += "\n";
            }

            _logger.LogInformation("playground generated\n"
                                  + logOutput);
        }
    }
}
