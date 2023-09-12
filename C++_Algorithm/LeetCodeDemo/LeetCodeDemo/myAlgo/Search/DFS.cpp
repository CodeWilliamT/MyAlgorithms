using namespace std;
#include "..\myHeader.h"
typedef pair<int, int> pii;

class DFSBasic {
#define MAXN 6000
#define MAXM 2
    struct Node {
        int x;
        int y;
        string path;
        bool operator==(Node const& a) const {
            return a.x == x && a.y == y;
        }
    };
public:
    void DFS()
    {
        vector<vector<int>> g;
        int n = g.size();
        int m = g[0].size();
        bool v[MAXN]{};
        auto judge = [&](Node& nd) {//��������߽�,����һ���򷵻�true
            return nd.x >= 0 && nd.x <= MAXN && nd.y >= 0 && nd.y <= MAXM;
        };
        function<int(Node&)> dfs = [&](Node& cur) {
            if (cur.x == n)//�ѿ��н⵽��ݹ�߽����������Ҫ�Ƿ��������++���������ý����
                return 1;
            Node next;
            for (int i = 0; i < 4; i++) {//����״̬ת�Ʋ���ѡ��,nΪ�ɽ��в�����
                next = cur;//������һ״̬��
                if (v[cur.x] && judge(next))//�ж�״̬�����ԣ���״̬���У�����г���
                {
                    //״̬�ƽ�; ��֦��Ǽ�Ϊ1;
                    dfs(next); //������һ�صݹ�
                    //��ǻ�ԭ;׼����һ��������
                    //״̬��ԭ;״̬��ԭΪδ���ж���ʱ
                }
            }
        };
    }
};