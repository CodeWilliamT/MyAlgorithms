using namespace std;
#include <vector>
#include <map>
//简单模拟 哈希
class Solution {
public:
    vector<vector<int>> mergeSimilarItems(vector<vector<int>>& items1, vector<vector<int>>& items2) {
        map<int, int> mp;
        for (auto& e : items1) {
            mp[e[0]] = e[1];
        }
        for (auto& e : items2) {
            mp[e[0]]+= e[1];
        }
        vector<vector<int>> ret;
        for (auto& e : mp) {
            ret.push_back({ e.first, e.second });
        }
        return ret;
    }
};