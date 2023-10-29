#include "myAlgo\Structs\TreeNode.cpp"
using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <numeric>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
typedef pair<int, bool> pib;
typedef pair<int, int> pii;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<ll, int> pli;
#define MAXM (int)(1e5+1)
#define MOD (int)(1e9+7)
//回溯(深搜) 动态规划
class Solution {
#define MAXN (int)1001
public:
    int diffs(string x, string y) {
        int len = x.size();
        int diffs = 0;
        for (int i = 0; i < len; i++) {
            if(x[i]!=y[i])
                diffs++;
        }
        return diffs;
    }
    vector<string> getWordsInLongestSubsequence(int n, vector<string>& w, vector<int>& g) {
        int next[MAXN]{};//以其为起点，能构成其最长子序列时的 下一元素
        int len[MAXN]{};//以其为起点的子序列长度
        function<int(int, int)> dfs = [&](int i, int depth) {
            if (len[i])
                return len[i];
            len[i] = 1;
            int clen;
            for (int j = i+1; j < n; j++) {//遍历状态转移操作选项,n为可进行操作数

                if (g[i] != g[j] && w[i].size() == w[j].size() && diffs(w[i], w[j]) == 1) {
                    clen = dfs(j, depth + 1); //进行下一重递归
                    if (clen+1 > len[i]) {
                        len[i] = clen + 1;
                        next[i] = j;
                    }
                }
            }
            return len[i];
            };
        int mxhead = 0,mxlen=0;
        for (int i = 0; i < n; i++) {
            if (mxlen < dfs(i, 0)) {
                mxlen = len[i];
                mxhead = i;
            }
        }
        vector<string> rst;
        int cur = mxhead; 
        rst.push_back(w[cur]);
        while (cur<next[cur]) {
            rst.push_back(w[next[cur]]);
            cur = next[cur];
        }
        return rst;
    }
};