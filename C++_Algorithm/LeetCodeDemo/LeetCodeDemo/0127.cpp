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
//模拟
//看beginWord与第一个单词是否相差一。
//看endWord在数组中的索引。
class Solution {
public:
    int ladderLength(string beginWord, string endWord, vector<string>& wordList) {
        int diff=0;
        int len = beginWord.size();
        for (int i = 0; i < len; i++) {
            if (beginWord[i] != wordList[0][i]) {
                diff++;
            }
        }
        if (diff != 1)return 0;
        int idx = -1;
        for (int i = 0; i < wordList.size();i++) {
            if (wordList[i] == endWord) {
                idx = i;
                break;
            }
        }
        return idx>;
    }
};