using NUnit.Framework;
using SignaturitClient = Signaturit.Client;

namespace Signaturit.Tests
{
    [TestFixture]
    public class Client
    {
        [Test]
        public void Initialization()
        {
            SignaturitClient client = new SignaturitClient("a_token");

            Assert.That(client, Is.InstanceOf<SignaturitClient>());
        }
    }
}