using namespace std;
#include <iostream>
#include <unordered_set>
#include <functional>

//两分查找 字符串哈希 O(NlogN)
//两分查找O(logN) 尝试不同长度是否可行。返回最终结果。
//两分缘由：结果长度可行结果单调，如果短的可行，则可能有长的可行。
//查找函数中的 Rabin-Karp字符串哈希O(N)：对字符串求哈希值进行比较。
//以27以及ULLONG_MAX为进制计算的字符串的哈希值，若相同长度下的字符串哈希值相等则字符串相等。
//做法等于用了两套互质的进制作为哈希函数进行哈希获取哈希值，当两个哈希值一致则可判定字符串相等，
//由于power *= prime溢出则相当于一套unsigned long long max大小2^64次进制，跟手动取的prime=27。
class Solution {
public:
    string longestDupSubstring(string s) {
        int n = s.size();
        unsigned long long prime = 27;
        int l = 0, r = n - 1, m;
        unordered_set<unsigned long long> lst;
        string rst;
        function<bool(int)> check = [&](int len) {
            lst.clear();
            unsigned long long key = 0, power = 1;;
            for (int i = 0; i < len; i++) {
                key = key * prime + (s[i] - 'a');
                power *= prime;
            }
            lst.insert(key);
            for (int i = 1; i + len - 1 < n; i++) {
                key = key * prime + (s[i + len - 1] - 'a') - power * (s[i - 1] - 'a');
                if (lst.count(key)) {
                    rst = s.substr(i, len);
                    return true;
                }
                else {
                    lst.insert(key);
                }
            }
            return false;
        };
        while (l < r) {
            m = (l + r + 1) / 2;
            if (check(m)) {
                l = m;
            }
            else {
                r = m - 1;
            }
        }
        return rst;
    }
};