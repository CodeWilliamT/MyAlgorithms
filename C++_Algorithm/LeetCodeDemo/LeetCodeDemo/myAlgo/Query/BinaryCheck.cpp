#include"myHeader.h"

class BinaryCheck {
private:
    vector<int> nums;
    vector<vector<int>> g;
    int N, M;
public:

    //��false��true�����ֲ���ʹcheckΪtrue����Сֵ
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
	//��true��false�����ֲ���ʹcheckΪtrue�����ֵ
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