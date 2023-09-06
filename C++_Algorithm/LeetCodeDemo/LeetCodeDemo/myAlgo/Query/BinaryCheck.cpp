#include"myHeader.h"

class BinaryCheck {
private:
    vector<int> nums;
    vector<vector<int>> g;
    int N, M;
public:

    //左false右true，两分查找使check为true的最小值
    int GetFTEdge(vector<int> a, int start, int end) {
        int l = start, r = end;

        auto check = [&](int x) {
            return true;
        };
        int m;
        while (l < r) {
            m = (l + r) / 2;
            if (check(m))
                r = m;
            else
                l = m + 1;
        }
        return r;
    }
	//左true右false，两分查找使check为true的最大值
    int GetTFEdge(vector<int> a,int start,int end){
        int l = start, r = end;
        auto check = [&](int x) {
            return true;
        };
        int m;
        while (l < r) {
            m = (l + r + 1) / 2;
            if (check(m))
                l = m;
            else
                r = m - 1;
        }
        return l;
	}
};