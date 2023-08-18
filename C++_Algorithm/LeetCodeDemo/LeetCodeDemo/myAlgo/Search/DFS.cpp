using namespace std;
#include "..\myHeader.h"
typedef pair<int, int> pii;

class DFS {
private:
    vector<vector<int>> g;
public:
    int minSteps;//抵达终点的步骤数，不能则-1
    //处理特殊边界,能下一步则返回true
    struct state {
        int a=0;
    }; 
    struct state2 {
        int a = 0;
    };
    //位图广搜，返回抵达终点步骤数，不能则返回-1
    void DFS(state st, state2 st2)
    {

    }
};