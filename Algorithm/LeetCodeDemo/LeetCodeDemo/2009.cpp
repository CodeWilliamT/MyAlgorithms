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
//
//ÏàÍ¬µÄ
class Solution {
public:
    int minOperations(vector<int>& a) {
        int n = a.size();
        int mid = n / 2;
        auto imid = a.begin() + mid;
        nth_element(a.begin(), imid, a.end());
        int vmid = *imid;
        unordered_set<int> st;
        for (int i = 0; i < n; i++)
        {
        }
    }
};