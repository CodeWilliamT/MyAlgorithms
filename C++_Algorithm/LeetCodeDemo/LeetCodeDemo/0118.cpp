using namespace std;
#include <vector>
//简单题 朴素实现
class Solution {
public:
    vector<vector<int>> generate(int n) {
        vector<vector<int>> rst(n);
        rst[0].push_back(1);
        if (n == 1)return rst;
        rst[1] = {1,1};
        if (n == 2)return rst;
        for (int i = 2; i < n; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                if (j == 0 || j == i) {
                    rst[i].push_back(1);
                    continue;
                }
                rst[i].push_back(rst[i-1][j-1]+ rst[i - 1][j]);
            }
        }
        return rst;
    }
};