using System;
using Xunit;

namespace Forest.Test
{
    public abstract class DoubleArrayTrieTests
    {
        public class TheAddMethod : DoubleArrayTrieTests
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

        public class TheContainsKeyMethod : DoubleArrayTrieTests
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

                var @base = new [] { 0, 4, 0, 1, -15, -1, -12, 1, 0, 0, 0, 0, 0, 0, 0, -9 };
                var check = new [] { 0, 0, 0, 7,   3,  3,   3, 1, 0, 0, 0, 0, 0, 0, 0,  1 };
                var tail = "0helor#??ar#ge#y#".ToCharArray();

                trie.SetArrays(@base, check, tail);

                // Assert
                Assert.True(trie.ContainsKey(key));
            }
        }
    }
}