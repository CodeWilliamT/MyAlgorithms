using namespace std;
#include <iostream>
#include <vector>
//简单模拟 哈希
//遍历数组，记录被入点+=出点编号，返回最大的被入点编号。
//坑：相等时，取编号最小的;和的范围大于int32。
typedef long long ll;
class Solution {
public:
    int edgeScore(vector<int>& edges) {
        int n = edges.size();
        ll cnts[100001]{};
        int rst=0, maxn=0;
        for (int i = 0; i < edges.size(); i++) {
            cnts[edges[i]] += i;
            if (cnts[edges[i]] > maxn|| cnts[edges[i]] == maxn&& edges[i] < rst) {
                rst = edges[i];
                maxn = cnts[edges[i]];
            }
        }
        return rst;
    }
};