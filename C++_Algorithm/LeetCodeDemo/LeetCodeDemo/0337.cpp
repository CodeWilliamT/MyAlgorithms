using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>b
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
#include "myAlgo\Structs\TreeNode.cpp"
typedef pair<int, bool> pib;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;

class Solution {
    struct treeNodeSeletion {
        int selected;
        int noSelected;
    };
public:
    int rob(TreeNode* root) {

        function<treeNodeSeletion(TreeNode*, int,int, int)> dfs = [&](TreeNode* cur, int premx,int prepremx) {
            //搜可行解到达递归边界便跳出
            if (!cur)//判定状态可行性，若状态不可行，则跳过
                return 0;
            int rst = (prepremx + cur->val) > premx ? cur->val : 0;
            int nowmx = max(prepremx + cur->val, premx);
            if(cur->left)
                rst+=dfs(cur->left, nowmx,premx, lv + 1);
            if (cur->right)
                rst += dfs(cur->right, nowmx, premx, lv + 1);
            return rst;
        };
        
        return dfs(root, 0,0, 0);
    }
};