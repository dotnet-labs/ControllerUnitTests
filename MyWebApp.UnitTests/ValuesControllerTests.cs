using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWebApp.Controllers;

namespace MyWebApp.UnitTests
{
    [TestClass]
    public class ValuesControllerTests
    {
        [TestMethod]
        public void BasicUser_GetValues_Should_BeEmpty()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, "01234568"),
                new Claim(ClaimTypes.Name, "first last"),
                new Claim(ClaimTypes.Role, "BasicUser")
            }));

            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<ValuesController>();
            var controller = new ValuesController(logger)
            {
                ControllerContext = { HttpContext = new DefaultHttpContext { User = user } }
            };
            var response = controller.Get();

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var result = ((OkObjectResult)response).Value as string[];
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void AdminUser_GetValues_Should_ContainTwoValues()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, "11234568"),
                new Claim(ClaimTypes.Name, "admin last"),
                new Claim(ClaimTypes.Role, "Admin")
            }));

            var controller = new ValuesController(new NullLogger<ValuesController>())
            {
                ControllerContext = {HttpContext = new DefaultHttpContext {User = user}}
            };
            var response = controller.Get();

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var result = ((OkObjectResult)response).Value as string[];
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("value1", result[0]);
            Assert.AreEqual("value2", result[1]);
        }

        [DataTestMethod]
        [DataRow("spa", 1, "1")]
        [DataRow("eng", 1, "value1")]
        public void BasicUser_GetValueById_Should_ReturnValue(string language, int id, string value)
        {
            var controller = new ValuesController(new NullLogger<ValuesController>())
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.Request.Headers["lang"] = language;
            // similarly, we can manipulate ModelState based on our needs
            // controller.ModelState.Clear();
            // controller.ModelState.AddModelError("test", "test");

            var response = controller.GetValueById(id);

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            var result = ((OkObjectResult)response).Value as string;
            Assert.IsNotNull(result);
            Assert.AreEqual(value, result);
        }
    }
}
