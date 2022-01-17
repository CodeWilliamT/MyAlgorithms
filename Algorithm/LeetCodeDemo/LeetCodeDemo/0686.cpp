using namespace std;
#include <iostream>
#include <vector>
#include<string>
//字符串匹配
//用轮子s.find
//class Solution {
//public:
//    int repeatedStringMatch(string a, string b) {
//        string s = a;
//        int rst = 1;
//        while (s.size() < a.size()+b.size()) {
//            s += a;
//            rst++;
//        }
//        int idx = s.find(b);
//        if (idx == -1)return idx;
//        return (b.size() + idx - 1) /a.size()+1;
//    }
//};
//KMP
//造轮子 写KMP
//如果a中不包含b的所有字符
//对b做自匹配得前驱索引数组。
//对a做匹配找答案。
class Solution {
public:
    int repeatedStringMatch(string a, string b) {
        bool letter[26]{};
        int n = b.size();
        vector<int> f(n+1, 0);
        for (int i = 0,j=1; i < n;i++) {
            while(b[i] != b[j]&&j>0) {
                j = f[j-1];
            }
            letter[b[i] - 'a'] = 1;
            f[i+1] = j;
            if (b[i] == b[j])j++;
        }
        for (char c : a)
            letter[c - 'a'] = 0;
        for (bool& e : letter)
            if (e)
                return -1;
        int m = a.size();
        for (int i = 0,j=0;i<n+m; i++)
        {
            while (a[i%m] != b[j] && j > 0) {
                j = f[j - 1];
            }
            if (j == n - 1)
                return i/ m+1;
            if (a[i % m] == b[j])j++;
        }
        return -1;
    }
};