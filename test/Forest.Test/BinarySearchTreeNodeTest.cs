// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Forest.Test
{
    public class BinarySearchTreeNodeTest
    {
        protected static BinarySearchTree<int, int> tree;

        [TestClass]
        public class TheGetMaximumMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 10, 10 } };
            }

            [TestMethod, ExpectedException(typeof(InvalidOperationException))]
            public void ShouldThrowInvalidOperationException()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                node.GetMaximum();
            }

            [TestMethod]
            public void ShouldReturnNodeWithKeyTen()
            {
                Assert.AreEqual(10, tree.Root.GetMaximum().Key);
            }

            [TestMethod]
            public void ShouldReturnSelf()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                tree.Add(node);

                Assert.AreEqual(1, node.GetMaximum().Key);
            }
        }

        [TestClass]
        public class TheGetMinimumMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 1, 1 } };
            }

            [TestMethod, ExpectedException(typeof(InvalidOperationException))]
            public void ShouldThrowInvalidOperationException()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                node.GetMinimum();
            }

            [TestMethod]
            public void ShouldReturnNodeWithKeyOne()
            {
                Assert.AreEqual(1, tree.Root.GetMinimum().Key);
            }

            [TestMethod]
            public void ShouldReturnSelf()
            {
                var node = new BinarySearchTreeNode<int, int>(10, 10);

                tree.Add(node);

                Assert.AreEqual(10, node.GetMinimum().Key);
            }
        }

        [TestClass]
        public class TheGetPredecessorMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 9, 9 }, { 6, 6 }, { 10, 10 } };
            }

            [TestMethod, ExpectedException(typeof(InvalidOperationException))]
            public void ShouldThrowInvalidOperationException()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                node.GetPredecessor();
            }

            [TestMethod]
            public void ShouldReturnNodesReversedInOrder()
            {
                var expectedKeys = new List<int?>() { 10, 9, 7, 6, 5, 2, 1, null };
                var actualKeys = new List<int?>();

                BinarySearchTreeNode<int, int> node = tree.Root.GetMaximum();

                while (true)
                {
                    if (node == null)
                    {
                        actualKeys.Add(null);
                        break;
                    }

                    actualKeys.Add(node.Key);

                    node = node.GetPredecessor();
                }

                CollectionAssert.AreEqual(expectedKeys, actualKeys);
            }
        }

        [TestClass]
        public class TheGetSuccessorMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 9, 9 }, { 6, 6 }, { 10, 10 } };
            }

            [TestMethod, ExpectedException(typeof(InvalidOperationException))]
            public void ShouldThrowInvalidOperationException()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                node.GetSuccessor();
            }

            [TestMethod]
            public void ShouldReturnNodesReversedInOrder()
            {
                var expectedKeys = new List<int?>() { 1, 2, 5, 6, 7, 9, 10, null };
                var actualKeys = new List<int?>();

                BinarySearchTreeNode<int, int> node = tree.Root.GetMinimum();

                while(true)
                {
                    if(node == null)
                    {
                        actualKeys.Add(null);
                        break;
                    }

                    actualKeys.Add(node.Key);

                    node = node.GetSuccessor();
                }

                CollectionAssert.AreEqual(expectedKeys, actualKeys);
            }
        }
    }
}
