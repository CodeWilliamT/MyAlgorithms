using namespace std;
#include <vector>
#include <algorithm>
//找规律 细致条件分析
//电池数大于电脑数，则较少的len-n个为多出来的，其总和为补短能力，则答案为补短之后最短的那个。
//答案为 补充后的平均电量 (补短能力+n个中较小电量的总和)/n个中较小电量的数量<=未计算那个的电量时 的平均电量。
class Solution {
public:
    long long maxRunTime(int n, vector<int>& batteries) {
        int len = batteries.size();
        sort(batteries.begin(), batteries.end());
        long long sum = 0;
        long long average=0;
        int cnt = 0;
        for(int i = 0; i<len; i++) {
            sum += batteries[i];
            if (i >= len - n){
                cnt++; 
                average = sum / cnt;
                if (i<len-1&&average <= batteries[i+1]) {
                    break;
                }
            }
        }
        return average;
    }
};