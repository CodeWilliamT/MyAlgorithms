using namespace std;

#include <iostream>

class Solution {
public:
    int hammingWeight(uint32_t n) {
        if (!n) return 0;
        int ans = n % 2;
        return ans+ hammingWeight(n/2);
    }
};

//int main()
//{
//    Solution s;
//    uint32_t n;
//    while (cin >> n)
//        cout << s.hammingWeight(n) << endl;
//    return 0;
//}