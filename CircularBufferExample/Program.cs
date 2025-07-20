using System;
using CircularBuffer;

namespace CircularBufferExample
{
    static class MainClass
    {
        public static void Main(string[] args)
        {
            var buffer = new CircularBuffer<int>(5);

            // Fill the buffer
            for (int i = 0; i < 5; i++)
            {
                buffer.PushBack(i);
            }

            Console.WriteLine("Initial buffer:");
            for (int i = 0; i < buffer.Size; i++)
            {
                Console.WriteLine(buffer[i]);
            }

            Console.WriteLine("---");

            // Add more elements (will overflow and replace oldest)
            for (int i = 0; i < 3; i++)
            {
                buffer.PushBack(10 + i);
            }
            
            Console.WriteLine("After overflow:");
            for (int i = 0; i < buffer.Size; i++)
            {
                Console.WriteLine(buffer[i]);
            }

            Console.WriteLine("---");

            // Demonstrate new functionality
            Console.WriteLine("Demonstrating new features:");
            
            // TryPeek methods
            if (buffer.TryPeekFront(out int front))
            {
                Console.WriteLine($"Front element (safe): {front}");
            }

            if (buffer.TryPeekBack(out int back))
            {
                Console.WriteLine($"Back element (safe): {back}");
            }

            // Contains method
            Console.WriteLine($"Contains 11: {buffer.Contains(11)}");
            Console.WriteLine($"Contains 99: {buffer.Contains(99)}");

            // CopyTo method
            int[] array = new int[buffer.Size];
            buffer.CopyTo(array, 0);
            Console.WriteLine($"Copied to array: [{string.Join(", ", array)}]");

            // Clear method
            buffer.Clear();
            Console.WriteLine($"After clear - Size: {buffer.Size}, IsEmpty: {buffer.IsEmpty}");
        }
    }
}
