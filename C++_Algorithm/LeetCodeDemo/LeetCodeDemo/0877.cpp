using namespace std;
#include <iostream>
#include <vector>

//数学，偶数个石头堆，先手的就能保证总是拿奇数堆或偶数堆
//class Solution {
//public:
//    bool stoneGame(vector<int>& piles) {
//        return true;
//    }
//};

//动态规划，若没有偶数个石头堆条件
//核心，考虑开始拿的人每个区间能拿完后能造成的最大分差，为核心变量。
//边界，反向考虑，最后拿的那堆值，为每个1大小的区间拿完后能造成的最大分差。
//则状态转移方程为：开始拿的人每个区间能拿完后能造成的最大分差=（前后拿的那个分值-前一个人拿出的最大分差）的较大值
class Solution {
public:
    bool stoneGame(vector<int>& piles) {
        int length = piles.size();
        auto dp = vector<vector<int>>(length, vector<int>(length));
        for (int i = 0; i < length; i++) {
            dp[i][i] = piles[i];
        }
        for (int i = length - 2; i >= 0; i--) {
            for (int j = i + 1; j < length; j++) {
                dp[i][j] = max(piles[i] - dp[i + 1][j], piles[j] - dp[i][j - 1]);
            }
        }
        return dp[0][length - 1] > 0;
    }
};