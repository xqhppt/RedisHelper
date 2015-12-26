using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Xu.Redis.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async Task TestString()
        {
            bool flag = await RedisHelper.Set("a", "hello redis");
            Assert.AreEqual(true, flag);

            string val = await RedisHelper.Get("a");
            Assert.AreEqual("hello redis", val);
        }
    }
}
