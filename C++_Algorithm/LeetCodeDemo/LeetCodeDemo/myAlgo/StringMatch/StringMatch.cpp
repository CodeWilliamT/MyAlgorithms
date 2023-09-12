
using namespace std;
#include <iostream>
#include <vector>

		class StringMatch {
		public:
			//Rabin-Karp字符串哈希O(N)：对字符串求哈希值进行比较。
			//以27以及ULLONG_MAX为进制计算的字符串的哈希值，若相同长度下的字符串哈希值相等则字符串相等。
			//做法等于用了两套互质的进制作为哈希函数进行哈希获取哈希值，当两个哈希值一致则可判定字符串相等，
			int RK(string s1, string s2) {
				unsigned long long prime  = 31;
				unsigned long long key  = 0,targetkey = 0, power  = 1;
				int n  = s1.size();
				int len  = s2.size();
				for (int i  = 0; i  < len; i++) {
					targetkey  = targetkey  * prime  + (s2[i] - 'a');
				}
				for (int i  = 0; i  < len; i++) {
					key  = key  * prime  + (s1[i] - 'a');
					power  *= prime;
				}
				if (key  == targetkey)return 0;
				for (int i  = 1; i  + len  - 1 < n; i++) {
					key  = key  * prime  + (s1[i  + len  - 1] - 'a') - power  * (s1[i  - 1] - 'a');
					if (key  == targetkey) {
						return i;
					}
				}
				return  - 1;
			}
			//KMP算法(双指针)O(M+N)
			//预处理匹配串获得前缀函数,前缀函数用作s1的i位置匹配s2 j位置匹配失败后转移向f[j-1];
			//f[i]存储 匹配串自匹配 得到后缀i位置匹配的前缀字符串的结尾字符的下标+1。用作主串匹配失败后的位置递推。
			int KMP(string s1, string s2) {
				if (!s2.size())return 0;
				int n = s1.size();
				int m = s2.size();
				vector<int> f(m, 0);
				//对匹配串生成前缀表
				for (int i = 1, j = 0; i < m; i++) {
					//找到一个匹配s2[i]的前缀字符s2[j]
					//如果后缀字符s2[i]跟前缀字符s2[j]不匹配，则看看j的前一个字符j-1指向的前缀字符s2[f[j-1]]与s2[i]是否匹配，若无匹配字符则j=0。
					while (j > 0 && s2[i] != s2[j])j = f[j - 1];
					if (s2[i] == s2[j])j++;
					f[i] = j;
				}
				for (int i = 0, j = 0; i < n; i++) {
					//找到一个匹配s1[i]的前缀字符s2[j]
					//如果后缀字符s1[i]跟前缀字符s2[j]不匹配，则看看j的前一个字符j-1指向的前缀字符s2[f[j-1]]与s1[i]是否匹配,若无匹配字符则j=0。
					while (j > 0 && s1[i] != s2[j])j = f[j - 1];
					if (s1[i] == s2[j])j++;
					if (j == m)return i - m + 1;
				}
				return  -1;
			}
			//O(M+N)
			//用string的strStr函数，相当于kmp
			int strStr(string s1, string s2) {
				const char* rst = strstr(s1.c_str(), s2.c_str());
				return !rst ? -1 : rst - s1.c_str();
			}
			//O(M+N)
			//用string的find函数，比kmp慢点
			int strStr(string s1, string s2) {
				return s1.find(s2);
			}
	};
	//简单题 朴素实现O((N-M)*M)
	//按位遍历父串元素，每次按位比较模板串
	//class Solution {
	//public:
	//    int strStr(string s1, string s2) {
	//        if (!s2.size())return 0;
	//        int n=s1.size();
	//        int m = s2.size();
	//        for (int i = 0; i < n-m+1; i++)
	//        {
	//            int j = 0;
	//            for (; j < m; j++)
	//            {
	//                if (s1[i+j] != s2[j])break;
	//            }
	//            if (j == m)return i;
	//        }
	//        return -1;
	//    }
	//};