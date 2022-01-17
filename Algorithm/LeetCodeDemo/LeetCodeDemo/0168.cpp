using namespace std;
#include <iostream>
//简单题
//注意Z
class Solution {
public:
    string convertToTitle(int n) {
        string ans;
        long long tmp = 0;
        while (n > 0)
        {
            tmp = n % 26;
            n /= 26;
            if (tmp == 0)
            {
                ans += 'Z';
                n--;
            }
            else
                ans += tmp + 'A' - 1;
        }
        reverse(ans.begin(), ans.end());
        return ans;
    }
};