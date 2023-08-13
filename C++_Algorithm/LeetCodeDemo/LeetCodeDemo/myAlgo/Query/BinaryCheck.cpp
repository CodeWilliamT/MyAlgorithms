#include"myHeader.h"

class BinaryCheck {
private:
    vector<int> nums;
    vector<vector<int>> g;
    int N, M;
public:
    bool check(int x) {
        return true;
    }
    //位图中两分查找,返回满足check的最大值
    int GetTEdge(vector<vector<int>> grid, int l, int r) {
        g = grid;
        N = g.size();
        M = g[0].size();
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
	//一维图中两分查找
    int GetTEdge(vector<int> a,int l,int r){
        nums = a;
        N = nums.size();
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