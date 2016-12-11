using System;
using Xunit;

namespace Forest.Test
{
    public class DoubleArrayTrieTests
    {
        public class TheAddMethod
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
                trie.ContainsKey(key);
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
                trie.ContainsKey(key);
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
                trie.ContainsKey(key);
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

        public class TheContainsKeyMethod
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
        }
    }
}