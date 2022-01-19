using namespace std;
#include <iostream>
#include <map>
//巧思 哈希
//先遍历字符串用字符表统计字符出现频次，然后用哈希表统计频次出现频次，
//然后从后往前遍历哈希,直到遍历到出现0次出现的频率或遍历到头了，
//如果(*e).second>1则hash[(*e).first-1]+=(*e).second-1;rst+=(*e).second-1;(*e).second=1;
class Solution {
public:
    int minDeletions(string s) {
        if (s == "")return 0;
        int lst[26]{};
        for (char& c : s)
        {
            lst[c - 'a']++;
        }
        map<int, int> hash;
        for (int i = 0; i < 26; i++)
        {
            if (lst[i])hash[lst[i]]++;
        }
        auto e = prev(hash.end());
        int rst = 0;
        while ((*e).first)
        {
            if ((*e).second > 1) {
                hash[(*e).first - 1] += (*e).second - 1;
                rst += (*e).second - 1;
                (*e).second = 1;
            }
            if (e == hash.begin())break;
            e = prev(e);
        }
        return rst;
    }
};