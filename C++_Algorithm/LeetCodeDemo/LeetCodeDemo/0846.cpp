using namespace std;
#include <vector>
#include <map>
//朴素实现 哈希
//如果不能整除，那就不行，可以，则做哈希根据值统计频率，从最小值出发每groupSize个一组查看是否符合条件。
class Solution {
public:
    bool isNStraightHand(vector<int>& hand, int groupSize) {
        if (hand.size() % groupSize)return false;
        map<int, int> mp;
        for (int& e : hand)
            mp[e]++;
        for (auto it = mp.begin(); it != mp.end(); ++it) {
            auto& e = *it;
            while (e.second > 0) {
                for(int i=0;i< groupSize;i++)
                if (mp[e.first + i]>0) {
                    mp[e.first + i]--;
                }
                else {
                    return false;
                }
            }
        }
        return true;
    }
};