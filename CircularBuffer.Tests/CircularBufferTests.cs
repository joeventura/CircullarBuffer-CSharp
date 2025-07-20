using NUnit.Framework;
using System;
using System.Linq;

namespace CircularBuffer.Tests
{
    [TestFixture]
    public class CircularBufferTests
    {
        [Test]
        public void CircularBuffer_ConstructorSizeIndexAccess_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            Assert.That(buffer.Capacity, Is.EqualTo(5));
            Assert.That(buffer.Size, Is.EqualTo(4));
            for (int i = 0; i < 4; i++)
            {
                Assert.That(buffer[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void CircularBuffer_Constructor_ExceptionWhenSourceIsLargerThanCapacity()
        {
            Assert.That(() => new CircularBuffer<int>(3, new[] { 0, 1, 2, 3 }),
                        Throws.Exception.TypeOf<ArgumentException>());
        }

        [Test]
        public void CircularBuffer_GetEnumeratorConstructorDefinedArray_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            int x = 0;
            foreach (var item in buffer)
            {
                Assert.That(item, Is.EqualTo(x));
                x++;
            }
        }

        [Test]
        public void CircularBuffer_PushBack_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 5; i++)
            {
                buffer.PushBack(i);
            }

            Assert.That(buffer.Front(), Is.EqualTo(0));
            for (int i = 0; i < 5; i++)
            {
                Assert.That(buffer[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void CircularBuffer_PushBackOverflowingBuffer_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 5, 6, 7, 8, 9 }));
        }

        [Test]
        public void CircularBuffer_GetEnumeratorOverflowedArray_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            // buffer should have [5,6,7,8,9]
            int x = 5;
            foreach (var item in buffer)
            {
                Assert.That(item, Is.EqualTo(x));
                x++;
            }
        }

        [Test]
        public void CircularBuffer_ToArrayConstructorDefinedArray_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3 });

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void CircularBuffer_ToArrayOverflowedBuffer_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushBack(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 5, 6, 7, 8, 9 }));
        }

        [Test]
        public void CircularBuffer_PushFront_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 5; i++)
            {
                buffer.PushFront(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 4, 3, 2, 1, 0 }));
        }

        [Test]
        public void CircularBuffer_PushFrontAndOverflow_CorrectContent()
        {
            var buffer = new CircularBuffer<int>(5);

            for (int i = 0; i < 10; i++)
            {
                buffer.PushFront(i);
            }

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 9, 8, 7, 6, 5 }));
        }

        [Test]
        public void CircularBuffer_Front_CorrectItem()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            Assert.That(buffer.Front(), Is.EqualTo(0));
        }

        [Test]
        public void CircularBuffer_Back_CorrectItem()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            Assert.That(buffer.Back(), Is.EqualTo(4));
        }

        [Test]
        public void CircularBuffer_BackOfBufferOverflowByOne_CorrectItem()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            buffer.PushBack(42);
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4, 42 }));
            Assert.That(buffer.Back(), Is.EqualTo(42));
        }

        [Test]
        public void CircularBuffer_Front_EmptyBufferThrowsException()
        {
            var buffer = new CircularBuffer<int>(5);

            Assert.That(() => buffer.Front(),
                         Throws.Exception.TypeOf<InvalidOperationException>().
                         With.Property("Message").Contains("empty buffer"));
        }

        [Test]
        public void CircularBuffer_Back_EmptyBufferThrowsException()
        {
            var buffer = new CircularBuffer<int>(5);
            Assert.That(() => buffer.Back(),
                         Throws.Exception.TypeOf<InvalidOperationException>().
                         With.Property("Message").Contains("empty buffer"));
        }

        [Test]
        public void CircularBuffer_PopBack_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            Assert.That(buffer.Size, Is.EqualTo(5));

            buffer.PopBack();

            Assert.That(buffer.Size, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void CircularBuffer_PopBackInOverflowBuffer_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            buffer.PushBack(5);

            Assert.That(buffer.Size, Is.EqualTo(5));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));

            buffer.PopBack();

            Assert.That(buffer.Size, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void CircularBuffer_PopFront_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            Assert.That(buffer.Size, Is.EqualTo(5));

            buffer.PopFront();

            Assert.That(buffer.Size, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void CircularBuffer_PopFrontInOverflowBuffer_RemovesBackElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
            buffer.PushFront(5);

            Assert.That(buffer.Size, Is.EqualTo(5));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 5, 0, 1, 2, 3 }));

            buffer.PopFront();

            Assert.That(buffer.Size, Is.EqualTo(4));
            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
        }

        [Test]
        public void CircularBuffer_SetIndex_ReplacesElement()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            buffer[1] = 10;
            buffer[3] = 30;

            Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 10, 2, 30, 4 }));
        }

        [Test]
        public void CircularBuffer_WithDifferentSizeAndCapacity_BackReturnsLastArrayPosition()
        {
            // test to confirm this issue does not happen anymore:
            // https://github.com/joaoportela/CircullarBuffer-CSharp/issues/2

            var buffer = new CircularBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

            buffer.PopFront(); // (make size and capacity different)

            Assert.That(buffer.Back(), Is.EqualTo(4));
        }

        [Test]
        public void CircularBuffer_TryPeekFront_EmptyBuffer_ReturnsFalse()
        {
            var buffer = new CircularBuffer<int>(5);
            
            bool result = buffer.TryPeekFront(out int item);
            
            Assert.That(result, Is.False);
            Assert.That(item, Is.EqualTo(default(int)));
        }

        [Test]
        public void CircularBuffer_TryPeekFront_NonEmptyBuffer_ReturnsTrue()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 1, 2, 3 });
            
            bool result = buffer.TryPeekFront(out int item);
            
            Assert.That(result, Is.True);
            Assert.That(item, Is.EqualTo(1));
        }

        [Test]
        public void CircularBuffer_TryPeekBack_EmptyBuffer_ReturnsFalse()
        {
            var buffer = new CircularBuffer<int>(5);
            
            bool result = buffer.TryPeekBack(out int item);
            
            Assert.That(result, Is.False);
            Assert.That(item, Is.EqualTo(default(int)));
        }

        [Test]
        public void CircularBuffer_TryPeekBack_NonEmptyBuffer_ReturnsTrue()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 1, 2, 3 });
            
            bool result = buffer.TryPeekBack(out int item);
            
            Assert.That(result, Is.True);
            Assert.That(item, Is.EqualTo(3));
        }

        [Test]
        public void CircularBuffer_Clear_EmptiesBuffer()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 1, 2, 3, 4, 5 });
            
            Assert.That(buffer.Size, Is.EqualTo(5));
            Assert.That(buffer.IsEmpty, Is.False);
            
            buffer.Clear();
            
            Assert.That(buffer.Size, Is.EqualTo(0));
            Assert.That(buffer.IsEmpty, Is.True);
            Assert.That(buffer.Capacity, Is.EqualTo(5));
        }

        [Test]
        public void CircularBuffer_Contains_ExistingItem_ReturnsTrue()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 1, 2, 3, 4, 5 });
            
            Assert.That(buffer.Contains(3), Is.True);
            Assert.That(buffer.Contains(1), Is.True);
            Assert.That(buffer.Contains(5), Is.True);
        }

        [Test]
        public void CircularBuffer_Contains_NonExistingItem_ReturnsFalse()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 1, 2, 3, 4, 5 });
            
            Assert.That(buffer.Contains(6), Is.False);
            Assert.That(buffer.Contains(0), Is.False);
        }

        [Test]
        public void CircularBuffer_Contains_EmptyBuffer_ReturnsFalse()
        {
            var buffer = new CircularBuffer<int>(5);
            
            Assert.That(buffer.Contains(1), Is.False);
        }

        [Test]
        public void CircularBuffer_CopyTo_ValidArray_CopiesElements()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 1, 2, 3, 4 });
            var array = new int[10];
            
            buffer.CopyTo(array, 2);
            
            Assert.That(array[2], Is.EqualTo(1));
            Assert.That(array[3], Is.EqualTo(2));
            Assert.That(array[4], Is.EqualTo(3));
            Assert.That(array[5], Is.EqualTo(4));
        }

        [Test]
        public void CircularBuffer_CopyTo_NullArray_ThrowsException()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 1, 2, 3 });
            
            Assert.That(() => buffer.CopyTo(null, 0),
                        Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void CircularBuffer_CopyTo_InvalidIndex_ThrowsException()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 1, 2, 3 });
            var array = new int[5];
            
            Assert.That(() => buffer.CopyTo(array, -1),
                        Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CircularBuffer_CopyTo_InsufficientSpace_ThrowsException()
        {
            var buffer = new CircularBuffer<int>(5, new[] { 1, 2, 3, 4 });
            var array = new int[5];
            
            Assert.That(() => buffer.CopyTo(array, 2),
                        Throws.Exception.TypeOf<ArgumentException>());
        }
    }
}
