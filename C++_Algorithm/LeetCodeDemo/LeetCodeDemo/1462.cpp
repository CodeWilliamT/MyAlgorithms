using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
#include "myAlgo\Structs\TreeNode.cpp"
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;

class Solution {
    //��������߽�,����һ���򷵻�true
    //�ڽӱ���ѣ����صִ��յ㲽�����������򷵻�-1
    bool BFS(int n, vector<vector<int>>&g, int start, int end)
    {
        vector<int> v(n, 0);
        queue<int> q;
        q.push(start);
        int witdh;
        int cur;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (v[cur]) {
                    continue;//����߽����
                }
                v[cur] = 1;//����
                if (cur == end) {//�ִ��յ�
                    return true;
                }
                for (auto& e : g[cur]) {
                    if (v[e]) {
                        continue;//����߽����
                    }
                    q.push(e);//������һ��
                }
            }
        }
        return false;
    }
public:
    vector<bool> checkIfPrerequisite(int n, vector<vector<int>>& ps, vector<vector<int>>& qs) {
        vector<vector<int>> g(n);
        for (auto& p : ps) {
            g[p[0]].push_back(p[1]);
        }
        vector<bool> rst;
        for (auto& q : qs) {
            rst.push_back(BFS(n, g, q[0], q[1]));
        }
        return rst;
    }
};