using namespace std;
#include <vector>
#include <queue>
//堆排序
//每次取最大的，和-掉该数，减半该数,重新求和。
//坑:用double
class Solution {
public:
    int halveArray(vector<int>& nums) {
        priority_queue<double> pq;
        double sum = 0,nsum;
        for (int& e : nums) {
            pq.push(e*1.0);
            sum +=e;
        }
        nsum = sum;
        double val;
        int rst = 0;
        while (2 * nsum > sum) {
            val = pq.top();
            pq.pop();
            nsum -= val;
            val = val / 2;
            nsum += val;
            pq.push(val* 1.0);
            rst++;
        }
        return rst;
    }
};