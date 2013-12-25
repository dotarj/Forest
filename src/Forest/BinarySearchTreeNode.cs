// Copyright (c) Arjen Post. See License.txt in the project root for license information.

using System;

namespace Forest
{
    /// <summary>Defines a <see cref="BinarySearchTreeNode{TKey, TValue}"/> that can be used with a 
    /// <see cref="BinarySearchTree{TKey, TValue}"/>.</summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class BinarySearchTreeNode<TKey, TValue>
    {
        /// <summary>Initializes a new instance of a <see cref="BinarySearchTreeNode{TKey, TValue}"/> with the specified 
        /// key and value.</summary>
        /// <param name="key">The <typeparamref name="TKey"/> associated with <paramref name="key"/>.</param>
        /// <param name="value">The <typeparamref name="TValue"/> associated with <paramref name="value"/>.</param>
        public BinarySearchTreeNode(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Key = key;
            Value = value;
        }

        /// <summary>Gets the key in the <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</summary>
        /// <returns>A <typeparamref name="TKey"/> that is the key of the 
        /// <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</returns>
        public TKey Key { get; private set; }

        /// <summary>Gets the left <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</summary>
        /// <returns>A <see cref="BinarySearchTreeNode{TKey, TValue}"/> with a smaller key.</returns>
        public BinarySearchTreeNode<TKey, TValue> Left { get; internal set; }

        internal BinarySearchTree<TKey, TValue> Owner { get; set; }

        /// <summary>Gets the right <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</summary>
        /// <returns>A <see cref="BinarySearchTreeNode{TKey, TValue}"/> with a larger key.</returns>
        public BinarySearchTreeNode<TKey, TValue> Right { get; internal set; }

        /// <summary>Gets or sets the value in the <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</summary>
        /// <returns>A <typeparamref name="TValue"/> that is the value of the 
        /// <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</returns>
        public TValue Value { get; set; }

        /// <summary>Returns the <see cref="BinarySearchTreeNode{TKey, TValue}"/> with largest key.</summary>
        /// <returns>The <see cref="BinarySearchTreeNode{TKey, TValue}"/> with the largest key, or the 
        /// <see cref="BinarySearchTreeNode{TKey, TValue}"/> self if it does not have a larger 
        /// <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="BinarySearchTreeNode{TKey, TValue}"/> is not part 
        /// of a <see cref="BinarySearchTree{TKey, TValue}"/>.</exception>
        public BinarySearchTreeNode<TKey, TValue> GetMaximum()
        {
            if (Owner == null)
            {
                throw new InvalidOperationException("The node is not part of a tree.");
            }

            var node = this;

            while (node.Right != null)
            {
                node = node.Right;
            }

            return node;
        }

        /// <summary>Returns the <see cref="BinarySearchTreeNode{TKey, TValue}"/> with smallest key.</summary>
        /// <returns>The <see cref="BinarySearchTreeNode{TKey, TValue}"/> with the smallest key, or the 
        /// <see cref="BinarySearchTreeNode{TKey, TValue}"/> self if it does not have a smaller 
        /// <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="BinarySearchTreeNode{TKey, TValue}"/> is not part 
        /// of a <see cref="BinarySearchTree{TKey, TValue}"/>.</exception>
        public BinarySearchTreeNode<TKey, TValue> GetMinimum()
        {
            if (Owner == null)
            {
                throw new InvalidOperationException("The node is not part of a tree.");
            }

            var node = this;

            while (node.Left != null)
            {
                node = node.Left;
            }

            return node;
        }

        /// <summary>Returns the preceding <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</summary>
        /// <returns>The preceding <see cref="BinarySearchTreeNode{TKey, TValue}"/>, or null if there is no 
        /// predecessor.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="BinarySearchTreeNode{TKey, TValue}"/> is not part 
        /// of a <see cref="BinarySearchTree{TKey, TValue}"/>.</exception>
        public BinarySearchTreeNode<TKey, TValue> GetPredecessor()
        {
            if (Owner == null)
            {
                throw new InvalidOperationException("The node is not part of a tree.");
            }

            if (Left != null)
            {
                return Left.GetMaximum();
            }

            var current = Owner.Root;
            BinarySearchTreeNode<TKey, TValue> predecessor = null;

            while (current != null)
            {
                var result = Owner.Comparer.Compare(Key, current.Key);

                if (result > 0)
                {
                    predecessor = current;
                    current = current.Right;
                }
                else if (result < 0)
                {
                    current = current.Left;
                }
                else
                {
                    break;
                }
            }

            return predecessor;
        }

        /// <summary>Returns the succeeding <see cref="BinarySearchTreeNode{TKey, TValue}"/>.</summary>
        /// <returns>The succeeding <see cref="BinarySearchTreeNode{TKey, TValue}"/>, or null if there is no 
        /// successor.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="BinarySearchTreeNode{TKey, TValue}"/> is not part 
        /// of a <see cref="BinarySearchTree{TKey, TValue}"/>.</exception>
        public BinarySearchTreeNode<TKey, TValue> GetSuccessor()
        {
            if(Owner == null)
            {
                throw new InvalidOperationException("The node is not part of a tree.");
            }

            if (Right != null)
            {
                return Right.GetMinimum();
            }

            var current = Owner.Root;
            BinarySearchTreeNode<TKey, TValue> successor = null;

            while (current != null)
            {
                var result = Owner.Comparer.Compare(Key, current.Key);

                if (result < 0)
                {
                    successor = current;
                    current = current.Left;
                }
                else if (result > 0)
                {
                    current = current.Right;
                }
                else
                {
                    break;
                }
            }

            return successor;
        }
    }
}
