using namespace std;
#include <iostream>
//巧思
//数列递推，找规律
//索引0的数出现n次，索引1的数出现2(n-1次)，第i个数出现(i+1)*(n-i)次
class Solution {
public:
    long long countVowels(string s) {
        long long n = s.size();
        long long ans = 0;
        for (long long i = 0; i < n; i++)
        {
            long long tmp = (i + 1) * (n - i);
            if (s[i]=='a'|| s[i]=='e' || s[i] == 'i' || s[i] == 'u' || s[i] == 'o')ans = ans + tmp;
        }
        return ans;
    }
};