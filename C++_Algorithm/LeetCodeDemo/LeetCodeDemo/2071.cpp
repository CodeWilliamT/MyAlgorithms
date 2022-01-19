using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
#include <set>
//两分+贪心+两分+哈希集，困难题
//时间复杂度 O(min(n,m)*log(min(n,m))^2)
//排序两个数组
//最大完成任务数：
//所有任务都被完成或所有工人都干完一个活：r=min(n,m);
//最少任务数：l=0
//判断能否完成x个任务：
//x个最简单的任务由难到易判断，x个工人由强到弱，判断有没有工人能胜任这个任务，如果有那就让他上，否则就找一个吃完药能胜任中power最弱的工人上(再两分)
//坑：
//自带的两分时效高
//过不了 :
//auto target = lower_bound(ws.begin(), ws.end(), tasks[i] - strength);
//过得了：
//auto target = ws.lower_bound(tasks[i] - strength);
class Solution {
private:
    bool check(int a, vector<int>& tasks, vector<int>& workers, int pills, int strength)
    {
        int m = workers.size();
        multiset<int> ws;
        for (int i = m - a; i < m; ++i) {
            ws.insert(workers[i]);
        }
        for (int i = a-1; i >-1;i--)
        {
            auto it = prev(ws.end());
            if (*it >= tasks[i])
            {
                ws.erase(it);
            }
            else if (pills)
            {
                //过不了:
                //auto target = lower_bound(ws.begin(), ws.end(), tasks[i] - strength);
                auto target = ws.lower_bound(tasks[i] - strength);
                if (target ==ws.end())
                    return false;
                ws.erase(target);
                pills--;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
public:
    int maxTaskAssign(vector<int>& tasks, vector<int>& workers, int pills, int strength) {
        sort(tasks.begin(), tasks.end());
        sort(workers.begin(), workers.end());
        int r = min(tasks.size(), workers.size());
        int l = 0, mid;
        while (l < r)
        {
            mid = (l + r+1) / 2;
            if (check(mid, tasks, workers, pills, strength))
            {
                l = mid;
            }
            else
                r = mid - 1;
        }
        return l;
    }
};