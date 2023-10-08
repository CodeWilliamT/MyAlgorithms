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
//f[i]为0到i的最小操作代价和,i为偶数则为不统计第一个元素后凑对的最小操作代价和。
//f[0]=0,f[1]=min(q[1]-q[0],x)，f[2]=min(q[2]-q[1],x);
//f[i]=i%2?-1:
class Solution {
#define MAXN 501
public:
    int minOperations(string s1, string s2, int x) {
        int n = s1.size();
        int q[MAXN]{};
        int m=0;
        for (int i = 0; i < n; i++) {
            if (s1[i] != s2[i]) {
                diff++;
                q[m++] = i;
            }
        }
        if (m % 2)return -1;
        int  rst=0;
        int f[MAXN]{};
        f[1] = min(q[1]-q[0],x); 
        f[2] = min(q[2] - q[1], x);
        for (int i = 3; i < m; i++) {
            if (i % 2) {
            }
            else {
                f[i]=f[i-1]+f
            }
        }
        return rst;
    }
};