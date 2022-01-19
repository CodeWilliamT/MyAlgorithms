using namespace std;
#include <iostream>
#include <vector>


//性质巧思，利用不相交代表每个边缘区间只能相交一次,递增性质，轮流比较求交集
class Solution {
public:
    vector<vector<int>> intervalIntersection(vector<vector<int>>& s1, vector<vector<int>>& s2) {
        vector<vector<int>> ans;
        for (int i = 0, j = 0; i < s1.size() && j < s2.size(); )
        {
            int a = s1[i][0], b = s1[i][1], c = s2[j][0], d = s2[j][1];
            if (a <= c && d <= b)
            {
                ans.push_back({ c,d });
                j++;
                continue;
            }
            if (c <= a && b <= d)
            {
                ans.push_back({ a,b });
                i++;
                continue;
            }
            a < c ? i++ : j++;
            if (c <= b && a <= d)
            {
                if (a <= c)
                {
                    ans.push_back({ c,b });
                }
                else if (c <= a)
                {
                    ans.push_back({ a,d });
                }
            }

        }
        return ans;
    }
};
////朴素，直接遍历求交集
//class Solution {
//public:
//    vector<vector<int>> intervalIntersection(vector<vector<int>>& s1, vector<vector<int>>& s2) {
//        vector<vector<int>> ans;
//        for (int i = 0; i < s1.size(); i++)
//        {
//            for (int j = 0; j < s2.size(); j++)
//            {
//                int a = s1[i][0], b = s1[i][1], c = s2[j][0], d = s2[j][1];
//                if (a <= c && d <= b)
//                {
//                    ans.push_back({ c,d });
//                    continue;
//                }
//                if (c <= a && b <= d)
//                {
//                    ans.push_back({ a,b });
//                    continue;
//                }
//                if (c <= b && a <= d)
//                {
//                    if (a <= c)
//                    {
//                        ans.push_back({c,b});
//                    }
//                    else if (c <= a)
//                    {
//                        ans.push_back({a,d});
//                    }
//                }
//            }
//        }
//        return ans;
//    }
//};