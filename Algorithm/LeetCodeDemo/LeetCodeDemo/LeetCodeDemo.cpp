// LeetCode.cpp : This file contains the 'main' function. Program execution begins and ends there.
//


//时间复杂度O(DigitN(3n+Radix))=O(DigitN(n+Radix))
//空间复杂度O(2n+Radix)=O(n+Radix)
using namespace std;
#include <iostream>

#define Radix 8		//位值范围数,一个位对应多少个值
#define RadixF 8.0	//位值范围数的浮点型
#define DigitN 2		//位数

//按位的计数排序
//乱序数组，临时缓存排序组，数组大小，位数
void countSort(int* a, int* temp, int n, int byte)
{
	int i, c[Radix] = { 0 }, digit, maxIndex;
	int weight = pow(RadixF, byte);
	memset(c, 0, sizeof(c));
	//数组c记录数组a在byte位上的各个值的数目
	for (i = 0; i < n; i++)
	{
		digit = a[i] / weight % Radix;
		c[digit]++;
	}
	//数组c改为记录数组a在byte位上的从小到大至各个值的总数
	//即记录数组a在byte位上的从小到大至各个值的最大序号+1
	for (i = 1; i < Radix; i++)
	{
		c[i] = c[i - 1] + c[i];
	}
	//因为利用最大序号所以需倒序
	for (i = n - 1; i > -1; i--)
	{
		//排序组根据记录的序号赋值；序号--；
		digit = a[i] / (int)weight % Radix;
		//最大序号
		maxIndex = c[digit] - 1;
		temp[maxIndex] = a[i];
		c[digit]--;
	}
	for (i = 0; i < n; i++)
	{
		a[i] = temp[i];
	}
}

//乱序数组，临时缓存排序组，数组大小，位数
void radixSort(int* a, int* temp, int n, int size = DigitN)
{
	int i;
	//按位做计数排序
	for (i = 0; i < size; i++)
	{
		countSort(a, temp, n, i);
	}
}

int main()
{
	int a[10] = { 9,8,7,6,5,4,3,2,1,0 }, temp[11], n = 10;
	memset(temp, 0, sizeof(temp));
	radixSort(a, temp, n, DigitN);
	for (int i = 0; i < n; i++)
	{
		cout << a[i] << endl;
	}
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
