using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 7
    /// </summary>
    public class Day7
    {
        public int Part1(string[] input)
        {
            Dictionary<string, Directory> filesystem = BuildFilesystem(input);

            return filesystem.Values.Select(dir => dir.TotalSize()).Where(size => size <= 100_000).Sum();
        }

        public int Part2(string[] input)
        {
            Dictionary<string, Directory> filesystem = BuildFilesystem(input);

            int currentSize = filesystem["/"].TotalSize();
            int freeSpace = 70000000 - currentSize;
            int required = 30000000 - freeSpace;

            return filesystem.Values.Select(dir => dir.TotalSize()).Where(size => size >= required).Min();
        }

        private static Dictionary<string, Directory> BuildFilesystem(string[] input)
        {
            var filesystem = new Dictionary<string, Directory>();
            var stack = new Stack<Directory>();
            stack.Push(new Directory { Path = "/" });
            filesystem["/"] = stack.Peek();

            foreach (string line in input.Skip(1))
            {
                if (line.StartsWith("$ cd .."))
                {
                    stack.Pop();
                }
                else if (line.StartsWith("$ cd"))
                {
                    string name = line[5..];
                    string path = stack.Peek().Path + "/" + name;
                    stack.Push(filesystem[path]);
                }
                else if (line.StartsWith("$ ls"))
                {
                    continue;
                }
                else if (line.StartsWith("dir"))
                {
                    var current = stack.Peek();
                    var sub = current.AddSubdirectory(line[4..]);
                    filesystem[sub.Path] = sub;
                }
                else // file
                {
                    string[] parts = line.Split(' ');
                    int size = int.Parse(parts[0]);

                    var current = stack.Peek();
                    current.AddFile( size);
                }
            }

            return filesystem;
        }

        public class Directory
        {
            private readonly ICollection<Directory> subDirectories = new List<Directory>();
            private int fileSizeTotal;

            public string Path { get; init; }

            public Directory AddSubdirectory(string name)
            {
                var sub = new Directory { Path = this.Path + "/" + name };
                this.subDirectories.Add(sub);
                return sub;
            }

            public void AddFile(int size)
            {
                fileSizeTotal += size;
            }

            public int TotalSize()
            {
                return this.subDirectories.Select(s => s.TotalSize()).Sum() + this.fileSizeTotal;
            }
        }
    }
}
