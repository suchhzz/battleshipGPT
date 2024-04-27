using battleshipGPT.Models;
using battleshipGPT.Models.MainModels;
using battleshipGPT.Serives;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace battleshipGPT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoomService _roomService;

        public HomeController(ILogger<HomeController> logger, RoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Battleship(string roomId)
        {

            // var currentRoom = _roomService.GetRoomById(Guid.Parse(roomId));

            var currentRoom = _roomService.GetTestRoom();

            _logger.LogInformation("roomId: " + currentRoom.RoomId); 

            return View("Battleship", currentRoom);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
