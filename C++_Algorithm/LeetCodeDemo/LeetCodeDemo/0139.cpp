using namespace std;
#include <iostream>
#include <vector>
#include <string>
//动态规划
class Solution {
public:
    bool wordBreak(string s, vector<string>& a) {
        int m=s.size();
        int n=a.size();
        vector<bool> f(m + 1);
        vector<int> d(n);
        //bool f[m+1];
        //int d[n];
        //memset(f,false,sizeof(f));
        for(int j=0;j<n;j++)
        {
            d[j]=a[j].size();
            if(a[j]==s.substr(0,d[j]))
                f[d[j]]=true;
        }
        for(int i=1;i<=m;i++)
        {
            for(int j=0;j<n;j++)
            {
                if(i-d[j]>=0)
                    if(f[i-d[j]])
                        if(s.substr(i-d[j],d[j])==a[j])
                        {
                            f[i]=true;
                            break;
                        }
            }
        }
        return f[m];
    }
};