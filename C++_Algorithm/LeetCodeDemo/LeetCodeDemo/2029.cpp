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
class Solution {
public:
    bool stoneGameIX(vector<int>& stones) {
        swap(stones[0], stones[1]);
    }
};