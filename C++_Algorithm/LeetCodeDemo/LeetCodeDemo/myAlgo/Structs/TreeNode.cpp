using namespace std;

#include <iostream>
#include <string>
#include <queue>
#include <list>

struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};


class Codec {

public:
    string doSerialize(TreeNode* node)
    {
        string rst = "";
        if (node == nullptr) { rst = "null,"; return rst; }
        rst = to_string(node->val)+",";
        rst += doSerialize(node->left);
        rst += doSerialize(node->right);
        return rst;
    }
    // Encodes a tree to a single string.

    string serialize(TreeNode* root) {
        string rst = doSerialize(root);
        int len = rst.size();
        return rst.substr(0, len - 1);
    }

    TreeNode* doDeserialize(queue<string>& ndata)
    {
        if (ndata.front() == "null")
        {
            return nullptr;
        }
        queue<TreeNode*> q;
        TreeNode* root= new TreeNode(stoi(ndata.front()));
        q.push(root);
        ndata.pop();
        int cnt;
        while (!q.empty()&&!ndata.empty()) {
            cnt = q.size();
            while (cnt--&& !ndata.empty()) {
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
            while (data[i] != ','&& data[i]!='['&& data[i]!=']' && data[i] != ' ')
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