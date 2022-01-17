using namespace std;
#include <iostream>
#include <vector>
//细致条件分析,简单题，两分查找
class Solution {
public:
    bool searchMatrix(vector<vector<int>>& m, int t) {
        int l = 0, r = m.size()-1,mid;
        int row=0;
        while (l <= r)
        {
            mid = (l + r) / 2;
            if (m[mid][0] == t)return true;
            else if (m[mid][0] < t)
            {
                row = mid;
                l = mid+1;
            }
            else
            {
                r = mid-1;
            }
        }
        for (;row >= 0;row--)
        {
            l = 1; r = m[row].size() - 1;
            while (l <= r)
            {
                mid = (l + r) / 2;
                if (m[row][mid] == t)return true;
                else if (m[row][mid] < t)
                {
                    l = mid + 1;
                }
                else
                {
                    r = mid - 1;
                }
            }
        }
        return false;
    }
};