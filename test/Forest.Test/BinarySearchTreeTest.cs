// Copyright (c) Arjen Post. See License.txt and Notice.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Forest.Test
{
    public class BinarySearchTreeTest
    {
        protected static BinarySearchTree<int, int> tree;

        [TestClass]
        public class TheAddMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 1, 1 } };
            }

            [TestMethod, ExpectedException(typeof(ArgumentException))]
            public void ShouldThrowArgumentException()
            {
                tree.Add(1, 1);
            }

            [TestMethod]
            public void ShouldAddNodeWithKeyTwo()
            {
                tree.Add(2, 2);

                Assert.IsTrue(tree.ContainsKey(2));
            }
        }

        [TestClass]
        public class TheClearMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 1, 1 } };
            }

            [TestMethod]
            public void CountShouldEqualZero()
            {
                tree.Clear();

                Assert.AreEqual(0, tree.Count);
            }

            [TestMethod]
            public void ShouldNotContainKey()
            {
                tree.Clear();

                Assert.IsFalse(tree.ContainsKey(1));
            }
        }

        [TestClass]
        public class TheContainsMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>();
            }

            [TestMethod]
            public void ShouldReturnTrue()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                tree.Add(node);

                Assert.IsTrue(tree.Contains(node));
            }

            [TestMethod]
            public void ShouldReturnFalse()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                Assert.IsFalse(tree.Contains(node));
            }
        }

        [TestClass]
        public class TheCopyToMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>();

                tree.Add(5, 5);
                tree.Add(2, 2);
                tree.Add(7, 7);
                tree.Add(1, 1);
                tree.Add(3, 3);
                tree.Add(4, 4);
                tree.Add(9, 9);
                tree.Add(6, 6);
                tree.Add(10, 10);
            }

            [TestMethod]
            public void ShouldCopyNodesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 9, 10 };

                var array = new BinarySearchTreeNode<int, int>[9];

                tree.CopyTo(array, 0);

                var index = 0;

                foreach (var value in array)
                {
                    Assert.AreEqual(expectedResults[index], value.Value);

                    index++;
                }
            }
        }

        [TestClass]
        public class TheContainsKeyMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 1, 1 } };
            }

            [TestMethod]
            public void ShouldReturnTrue()
            {
                Assert.IsTrue(tree.ContainsKey(1));
            }

            [TestMethod]
            public void ShouldReturnFalse()
            {
                Assert.IsFalse(tree.ContainsKey(2));
            }
        }

        [TestClass]
        public class TheCountProperty : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>();

                tree.Add(5, 5);
                tree.Add(2, 2);
                tree.Add(7, 7);
                tree.Add(1, 1);
                tree.Add(3, 3);
                tree.Add(4, 4);
                tree.Add(9, 9);
                tree.Add(6, 6);
                tree.Add(10, 10);
            }

            [TestMethod]
            public void ShouldEqualNine()
            {
                Assert.AreEqual(9, tree.Count);
            }
        }

        [TestClass]
        public class TheGetEnumeratorMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>();

                tree.Add(5, 5);
                tree.Add(2, 2);
                tree.Add(7, 7);
                tree.Add(1, 1);
                tree.Add(3, 3);
                tree.Add(4, 4);
                tree.Add(9, 9);
                tree.Add(6, 6);
                tree.Add(10, 10);
            }

            [TestMethod]
            public void ShouldReturnNodesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 9, 10 };

                var index = 0;

                foreach (var value in tree)
                {
                    Assert.AreEqual(expectedResults[index], value.Value);

                    index++;
                }
            }
        }

        [TestClass]
        public class TheGetKeysMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 3, 3 }, { 9, 9 } };
            }

            [TestMethod]
            public void DefaultShouldReturnKeysInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetKeys())
                {
                    Assert.AreEqual(expectedResults[index], value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReturnKeysPreOrder()
            {
                var expectedResults = new List<int>() { 5, 2, 1, 3, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetKeys(BinaryTraversalMethod.PreOrder))
                {
                    Assert.AreEqual(expectedResults[index], value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReturnKeysInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetKeys(BinaryTraversalMethod.InOrder))
                {
                    Assert.AreEqual(expectedResults[index], value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReturnKeysPostOrder()
            {
                var expectedResults = new List<int>() { 1, 3, 2, 9, 7, 5 };

                var index = 0;

                foreach (var value in tree.GetKeys(BinaryTraversalMethod.PostOrder))
                {
                    Assert.AreEqual(expectedResults[index], value);

                    index++;
                }
            }
        }

        [TestClass]
        public class TheGetNodesMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 3, 3 }, { 9, 9 } };
            }

            [TestMethod]
            public void DefaultShouldReturnNodesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetNodes())
                {
                    Assert.AreEqual(expectedResults[index], value.Value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReturnNodesPreOrder()
            {
                var expectedResults = new List<int>() { 5, 2, 1, 3, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetNodes(BinaryTraversalMethod.PreOrder))
                {
                    Assert.AreEqual(expectedResults[index], value.Value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReturnNodesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetNodes(BinaryTraversalMethod.InOrder))
                {
                    Assert.AreEqual(expectedResults[index], value.Value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReturnNodesPostOrder()
            {
                var expectedResults = new List<int>() { 1, 3, 2, 9, 7, 5 };

                var index = 0;

                foreach (var value in tree.GetNodes(BinaryTraversalMethod.PostOrder))
                {
                    Assert.AreEqual(expectedResults[index], value.Value);

                    index++;
                }
            }
        }

        [TestClass]
        public class TheGetValuesMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 3, 3 }, { 9, 9 } };
            }

            [TestMethod]
            public void DefaultShouldReturnValuesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetValues())
                {
                    Assert.AreEqual(expectedResults[index], value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReturnValuesPreOrder()
            {
                var expectedResults = new List<int>() { 5, 2, 1, 3, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetValues(BinaryTraversalMethod.PreOrder))
                {
                    Assert.AreEqual(expectedResults[index], value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReturnValuesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetValues(BinaryTraversalMethod.InOrder))
                {
                    Assert.AreEqual(expectedResults[index], value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReturnValuesPostOrder()
            {
                var expectedResults = new List<int>() { 1, 3, 2, 9, 7, 5 };

                var index = 0;

                foreach (var value in tree.GetValues(BinaryTraversalMethod.PostOrder))
                {
                    Assert.AreEqual(expectedResults[index], value);

                    index++;
                }
            }
        }

        [TestClass]
        public class TheIndexerMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 1, 1 } };
            }

            [TestMethod]
            public void ShouldReturnTwo()
            {
                tree[1] = 2;

                Assert.AreEqual(2, tree[1]);
            }

            [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
            public void ShouldThrowKeyNotFoundException()
            {
                var value = tree[2];
            }
        }

        [TestClass]
        public class TheRemoveMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 9, 9 }, { 6, 6 }, { 10, 10 }, };
            }

            [TestMethod]
            public void ShouldRemoveNode()
            {
                Assert.IsTrue(tree.Remove(1));
                Assert.IsFalse(tree.ContainsKey(1));
            }

            [TestMethod]
            public void ShouldNotRemoveNode()
            {
                Assert.IsFalse(tree.Remove(8));
            }

            [TestMethod]
            public void ShouldReplaceRemovedNodeByLeftNode()
            {
                var expectedValues = new List<int>() { 1, 5, 6, 7, 9, 10 };

                var index = 0;

                tree.Remove(2);

                foreach (var node in tree.GetNodes(BinaryTraversalMethod.InOrder))
                {
                    Assert.AreEqual(expectedValues[index], node.Value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReplaceRemovedNodeByRightNode()
            {
                var expectedValues = new List<int>() { 1, 2, 5, 6, 9, 10 };

                var index = 0;

                tree.Remove(7);

                foreach (var node in tree.GetNodes(BinaryTraversalMethod.InOrder))
                {
                    Assert.AreEqual(expectedValues[index], node.Value);

                    index++;
                }
            }

            [TestMethod]
            public void ShouldReplaceRemovedNodeByMinimumOfRightNode()
            {
                var expectedValues = new List<int>() { 1, 2, 6, 7, 9, 10 };

                var index = 0;

                tree.Remove(5);

                Assert.AreEqual(6, tree.Root.Value);

                foreach (var node in tree.GetNodes(BinaryTraversalMethod.InOrder))
                {
                    Assert.AreEqual(expectedValues[index], node.Value);

                    index++;
                }
            }
        }

        [TestClass]
        public class TheTryGetValueMethod : BinarySearchTreeTest
        {
            [TestInitialize]
            public void TestInitialize()
            {
                tree = new BinarySearchTree<int, int>() { { 1, 1 } };
            }

            [TestMethod]
            public void ShouldReturnTrueAndOne()
            {
                var value = 0;

                Assert.IsTrue(tree.TryGetValue(1, out value));
                Assert.AreEqual(1, value);
            }

            [TestMethod]
            public void ShouldReturnFalseAndZero()
            {
                var value = 0;

                Assert.IsFalse(tree.TryGetValue(2, out value));
                Assert.AreEqual(0, value);
            }
        }
    }
}
