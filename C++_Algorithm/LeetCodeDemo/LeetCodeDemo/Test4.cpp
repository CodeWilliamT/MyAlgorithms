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
//找规律 模拟
//只能相邻换，构建回文串.
//单边出发配另一边。如果是单数的中间那个位置，直接计算差值删了该元素。
class Solution {
public:
    int minMovesToMakePalindrome(string s) {
        int n = s.size();
        int rst = 0;
        for(int i=0;i<n/2;i++){
            if (s[i] != s[n - 1 - i]) {
                int j = n - 1 - i;
                for (; j > i; j--) {
                    if (s[j] == s[i])break;
                }
                s.erase(s.begin() + j);
                if (j == i) {
                    rst += abs(n / 2 - i);
                    i--;
                    n--;
                }
                else {
                    s.insert(s.begin() + n-1 - i, s[i]);
                    rst += n-1-i-j;
                }
            }
        }
        return rst;
    }
};