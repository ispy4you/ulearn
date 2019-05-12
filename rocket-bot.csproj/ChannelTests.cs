using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using rocket_bot;

namespace rocket_bot
{
    [TestFixture]
    public class ChannelTests
    {
        private Channel<string> channel;
        private readonly Random rnd = new Random();

        [SetUp]
        public void Init()
        {   
            channel = new Channel<string>();
        }

        [Test]
        public void AddAndGetLastOneItem()
        {
            channel.AppendIfLastItemIsUnchanged("a", null);
            Assert.AreEqual(channel.LastItem(), "a");
        }

        [Test]
        public void AddAndGetLastManyItems()
        {
            channel.AppendIfLastItemIsUnchanged("a", null);
            channel.AppendIfLastItemIsUnchanged("b", "a");
            channel.AppendIfLastItemIsUnchanged("c", "b");
            Assert.AreEqual(channel.LastItem(), "c");
        }

        [Test]
        public void GetAfterPointerReadFromBegin()
        {
            string[] items = {"a", "b", "c", "d"};
            for (var index = 0; index < items.Length; index++)
            {
                AppendToChannel(items[index]);
            }

            for (int i = 0; i < items.Length; ++i)
            {
                Assert.AreEqual(items[i], channel[i]);
            }
        }

        [Test]
        public void TestCount()
        {
            for (int i = 0; i < 5; ++i)
            {
                AppendToChannel("a");
                Assert.AreEqual(i + 1, channel.Count);
            }
        }

        [Test]
        public void AddAfterPointerInvalidateAllHistoryTestCount()
        {
            for (int i = 0; i < 10; ++i)
            {
                AppendToChannel("a");
            }

            channel[0] = "b";
            Assert.AreEqual(1, channel.Count);
        }

        [Test]
        public void AddAfterPointerInvalidateAllHistory()
        {
            for (int i = 0; i < 10; ++i)
            {
                AppendToChannel("a");
            }

            channel[2] = "b";
            channel[3] = "c";
            AppendToChannel("d");

            string[] expectedResult = { "a", "a", "b", "c", "d" };
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], channel[i]);
            }
        }

        [Test]
        public void TestAppendIfLastItemIs()
        {
            channel.AppendIfLastItemIsUnchanged("a", null);
            channel.AppendIfLastItemIsUnchanged("b", null);
            channel.AppendIfLastItemIsUnchanged("c", "a");

            string[] expectedResult = { "a", "c"};
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], channel[i]);
            }
        }

        private volatile bool timeToEndTest;
        
        [Test]
        public void ParallelTestForChannel()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 1; ++i)
            {
                //tasks.Add(Task.Factory.StartNew(Get));
                tasks.Add(Task.Factory.StartNew(LoopResetHistory));
                //tasks.Add(Task.Factory.StartNew(LastItem));
                tasks.Add(Task.Factory.StartNew(LoopAppendIfLasItemIs));
            }
            Thread.Sleep(1000);
            timeToEndTest = true;
            Task.WhenAll(tasks).Wait();
        }


        public void AppendToChannel(string item)
        {
            channel.AppendIfLastItemIsUnchanged(item, channel.LastItem());
        }

        public void LoopGetRandom()
        {
            var timer = new Stopwatch();
            timer.Start();
            while (!timeToEndTest)
            {
                var index = rnd.Next(100);
                var rocket = channel[index];
            }
        }

        public void LoopResetHistory()
        {
            var timer = new Stopwatch();
            timer.Start();
            while (!timeToEndTest)
            {
                channel[0] = "a";
            }
        }

        public void LoopGetLastItem()
        {
            var timer = new Stopwatch();
            timer.Start();
            while (!timeToEndTest)
            {
                var rocket = channel.LastItem();
            }
        }

        public void LoopAppendIfLasItemIs()
        {
            var timer = new Stopwatch();
            timer.Start();
            while (!timeToEndTest)
            {
                channel.AppendIfLastItemIsUnchanged("a", "a");
            }
        }
    }
}