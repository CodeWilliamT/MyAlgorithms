using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>

//二刷
//模拟
//排序后判断处理
class Solution {
public:
    vector<vector<int>> merge(vector<vector<int>>& t) {
        sort(t.begin(), t.end());
        vector<vector<int>> rst = { {t[0][0],t[0][1]} };
        for (auto& e : t) {
            if (e[0] <= rst.back()[1]) {
                rst.back()[1] = max(rst.back()[1], e[1]);
            }
            else {
                rst.push_back({ e[0],e[1] });
            }
        }
        return rst;
    }
};

//排序后硬推
//class Solution {
//public:
//    vector<vector<int>> merge(vector<vector<int>>& intervals) {
//        int n = intervals.size();
//        if (!n)return {};
//        vector<vector<int>> ans;
//        vector<int> h;
//        sort(intervals.begin(), intervals.end());
//        h = intervals[0];
//        for (int i = 1; i < n; i++)
//        {
//            if (h[1] >= intervals[i][0])
//            {
//                if (h[1] < intervals[i][1])
//                    h[1] = intervals[i][1];
//            }
//            else
//            {
//                ans.push_back(h);
//                h = intervals[i];
//            }
//        }
//        ans.push_back(h);
//        return ans;
//    }
//};
////并查集 很慢O(n^2)
//class Solution {
//    vector<int> f;
//    int findf(int i)
//    {
//        if (i == f[i])return i;
//        return findf(f[i]);
//    }
//public:
//    vector<vector<int>> merge(vector<vector<int>>& intervals) {
//        vector<vector<int>> ans;
//        auto a= intervals;
//        f.clear();
//        int n = intervals.size();
//        for (int i = 0; i < n; i++)
//        {
//            f.push_back(i);
//        }
//        for (int i = 0; i < n; i++)
//        {
//            for (int j = i+1; j < n; j++)
//            {
//                int fi = findf(i);
//                int fj = findf(j);
//                if (fi == fj)continue;
//                if (!(a[fi][0] > a[fj][1] || a[fj][0] > a[fi][1]))
//                {
//                    int minidx = min(fi, fj);
//                    int maxidx = max(fi, fj);
//                    a[minidx] = {min(a[fi][0],a[fj][0]),max(a[fi][1],a[fj][1])};
//                    f[maxidx] = minidx;
//                }
//            }
//        }
//        for (int i = 0; i < n; i++)
//        {
//            int fi = findf(i);
//            if(fi!=i)
//            {
//                ans.push_back(a[fi]);
//            }
//        }
//        return ans;
//    }
//};
//int main()
//{
//    Solution s;
//    vector<vector<int>> x = { {2, 3},{4, 6},{5, 7},{3, 4}};
//    s.merge(x);
//    return 0;
//}