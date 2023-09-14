using namespace std;
#include <vector>
//模拟
//一圈圈判断。
class Solution {
public:
    vector<vector<int>> queensAttacktheKing(vector<vector<int>>& q, vector<int>& k) {
        int d[8][2] = { {1,0},{0,1},{-1,0},{0,-1},{1,1},{1,-1},{-1,1},{-1,-1} };//方向
        int cur[2];
        bool ban[8]{};
        bool mp[8][8]{};
        for (auto& e : q) {
            mp[e[0]][e[1]] = 1;
        }
        vector<vector<int>> rst;
        for(int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                if (ban[j])continue;
                cur[0] = k[0] + i * d[j][0];
                cur[1] = k[1] + i * d[j][1];
                if (cur[0] > -1 && cur[0] <8 && cur[1] >-1 && cur[1] < 8&&mp[cur[0]][cur[1]]) {
                    rst.push_back({cur[0], cur[1]});
                    ban[j] = 1;
                }
            }
        }
        return rst;
    }
};