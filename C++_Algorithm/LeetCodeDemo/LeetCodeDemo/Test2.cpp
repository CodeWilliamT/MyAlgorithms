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
//哈希 麻烦模拟 细致条件分析 
class Solution {
public:
    int minimumCardPickup(vector<int>& cards) {
        int n = cards.size();
        unordered_map<int, pair<int,int>>mp;
        int rst = -1;
        for (int i = 0; i < n; i++) {
            if (!mp.count(cards[i])) {
                mp[cards[i]] = {i,-1};
            }
            else if(mp.count(cards[i])) {
                if (mp[cards[i]].second == -1) {
                    mp[cards[i]].second = i;
                    if (rst == -1) {
                        rst = mp[cards[i]].second - mp[cards[i]].first + 1;
                    }
                    else {
                        rst = min(mp[cards[i]].second - mp[cards[i]].first + 1, rst);
                    }
                }
                else{
                    if (i - mp[cards[i]].second + 1 < mp[cards[i]].second - mp[cards[i]].first + 1) {
                        mp[cards[i]].first = mp[cards[i]].second;
                        mp[cards[i]].second = i;
                        if (rst == -1) {
                            rst = mp[cards[i]].second - mp[cards[i]].first + 1;
                        }
                        else {
                            rst = min(mp[cards[i]].second - mp[cards[i]].first + 1, rst);
                        }
                    }
                }
            }
        }
        return rst;
    }
};