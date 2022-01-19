using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>

//巧思，哈希，滑动窗口思想
//sum存总和
//a[j]改的位置，a[i]划分的位置
//sums[i]存前i个元素和(包含a[i])
//用lhash(d,cnt)记录左侧前缀和sum-2*sums[i]==d的数目cnt，
//用rhash(d,cnt)记录右侧后缀和2*sums[i]-sum==d的数目cnt，
//令d=sum-2*sums[i]
//从i=0遍历到n-2当d==0,则符合，累加为初始答案,用rhash(d,cnt)累加记录右侧后缀和2*sums[i]-sum==d的数目cnt，
//从i=0开始遍历到n-1各个修改a[i]为k的情形，
//ans = max(ans,lhash[a[i] - k] + rhash[k - a[i]]);
//d = sum - 2 * sums[i];lhash[d]++;rhash[d]--;
class Solution {
public:
    int waysToPartition(vector<int>& a, int k) {
        int n = a.size();
        long long sum = 0;
        unordered_map<long long, int> lhash, rhash;
        vector<long long>sums(n);
        for (int i = 0; i < n; i++)
        {
            sum += a[i];
            sums[i] = sum;
        }
        int ans = 0;
        long long d;
        for (int i = 0; i < n-1; i++)
        {
            d = sum - 2 * sums[i];
            rhash[d]++;
            if (!d)
                ans++;
        }
        for (int i = 0; i < n; i++)
        {
            d = sum - 2 * sums[i];
            ans = max(ans,lhash[a[i] - k] + rhash[k - a[i]]);
            lhash[d]++;
            rhash[d]--;
        }
        return ans;
    }
};