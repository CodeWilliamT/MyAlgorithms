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
//两分
//判断访问时间在多少个start以及之后，多少个end后。rst=count(start)-count(end);
class Solution {
public:
    vector<int> fullBloomFlowers(vector<vector<int>>& flowers, vector<int>& persons) {
        int m = flowers.size();
        vector<int> starts(m),ends(m);
        for (int i = 0; i < m; i++) {
            starts[i] = flowers[i][0];
            ends[i] = flowers[i][1];
        }
        sort(starts.begin(), starts.end());
        sort(ends.begin(), ends.end());
        int n = persons.size();
        vector<int> rst(n);
        int cnts, cnte;
        for (int i = 0; i < n; i++) {
            cnts = upper_bound(starts.begin(), starts.end(), persons[i])-starts.begin();
            cnte = lower_bound(ends.begin(), ends.end(), persons[i]) - ends.begin();
            rst[i] = cnts - cnte;
        }
        return rst;
    }
};