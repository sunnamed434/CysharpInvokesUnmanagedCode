# Executing unamaged functions in managed code

'Math.h' C++

To avoid of containing in your methods name mangling (or '?Calculate@@YAHHH@Z') don`t forget to add the ``` extern "C" { code... }``` to your header.
```c++
extern "C" 
{
    DLL_API int Calculate(int a, int b);
    DLL_API int Multiply(int a, int b);
}
```

'Math.cpp' C++
```c++
DLL_API int Calculate(int a, int b)
{
    return a + b;
}
DLL_API int Multiply(int a, int b)
{
    return a * b;
}
```

C#
```c#
var calculate = FunctionDelegateAccessor.GetMethod<Calculate>(handle, "Calculate");
Console.WriteLine("10 + 10 = " + calculate(10, 10));

var multiply = FunctionDelegateAccessor.GetMethod<Multiply>(handle, "Multiply");
Console.WriteLine("10 * 10 = " + multiply(10, 10));
```

Result after executing upper code:

```
10 + 10 = 20 
10 * 10 = 100
```
