using namespace std;
#include <vector>
#include <unordered_map>
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//哈希 栈
//遍历根节点回溯组，通过比对回溯组前后元素a[i],a[i-1] 在比对组b[i]索引得知 回溯组当前元素a[i]是否是a[i-1]的左节点，如果不是，则根据遍历顺序后进先出原则安排元素位置。
class Solution {
public:
    TreeNode* buildTree(vector<int>& a, vector<int>& b) {
        unordered_map<short, short> mp;
        for (int i = 0; i < b.size(); i++)
        {
            mp[b[i]] = i;
        }
        TreeNode* root = new TreeNode(a[0]);
        TreeNode* cur, *prev;
        vector<TreeNode*> s;
        s.push_back(root);
        for (int i = 1; i < a.size(); i++)
        {
            cur = new TreeNode(a[i]);
            prev = s.back();
            if(mp[cur->val] <mp[prev->val])
                prev->left = cur;
            else
            {
                s.pop_back();
                while (!s.empty()&&mp[cur->val] > mp[s.back()->val])
                {
                    prev = s.back();
                    s.pop_back();
                }
                prev->right = cur;
            }
            s.push_back(cur);
        }
        return root;
    }
};