using namespace std;
#include <vector>
#include <algorithm>
//给一堆白色瓷砖位置，求一块一定长度x的毯子最多遮盖的白色瓷砖数目。
//两分 前缀和
//遍历遮盖每个连续块的开头，两分查找头部位置大于他的尾部位置的连续块索引，去计算遮盖数,记录最大的。
//计算各个连续块的前缀和，方便区间计算数目。
class Solution {
public:
    int maximumWhiteTiles(vector<vector<int>>& tiles, int carpetLen) {
        int n = tiles.size();
        sort(tiles.begin(), tiles.end());
        vector<int> sums(n, 0);
        sums[0] = tiles[0][1] + 1 - tiles[0][0];
        for (int i = 1; i < n; i++) {
            sums[i] += tiles[i][1] + 1 - tiles[i][0] + sums[i - 1];
        }
        int rst = 0, tail, idx, tmp;
        for (int i = 0; i < n; i++) {
            tail = tiles[i][0] + carpetLen - 1;
            idx = upper_bound(tiles.begin() + i, tiles.end(), vector<int>{tail, INT32_MAX}) - tiles.begin();
            if (idx == n) {
                tmp = sums[idx - 1] - (i > 0 ? sums[i - 1] : 0);
            }
            else {
                tmp = ((idx - 2 >= 0)&&(idx-2>=i) ? sums[idx - 2] : 0) - (i > 0 ? sums[i - 1] : 0) + (min(tail, tiles[idx - 1][1]) - tiles[idx - 1][0] + 1);
            }
            rst = max(rst, tmp);
        }
        return rst;
    }
};