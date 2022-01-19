using namespace std;
#include <iostream>
#include <vector>
#include <string>

//滑动窗口
class Solution {
public:
    int maxConsecutiveAnswers(string ans, int k) {
        int n = ans.size();
        int l = 0;
        int r = 0;
        int tNums = 0;
        int fNums = 0;
        while (r < n) {
            if (ans[r] == 'T') {
                tNums++;
            }
            else {
                fNums++;
            }
            int mx = max(tNums, fNums);
            if (r - l + 1 - mx > k) {
                if (ans[l] == 'T') {
                    tNums--;
                }
                else {
                    fNums--;
                }
                ++l;
            }
            ++r;
        }
        return r - l;
    }
};
//巧思
//状态量t[i]：到i位置T数。
//状态量f[i]：到i位置F数。
//hashT:T数刚为j个的位置
//hashF:F数刚为j个的位置
//i位置的最大T数为：lenT=f[i] <= k？f[i] + t[i]:t[i]-t[hashF[f[i] - k]]+k;
//i位置的最大F数为：lenT=t[i] <= k？f[i] + t[i]:f[i] - f[hashT[t[i] - k]] + k;
//ans=max(lenT,lenF,ans)
//class Solution {
//public:
//    int maxConsecutiveAnswers(string s, int k) {
//        int n = s.size();
//        vector<int> t(n,0), f(n, 0);
//        int hashT[50001]{}, hashF[50001]{};
//        int countT = 0, countF = 0;
//        for (int i = 0; i < n; i++)
//        {
//            if (s[i] == 'T')
//            {
//                countT++;
//                hashT[countT] = i;
//            }
//            if (s[i] == 'F')
//            {
//                countF++;
//                hashF[countF] = i;
//            }
//            t[i] = countT;
//            f[i] = countF;
//        }
//        int lenT, lenF;
//        int ans = 0;
//        for (int i = 0; i < n; i++)
//        {
//            if (f[i] <= k)
//                lenT = f[i] + t[i];
//            else
//            {
//                lenT = t[i]-t[hashF[f[i] - k]]+k;
//            }
//            if (t[i] <= k)lenF = f[i] + t[i];
//            else
//            {
//                lenF = f[i] - f[hashT[t[i] - k]] + k;
//            }
//            ans = max(ans, max(lenT, lenF));
//        }
//        return ans;
//    }
//};