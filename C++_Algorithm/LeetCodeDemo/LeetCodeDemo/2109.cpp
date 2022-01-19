using namespace std;
#include <iostream>
#include <vector>
//朴素实现 双指针
class Solution {
public:
    string addSpaces(string s, vector<int>& spaces) {
        string rst;
        for (int i = 0,j=0; i < s.size(); i++) {
            if (j<spaces.size()&&i== spaces[j])
                rst.push_back(' '),j++;
            rst.push_back(s[i]);
        }
        return rst;
    }
};