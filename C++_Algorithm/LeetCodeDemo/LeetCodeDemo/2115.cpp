using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>
#include <queue>
//拓扑排序 哈希集合 
//把每一种原材料（菜也算一种原材料）看成图上的一个节点，如果某一道菜需要一种原材料，就添加一条从原材料到菜的有向边。
//如果图上的一个节点的入度为 0 则可以直接用，如果是菜则可以直接做出来。
class Solution {
public:
    vector<string> findAllRecipes(vector<string>& recipes, vector<vector<string>>& ingredients, vector<string>& supplies) {
        int n = recipes.size();
        unordered_map<string, int> v;
        queue<string> q;
        unordered_map<string, vector<string>> lst;
        for (int i = 0; i < n; i++) {
            v[recipes[i]] = ingredients[i].size();
            for (auto& e : ingredients[i]) {
                lst[e].push_back(recipes[i]);
            }
        }
        for (auto& e : supplies) {
            q.push(e);
        }
        string cur;
        vector<string> rst;
        while (!q.empty()) {
            cur = q.front();
            q.pop();
            if (v.count(cur))rst.push_back(cur);
            for (auto& e : lst[cur]) {
                v[e]--;
                if(!v[e])
                    q.push(e);
            }
        }
        return rst;
    }
};