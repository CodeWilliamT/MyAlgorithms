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
//哈希
//遍历一次，记录每个攻击下的最大防御
//然后倒序遍历比较就能记录下，大于某个攻击的最大防御了
//然后再遍历一次，比较当前攻击与大于当前攻击的最大防御。
class Solution {
public:
    int numberOfWeakCharacters(vector<vector<int>>& properties) {
        int v[100002]{},mx=0;
        for (auto& e : properties) {
            v[e[0]] = max(v[e[0]], e[1]);
            mx = max(e[0], mx);
        }
        for (int i = mx-1; i > 0; i--) {
            v[i] = max(v[i], v[i + 1]);
        }
        int rst = 0;
        int tmp;
        for (auto& e : properties) {
            tmp =v[e[0]+1];
            if (e[1] < tmp) {
                rst++;
            }
        }
        return rst;
    }
};