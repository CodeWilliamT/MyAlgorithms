using namespace std;
#include <iostream>
//细致条件分析
//前面有桶了就不放了
//前面没桶后面能放放后面
//前面没桶后面也放不了，则前面放桶
//都放不了，告辞
class Solution {
public:
    int minimumBuckets(string s) {
        int n = s.size();
        int rst = 0;
        for (int i = 0; i < n; i++)
        {
            if (s[i] == 'H')
            {
                if (i > 0 && s[i - 1] == '1')
                    continue;
                if (i < n - 1 && s[i + 1] == '.')
                {
                    rst++;
                    s[i + 1] = '1';
                }
                else if (i > 0 && s[i - 1] == '.')
                {
                    rst++;
                    s[i - 1] = '1';
                }
                else
                    return -1;
            }
        }
        return rst;
    }
};