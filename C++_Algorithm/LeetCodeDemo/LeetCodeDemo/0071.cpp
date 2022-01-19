using namespace std;
#include <iostream>
//朴素实现 细致条件分析
//遇到多//杠变/
//遇到/./无动作
//遇到/../删除上一级
class Solution {
public:
    string simplifyPath(string path) {
        string rst="/";
        int n = path.size();
        for (int i = 1; i < n;i++) {
            if (path[i - 1] == '/' && (path[i] == '/' || path[i] == '.' && (i + 1 == n || path[i + 1] == '/'))){
                continue;
            }
            else if (i>1&&path[i-2]=='/'&&path[i - 1] == '.' && path[i] == '.'&&(i+1==n||path[i+1]=='/')) {
                rst.pop_back();
                if(rst.size()>1)rst.pop_back();
                while(rst.back()!='/')
                    rst.pop_back();
                i++;
            }
            else {
                if(rst.back()!='/'||path[i] != '/')rst.push_back(path[i]);
            }
        }
        if (rst.back() == '/'&&rst.size()>1)rst.pop_back();
        return rst;
    }
};