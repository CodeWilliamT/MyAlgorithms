using namespace std;
#include <vector>
#include <set>
#include <functional>
//巧思 深搜(回溯) 记忆化搜索
//从200个格子构成的最大封闭区域走出的最大面积数为15000左右，
//所以从source能走的格子总数超过20000步就代表没被封闭。
//所以从target能走的格子总数超过20000步就代表没被封闭。
//如果过程中直接走到了，那就代表可行。
//优化：可用long long压缩哈希记录表。
class Solution {
public:
    bool isEscapePossible(vector<vector<int>>& blocked, vector<int>& source, vector<int>& target) {
        bool flag = false;
        const int MAXN = 1000000;
        set<pair<int, int>> block;
        set<pair<int, int>> visit;
        int dir[4][2] = { {1,0},{0,1},{-1,0},{0,-1} };
        pair<int, int> cur;
        for (auto e : blocked) {
            block.insert({ e[0], e[1] });
        }
        function<bool(int,int, vector<int>&)> dfs = [&](int x,int y, vector<int>& t) {
            cur = { x,y };
            if (x == t[0] && y == t[1]) {
                visit.clear();
                flag = true;
                return true;
            }
            if (block.count(cur) || visit.count(cur) || cur.first < 0 || cur.first >= MAXN || cur.second < 0 || cur.second >= MAXN) {
                return false;
            }
            visit.insert(cur);
            if (visit.size() > 16000) {
                visit.clear();
                return true;
            }
            for (int i = 0; i < 4; i++) {
                if (dfs(x + dir[i][0], y + dir[i][1],t))
                    return true;
            }
            return false;
        };
        if (!dfs(source[0], source[1],target))return false;
        return flag||dfs(target[0], target[1],source);
    }
};