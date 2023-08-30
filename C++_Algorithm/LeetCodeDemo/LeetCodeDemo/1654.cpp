using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <queue>
//���� ��ϣ
//ע��߽�ΪMAXX*3;
class Solution {
#define MAXX 6000
#define MAXY 2
    struct Node {
        int x;
        int y;
    };
public:
    int minimumJumps(vector<int>& forbidden, int a, int b, int x) {
        unordered_set<int> fb(forbidden.begin(), forbidden.end());
        typedef pair<int, int> pii;
        bool v[MAXX * MAXY + MAXY +1]{};
        queue<Node> q;
        auto judge = [&](Node& nd) {//��������߽�,����һ���򷵻�true
            return !fb.count(nd.x)&& nd.x >= 0 && nd.x <= MAXX;
        };
        q.push({ 0,0});
        int witdh;
        int steps = 0;//������
        Node cur,next;
        while (!q.empty()) {
            witdh = q.size();
            while (witdh--) {
                cur = q.front();
                q.pop();
                if (!judge(cur) || v[cur.x * 2 + cur.y]) {
                    continue;//����߽����
                }
                v[cur.x * 2 + cur.y] = 1;//����
                //�ִ��յ�
                if (cur.x == x) {
                    return steps;
                }
                next = { cur.x + a ,0};
                if(judge(next)) {
                    q.push(next);//������һ��
                }
                next = { cur.x - b,1};
                if(judge(next) && cur.y != 1) {
                    q.push(next);//������һ��
                }
            }
            steps++;
        }
        return -1;
    }
};