using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>
//两分查找,哈希表
//坑：哈希表遍历时效低，对哈希表做两分要用自带的bound
class Solution {
public:
    vector<int> maximumBeauty(vector<vector<int>>& items, vector<int>& queries) {
        sort(items.begin(), items.end());
        unordered_map<int, int> mp;
        int last=0;
        for (auto& e : items)
        {
            mp[e[0]] = max(last, e[1]);
            last = mp[e[0]];
        }
        vector<int> ans;
        for (auto& q : queries)
        {
            if (q < items[0][0])
            {
                ans.push_back(0);
                continue;
            }
            int index = upper_bound(items.begin(), items.end(), vector<int>{q, INT_MAX}) - items.begin();
            ans.push_back(mp[items[index - 1][0]]);
        }
        return ans;
    }
};