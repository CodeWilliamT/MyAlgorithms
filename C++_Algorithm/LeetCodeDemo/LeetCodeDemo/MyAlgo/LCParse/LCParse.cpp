using namespace std;
#include <vector>
#include <string>
#include <queue>
//Parse LeedCode input string to cpp data 
static vector<int> stov(string s) {
	string tmp;
	vector<int> v;
	for (char& c : s) {
		if ('0' <= c && c <= '9')
			tmp.push_back(c);
		if (c == ',' || c == ']') {
			v.push_back(stoi(tmp));
			tmp.clear();
		}
	}
	return v;
}
static vector<string> stovs(string s) {
	string tmp;
	vector<string> vs;

	for (int j = 1; j < s.size(); j++) {
		char c = s[j];
		if (c != ',' && c != '"')
			tmp.push_back(c);
		if (c == ',' || c == ']') {
			vs.push_back(tmp);
			tmp.clear();
		}
	}
	return vs;
}
static vector<vector<int>> stovvi(string s) {
	string tmp;
	vector<vector<int>> vvi;

	for (int j = 1; j < s.size() - 1; j++) {
		char c = s[j];
		if (c == '[')
			vvi.push_back(vector<int>());
		if ('0' <= c && c <= '9')
			tmp.push_back(c);
		if (c == ','&& s[j - 1]!=']' || c == ']') {
			vvi.back().push_back(stoi(tmp));
			tmp.clear();
		}
	}
	return vvi;
}
static vector<vector<char>> stovvc(string s) {
	vector<vector<char>> vvc;
	for (int j = 1; j < s.size() - 1; j++) {
		char c = s[j];
		if (c == '[')
			vvc.push_back(vector<char>());
		if (c != '[' && c != ']' && c != '\'' && c != ',')
			vvc.back().push_back(c);
	}
	return vvc;
}
static vector<vector<string>> stovvs(string s) {
	vector<vector<string>> vvs;
	for (int j = 1; j < s.size() - 1; j++) {
		char c = s[j];
		if (c == '[') {
			vvs.push_back(vector<string>());
			vvs.back().push_back(string());
		}
		if (c == ','&&s[j-1]!=']')
			vvs.back().push_back(string());
		if (c != '[' && c != ']' && c != '\"' && c != ',')
			vvs.back().back().push_back(c);
	}
	return vvs;
}
struct TreeNode {
    int val;
    TreeNode* left;
    TreeNode* right;
    TreeNode() : val(0), left(nullptr), right(nullptr) {}
    TreeNode(int x) : val(x), left(nullptr), right(nullptr) {}
    TreeNode(int x, TreeNode* left, TreeNode* right) : val(x), left(left), right(right) {}
};
//¶þ²æÊ÷ÐòÁÐ»¯
//Encodes a tree to a single string.
string treetos(TreeNode* root) {
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
TreeNode* stotree(string data) {
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
