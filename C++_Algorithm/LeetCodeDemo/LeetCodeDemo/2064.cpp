using namespace std;
#include <iostream>
#include <queue>
//两分查找
class Solution {
public:
    int minimizedMaximum(int n, vector<int>& q) {
        sort(q.begin(), q.end());
        int m = q.size();
        int l = 1;
        int r = q[m-1],mid;
        while (l < r)
        {
            mid = (l + r) / 2;
            if (checkNum(mid, n, q))
            {
                l = mid + 1;
            }
            else
            {
                r = mid;
            }
        }
        return r;
    }
    bool checkNum(int num,int n, vector<int>& q)
    {
        int cnt=0;
        for (int i = 0; i < q.size(); i++)
        {
            cnt += (q[i] / num + (q[i] % num?1:0));
        }
        return cnt > n;
    }
};