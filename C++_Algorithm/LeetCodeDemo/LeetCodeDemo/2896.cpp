using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <numeric>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
#include "myAlgo\Structs\TreeNode.cpp"
typedef pair<int, bool> pib;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//动态规划
//选择反转i j， cost x；连续反转cost 1
//通过连续两两反转 j到i 实现选择反转，cost i-j;
//比较 x跟 i-j
//将不同的位置i保存到数组q中
//f[i]为索引0到i的最小操作代价和
//f[0]=0;
//f[i]=min(f[i-1]+x/2,f[i-2]+q[i]-q[i-1]);
class Solution {
#define MAXN 501
public:
    int minOperations(string s1, string s2, int x) {
        int n = s1.size();
        int q[MAXN]{};
        int m = 0;
        for (int i = 0; i < n; i++) {
            if (s1[i] != s2[i]) {
                q[m++] = i;
            }
        }
        if (m % 2)return -1;
        if (!m)return 0;
        int  rst = 0;
        int f[MAXN]{};
        f[0] = x;
        f[1] = min(2 * (q[1] - q[0]), 2 * x);
        for (int i = 2; i < m; i++) {
            f[i] = min(f[i - 1] + x, f[i - 2] + 2 * (q[i] - q[i - 1]));
        }
        return f[m - 1] / 2;
    }
};