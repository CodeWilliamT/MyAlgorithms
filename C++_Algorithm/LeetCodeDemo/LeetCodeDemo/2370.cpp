using namespace std;
#include <iostream>
#include <vector>
//动态规划 贪心
//用数组记录 以x位置字符为结尾的最长子序列长度。最大值为答案
//求以x为索引的字符结尾的最长子序列长度：其之前的k范围内字符为结尾的最长子序列长度的最大值+1。
//用个26长度的字母数组记录每个字母结尾时有其最长子序列的位置
class Solution {
public:
    int longestIdealString(string s, int k) {
        int n = s.size();
        int maxl = 0;
        int rst = 0;
        vector<int> cnts(n, 0);
        vector<int> dicts(26, -1);
        for (int i = 0; i < n; i++) {
            maxl = 0;
            for (int j = s[i] - k < 'a' ? 0 : (s[i] - k - 'a'); j <= s[i] + k - 'a' && j < 26; j++) {
                if (dicts[j] == -1)continue;
                maxl = max(maxl, cnts[dicts[j]]);
            }
            cnts[i] = maxl + 1;
            if (dicts[s[i] - 'a'] == -1 || cnts[dicts[s[i] - 'a']] < cnts[i])dicts[s[i] - 'a'] = i;
            rst = max(rst, cnts[i]);
        }
        return rst;
    }
};