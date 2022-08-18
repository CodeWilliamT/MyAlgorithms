using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//先排序，倒序2个2个遍历,记录只出现单数次的最大的那个数。
//如果相同则记下，不同则记下大的，i+1；
//exception:
class Solution {
public:
    string largestPalindromic(string num) {
        int n = num.size();
        if (n == 1)
            return num;
        char maxSingle=0;
        string rst,tmp;
        sort(num.begin(), num.end());
        for (int i = n - 2; i > -2; i-=2) {
            if (i == -1) {
                if (!maxSingle)
                    maxSingle = num[i + 1];
                break;
            }
            if (num[i] == num[i + 1]) {
                if (num[i + 1] == '0' && !rst.size())
                {
                    if (!maxSingle)
                        maxSingle = '0';
                    break;
                }
                rst.push_back(num[i]);
            }
            else {
                if(!maxSingle)
                    maxSingle = num[i+1];
                i++;
            }
        }
        tmp = rst;
        reverse(tmp.begin(), tmp.end());
        if (maxSingle)rst.push_back(maxSingle);
        rst += tmp;
        return rst;
    }
};