using namespace std;
#include <string>
//细致条件分析 找规律 模拟
//分类讨论
//类："0";10的次方;其他(比较，大，较小，本身)
class Solution {
public:
    string nearestPalindromic(string n) {
        int len = n.size();
        if (len == 1 && n[0] == '0')return "1";
        if (n.compare("11") == 0)return "9";
        long long num = stoll(n), tmp = num;
        while (tmp % 10 == 0) {
            tmp /= 10;
        }
        if (tmp == 1)
            return to_string(num - 1);
        string sub = n.substr(0, (len + 1) / 2);
        long long subN = stoll(sub);
        string bigSub = to_string(subN + 1);
        string smallSub = to_string(subN - 1);
        int lenb = bigSub.size(), lens = smallSub.size();
        for (int i = 0; i < len / 2; i++) {
            bigSub.push_back(bigSub[i]);
            smallSub.push_back(smallSub[i]);
        }
        reverse(bigSub.begin() + lenb, bigSub.end());
        reverse(smallSub.begin() + lens, smallSub.end());
        long long big = stoll(bigSub);
        long long small = stoll(smallSub);
        for (int i = 0; i < len / 2; i++) {
            n[len - 1 - i] = n[i];
        }
        long long nn = stoll(n);
        if(nn==num)
            return abs(big - num) >= abs(num - small) ? smallSub : bigSub;
        else if(nn<num)
            return abs(big - num) >= abs(num - nn) ? n : bigSub;
        else 
            return abs(nn - num) >= abs(num - small) ? smallSub : n;
    }
};