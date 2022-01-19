using namespace std;
#include <iostream>
#include <vector>
//回溯
//以合法的规则生成所有可能性。cntr<cntl<n
class Solution {
private:
    void dfs(int cntl,int cntr,int n,string s,vector<string>& rst)
    {
        if (cntl == n && cntr == cntl)
        {
            rst.push_back(s);
            return;
        }
        if (cntr < cntl )
        {
            dfs(cntl, cntr +1, n, s+')', rst);
        }
        if (cntl < n)
        {
            dfs(cntl +1, cntr, n, s+'(', rst);
        }
    }
public:
    vector<string> generateParenthesis(int n) {
        vector<string> rst;
        dfs(0, 0, n, "", rst);
        return rst;
    }
};