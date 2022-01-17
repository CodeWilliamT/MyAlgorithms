using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划
//状态量为：所求位置最小路径和
//边界为d[0][0]=g[0][0];d[0][j]=d[0][j-1]+g[0][j],d[i][0]=d[i-1][0]+g[i][0];
//转换方程为d[i][j]=g[i][j]+min(d[i-1][j],d[i][j-1]);
class Solution {
public:
    int minPathSum(vector<vector<int>>& g) {
        int n=g.size();
        if(n<1)return 0;
        int m=g[0].size();
        if(m<1)return 0;
        vector<vector<int>> d(n,vector<int>(m));
        d[0][0]=g[0][0];
        for(int i=1;i<n;i++)
            d[i][0]=d[i-1][0]+g[i][0];
        for(int j=1;j<m;j++)
            d[0][j]=d[0][j-1]+g[0][j];
            
        for(int i=1;i<n;i++)
            for(int j=1;j<m;j++)
            {
                d[i][j]=g[i][j]+min(d[i-1][j],d[i][j-1]);
            }
        return d[n-1][m-1];
    }
};