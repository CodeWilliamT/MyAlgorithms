using namespace std;
#include <iostream>
//哈希
//统计各个字母出现数，答案为balloon单词内出现最缺的字母的次数。
//min({ l[0],l[1],l['l'-'a']/2,l['o' - 'a']/2,l['n' - 'a'] });
class Solution {
public:
    int maxNumberOfBalloons(string text) {
        int l[26]{};
        for (char& c : text) {
            l[c-'a']++;
        }
        return min({ l[0],l[1],l['l'-'a']/2,l['o' - 'a']/2,l['n' - 'a'] });
    }
};