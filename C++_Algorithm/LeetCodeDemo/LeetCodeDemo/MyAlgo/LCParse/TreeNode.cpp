using namespace std;

#include <iostream>
#include <string>
#include <queue>

struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//二叉树序列化
class Codec {
public:
    // Encodes a tree to a single string.
    string serialize(TreeNode* root) {
        if (!root)return"null";
        string rst = to_string(root->val);
        queue<TreeNode*> q;
        q.push(root);
        int witdh;
        TreeNode* cur, next;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (cur->left) {
                    q.push(cur->left);
                    rst += "," + to_string(cur->left->val);
                }
                else
                    rst += ",null";

                if (cur->right) {
                    q.push(cur->right);
                    rst += "," + to_string(cur->right->val);
                }
                else
                    rst += ",null";
            }
        }
        return rst;
    }

    TreeNode* doDeserialize(queue<string>& ndata)
    {
        if (ndata.front() == "null")
        {
            return nullptr;
        }
        queue<TreeNode*> q;
        TreeNode* root = new TreeNode(stoi(ndata.front()));
        q.push(root);
        ndata.pop();
        int cnt;
        while (!q.empty() && !ndata.empty()) {
            cnt = q.size();
            while (cnt-- && !ndata.empty()) {
                TreeNode* cur = q.front();
                q.pop();
                if (ndata.front() == "null")
                    cur->left = nullptr;
                else {
                    cur->left = new TreeNode(stoi(ndata.front()));
                    q.push(cur->left);
                }
                ndata.pop();
                if (ndata.front() == "null")
                    cur->right = nullptr;
                else {
                    cur->right = new TreeNode(stoi(ndata.front()));
                    q.push(cur->right);
                }
                ndata.pop();
            }
        }
        return root;
    }

    // Decodes your encoded data to tree.
    TreeNode* deserialize(string data) {
        if (data == "") return NULL;
        int len = data.length();
        queue<string> ndata;
        for (int i = 0, s = 0, e = 0; i < len; i++)
        {
            while (data[i] != ',' && data[i] != '[' && data[i] != ']' && data[i] != ' ')
            {
                i++;
                if (i >= len)break;
            }
            e = i;
            if (e - s) {
                string d = data.substr(s, e - s);
                ndata.push(d);
            }
            s = e + 1;
        }
        return doDeserialize(ndata);
    }
};
