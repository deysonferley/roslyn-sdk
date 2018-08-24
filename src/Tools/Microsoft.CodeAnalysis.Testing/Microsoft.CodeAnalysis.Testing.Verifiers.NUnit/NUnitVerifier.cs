﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Microsoft.CodeAnalysis.Testing.Verifiers
{
    public class NUnitVerifier : IVerifier
    {
        public void Empty<T>(string collectionName, IEnumerable<T> collection)
        {
            Assert.IsEmpty(collection, $"Expected '{collectionName}' to be empty, contains '{collection?.Count()}' elements");
        }

        public void Equal<T>(T expected, T actual, string message = null)
        {
            if (message is null)
            {
                Assert.AreEqual(expected, actual);
            }
            else
            {
                Assert.AreEqual(expected, actual, message);
            }
        }

        public void False(bool assert, string message = null)
        {
            if (message is null)
            {
                Assert.IsFalse(assert);
            }
            else
            {
                Assert.IsFalse(assert, message);
            }
        }

        public void LanguageIsSupported(string language)
        {
            Assert.IsFalse(language != LanguageNames.CSharp && language != LanguageNames.VisualBasic, $"Unsupported Language: '{language}'");
        }

        public void NotEmpty<T>(string collectionName, IEnumerable<T> collection)
        {
            Assert.IsNotEmpty(collection, $"expected '{collectionName}' to be non-empty, contains");
        }

        public void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message = null)
        {
            var comparer = new SequenceEqualEnumerableEqualityComparer<T>();
            var areEqual = comparer.Equals(expected, actual);
            if (!areEqual)
            {
                Assert.Fail(message);
            }
        }

        public void True(bool assert, string message = null)
        {
            if (message is null)
            {
                Assert.IsTrue(assert);
            }
            else
            {
                Assert.IsTrue(assert, message);
            }
        }

        private sealed class SequenceEqualEnumerableEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
        {
            private readonly IEqualityComparer<T> _itemEqualityComparer;

            public SequenceEqualEnumerableEqualityComparer()
                : this(EqualityComparer<T>.Default)
            {
            }

            public SequenceEqualEnumerableEqualityComparer(IEqualityComparer<T> itemEqualityComparer)
            {
                _itemEqualityComparer = itemEqualityComparer;
            }

            public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
            {
                if (ReferenceEquals(x, y)) { return true; }
                if (x is null || y is null) { return false; }

                return x.SequenceEqual(y, _itemEqualityComparer);
            }

            public int GetHashCode(IEnumerable<T> obj)
            {
                // From System.Tuple
                return obj
                    .Select(item => _itemEqualityComparer.GetHashCode(item))
                    .Aggregate(
                        0,
                        (aggHash, nextHash) => ((aggHash << 5) + aggHash) ^ nextHash);
            }
        }
    }
}
