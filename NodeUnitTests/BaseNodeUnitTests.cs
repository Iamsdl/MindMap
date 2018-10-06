using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDL.MindMap.NodeLibrary;
 
namespace NodeUnitTests
{
    [TestClass]
    public class BaseNodeUnitTests
    {
        [TestMethod]
        public void AddChildTest()
        {
            BaseNode parent = new BaseNode();
            BaseNode child0 = new BaseNode();
            BaseNode child1 = new BaseNode();
            BaseNode child2 = new BaseNode();
            BaseNode child20 = new BaseNode();
            parent.AddChild(child0);
            parent.AddChild(child1);
            parent.AddChild(child2);
            child2.AddChild(child20);
            Assert.AreEqual("a00", child0.Name);
            Assert.AreEqual("a01", child1.Name);
            Assert.AreEqual("a02", child2.Name);
            Assert.AreEqual("a0200", child20.Name);
        }

        [TestMethod]
        public void RenameTest()
        {
            BaseNode parent = new BaseNode();
            BaseNode child0 = new BaseNode();
            BaseNode child00 = new BaseNode();
            parent.AddChild(child0);
            child0.AddChild(child00);
            child0.Name = "a01";
            child0.Rename();
            Assert.AreEqual("a0100", child00.Name);
            parent.Rename();
            Assert.AreEqual("a00", child0.Name);
            Assert.AreEqual("a0000", child00.Name);
        }

        [TestMethod]
        public void DeleteTest()
        {
            BaseNode parent = new BaseNode();
            BaseNode child00 = new BaseNode();
            BaseNode child0000 = new BaseNode();
            BaseNode child0001 = new BaseNode();
            BaseNode child000000 = new BaseNode();
            parent.AddChild(child00);
            child00.AddChild(child0000);
            child00.AddChild(child0001);
            child0000.AddChild(child000000);

            child00.Delete(false);
            Assert.AreEqual(2,parent.Children.Count);
            Assert.AreEqual("a00", child0000.Name);
            Assert.AreEqual("a01", child0001.Name);
            Assert.AreEqual("a0000", child000000.Name);

            child0001.Delete(true);
            Assert.AreEqual(1, parent.Children.Count);
            child0000.Delete(true);
            Assert.AreEqual(0, parent.Children.Count);
        }

        [TestMethod]
        public void MakeChildOfTest()
        {
            BaseNode parent = new BaseNode();
            BaseNode child0 = new BaseNode();
            BaseNode child00 = new BaseNode();
            BaseNode child01 = new BaseNode();
            parent.AddChild(child0);
            child0.AddChild(child00);
            child0.AddChild(child01);

            child01.MakeChildOf(parent);
            Assert.AreEqual(1, child0.Children.Count);
            Assert.AreEqual(2, parent.Children.Count);
            Assert.AreEqual("a01", child01.Name);
        }

        [TestMethod]
        public void MoveTest()
        {
            BaseNode node = new BaseNode();
            Assert.AreEqual(0, node.X);
            Assert.AreEqual(0, node.Y);
            node.MoveTo(10, 10);
            Assert.AreEqual(10, node.X);
            Assert.AreEqual(10, node.Y);
            node.MoveBy(10, 10);
            Assert.AreEqual(20, node.X);
            Assert.AreEqual(20, node.Y);
        }

        [TestMethod]
        public void FindTest()
        {
            BaseNode parent = new BaseNode();
            BaseNode child00 = new BaseNode();
            BaseNode child01 = new BaseNode();
            BaseNode child0000 = new BaseNode();
            parent.AddChild(child00);
            parent.AddChild(child01);
            child00.AddChild(child0000);
            BaseNode found = parent.Find("a00");
            Assert.AreSame(child00, found);
            found = parent.Find("a01");
            Assert.AreSame(child01, found);
            found = parent.Find("a0000");
            Assert.AreSame(child0000, found);

        }
    }
}
