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
            if (v[hash(cur)] && !judge(cur))//�ж�״̬�����ԡ��߽硢ȥ�أ���״̬�����У�������
                return 0;
            if (cur.x == n)//�ִ��ص㣬�ж�
                return 1;
            Node next;
            for (int i = 0; i < 4; i++) {//����״̬ת�Ʋ���ѡ��,nΪ�ɽ��в�����
                next = cur;//������һ״̬��
                if (v[hash(next)] && !judge(next))//�ж�״̬�����ԣ���״̬�����У�������
                    continue;
                //״̬�ƽ�; ��֦��Ǽ�Ϊ1;
                dfs(next); //������һ�صݹ�
                //״̬��ԭ;�ָ����;״̬��ԭΪδ���ж���ʱ
            }
        };
    }
};
class DFSTree {
    struct RstNode {
        int selected = 0;
        int noSelected = 0;
    };
public:
    RstNode dfs(TreeNode* cur) {
        if (!cur)//�ж�״̬�����ԣ���״̬�����У�������
            return { 0,0 };
        RstNode l = dfs(cur->left);
        RstNode r = dfs(cur->right);
        return { cur->val + l.noSelected + r.noSelected,max(l.noSelected,l.selected) + max(r.noSelected,r.selected) };
    }
    int DFS(TreeNode* root) {
        RstNode rst = dfs(root);
        return max(rst.selected, rst.noSelected);
    }
};