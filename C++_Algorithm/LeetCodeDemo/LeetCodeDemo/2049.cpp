using namespace std;
#include <vector>
#include <functional>
//回溯 树
//三个树为根树-当前节点子树，当前节点左子树，当前节点右子树。
//构造树
//回溯遍历树，用个数组记录每个节点包含当前节点的的子节点总数
//比较求解
class Solution {
public:
    int countHighestScoreNodes(vector<int>& parents) {
        int n = parents.size();
        vector<int>f(n, 1);
        unordered_map<int, vector<int>> mp;
        for (int i = 0; i < n; i++) {
            mp[parents[i]].push_back(i);
        }
        function<void(int)> countP = [&](int x) {
            if (!mp.count(x))return;
            for (int& e : mp[x]) {
                countP(e);
                f[x] += f[e];
            }

        };
        countP(0);
        long long mx = 0, tmp = 1;
        int rst = 0;
        for (int i = 0; i < n; i++) {
            if (i > 0)tmp = f[0] - f[i];
            if (mp.count(i)) {
                for (int& e : mp[i]) {
                    tmp *= f[e];
                }
            }
            if (tmp > mx) {
                mx = tmp;
                rst = 1;
            }
            else if (tmp == mx) {
                rst++;
            }
        }
        return rst;
    }
};