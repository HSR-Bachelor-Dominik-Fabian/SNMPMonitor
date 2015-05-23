using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SNMPMonitor.PresentationLayer.Hubs;
using System.Dynamic;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System.Security.Principal;

namespace SNMPMonitor.PresentationLayer.Tests.Hubs
{
    [TestClass]
    public class SNMPDataHubTest
    {
        /*
        [TestMethod]
        public void TestMethod()
        {
            var hub = new SNMPDataHub();
            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            var groups = new Mock<IClientContract>();
            var mockUser = new Mock<IPrincipal>();
            var groupManager = new Mock<IGroupManager>();           
            
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(r => r.User).Returns(mockUser.Object);
            hub.Groups = groupManager.Object;
            hub.Context = new HubCallerContext(mockRequest.Object, "12");
            hub.JoinDataGroup("Agent_General").Wait();
            groups.Setup(m => m.receiveData(It.IsAny<JObject>())).Verifiable();
            mockClients.Setup(m => m.Group("Agent_General")).Returns(groups.Object);
            
            hub.SendSNMPData(new PresentationLayer.Models.MonitorDataModel(new BusinessLayer.MonitorData(DateTime.Now, "Result", 123, "123")));

            groups.VerifyAll();

        }

        public interface IClientContract 
        {
            void receiveData(JObject messages); 
        } 
        */
    }
}
