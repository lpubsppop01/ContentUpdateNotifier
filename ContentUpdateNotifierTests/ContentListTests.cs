using lpubsppop01.ContentUpdateNotifier;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.IO;
using System.Reflection;

namespace lpubsppop01.ContentUpdateNotifierTests
{
    [TestClass]
    public class ContentListTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string workDirPath = Path.GetDirectoryName(assemblyPath);
            string dbPath = Path.Combine(workDirPath, "testdb.sqlite");
            var contentList = new ContentList(dbPath);
            contentList.Clear();
            string testPath = assemblyPath;
            var testTimestamp = File.GetLastWriteTime(testPath).ToUnixTimestamp();
            contentList.Add(testPath, testTimestamp, removed: false);
            Assert.IsTrue(contentList.ContainsKey(testPath));
            Assert.AreEqual(1, contentList.Count);
            Assert.AreEqual(contentList.Keys.First(), testPath);
            Assert.IsTrue(contentList.TryGetValue(testPath, out int storedTimestamp, out bool removed));
            Assert.AreEqual(testTimestamp, storedTimestamp);
            contentList.Remove(testPath);
            Assert.AreEqual(0, contentList.Count);
            contentList.Dispose();
            File.Delete(dbPath);
        }
    }
}
