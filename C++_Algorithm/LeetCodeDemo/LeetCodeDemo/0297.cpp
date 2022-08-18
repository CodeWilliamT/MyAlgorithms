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

    TreeNode* doDeserialize(list<string>& ndata)
    {
        if (!ndata.size())return nullptr;
        if (ndata.front()=="null")
        {
            ndata.pop_front();
            return nullptr;
        }
        TreeNode* node= new TreeNode(stoi(ndata.front()));
        ndata.pop_front();
        node->left = doDeserialize(ndata);
        node->right = doDeserialize(ndata);
        return node;
    }

    // Decodes your encoded data to tree.

    TreeNode* deserialize(string data) {
        if (data == "") return NULL;
        int len = data.length();
        list<string> ndata;
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
                ndata.push_back(d);
            }
            s = e + 1;
        }
        return doDeserialize(ndata);
    }
};


//int main()
//{
//    Codec c;
//    TreeNode* root;
//    root=c.deserialize("5,2,3,null,null,2,4,3,1");
//    cout << c.serialize(root) << endl;
//    return 0;
//}