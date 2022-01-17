using namespace std;
#include <iostream>
//巧思 找规律 观察递推 分治
//一个a结尾的可能性由上一个字符e结尾+i结尾+o结尾的可能性之和。其他类推。
class Solution {
public:
    int countVowelPermutation(int n) {
        long long lst[5] = { 1,1,1,1,1 };
        long long nxt[5] = { 1,1,1,1,1 };
        const int MOD = 1000000007;
        n--;
        while (n--) {
            nxt[0] =(lst[1] + lst[2]+lst[4])%MOD;
            nxt[1] = (lst[0] + lst[2]) % MOD;
            nxt[2] = (lst[1]+ lst[3]) % MOD;
            nxt[3] = lst[2] % MOD;
            nxt[4] = (lst[2] + lst[3]) % MOD;
            copy(nxt, nxt + 5, lst);
        }
        int rst = (lst[0] + lst[1]+ lst[2] + lst[3]+ lst[4]) % MOD;
        return rst;
    }
};
//失败惯性暴力回溯思路
//class Solution {
//public:
//    int countVowelPermutation(int n) {
//        vector<char> lst = { 'a','e','i','o','u' };
//        unordered_map<char, vector<char>> mp = { {'a',{'e'}},{'e',{'a','i'}},{'i',{'a','e','o','u'}},{'o',{'i','u'}},{'u',{'a'}} };
//        int rst = 0;
//        function<void(char, int)> dfs = [&](char c, int len) {
//            if (len == n) {
//                rst++;
//                return;
//            }
//            for (char& e : mp[c]) {
//                dfs(e, len + 1);
//            }
//        };
//        for (char& e : lst) {
//            dfs(e, 1);
//        }
//        return rst;
//    }
//};