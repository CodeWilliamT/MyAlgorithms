using namespace std;
#include <iostream>
#include <vector>
//��ģ��
//����ÿ��f[i]==1��f[j]==-1֮������0����Ŀ
class Solution {
public:
    int captureForts(vector<int>& f) {
        int rst = 0, tmp = 0;
        bool met1 = 0,met2=0;
        for (int& e : f) {
            if (e == -1) {
                if (met1)rst=max(rst,tmp);
                tmp = 0;
                met1 = 0;
                met2 = 1;
            }
            else if (!e)
                tmp++;
            else {
                if (met2)rst = max(rst, tmp);
                tmp = 0;
                met1 = 1;
                met2 = 0;
            }
        }
        return rst;
    }
};