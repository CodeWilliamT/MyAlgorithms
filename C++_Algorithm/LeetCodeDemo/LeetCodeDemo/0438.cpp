using namespace std;
#include <iostream>
#include <vector>
#include <string>
//哈希 滑动窗口
//统计p的频率，用letters[x]存储为负值; ，遍历0位置开始p长度len的子串的频率数据letters[x]累加为正值，
//遍历s,对每个子串频率，letters[s[i - 1]]--,letters[s[i+m-1]]++，然后比较，对的加入答案。
class Solution {
public:
    vector<int> findAnagrams(string s, string p) {
        int n = s.size();
        int m = p.size();
        if (m > n)return {};
        int letters[26]{};
        for (char& c : p)
        {
            letters[c - 'a']--;
        }
        for (int i = 0; i < m; i++)
        {
            letters[s[i] - 'a']++;
        }
        auto check = [&]() {
            for (int i = 0; i < 26; i++)
            {
                if (letters[i] != 0)
                    return false;
            }
            return true;
        };
        vector<int> rst;
        if (check())rst.push_back(0);
        for (int i=1;i<n-m+1;i++)
        {
            letters[s[i - 1]-'a']--, letters[s[i + m - 1]- 'a']++;
            if (check())rst.push_back(i);
        }
        return rst;
    }
};