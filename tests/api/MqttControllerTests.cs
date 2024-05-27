using api.Controllers;
using api.Interfaces;
using Moq;

namespace tests.api
{
    public class MqttControllerTests
    {
        private readonly MqttController _controller;
        private readonly Mock<IMqttService> _mockMqttService;
        private readonly Mock<IWebSocketServer> _mockWebSocketServer;

        public MqttControllerTests()
        {
            _mockMqttService = new Mock<IMqttService>();
            _mockWebSocketServer = new Mock<IWebSocketServer>();
            _controller = new MqttController(_mockMqttService.Object, _mockWebSocketServer.Object);
        }

        // Add your test methods here
    }
}