using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <map>
//找规律 哈希
//规律：1、4个顶点只出现一次 2、其他顶点只出现2次或4次 3、总面积=分面积和
class Solution {
public:
    bool isRectangleCover(vector<vector<int>>& rec) {
        map<pair<int, int>, int> mp;
        int n = rec.size();
        long long size = 0;
        int xmin = rec[0][0], ymin = rec[0][1], xmax = rec[0][2], ymax = rec[0][3];
        for (auto& e : rec)
        {
            xmin = min(xmin, e[0]);
            ymin = min(ymin, e[1]);
            xmax = max(xmax, e[2]);
            ymax = max(ymax, e[3]);
            size += (e[2] - e[0]) * (e[3] - e[1]);
            mp[{e[0], e[1]}]++;
            mp[{e[2], e[1]}]++;
            mp[{e[2], e[3]}]++;
            mp[{e[0], e[3]}]++;
        }
        pair<int, int> p0 = { xmin, ymin };
        pair<int, int> p1 = { xmax, ymin };
        pair<int, int> p2 = { xmax, ymax };
        pair<int, int> p3 = { xmin, ymax };
        if (size != (xmax - xmin) * (ymax - ymin) || !mp[p0] || !mp[p1]
            || !mp[p2]|| !mp[p3])
            return false;
        mp.erase(mp.find(p0));
        mp.erase(mp.find(p1));
        mp.erase(mp.find(p2));
        mp.erase(mp.find(p3));
        for (auto& e : mp)
        {
            if (e.second != 2 && e.second != 4)
            {
                return false;
            }

        }
        return true;
    }
};