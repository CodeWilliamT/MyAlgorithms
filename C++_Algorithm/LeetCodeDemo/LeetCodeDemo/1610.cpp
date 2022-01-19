using namespace std;
#include <vector>
#include <algorithm>
//两分查找 nlogn
//计算各个点与东向的角度，按角度排序，遍历每个顶点，两分查找刚好大于每个顶点i角度+angle的索引idx，比较为idx-i的最大值
//坑：同点 解决：累加
//坑：精度 解决：存弧度转角度 乘1000的数值
//坑：-359到0度 解决：v[i] + 1000 * angle>=360000则加取模后的
class Solution {
public:
    int visiblePoints(vector<vector<int>>& points, int angle, vector<int>& location) {
        int n = points.size();
        vector<int> v;
        int same = 0;
        int ang = 0;
        for (int i = 0; i < n; i++) {
            if (!(points[i][0] - location[0]) && !(points[i][1] - location[1])) {
                same++; continue;
            }
            ang = acos((points[i][0] - location[0]) * 1.0 / sqrt(pow(points[i][1] - location[1], 2) + pow(points[i][0] - location[0], 2))) * 180000 / acos(-1);
            if (points[i][1] - location[1] < 0)
                ang = 360000 - ang;
            v.push_back(ang);

        }
        sort(v.begin(), v.end());
        int rst = 0;
        int idx = 0;
        for (int i = 0; i < v.size(); i++) {
            idx = upper_bound(v.begin(), v.end(), (v[i] + 1000 * angle)) - v.begin();
            if(v[i] + 1000 * angle>=360000)
                idx += upper_bound(v.begin(), v.end(), (v[i] + 1000 * angle) % 360000) - v.begin();
            rst = max(rst, idx - i);
        }
        return rst + same;
    }
};