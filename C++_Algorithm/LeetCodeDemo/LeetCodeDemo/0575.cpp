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
//哈希
//min(n/2,type)
class Solution {
public:
    int distributeCandies(vector<int>& candyType) {
        unordered_set<int> st;
        int n = candyType.size();
        for (auto e : candyType)
        {
            st.insert(e);
        }
        int types = st.size();
        return min(n / 2, types);
    }
};