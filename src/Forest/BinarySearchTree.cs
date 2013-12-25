// Copyright (c) Arjen Post. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Forest
{
    /// <summary>Represents a collection of keys and values.</summary>
    /// <typeparam name="TKey">The type of the keys in the tree.</typeparam>
    /// <typeparam name="TValue">The type of the values in the tree.</typeparam>
    public class BinarySearchTree<TKey, TValue> : ICollection<BinarySearchTreeNode<TKey, TValue>>
    {
        /// <summary>Initializes a new instance of the <see cref="BinarySearchTree{TKey, TValue}"/> class that is empty, 
        /// and uses the default comparer for the key type.</summary>
        public BinarySearchTree()
        {
            Comparer = Comparer<TKey>.Default;
        }

        /// <summary>Initializes a new instance of the <see cref="BinarySearchTree{TKey, TValue}"/> class that is empty, 
        /// and uses the specified <see cref="IComparer{T}"/>.</summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing keys, or null to 
        /// use the default <see cref="Comparer{T}"/> for the type of the key.</param>
        public BinarySearchTree(IComparer<TKey> comparer)
        {
            Comparer = comparer ?? Comparer<TKey>.Default;
        }

        /// <summary>Gets the <see cref="IComparer{T}"/> that is used to compare the keys for the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <returns>The <see cref="IComparer{T}"/> generic interface implementation that is used to compare the keys for 
        /// the current <see cref="BinarySearchTree{TKey, TValue}"/>.</returns>
        public IComparer<TKey> Comparer { get; private set; }

        /// <summary>Gets the number of elements contained in the <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <returns>The number of elements contained in the <see cref="BinarySearchTree{TKey, TValue}"/>.</returns>
        public int Count { get; private set; }

        /// <summary>Gets a value indicating whether the <see cref="BinarySearchTree{TKey, TValue}"/> is 
        /// read-only.</summary>
        /// <returns>true if the <see cref="BinarySearchTree{TKey, TValue}"/> is read-only; otherwise, false.</returns>
        bool ICollection<BinarySearchTreeNode<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>Gets the root <see cref="BinarySearchTreeNode{TKey, TValue}"/> of the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <returns>The root <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</returns>
        public BinarySearchTreeNode<TKey, TValue> Root { get; private set; }

        /// <summary>Gets or sets the value associated with the specified <paramref name="key"/>.</summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified <paramref name="key"/>. If the specified 
        /// <paramref name="key"/> is not found, a get operation throws a <see cref="KeyNotFoundException"/>, and a set 
        /// operation creates a new element with the specified <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="key"/> does not exist in 
        /// the collection.</exception>
        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                var node = FindNode(key);

                if (node == null)
                {
                    throw new KeyNotFoundException();
                }

                return node.Value;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                var current = Root;
                BinarySearchTreeNode<TKey, TValue> parent = null;
                int result = 0;

                while (current != null)
                {
                    result = Comparer.Compare(key, current.Key);

                    if (result > 0)
                    {
                        parent = current;
                        current = current.Right;
                    }
                    else if (result < 0)
                    {
                        parent = current;
                        current = current.Left;
                    }
                    else
                    {
                        current.Value = value;

                        return;
                    }
                }

                var node = new BinarySearchTreeNode<TKey, TValue>(key, value) { Owner = this };

                if (parent != null)
                {
                    if (result > 0)
                    {
                        parent.Right = node;
                    }
                    else if (result < 0)
                    {
                        parent.Left = node;
                    }
                }
                else
                {
                    Root = node;
                }

                Count++;
            }
        }

        /// <summary>Adds the specified <paramref name="key"/> and value to the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentException">An element with the same <paramref name="key"/> already exists in the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</exception>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            var current = Root;
            BinarySearchTreeNode<TKey, TValue> parent = null;
            var result = 0;

            while (current != null)
            {
                result = Comparer.Compare(key, current.Key);

                if (result > 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else
                {
                    throw new ArgumentException("An item with the same key has already been added.");
                }
            }

            var node = new BinarySearchTreeNode<TKey, TValue>(key, value) { Owner = this };

            if (parent != null)
            {
                if (result > 0)
                {
                    parent.Right = node;
                }
                else if (result < 0)
                {
                    parent.Left = node;
                }
            }
            else
            {
                Root = node;
            }

            Count++;
        }

        /// <summary>Adds an node to the <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <param name="item">The <see cref="BinarySearchTreeNode{TKey, TValue}"/> to add to the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</param>
        public void Add(BinarySearchTreeNode<TKey, TValue> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            
            if (item.Owner != null)
            {
                throw new InvalidOperationException("Node is already the child of another tree.");
            }

            var current = Root;
            BinarySearchTreeNode<TKey, TValue> parent = null;
            var result = 0;

            while (current != null)
            {
                result = Comparer.Compare(item.Key, current.Key);

                if (result > 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else
                {
                    throw new ArgumentException("An item with the same key has already been added.");
                }
            }

            item.Owner = this;

            if (parent != null)
            {
                if (result > 0)
                {
                    parent.Right = item;
                }
                else if (result < 0)
                {
                    parent.Left = item;
                }
            }
            else
            {
                Root = item;
            }

            Count++;
        }

        /// <summary>Removes all keys and values from the <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        public void Clear()
        {
            foreach (var node in EnumeratePostOrder())
            {
                node.Left = null;
                node.Right = null;
                node.Owner = null;
            }

            Root = null;

            Count = 0;
        }

        /// <summary>Determines whether the <see cref="BinarySearchTree{TKey, TValue}"/> contains a specific 
        /// value.</summary>
        /// <param name="item">The <see cref="BinarySearchTreeNode{TKey, TValue}"/> to locate in the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</param>
        /// <returns>true if the <see cref="BinarySearchTreeNode{TKey, TValue}"/> is found in the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public bool Contains(BinarySearchTreeNode<TKey, TValue> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item == FindNode(item.Key);
        }

        /// <summary>Determines whether the <see cref="BinarySearchTree{TKey, TValue}"/> contains the specified 
        /// <paramref name="key"/>.</summary>
        /// <param name="key">The key to locate in the <see cref="BinarySearchTree{TKey, TValue}"/>.</param>
        /// <returns>true if the <see cref="BinarySearchTree{TKey, TValue}"/> contains an element with the specified 
        /// <paramref name="key"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            var node = FindNode(key);

            return node != null;
        }

        /// <summary>Copies the elements of the <see cref="BinarySearchTree{TKey, TValue}"/> to an <see cref="Array"/>, 
        /// starting at a particular <see cref="Array"/> index.</summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied 
        /// from <see cref="BinarySearchTree{TKey, TValue}"/>. The <see cref="Array"/> must have zero-based 
        /// indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="ArgumentException">array is multidimensional.  -or- arrayIndex is equal to or greater than 
        /// the length of array.  -or- The number of elements in the source <see cref="BinarySearchTree{TKey, TValue}"/> 
        /// is greater than the available space from arrayIndex to the end of the destination array.  -or- Type T cannot 
        /// be cast automatically to the type of the destination array.</exception>
        public void CopyTo(BinarySearchTreeNode<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex < 0 || arrayIndex > array.Length)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", "Non-negative number required.");
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
            }

            foreach (var node in EnumerateInOrder())
            {
                array[arrayIndex] = node;

                arrayIndex++;
            }
        }
        private IEnumerable<BinarySearchTreeNode<TKey, TValue>> EnumerateInOrder()
        {
            var nodes = new Stack<BinarySearchTreeNode<TKey, TValue>>();

            nodes.Push(null);

            var current = Root;

            while (current != null)
            {
                while (current != null)
                {
                    nodes.Push(current);

                    current = current.Left;
                }

                current = nodes.Pop();

                while (current != null)
                {
                    yield return current;

                    if (current.Right != null)
                    {
                        current = current.Right;

                        break;
                    }

                    current = nodes.Pop();
                }
            }
        }

        private IEnumerable<BinarySearchTreeNode<TKey, TValue>> EnumeratePostOrder()
        {
            var nodes = new Stack<PostOrderNode>();

            nodes.Push(null);

            var current = Root;

            while (current != null)
            {
                while (current != null)
                {
                    nodes.Push(new PostOrderNode(current, false));

                    if (current.Right != null)
                    {
                        nodes.Push(new PostOrderNode(current.Right, true));
                    }

                    current = current.Left;
                }

                while (true)
                {
                    var node = nodes.Pop();

                    if (node == null)
                    {
                        yield break;
                    }

                    current = node.Node;

                    if (node.IsRight)
                    {
                        break;
                    }

                    yield return current;
                }
            }
        }

        private IEnumerable<BinarySearchTreeNode<TKey, TValue>> EnumeratePreOrder()
        {
            var nodes = new Stack<BinarySearchTreeNode<TKey, TValue>>();

            nodes.Push(null);

            var current = Root;

            while (current != null)
            {
                while (current != null)
                {
                    yield return current;

                    if (current.Right != null)
                    {
                        nodes.Push(current.Right);
                    }

                    current = current.Left;
                }

                current = nodes.Pop();
            }
        }

        private BinarySearchTreeNode<TKey, TValue> FindNode(TKey key)
        {
            var current = Root;

            while (current != null)
            {
                var result = Comparer.Compare(key, current.Key);

                if (result > 0)
                {
                    current = current.Right;
                }
                else if (result < 0)
                {
                    current = current.Left;
                }
                else
                {
                    return current;
                }
            }

            return null;
        }

        private BinarySearchTreeNode<TKey, TValue> FindNode(TKey key, out BinarySearchTreeNode<TKey, TValue> parent)
        {
            parent = null;
            var current = Root;

            while (current != null)
            {
                var result = Comparer.Compare(key, current.Key);

                if (result > 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else
                {
                    return current;
                }
            }

            return null;
        }

        /// <summary>Returns an enumerator that iterates through the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <returns>An enumerator for the <see cref="BinarySearchTree{TKey, TValue}"/>.</returns>
        public IEnumerator<BinarySearchTreeNode<TKey, TValue>> GetEnumerator()
        {
            return EnumerateInOrder().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>Returns a collection containing the keys in the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <param name="method">One of the <see cref="BinaryTraversalMethod"/> values that specifies the order in which 
        /// the keys are returned.</param>
        /// <returns>A <see cref="IEnumerable{TKey}"/> containing the keys in the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="method"/> is not a valid 
        /// <see cref="BinaryTraversalMethod"/> value.</exception>
        public IEnumerable<TKey> GetKeys(BinaryTraversalMethod method = BinaryTraversalMethod.InOrder)
        {
            if (method != BinaryTraversalMethod.PreOrder && method != BinaryTraversalMethod.InOrder && method != BinaryTraversalMethod.PostOrder)
            {
                throw new ArgumentOutOfRangeException("method", "Enum value was out of legal range.");
            }

            var nodes = GetNodes(method);

            foreach (var node in nodes)
            {
                yield return node.Key;
            }
        }

        /// <summary>Returns the <see cref="BinarySearchTreeNode{TKey, TValue}"/> with largest key.</summary>
        /// <returns>The <see cref="BinarySearchTreeNode{TKey, TValue}"/> with the largest key, or null if the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/> is empty.</returns>
        public BinarySearchTreeNode<TKey, TValue> GetMaximum()
        {
            if (Root == null)
            {
                return null;
            }

            return Root.GetMaximum();
        }

        /// <summary>Returns the <see cref="BinarySearchTreeNode{TKey, TValue}"/> with smallest key.</summary>
        /// <returns>The <see cref="BinarySearchTreeNode{TKey, TValue}"/> with the smallest key, or null if the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/> is empty.</returns>
        public BinarySearchTreeNode<TKey, TValue> GetMinimum()
        {
            if (Root == null)
            {
                return null;
            }

            return Root.GetMinimum();
        }

        /// <summary>Returns a collection containing the nodes in the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <param name="method">One of the <see cref="BinaryTraversalMethod"/> values that specifies the order in which 
        /// the nodes are returned.</param>
        /// <returns>A <see cref="IEnumerable{TKey}"/> containing the nodes in the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="method"/> is not a valid 
        /// <see cref="BinaryTraversalMethod"/> value.</exception>
        public IEnumerable<BinarySearchTreeNode<TKey, TValue>> GetNodes(BinaryTraversalMethod method = BinaryTraversalMethod.InOrder)
        {
            if (method != BinaryTraversalMethod.PreOrder && method != BinaryTraversalMethod.InOrder && method != BinaryTraversalMethod.PostOrder)
            {
                throw new ArgumentOutOfRangeException("method", "Enum value was out of legal range.");
            }

            IEnumerable<BinarySearchTreeNode<TKey, TValue>> nodes = null;

            switch (method)
            {
                case BinaryTraversalMethod.PreOrder:
                    nodes = EnumeratePreOrder();
                    break;
                case BinaryTraversalMethod.InOrder:
                    nodes = EnumerateInOrder();
                    break;
                case BinaryTraversalMethod.PostOrder:
                    nodes = EnumeratePostOrder();
                    break;
            }

            return nodes;
        }

        /// <summary>Returns a collection containing the values in the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <param name="method">One of the <see cref="BinaryTraversalMethod"/> values that specifies the order in which 
        /// the values are returned.</param>
        /// <returns>A <see cref="IEnumerable{TKey}"/> containing the values in the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="method"/> is not a valid 
        /// <see cref="BinaryTraversalMethod"/> value.</exception>
        public IEnumerable<TValue> GetValues(BinaryTraversalMethod method = BinaryTraversalMethod.InOrder)
        {
            if (method != BinaryTraversalMethod.PreOrder && method != BinaryTraversalMethod.InOrder && method != BinaryTraversalMethod.PostOrder)
            {
                throw new ArgumentOutOfRangeException("method", "Enum value was out of legal range.");
            }

            var nodes = GetNodes(method);

            foreach (var node in nodes)
            {
                yield return node.Value;
            }
        }

        /// <summary>Removes the value with the specified <paramref name="key"/> from the 
        /// <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method returns false 
        /// if <paramref name="key"/> is not found in the <see cref="BinarySearchTree{TKey, TValue}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            BinarySearchTreeNode<TKey, TValue> parent;
            var node = FindNode(key, out parent);

            if (node != null)
            {
                RemoveNode(node, parent);

                return true;
            }

            return false;
        }

        /// <summary>Removes the node from the <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
        /// <param name="node">The element to remove.</param>
        /// <returns>true if the node is successfully found and removed; otherwise, false. This method returns false if 
        /// <paramref name="node"/> is not found in the <see cref="BinarySearchTree{TKey, TValue}"/>.</returns>
        /// <exception cref="ArgumentNullException">node is null.</exception>
        public bool Remove(BinarySearchTreeNode<TKey, TValue> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            BinarySearchTreeNode<TKey, TValue> parent;

            if (node == FindNode(node.Key, out parent))
            {
                RemoveNode(node, parent);

                return true;
            }

            return false;
        }

        private void RemoveNode(BinarySearchTreeNode<TKey, TValue> node, BinarySearchTreeNode<TKey, TValue> parent)
        {
            if (node.Right == null)
            {
                ReplaceNodeWithLeftNode(node, parent);
            }
            else if (node.Right.Left == null)
            {
                ReplaceNodeWithRightNode(node, parent);
            }
            else
            {
                ReplaceNodeWithMinimumNode(node, parent);
            }

            node.Left = null;
            node.Right = null;
            node.Owner = null;

            Count--;
        }

        private void ReplaceNodeWithLeftNode(BinarySearchTreeNode<TKey, TValue> node, BinarySearchTreeNode<TKey, TValue> parent)
        {
            if (node == Root)
            {
                Root = node.Left;
            }
            else
            {
                var result = Comparer.Compare(parent.Key, node.Key);

                if (result > 0)
                {
                    parent.Left = node.Left;
                }
                else if (result < 0)
                {
                    parent.Right = node.Left;
                }
            }
        }

        private void ReplaceNodeWithMinimumNode(BinarySearchTreeNode<TKey, TValue> node, BinarySearchTreeNode<TKey, TValue> parent)
        {
            BinarySearchTreeNode<TKey, TValue> minimum = node.Right.Left;
            BinarySearchTreeNode<TKey, TValue> minimumParent = node.Right;

            while (minimum.Left != null)
            {
                minimumParent = minimum;
                minimum = minimum.Left;
            }

            minimumParent.Left = minimum.Right;
            minimum.Left = node.Left;
            minimum.Right = node.Right;

            if (node == Root)
            {
                Root = minimum;
            }
            else
            {
                var result = Comparer.Compare(parent.Key, node.Key);

                if (result > 0)
                {
                    parent.Left = minimum;
                }
                else if (result < 0)
                {
                    parent.Right = minimum;
                }
            }
        }

        private void ReplaceNodeWithRightNode(BinarySearchTreeNode<TKey, TValue> node, BinarySearchTreeNode<TKey, TValue> parent)
        {
            node.Right.Left = node.Left;

            if (node == Root)
            {
                Root = node.Right;
            }
            else
            {
                var result = Comparer.Compare(parent.Key, node.Key);

                if (result > 0)
                {
                    parent.Left = node.Right;
                }
                else if (result < 0)
                {
                    parent.Right = node.Right;
                }
            }
        }

        /// <summary>Gets the value associated with the specified <paramref name="key"/>.</summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified 
        /// <paramref name="key"/>, if the <paramref name="key"/> is found; otherwise, the default value for the type of 
        /// the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the <see cref="BinarySearchTree{TKey, TValue}"/> contains an element with the specified 
        /// <paramref name="key"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            var node = FindNode(key);

            if (node != null)
            {
                value = node.Value;

                return true;
            }

            value = default(TValue);

            return false;
        }

        private class PostOrderNode
        {
            internal PostOrderNode(BinarySearchTreeNode<TKey, TValue> node, bool isRight)
            {
                Node = node;
                IsRight = isRight;
            }

            internal BinarySearchTreeNode<TKey, TValue> Node { get; private set; }
            internal bool IsRight { get; private set; }
        }
    }
}
