using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_map>
//哈希
class Solution {
public:
    string kthDistinct(vector<string>& arr, int k) {
        unordered_map<string,int> st;
        for (auto e : arr)
        {
            auto a = st.find(e);
            st[e]++;
        }
        int i = 0;
        for (auto e : arr)
        {
            if(st[e]==1)i++;
            if (i == k)return e;
        }
        return "";
    }
};