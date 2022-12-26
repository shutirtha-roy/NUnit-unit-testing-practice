using NUnit.Framework;
using Shouldly;
using System;
using UnitLibrary.ClassesWithNoDependencies;

namespace Unit.NonDependency.Tests
{
    class StackTests
    {
        private Stack<int> _stack;

        [SetUp]
        public void Setup()
        {
            _stack = new Stack<int>();
            _stack.Push(1);
            _stack.Push(2);
            _stack.Push(3);
        }

        [Test]
        public void Push_WhenObjectIsAdded_CountIncreasesByOne()
        {
            var expectedCount = _stack.Count + 1;

            _stack.Push(1);

            var result = _stack.Count;

            Assert.AreEqual(expectedCount, result);
        }

        [Test]
        public void Pop_WhenCalled_ReturnValueAndCountDecresesByOne()
        {
            var expectedCount = _stack.Count - 1;

            var result = _stack.Pop();

            var resultCount = _stack.Count;

            Assert.AreNotEqual(null, result);
            Assert.AreEqual(expectedCount, resultCount);
        }

        [Test]
        public void Peek_WhenCalled_ReturnValueAndSameCount()
        {
            var expectedCount = _stack.Count;

            var result = _stack.Peek();

            var resultCount = _stack.Count;

            Assert.AreNotEqual(null, result);
            Assert.AreEqual(expectedCount, resultCount);
        }

        [Test]
        public void Pop_WhenCalled_ReturnValueAtTheTopOfStack()
        {
            Stack<int> stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);

            var expected = 2;

            var result = stack.Pop();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Peek_WhenCalled_ReturnValueAtTheTopOfStack()
        {
            Stack<int> stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);

            var expected = 2;

            var result = stack.Peek();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Peek_WhenStackIsEmpty_ThrowException()
        {
            Stack<int> stack = new Stack<int>();

            Should.Throw<InvalidOperationException>(() =>
                stack.Pop()
            );
        }
    }
}
