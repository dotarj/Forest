// Copyright (c) Arjen Post. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Xunit;

namespace Forest.Test
{
    public class BinarySearchTreeNodeTest
    {
        protected BinarySearchTree<int, int> tree;

        public class TheGetMaximumMethod : BinarySearchTreeNodeTest
        {
            public TheGetMaximumMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 10, 10 } };
            }

            [Fact]
            public void ShouldThrowInvalidOperationException()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                Assert.Throws<InvalidOperationException>(() => node.GetMaximum());
            }

            [Fact]
            public void ShouldReturnNodeWithKeyTen()
            {
                Assert.Equal(10, tree.Root.GetMaximum().Key);
            }

            [Fact]
            public void ShouldReturnSelf()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                tree.Add(node);

                Assert.Equal(1, node.GetMaximum().Key);
            }
        }

        public class TheGetMinimumMethod : BinarySearchTreeNodeTest
        {
            public TheGetMinimumMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 1, 1 } };
            }

            [Fact]
            public void ShouldThrowInvalidOperationException()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                Assert.Throws<InvalidOperationException>(() => node.GetMinimum());
            }

            [Fact]
            public void ShouldReturnNodeWithKeyOne()
            {
                Assert.Equal(1, tree.Root.GetMinimum().Key);
            }

            [Fact]
            public void ShouldReturnSelf()
            {
                var node = new BinarySearchTreeNode<int, int>(10, 10);

                tree.Add(node);

                Assert.Equal(10, node.GetMinimum().Key);
            }
        }

        public class TheGetPredecessorMethod : BinarySearchTreeNodeTest
        {
            public TheGetPredecessorMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 9, 9 }, { 6, 6 }, { 10, 10 } };
            }

            [Fact]
            public void ShouldThrowInvalidOperationException()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                Assert.Throws<InvalidOperationException>(() => node.GetPredecessor());
            }

            [Fact]
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

                Assert.Equal(expectedKeys, actualKeys);
            }
        }

        public class TheGetSuccessorMethod : BinarySearchTreeNodeTest
        {
            public TheGetSuccessorMethod()
            {
                tree = new BinarySearchTree<int, int>() { { 5, 5 }, { 2, 2 }, { 7, 7 }, { 1, 1 }, { 9, 9 }, { 6, 6 }, { 10, 10 } };
            }

            [Fact]
            public void ShouldThrowInvalidOperationException()
            {
                var node = new BinarySearchTreeNode<int, int>(1, 1);

                Assert.Throws<InvalidOperationException>(() => node.GetSuccessor());
            }

            [Fact]
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

                Assert.Equal(expectedKeys, actualKeys);
            }
        }
    }
}
