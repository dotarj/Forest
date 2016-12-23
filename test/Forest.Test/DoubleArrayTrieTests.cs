using System;
using Xunit;

namespace Forest.Test
{
    public class DoubleArrayTrieTests_TheAddMethod
    {
        [Fact]
        public void ShouldThrowIfKeyIsNull()
        {
            // Arrange
            string key = null;
            var trie = new DoubleArrayTrie();

            // Assert
            Assert.Throws<ArgumentNullException>(() => trie.Add(key));
        }

        [Fact]
        public void ShouldAddKey()
        {
            // Arrange
            var key = "bachelor";
            var trie = new DoubleArrayTrie();

            // Act
            trie.Add(key);

            // Assert
            Assert.True(trie.ContainsKey(key));
        }

        [Fact]
        public void ShouldReturnFalseIfKeyIsAlreadyPresent()
        {
            // Arrange
            var key = "bachelor";
            var trie = new DoubleArrayTrie();

            trie.Add(key);

            // Act
            var result = trie.Add(key);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShouldAddAKeyWithoutCollisions()
        {
            // Arrange
            var key = "jar";
            var trie = new DoubleArrayTrie();

            trie.Add("bachelor");

            // Act
            trie.Add(key);

            // Assert
            Assert.True(trie.ContainsKey(key));
        }

        [Fact]
        public void ShouldLeaveOtherKeyUntouchedAfterAddedAKeyWithoutCollisions2()
        {
            // Arrange
            var key = "jar";
            var trie = new DoubleArrayTrie();

            trie.Add("bachelor");

            // Act
            trie.Add(key);

            // Assert
            Assert.True(trie.ContainsKey("bachelor"));
        }

        [Fact]
        public void ShouldAddAKeyWithCollisions1()
        {
            // Arrange
            var key = "badge";
            var trie = new DoubleArrayTrie();

            trie.Add("bachelor");
            trie.Add("jar");

            // Act
            trie.Add(key);

            // Assert
            Assert.True(trie.ContainsKey(key));
        }

        [Fact]
        public void ShouldAddAKeyWithCollisions2()
        {
            // Arrange
            var key = "baby";
            var trie = new DoubleArrayTrie();

            trie.Add("bachelor");
            trie.Add("jar");
            trie.Add("badge");

            // Act
            trie.Add(key);

            // Assert
            trie.ContainsKey(key);
        }
    }

    public class DoubleArrayTests_TheContainsKeyMethod
    {
        [Fact]
        public void ShouldThrowIfKeyIsNull()
        {
            // Arrange
            string key = null;
            var trie = new DoubleArrayTrie();

            // Assert
            Assert.Throws<ArgumentNullException>(() => trie.ContainsKey(key));
        }

        [Fact]
        public void ShouldReturnTrueIfKeyIsFound()
        {
            // Arrange
            var key = "bachelor";
            var trie = new DoubleArrayTrie();

            trie.Add(key);

            // Act
            var containsKey = trie.ContainsKey(key);

            // Assert
            Assert.True(containsKey);
        }
    }
}