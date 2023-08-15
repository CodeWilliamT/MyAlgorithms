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
//麻烦题 复杂模拟
//按索引找子子字符串，记录有无，最后生成。
typedef pair<int, string> pis;
class Solution {
public:
    string findReplaceString(string s, vector<int>& indices, vector<string>& sources, vector<string>& targets) {
        int n = indices.size();
        unordered_map<int,pis> rep;
        string rst;
        string sub;
        for (int i = 0; i < n; i++) {
            sub = s.substr(indices[i], sources[i].size());
            if (sub == sources[i]) {
                rep[indices[i]] = {sources[i].size(),targets[i]};
            }
        }
        for (int i = 0; i < s.size(); ) {
            if (rep.count(i)) {
                rst += rep[i].second;
                i += rep[i].first;
                continue;
            }
            rst += s[i];
            i++;
        }
        return rst;
    }
};