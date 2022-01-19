using namespace std;
#include <iostream>
#include <vector>
#include <unordered_map>
//巧思
//选过的就映射边界。边界原本被选过了，映射了别的那就映射边界原本映射的。
class Solution {
private:
    int mlen, nlen, cur, len;
    unordered_map<int, int> mp;
public:
    Solution(int m, int n) {
        cur = m * n;
        mlen =m;
        nlen =n;
        mp.clear();
    }

    vector<int> flip() {
        int x = rand() % cur;
        int ans=x;
        if (mp.count(x))ans = mp[x];
        cur--;
        if (mp.count(cur)) {
            mp[x] = mp[cur];
        }
        else {
            mp[x] = cur;
        }
        return { ans / nlen,ans % nlen };
    }

    void reset() {
        mp.clear();
        cur = mlen * nlen;
    }
};