Advent of Code 2022
===================

Solutions to [Advent Of Code 2022](http://adventofcode.com/2022)

![](https://github.com/adamrodger/advent-2022/workflows/Build%20and%20Test/badge.svg)

Benchmarks
----------

``` ini
BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19045.2251)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
```

| Method | Day | Part |          Mean |      Error |     StdDev |
|------- |---- |----- |--------------:|-----------:|-----------:|
|  **Solve** |   **1** |    **1** |      **24.790 μs** |     **0.0600 μs** |     **0.0532 μs** |
|  **Solve** |   **1** |    **2** |      **28.957 μs** |     **0.0775 μs** |     **0.0725 μs** |
|  **Solve** |   **2** |    **1** |      **34.991 μs** |     **0.0693 μs** |     **0.0648 μs** |
|  **Solve** |   **2** |    **2** |      **32.597 μs** |     **0.0926 μs** |     **0.0867 μs** |
|  **Solve** |   **3** |    **1** |     **167.470 μs** |     **0.6133 μs** |     **0.5736 μs** |
|  **Solve** |   **3** |    **2** |     **178.204 μs** |     **0.5955 μs** |     **0.5570 μs** |
|  **Solve** |   **4** |    **1** |     **163.524 μs** |     **0.3694 μs** |     **0.3275 μs** |
|  **Solve** |   **4** |    **2** |     **164.259 μs** |     **0.9345 μs** |     **0.8284 μs** |
|  **Solve** |   **5** |    **1** |      **65.193 μs** |     **0.2348 μs** |     **0.2196 μs** |
|  **Solve** |   **5** |    **2** |      **94.967 μs** |     **0.4889 μs** |     **0.4334 μs** |
|  **Solve** |   **6** |    **1** |      **49.606 μs** |     **0.0584 μs** |     **0.0546 μs** |
|  **Solve** |   **6** |    **2** |      **98.153 μs** |     **0.4445 μs** |     **0.4158 μs** |
|  **Solve** |   **7** |    **1** |     **194.346 μs** |     **0.2886 μs** |     **0.2700 μs** |
|  **Solve** |   **7** |    **2** |     **211.033 μs** |     **1.1534 μs** |     **1.0225 μs** |
|  **Solve** |   **8** |    **1** |   **4,409.259 μs** |     **7.0085 μs** |     **6.5557 μs** |
|  **Solve** |   **8** |    **2** |   **5,304.342 μs** |    **12.5691 μs** |    **11.1422 μs** |
|  **Solve** |   **9** |    **1** |     **613.292 μs** |     **0.9218 μs** |     **0.8622 μs** |
|  **Solve** |   **9** |    **2** |   **1,493.396 μs** |     **2.2559 μs** |     **2.1102 μs** |
|  **Solve** |  **10** |    **1** |       **9.898 μs** |     **0.0578 μs** |     **0.0541 μs** |
|  **Solve** |  **10** |    **2** |      **11.820 μs** |     **0.2168 μs** |     **0.1921 μs** |
|  **Solve** |  **11** |    **1** |      **30.143 μs** |     **0.0329 μs** |     **0.0292 μs** |
|  **Solve** |  **11** |    **2** |  **12,876.770 μs** |    **16.5389 μs** |    **15.4705 μs** |
|  **Solve** |  **12** |    **1** |       **2.855 μs** |     **0.0230 μs** |     **0.0215 μs** |
|  **Solve** |  **12** |    **2** |      **14.682 μs** |     **0.0790 μs** |     **0.0739 μs** |
|  **Solve** |  **13** |    **1** |   **1,602.904 μs** |     **7.0873 μs** |     **6.6295 μs** |
|  **Solve** |  **13** |    **2** |  **25,893.207 μs** |   **100.2425 μs** |    **83.7070 μs** |
|  **Solve** |  **14** |    **1** |     **478.246 μs** |     **2.1639 μs** |     **1.9183 μs** |
|  **Solve** |  **14** |    **2** |     **557.607 μs** |     **2.0054 μs** |     **1.7777 μs** |
|  **Solve** |  **15** |    **1** |      **21.413 μs** |     **0.1299 μs** |     **0.1216 μs** |
|  **Solve** |  **15** |    **2** |     **303.529 μs** |     **1.1924 μs** |     **1.1154 μs** |
