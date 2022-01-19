using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <unordered_set>
#include <queue>
//广度优先搜索,记忆化搜索,哈希
//最小+小数据→广搜,字符串重复→哈希+记忆化搜索
class Solution {
private:
    bool judge(string s)
    {
        int lcnt = 0, rcnt = 0;
        int n = s.size();
        for (int i = 0; i < n; i++)
        {
            if (s[i] == '(')
            {
                lcnt++;
            }
            else if (s[i] == ')')
            {
                rcnt++;
                if (rcnt > lcnt)
                {
                    return false;
                }
            }
        }
        if (rcnt != lcnt)return false;
        return true;
    }
    struct Node
    {
        string s="";
        int depth=0;
        Node(string a, int b) :s(a), depth(b) {};
        Node(){};
    };
public:
    vector<string> removeInvalidParentheses(string s) {
        Node* root = new Node(s,0),*cur;
        queue<Node*> q;
        q.push(root);
        vector<string> ans;
        unordered_set<string> set;
        int deep=-1;
        while (!q.empty())
        {
            cur = q.front();
            q.pop();
            if (deep != -1 && cur->depth != deep)
            {
                break;
            }
            if(judge(cur->s))ans.push_back(cur->s),deep=cur->depth;
            for (int i = 0; i < cur->s.size(); i++)
            {
                string tmps = cur->s;
                tmps.erase(tmps.begin() + i);
                if (set.count(tmps))continue;
                set.insert(tmps);
                Node* tmp = new Node(tmps, cur->depth + 1);
                q.push(tmp);
            }
        }
        return ans;
    }
};