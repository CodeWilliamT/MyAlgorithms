using namespace std;
#include <iostream>
#include <vector>
//动态规划 前缀和
//最大前半和[x-1]=arr[i]+(最大前半和[i-1]<0?0:最大前半和[i-1])
//最大后半和[n-1-i]=arr[n-1-i]+(最大后半和[n-1-(i-1)]<0?0:最大后半和[n-1-(i-1)]);
//rst=最大前半和[x-1]+最大后半和[x+1]+max(0,arr[x]);
class Solution {
public:
    int maximumSum(vector<int>& arr) {
        int n=arr.size();
        if(n==1)return arr[0];
        vector<int> subHeadMax=vector<int>(n,0),subTailMax=vector<int>(n,0);
        int rst;
        subHeadMax[0]=arr[0];
        subTailMax[n-1]=arr[n-1];
        for(int i=1;i<n-1;i++){
            subHeadMax[i]=arr[i]+max(0,subHeadMax[i-1]);
            subTailMax[n-1-i]=arr[n-1-i]+max(0,subTailMax[n-1-(i-1)]);
        }
        rst=max(max(0,arr[0])+subTailMax[1],max(0,arr[n-1])+subHeadMax[n-2]);
        int tmpmax;
        for(int i=1;i<n-1;i++){
            tmpmax=max(0,arr[i])+max({subHeadMax[i-1],subTailMax[i+1],subHeadMax[i-1]+subTailMax[i+1]});
            rst=max(rst,tmpmax);
        }
        return rst;
    }
};