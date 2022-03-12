using namespace std;
#include <vector>
#include <bitset>
//麻烦题 模拟 位运算
class Solution {
public:
    bool validUtf8(vector<int>& data) {
        int n = data.size();
        int c;
        int len = 1;
        for (int i = 0; i < n; i += max(len, 1)) {
            c = data[i];
            len = (c >> 7 & 1) == 0 ? 1 : (c >> 6 & 1) == 0 ? -1 : (c >> 5 & 1) == 0 ? 2 : (c >> 4 & 1) == 0 ? 3 : (c >> 3 & 1) == 0 ? 4 : -1;
            if (len == -1 || i + len > n)
                return false;
            for (int j = i + 1; j < i + len; j++) {
                c = data[j];
                if ((c >> 7 & 1) != 1 || (c >> 6 & 1) != 0)
                    return false;
            }
        }
        return true;
    }
};
//麻烦题 模拟 位图
//前缀为哪一类，长度是否符合该类、前缀10
class Solution {
public:
    bool validUtf8(vector<int>& data) {
        int n = data.size();
        char c;
        int len=1;
        bitset<8> b;
        for (int i = 0; i < n; i+=max(len,1)) {
            c = data[i];
            b = bitset<8>(c);
            len = !b[7] ? 1 : !b[6] ? -1 : !b[5] ? 2 : !b[4] ? 3 : !b[3] ? 4 : -1;
            if (len == -1 || i+len > n)
                return false;
            for (int j = i+1; j < i+len; j++) {
                c = data[j];
                b = bitset<8>(c);
                if (b[7] != 1 || b[6] != 0)
                    return false;
            }
        }
        return true;    
    }
};