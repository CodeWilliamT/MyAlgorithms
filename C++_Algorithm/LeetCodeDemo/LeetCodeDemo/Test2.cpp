using namespace std;
#include <iostream>
#include <vector>
#include <string>
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
//哈希 枚举
//枚举最高塔高度
class Solution {
public:
    long long maximumSumOfHeights(vector<int>& mh) {
        typedef long long ll;
        int n = mh.size();
        ll rst = 0,tmp=0;
        int x;
        for (int i = 0; i < n;i++) {
            tmp = mh[i];
            x = mh[i];
            for (int j = i + 1; j < n; j++) {
                x= min(x, mh[j]);
                tmp += x;
            }
            x = mh[i];
            for (int j = i-1; j>-1; j--) {
                x = min(x, mh[j]);
                tmp += x;
            }
            rst = max(tmp, rst);
        }
        return rst;
    }
};