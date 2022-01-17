using namespace std;
#include <vector>
//巧思 哈希
//按年龄分组f记人数，年龄组间请求为 x != y ? f[x] * f[y] : f[x] * (f[y] - 1);
class Solution {
public:
    int numFriendRequests(vector<int>& ages) {
        int n = ages.size(),rst=0;
        int f[121]{};
        for (int i = 0; i < n; i++) {
            f[ages[i]]++;
        }
        for (int x = 1; x < 121; x++) {
            for (int y = 1; y < 121; y++) {
                if (y <= 0.5 * x + 7 ||y > x ||y > 100 && x < 100)
                    continue;
                rst += x != y ? f[x] * f[y] : f[x] * (f[y] - 1);
            }
        }
        return rst;
    }
};