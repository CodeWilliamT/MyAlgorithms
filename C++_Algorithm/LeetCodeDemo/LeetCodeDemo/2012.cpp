using namespace std;
#include <iostream>
#include <vector>
//巧思
//美丽值为2代表严格递增：对于每个a[i],求i前面[0,i-1]位置的最大值mx[i]，i后面[i+1,n-1]的最小值mn[i],
//满足mx[i]<a[i] && a[i]<mn[i]的a[i]美丽值为2。
//满足a[i - 1] < a[i] && a[i] < a[i+1]的美丽值为1。
class Solution {
public:
    int sumOfBeauties(vector<int>& a) {
        int n = a.size();
        if (n < 2)return 0;
        long long ans = 0;
        vector<int> mx(n);
        vector<int> mn(n);
        mx[1] = a[0];
        mn[n - 2] = a[n - 1];
        for (int i = 2; i < n - 1; i++)
        {
            mx[i] = max(mx[i - 1], a[i - 1]);
        }

        for (int i = n - 3; i > 0; i--)
        {
            mn[i] = min(mn[i + 1], a[i + 1]);
        }
        for (int i = 1; i < n - 1; i++)
        {
            if (mx[i] < a[i] && a[i] < mn[i])
            {
                ans += 2;
                continue;
            }
            if (a[i - 1] < a[i] && a[i] < a[i + 1])
            {
                ans++;
            }
        }
        return ans;
    }
};