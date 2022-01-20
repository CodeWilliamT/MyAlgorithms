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
//找规律
class Solution {
public:
    bool stoneGameIX(vector<int>& stones) {
        vector<bool> v(stones.size());
        for (int& e : stones) {
            e = e % 3?e % 3:3;
        }
        map<pair<short, int>, bool> mp;
        //1alice赢，0bob赢
        function<bool(int, int)> dfs = [&](int steps, int left) {
            if (mp.count({steps,left}))return (bool)mp[{steps, left}];
            bool turn = steps % 2;
            //当前玩家要拿的时候已经可判了，则当前玩家赢
            if (left != 0 && left % 3 == 0) {
                mp[{steps, left}] = !turn;
                return !turn;
            }
            if (steps == stones.size()) {
                mp[{steps, left}] = 0;
                return false;
            }
            bool rst = turn;
            bool nextrst;
            for (int i = 0; i < stones.size(); i++) {
                if (v[i])continue;
                v[i] = 1;
                //后续能赢，就赢了
                nextrst = dfs(steps + 1, left + stones[i]);
                v[i] = 0;
                if (nextrst == !turn) {
                    rst=!turn;
                    break;
                }
            }
            mp[{steps, left}] = rst;
            return rst;
        };
        return dfs(0, 0);
    }
};