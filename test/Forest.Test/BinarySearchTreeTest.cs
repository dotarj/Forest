// Copyright (c) Arjen Post. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Forest.Test
{
    public class BinarySearchTreeTest
    {
        protected BinarySearchTree<int, int> tree;

        public class TheAddMethod : BinarySearchTreeTest
        {
            public TheAddMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 1, 1 } };
            }

            [Fact]
            public void ShouldThrowArgumentException()
            {
                Assert.Throws<ArgumentException>(() => tree.Add(1, 1));
            }

            [Fact]
            public void ShouldAddNodeWithKeyTwo()
            {
                tree.Add(2, 2);

                Assert.True(tree.ContainsKey(2));
            }
        }

        public class TheClearMethod : BinarySearchTreeTest
        {
            public TheClearMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 1, 1 } };
            }

            [Fact]
            public void CountShouldEqualZero()
            {
                tree.Clear();

                Assert.Equal(0, tree.Count);
            }

            [Fact]
            public void ShouldNotContainKey()
            {
                tree.Clear();

                Assert.False(tree.ContainsKey(1));
            }
        }

        public class TheContainsMethod : BinarySearchTreeTest
        {
            public TheContainsMethod()
            {
                tree = new BinarySearchTree<int, int>();
            }

            [Fact]
            public void ShouldReturnTrue()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                tree.Add(node);

                Assert.True(tree.Contains(node));
            }

            [Fact]
            public void ShouldReturnFalse()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                Assert.False(tree.Contains(node));
            }
        }

        public class TheCopyToMethod : BinarySearchTreeTest
        {
            public TheCopyToMethod()
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

            [Fact]
            public void ShouldCopyNodesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 9, 10 };

                var array = new BinarySearchTreeNode<int, int>[9];

                tree.CopyTo(array, 0);

                Assert.Equal(expectedResults, array.Select(value => value.Value).ToList());
            }
        }

        public class TheContainsKeyMethod : BinarySearchTreeTest
        {
            public TheContainsKeyMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 1, 1 } };
            }

            [Fact]
            public void ShouldReturnTrue()
            {
                Assert.True(tree.ContainsKey(1));
            }

            [Fact]
            public void ShouldReturnFalse()
            {
                Assert.False(tree.ContainsKey(2));
            }
        }

        public class TheCountProperty : BinarySearchTreeTest
        {
            public TheCountProperty()
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

            [Fact]
            public void ShouldEqualNine()
            {
                Assert.Equal(9, tree.Count);
            }
        }

        public class TheGetEnumeratorMethod : BinarySearchTreeTest
        {
            public TheGetEnumeratorMethod()
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

            [Fact]
            public void ShouldReturnNodesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 9, 10 };

                var index = 0;

                foreach (var value in tree)
                {
                    Assert.Equal(expectedResults[index], value.Value);

                    index++;
                }
            }
        }

        public class TheGetKeysMethod : BinarySearchTreeTest
        {
            public TheGetKeysMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 3, 3 }, { 9, 9 } };
            }

            [Fact]
            public void DefaultShouldReturnKeysInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetKeys())
                {
                    Assert.Equal(expectedResults[index], value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReturnKeysPreOrder()
            {
                var expectedResults = new List<int>() { 5, 2, 1, 3, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetKeys(BinaryTraversalMethod.PreOrder))
                {
                    Assert.Equal(expectedResults[index], value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReturnKeysInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetKeys(BinaryTraversalMethod.InOrder))
                {
                    Assert.Equal(expectedResults[index], value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReturnKeysPostOrder()
            {
                var expectedResults = new List<int>() { 1, 3, 2, 9, 7, 5 };

                var index = 0;

                foreach (var value in tree.GetKeys(BinaryTraversalMethod.PostOrder))
                {
                    Assert.Equal(expectedResults[index], value);

                    index++;
                }
            }
        }

        public class TheGetNodesMethod : BinarySearchTreeTest
        {
            public TheGetNodesMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 3, 3 }, { 9, 9 } };
            }

            [Fact]
            public void DefaultShouldReturnNodesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetNodes())
                {
                    Assert.Equal(expectedResults[index], value.Value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReturnNodesPreOrder()
            {
                var expectedResults = new List<int>() { 5, 2, 1, 3, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetNodes(BinaryTraversalMethod.PreOrder))
                {
                    Assert.Equal(expectedResults[index], value.Value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReturnNodesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetNodes(BinaryTraversalMethod.InOrder))
                {
                    Assert.Equal(expectedResults[index], value.Value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReturnNodesPostOrder()
            {
                var expectedResults = new List<int>() { 1, 3, 2, 9, 7, 5 };

                var index = 0;

                foreach (var value in tree.GetNodes(BinaryTraversalMethod.PostOrder))
                {
                    Assert.Equal(expectedResults[index], value.Value);

                    index++;
                }
            }
        }

        public class TheGetValuesMethod : BinarySearchTreeTest
        {
            public TheGetValuesMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 3, 3 }, { 9, 9 } };
            }

            [Fact]
            public void DefaultShouldReturnValuesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetValues())
                {
                    Assert.Equal(expectedResults[index], value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReturnValuesPreOrder()
            {
                var expectedResults = new List<int>() { 5, 2, 1, 3, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetValues(BinaryTraversalMethod.PreOrder))
                {
                    Assert.Equal(expectedResults[index], value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReturnValuesInOrder()
            {
                var expectedResults = new List<int>() { 1, 2, 3, 5, 7, 9 };

                var index = 0;

                foreach (var value in tree.GetValues(BinaryTraversalMethod.InOrder))
                {
                    Assert.Equal(expectedResults[index], value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReturnValuesPostOrder()
            {
                var expectedResults = new List<int>() { 1, 3, 2, 9, 7, 5 };

                Assert.Equal(expectedResults, tree.GetValues(BinaryTraversalMethod.PostOrder).ToList());
            }
        }

        public class TheIndexerMethod : BinarySearchTreeTest
        {
            public TheIndexerMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 1, 1 } };
            }

            [Fact]
            public void ShouldReturnTwo()
            {
                tree[1] = 2;

                Assert.Equal(2, tree[1]);
            }

            [Fact]
            public void ShouldThrowKeyNotFoundException()
            {
                Assert.Throws<KeyNotFoundException>(() => tree[2]);
            }
        }

        public class TheRemoveMethod : BinarySearchTreeTest
        {
            public TheRemoveMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 9, 9 }, { 6, 6 }, { 10, 10 }, };
            }

            [Fact]
            public void ShouldRemoveNode()
            {
                Assert.True(tree.Remove(1));
                Assert.False(tree.ContainsKey(1));
            }

            [Fact]
            public void ShouldNotRemoveNode()
            {
                Assert.False(tree.Remove(8));
            }

            [Fact]
            public void ShouldReplaceRemovedNodeByLeftNode()
            {
                var expectedValues = new List<int>() { 1, 5, 6, 7, 9, 10 };

                var index = 0;

                tree.Remove(2);

                foreach (var node in tree.GetNodes(BinaryTraversalMethod.InOrder))
                {
                    Assert.Equal(expectedValues[index], node.Value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReplaceRemovedNodeByRightNode()
            {
                var expectedValues = new List<int>() { 1, 2, 5, 6, 9, 10 };

                var index = 0;

                tree.Remove(7);

                foreach (var node in tree.GetNodes(BinaryTraversalMethod.InOrder))
                {
                    Assert.Equal(expectedValues[index], node.Value);

                    index++;
                }
            }

            [Fact]
            public void ShouldReplaceRemovedNodeByMinimumOfRightNode()
            {
                var expectedValues = new List<int>() { 1, 2, 6, 7, 9, 10 };

                var index = 0;

                tree.Remove(5);

                Assert.Equal(6, tree.Root.Value);

                foreach (var node in tree.GetNodes(BinaryTraversalMethod.InOrder))
                {
                    Assert.Equal(expectedValues[index], node.Value);

                    index++;
                }
            }
        }

        public class TheTryGetValueMethod : BinarySearchTreeTest
        {
            public TheTryGetValueMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 1, 1 } };
            }

            [Fact]
            public void ShouldReturnTrueAndOne()
            {
                var value = 0;

                Assert.True(tree.TryGetValue(1, out value));
                Assert.Equal(1, value);
            }

            [Fact]
            public void ShouldReturnFalseAndZero()
            {
                var value = 0;

                Assert.False(tree.TryGetValue(2, out value));
                Assert.Equal(0, value);
            }
        }
    }
}
