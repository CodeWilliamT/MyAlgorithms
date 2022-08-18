using namespace std;
#include <vector>
#include <unordered_map>
//简单模拟 哈希 贪心
//能完成任务尽快完成
//需要个哈希记录每类任务的最后完成时间。
class Solution {
    typedef long long ll;
public:
    long long taskSchedulerII(vector<int>& tasks, int space) {
        ll rst = 0;//经历了的时间
        unordered_map<int, ll> mp;
        for (auto& e : tasks) {//遍历任务
            if (mp.count(e)/*没做过*/ && rst < mp[e] + space/*没cd*/) {
                rst = mp[e] + space;//等cd累计天数
            }
            rst++;//做任务用了一天
            mp[e] = rst;//记录cd
        }
        return rst;
    }
};