#pragma once

#define DLL_API __declspec(dllexport)

extern "C" 
{
	DLL_API int Calculate(int a, int b);
	DLL_API int Multiply(int a, int b);
}