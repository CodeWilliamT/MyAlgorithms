using namespace std;
#include "..\myHeader.h"
typedef pair<int, int> pii;

class DFSBasic {
private:
    vector<vector<int>> g;
public:
    int minSteps;//�ִ��յ�Ĳ�������������-1
    //��������߽�,����һ���򷵻�true
    struct state {
        int a=0;
    }; 
    struct state2 {
        int a = 0;
    };
    //λͼ���ѣ����صִ��յ㲽�����������򷵻�-1
    void DFS(state st, state2 st2)
    {
    }
    void DFS()
    {
        function<void(int, int, uint32_t)> dfs = [&](int idx, int sum, uint32_t rep) {
            return;
        };
    }
};