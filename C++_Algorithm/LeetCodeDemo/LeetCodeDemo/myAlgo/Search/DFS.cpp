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
        bool v[MAXN * MAXM + MAXM + 1]{};
        auto judge = [&](Node& nd) {//��������߽�,����һ���򷵻�true
            return nd.x >= 0 && nd.x <= MAXN && nd.y >= 0 && nd.y <= MAXM;
        };
        auto hash= [&](Node& nd) {//��������߽�,����һ���򷵻�true
            return nd.x*m+nd.y;
        };
        function<int(Node)> dfs = [&](Node cur) {
            //�ѿ��н⵽��ݹ�߽������
            if (v[hash(cur)] && !judge(cur))//�ж�״̬�����ԣ���״̬�����У�������
                return 0;
            if (cur.x == n)//�Ƿ��������++���������ý����
                return 1;
            Node next;
            for (int i = 0; i < 4; i++) {//����״̬ת�Ʋ���ѡ��,nΪ�ɽ��в�����
                next = cur;//������һ״̬��
                if (v[hash(next)] && !judge(next))//�ж�״̬�����ԣ���״̬�����У�������
                    continue;
                //״̬�ƽ�; ��֦��Ǽ�Ϊ1;
                dfs(next); //������һ�صݹ�
                //��ǻ�ԭ;׼����һ��������
                //״̬��ԭ;״̬��ԭΪδ���ж���ʱ
            }
        };
    }
};