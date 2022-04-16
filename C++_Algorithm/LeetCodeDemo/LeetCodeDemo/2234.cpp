using namespace std;
#include <vector>
#include <algorithm>
//枚举 两分 麻烦题 贪心 
//枚举不完美花园数，两分查找以f[x]为最大最小花数需要消耗的花数小于等于可用花数的x数目，
//当不完美花园数确定时，不完美花园花朵的最大最小值确定。
//从0开始枚举到f[i]>=target枚举不完美花园数
//记录前缀和
//(前x项和+可用花)/x=前x项的最大最小花数
//以f[i]为最大最小花数需要消耗的花数=(ll)i *f[i] - sums[i];
//x*target-后x项和=后x项完美所需的花数，n-这个数=可用花数。
//枚举不完美花园数，两分查找以f[x]为最大最小花数需要消耗的花数小于等于可用花数的x数目，
//i从0开始，所以即查找刚大于x的那个数作为x的数目
class Solution {
    typedef long long ll;
public:
    long long maximumBeauty(vector<int>& f, long long n, int target, int full, int partial) {
        sort(f.begin(), f.end());
        int len = f.size();
        int l=0, r=0;//最大不完美花园数
        r = lower_bound(f.begin(), f.end(), target) - f.begin();
        vector<ll> sums(r + 1, 0);//前缀和
        vector<ll> miss(r, 0);//前面i个花园的花数目均达到当前数f[i]的值需要多少朵花
        for (int i = 1; i <=r; i++) {
            sums[i] = sums[i - 1] + f[i - 1];
            if (i < r)miss[i] = (ll)i *f[i] - sums[i];
        }
        ll left;
        ll mn,perfect,tmp;
        ll rst = 0; 
        int idx;
        for (int i = 0; i <= r; i++) {
            left = n-((ll)(r - i) * target - (sums[r] - sums[i]));
            if (left < 0)continue;
            perfect = len - i;
            tmp = perfect * full;
            if (i > 0){
                idx = upper_bound(miss.begin(), miss.begin()+i, left)- miss.begin();
                mn = (sums[idx] + left) / idx;
                mn = mn >= target ? target - 1 : mn;
                tmp += mn * partial;
            } 
            rst = max(tmp, rst);
        }
        return rst;
    }
};