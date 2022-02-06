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
    vector<int> sortEvenOdd(vector<int>& nums) {
        vector<int> odd, even;
        int i=0;
        for (int& e : nums) {
            if(i%2)
                odd.push_back(e);
            else 
                even.push_back(e);
            i++;
        }
        sort(odd.begin(), odd.end());
        sort(even.begin(), even.end(), [](int& a, int& b) {return a > b; });
        i = 0;
        for (int& e : nums) {
            if (i % 2) {
                e = odd.back();
                odd.pop_back();
            }
            else {
                e = even.back();
                even.pop_back();
            }
            i++;
        }
        return nums;
    }
};