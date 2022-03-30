using namespace std;
#include <iostream>
#include <vector>
#include <string>
//按小到大构造指定长度的回文数，时间复杂度nlogn内。
//构造，排序
class Solution {
public:
    vector<long long> kthPalindrome(vector<int>& q, int intLength) {
        long long mn = pow(10, (intLength+1)/2-1);
        long long mx = mn * 10;
        long long mxidx = mx - mn;
        vector<long long> rst;
        string tmp,tmp2;
        long long num;
        for (int& e : q) {
            if(e>mxidx){
                rst.push_back(-1);
            }
            else {
                tmp = to_string(mn + e-1);
                tmp2 = tmp;
                reverse(tmp.begin(),tmp.end());
                if (intLength % 2)tmp2.pop_back();
                tmp2 += tmp;
                num = stoll(tmp2);
                rst.push_back(num);
            }
        }
        return rst;
    }
};