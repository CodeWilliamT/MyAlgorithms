using namespace std;
#include <vector>
#include <unordered_map>

//简单题 模拟
class Solution {
public:
    vector<int> executeInstructions(int n, vector<int>& startPos, string s) {
        vector<int> rst;
        int x = startPos[0];
        int y = startPos[1];
        unordered_map<char, vector<int>> mp = { {'D', {1,0}}, {'R',{0,1}}, {'U',{-1,0}}, {'L',{0,-1}}};
        int len = s.size(),cnt=0;
        for (int i = 0; i < len;i++) {
            x = startPos[0];
            y = startPos[1];
            cnt = 0;
            for (int j = i; j < len; j++) {
                x += mp[s[j]][0];
                y += mp[s[j]][1];
                if (x < 0 || x >= n || y < 0 || y >= n) {
                    break;
                }
                cnt++;
            }
            rst.push_back(cnt);
        }
        return rst;
    }
};